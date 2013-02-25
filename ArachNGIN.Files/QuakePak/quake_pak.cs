/*
 * Copyright (c) 2006-2013 Michal Kuncl <michal.kuncl@gmail.com> http://www.pavucina.info
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
 * associated documentation files (the "Software"), to deal in the Software without restriction,
 * including without limitation the rights to use, copy, modify, merge, publish, distribute,
 * sublicense, and/or sell copies of the Software, and to permit persons to whom the Software
 * is furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all copies or
 * substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
 * PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
 * FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
 * ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using System;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using ArachNGIN.Files.Streams;

namespace ArachNGIN.Files
{
    /// <summary>
    /// Tøída na ètení z PAK souborù Quaka
    /// </summary>
    public class QuakePak : IDisposable
    {
        private static readonly char[] PakId = new char[4] {'P', 'A', 'C', 'K'};
        private readonly BinaryReader _pakReader;
        private readonly FileStream _pakStream;

        /// <summary>
        /// Seznam souborù v PAKu
        /// </summary>
        public StringCollection PakFileList = new StringCollection();

        private int _pFatstart;
        private int _pFilecount;
        private PakFat[] _pakFat;

        /// <summary>
        /// Konstruktor - otevøe pak soubor a naète z nìj hlavièku.
        /// </summary>
        /// <param name="strFileName">jméno pak souboru</param>
        /// <param name="bAllowWrite">povolit zápis do souboru, když je true <c>true</c> tak ano.</param>
        /// <exception cref="System.IO.FileNotFoundException">
        /// nastane když se soubor nenajde
        /// </exception>
        public QuakePak(string strFileName, bool bAllowWrite)
        {
            var info = new FileInfo(strFileName);
            if (info.Exists == false)
            {
                throw new FileNotFoundException("Can''t open " + strFileName);
            }
            // soubor existuje
            if (bAllowWrite)
            {
                _pakStream = new FileStream(strFileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            }
            else
            {
                _pakStream = new FileStream(strFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            }
            _pakReader = new BinaryReader(_pakStream, Encoding.GetEncoding("Windows-1250"));
            //
            if (!ReadHeader())
            {
                _pakStream.Close();
                throw new FileNotFoundException("File " + strFileName + " has unsupported format");
            }
        }

        #region IDisposable Members

        /// <summary>
        /// Oficiální Destruktor
        /// </summary>
        public void Dispose()
        {
            try
            {
                Close();
            }
            catch
            {
                //
            }
        }

        #endregion

        /// <summary>
        /// Neoficiální destruktor
        /// </summary>
        public void Close()
        {
            _pakReader.Close();
            _pakStream.Close();
            _pakStream.Dispose();
        }

        private bool ReadHeader()
        {
            _pakStream.Position = 0;
            string pHeader = StreamHandling.PCharToString(_pakReader.ReadChars(PakId.Length));
            if (pHeader == StreamHandling.PCharToString(PakId))
            {
                // hned za hlavickou je pozice zacatku fatky
                _pFatstart = _pakReader.ReadInt32();
                // a pak je pocet souboru * 64
                _pFilecount = _pakReader.ReadInt32()/64;
                //
                // presuneme se na pozici fatky a nacteme ji
                _pakStream.Position = _pFatstart;
                _pakFat = new PakFat[_pFilecount];
                // vymazneme filelist
                PakFileList.Clear();
                for (int i = 0; i < _pFilecount; i++)
                {
                    // my radi lowercase. v tom se lip hleda ;-)
                    string sfile = StreamHandling.PCharToString(_pakReader.ReadChars(56)).ToLower();
                    sfile = sfile.Replace("/", "\\"); // unixovy lomitka my neradi.
                    // pridame soubor do filelistu a do PakFATky
                    PakFileList.Add(sfile);
                    _pakFat[i].FileName = sfile;
                    _pakFat[i].FileStart = _pakReader.ReadInt32();
                    _pakFat[i].FileLength = _pakReader.ReadInt32();
                }
                _pakStream.Position = 0;
                //
                return true;
            }
            else
            {
                _pakStream.Position = 0;
                return false;
            }
        }

        /// <summary>
        /// Zkontroluje, jestli je soubor zadaného jména v pak souboru
        /// </summary>
        /// <param name="strFileInPak">jméno hledaného souboru</param>
        /// <returns>je/není</returns>
        public bool PakFileExists(string strFileInPak)
        {
            return GetFileIndex(strFileInPak) != -1;
        }

        private int GetFileIndex(string strFileInPak)
        {
            for (int i = 0; i < _pakFat.Length; i++)
            {
                if (_pakFat[i].FileName.ToLower() == strFileInPak.ToLower())
                {
                    // soubor nalezen, vracime jeho cislo
                    return i;
                }
            }
            // soubor nenalezen
            return -1;
        }

        /// <summary>
        /// Rozbalí soubor z paku do proudu
        /// </summary>
        /// <param name="strFileInPak">jméno souboru v paku</param>
        /// <param name="sOutput">výstupní proud</param>
        public void ExtractStream(string strFileInPak, Stream sOutput)
        {
            int fIndex = GetFileIndex(strFileInPak);
            if (fIndex == -1) return; // soubor v paku neni, tudiz konec.
            sOutput.SetLength(0);
            _pakStream.Seek(_pakFat[fIndex].FileStart, SeekOrigin.Begin);
            StreamHandling.StreamCopy(_pakStream, sOutput, _pakFat[fIndex].FileLength);
        }

        /// <summary>
        /// Rozbalí soubor z paku na disk
        /// </summary>
        /// <param name="strFileInPak">jméno souboru v paku</param>
        /// <param name="strOutputFile">cesta k výstupnímu souboru</param>
        public void ExtractFile(string strFileInPak, string strOutputFile)
        {
            Stream fOutput = new FileStream(strOutputFile, FileMode.OpenOrCreate, FileAccess.ReadWrite,
                                            FileShare.ReadWrite);
            ExtractStream(strFileInPak, fOutput);
            fOutput.Close();
        }

        /// <summary>
        /// Vytvoøí nový prázdný pak soubor
        /// </summary>
        /// <param name="strFileName">jméno souboru.</param>
        /// <returns>jestli se zadaøí tak true, jinak false</returns>
        public static bool CreateNewPak(string strFileName)
        {
            // TODO: taky by to mohlo vracet true podle úspìšnosti :-)
            bool result = false;
            var FS = new FileStream(strFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            FS.Position = 0;
            var BW = new BinaryWriter(FS, Encoding.GetEncoding("Windows-1250"));
            BW.Write(PakId);
            Int32 pFatstart = PakId.Length;
            pFatstart += sizeof (Int32); // offset
            pFatstart += sizeof (Int32); // length
            const Int32 pFilecount = 0;
            BW.Write(pFatstart);
            BW.Write(pFilecount);
            BW.Close();
            FS.Close();
            return result;
        }

        private char[] PrepFileNameWrite(string filename)
        {
            var result = new char[56];
            // prepsat lomitka
            filename = filename.Replace("\\", "/");
            // kdyz to nekdo prepisk s nazvem souboru tak ho seriznout :-)
            if (filename.Length > 56)
            {
                filename.CopyTo(0, result, 0, 55);
            }
            else
            {
                filename.CopyTo(0, result, 0, filename.Length);
            }
            return result;
        }

        private void WriteFat()
        {
            // naseekovat startovní pozici fatky
            _pakStream.Seek(_pFatstart, SeekOrigin.Begin);
            var bw = new BinaryWriter(_pakStream, Encoding.GetEncoding("Windows-1250"));
            foreach (PakFat item in _pakFat)
            {
                // nazev souboru
                bw.Write(PrepFileNameWrite(item.FileName));
                bw.Write(item.FileStart);
                bw.Write(item.FileLength);
            }
            // naseekovat na pocet souboru a zapsat
            _pakStream.Seek(PakId.Length + sizeof (Int32), SeekOrigin.Begin);
            bw.Write(_pFilecount*64); // krat 64. z neznamych duvodu
            // bw.Close();
        }

        /// <summary>
        /// Pøidá proud do PAKu
        /// </summary>
        /// <param name="stream">proud</param>
        /// <param name="pakFileName">jméno souboru v PAKu</param>
        /// <param name="writeFAT">má se zapsat fatka? Pokud to není poslední pøidaný soubor, tak urèitì JO!</param>
        /// <returns>podle úspìšnosti buï true nebo false</returns>
        public bool AddStream(Stream stream, string pakFileName, bool writeFAT /*=true*/)
        {
            // soubor uz existuje --> dal se nebavime!
            if (PakFileExists(pakFileName)) return false;
            // novy soubor zapisujeme na pozici fatky
            _pakStream.Seek(_pFatstart, SeekOrigin.Begin);
            // vytvorit novou fatku a zapsat do ni novy soubor
            PakFat[] OldPakFAT = _pakFat;
            _pFilecount = OldPakFAT.Length + 1;
            _pakFat = new PakFat[_pFilecount];
            OldPakFAT.CopyTo(_pakFat, 0);
            _pakFat[_pFilecount - 1].FileName = pakFileName;
            _pakFat[_pFilecount - 1].FileLength = (int) stream.Length;
            _pakFat[_pFilecount - 1].FileStart = _pFatstart;
            // zapsat soubor
            StreamHandling.StreamCopy(stream, _pakStream, 0, _pakStream.Position);
            //StreamHandling.StreamCopyAsync(stream, PakStream);
            // po dokonceni zapisovani zjistit pozici streamu
            _pFatstart = (int) _pakStream.Position;
            //bw.Close();
            // zapsat fatku
            if (writeFAT)
            {
                _pakStream.Seek(PakId.Length, SeekOrigin.Begin);
                var bw = new BinaryWriter(_pakStream);
                // zapsat startovni pozici fatky
                bw.Write(_pFatstart);
                WriteFat();
            }
            return true;
        }

        /// <summary>
        /// Pøidá soubor do paku
        /// </summary>
        /// <param name="fileName">název souboru (napø. c:\windows\win.ini)</param>
        /// <param name="pakFileName">název souboru v paku (napø ini/win.ini)</param>
        /// <param name="writeFat">má se zapsat fatka? Pokud to není poslední pøidaný soubor, tak urèitì JO!</param>
        /// <returns>podle úspìšnosti buï true nebo false</returns>
        public bool AddFile(string fileName, string pakFileName, bool writeFat = true)
        {
            if (!File.Exists(fileName)) return false;
            bool result = false;
            try
            {
                Stream fstream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                result = AddStream(fstream, pakFileName, writeFat);
                fstream.Close();
            }
            catch
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// Pøidá soubor do paku
        /// </summary>
        /// <param name="fileName">název souboru (napø. c:\windows\win.ini)</param>
        /// <param name="pakFileName">název souboru v paku (napø ini/win.ini)</param>
        /// <returns>podle úspìšnosti buï true nebo false</returns>
        public bool AddFile(string fileName, string pakFileName)
        {
            return AddFile(fileName, pakFileName, true);
        }

        #region Nested type: PakFat

        private struct PakFat
        {
            public int FileLength;
            public string FileName;
            public int FileStart;
        }

        #endregion
    }
}