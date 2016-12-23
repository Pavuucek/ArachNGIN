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

using ArachNGIN.Files.Streams;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Text;

namespace ArachNGIN.Files.FileFormats
{
    /// <summary>
    ///     Class for reading Quake PAK files
    /// </summary>
    public partial class QuakePakFile : IDisposable
    {
        private static readonly char[] PakId = { 'P', 'A', 'C', 'K' };
        private readonly BinaryReader _pakReader;
        private readonly FileStream _pakStream;

        /// <summary>
        ///     The pak file list
        /// </summary>
        public StringCollection PakFileList = new StringCollection();

        /// <summary>
        ///     The position of start of FAT
        /// </summary>
        private int _pFatstart;

        /// <summary>
        ///     The count of files inside a PAK
        /// </summary>
        private int _pFilecount;

        /// <summary>
        ///     The PAK File Allocation Table
        /// </summary>
        private PakFat[] _pakFat;

        /// <summary>
        ///     Initializes a new instance of the <see cref="QuakePakFile" /> class.
        /// </summary>
        /// <param name="strFileName">Name of the PAK file.</param>
        /// <param name="bAllowWrite">if set to <c>true</c> [b allow write].</param>
        /// <exception cref="System.IO.FileNotFoundException">
        ///     Can''t open  + strFileName
        ///     or
        ///     File  + strFileName +  has unsupported format
        /// </exception>
        public QuakePakFile(string strFileName, bool bAllowWrite)
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
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Close();
        }

        #endregion IDisposable Members

        /// <summary>
        ///     Closes this instance.
        /// </summary>
        public void Close()
        {
            _pakReader.Close();
            _pakStream.Close();
            _pakStream.Dispose();
        }

        /// <summary>
        ///     Reads the PAK file header.
        /// </summary>
        /// <returns>true or false</returns>
        private bool ReadHeader()
        {
            _pakStream.Position = 0;
            string pHeader = StreamHandling.PCharToString(_pakReader.ReadChars(PakId.Length));
            if (pHeader == StreamHandling.PCharToString(PakId))
            {
                // hned za hlavickou je pozice zacatku fatky
                _pFatstart = _pakReader.ReadInt32();
                // a pak je pocet souboru * 64
                _pFilecount = _pakReader.ReadInt32() / 64;
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
            _pakStream.Position = 0;
            return false;
        }

        /// <summary>
        ///     Checks if a file exists in a PAK
        /// </summary>
        /// <param name="strFileInPak">The file in a PAK.</param>
        /// <returns>true or false</returns>
        public bool PakFileExists(string strFileInPak)
        {
            return GetFileIndex(strFileInPak) != -1;
        }

        /// <summary>
        ///     Gets the index of a file.
        /// </summary>
        /// <param name="strFileInPak">The file in a PAK.</param>
        /// <returns>-1 if not found</returns>
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
        ///     Extracts a file to a stream.
        /// </summary>
        /// <param name="strFileInPak">The file in pak.</param>
        /// <param name="sOutput">The stream to output.</param>
        public void ExtractStream(string strFileInPak, Stream sOutput)
        {
            int fIndex = GetFileIndex(strFileInPak);
            if (fIndex == -1) return; // soubor v paku neni, tudiz konec.
            sOutput.SetLength(0);
            _pakStream.Seek(_pakFat[fIndex].FileStart, SeekOrigin.Begin);
            StreamHandling.StreamCopy(_pakStream, sOutput, _pakFat[fIndex].FileLength);
        }

        /// <summary>
        ///     Extracts the file to disk.
        /// </summary>
        /// <param name="strFileInPak">The file in a pak.</param>
        /// <param name="strOutputFile">The output file.</param>
        public void ExtractFile(string strFileInPak, string strOutputFile)
        {
            Stream fOutput = new FileStream(strOutputFile, FileMode.OpenOrCreate, FileAccess.ReadWrite,
                FileShare.ReadWrite);
            ExtractStream(strFileInPak, fOutput);
            fOutput.Close();
        }

        /// <summary>
        ///     Creates a new PAK file.
        /// </summary>
        /// <param name="strFileName">Name of the file.</param>
        /// <returns>true or false</returns>
        public static bool CreateNewPak(string strFileName)
        {
            // TODO: taky by to mohlo vracet true podle úspěšnosti :-)
            bool result = false;
            var fs = new FileStream(strFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            fs.Position = 0;
            var bw = new BinaryWriter(fs, Encoding.GetEncoding("Windows-1250"));
            bw.Write(PakId);
            int pFatstart = PakId.Length;
            pFatstart += sizeof(Int32); // offset
            pFatstart += sizeof(Int32); // length
            const Int32 pFilecount = 0;
            bw.Write(pFatstart);
            bw.Write(pFilecount);
            bw.Close();
            fs.Close();
            return result;
        }

        /// <summary>
        ///     Prepares the file name to write.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        private static char[] PrepFileNameWrite(string filename)
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

        /// <summary>
        ///     Writes the FAT.
        /// </summary>
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
            _pakStream.Seek(PakId.Length + sizeof(Int32), SeekOrigin.Begin);
            bw.Write(_pFilecount * 64); // krat 64. z neznamych duvodu
            // bw.Close();
        }

        /// <summary>
        ///     Adds the stream to a PAK.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="pakFileName">Name of the file in a pak.</param>
        /// <param name="writeFat">if set to <c>true</c> [write fat].</param>
        /// <returns></returns>
        public bool AddStream(Stream stream, string pakFileName, bool writeFat /*=true*/)
        {
            // soubor uz existuje --> dal se nebavime!
            if (PakFileExists(pakFileName)) return false;
            // novy soubor zapisujeme na pozici fatky
            _pakStream.Seek(_pFatstart, SeekOrigin.Begin);
            // vytvorit novou fatku a zapsat do ni novy soubor
            PakFat[] oldPakFat = _pakFat;
            _pFilecount = oldPakFat.Length + 1;
            _pakFat = new PakFat[_pFilecount];
            oldPakFat.CopyTo(_pakFat, 0);
            _pakFat[_pFilecount - 1].FileName = pakFileName;
            _pakFat[_pFilecount - 1].FileLength = (int)stream.Length;
            _pakFat[_pFilecount - 1].FileStart = _pFatstart;
            // zapsat soubor
            StreamHandling.StreamCopy(stream, _pakStream, 0, _pakStream.Position);
            //StreamHandling.StreamCopyAsync(stream, PakStream);
            // po dokonceni zapisovani zjistit pozici streamu
            _pFatstart = (int)_pakStream.Position;
            //bw.Close();
            // zapsat fatku
            if (writeFat)
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
        ///     Adds the file to a PAK.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="pakFileName">Name of the file. in a pak</param>
        /// <param name="writeFat">if set to <c>true</c> [write fat].</param>
        /// <returns></returns>
        public bool AddFile(string fileName, string pakFileName, bool writeFat = true)
        {
            if (!File.Exists(fileName)) return false;
            bool result;
            try
            {
                Stream fstream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                result = AddStream(fstream, pakFileName, writeFat);
                fstream.Close();
                result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        ///     Adds the file to a PAK.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="pakFileName">Name of the file in a PAK.</param>
        /// <returns></returns>
        public bool AddFile(string fileName, string pakFileName)
        {
            return AddFile(fileName, pakFileName, true);
        }
    }
}