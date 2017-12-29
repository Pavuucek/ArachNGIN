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
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using ArachNGIN.Files.Streams;

namespace ArachNGIN.Files.FileFormats
{
    /// <summary>
    ///     Class for reading Quake PAK files
    /// </summary>
    public class QuakePakFile
    {
        private static readonly char[] PakId = {'P', 'A', 'C', 'K'};

        /// <summary>
        ///     The PAK File Allocation Table
        /// </summary>
        private PakFat[] _pakFat;

        /// <summary>
        ///     The position of start of FAT
        /// </summary>
        private int _pFatstart;

        /// <summary>
        ///     The count of files inside a PAK
        /// </summary>
        private int _pFilecount;

        /// <summary>
        ///     Initializes a new instance of the <see cref="QuakePakFile" /> class.
        /// </summary>
        /// <param name="fileName">Name of the PAK file.</param>
        /// <param name="writeAccess">if set to <c>true</c> allows writing to file.</param>
        /// <exception cref="System.IO.FileNotFoundException">
        ///     Can''t open  + fileName
        ///     or
        ///     File  + fileName +  has unsupported format
        /// </exception>
        public QuakePakFile(string fileName, bool writeAccess)
        {
            WriteAccess = writeAccess;
            FileName = fileName;
            var info = new FileInfo(fileName);
            if (!info.Exists)
                throw new FileNotFoundException("Can''t open " + fileName);
            // soubor existuje
            Stream pakStream = null;
            try
            {
                pakStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                using (var pakReader = new BinaryReader(pakStream, Encoding.ASCII))
                {
                    if (!ReadHeader(pakStream, pakReader))
                        throw new FileNotFoundException("File " + fileName + " has unsupported format");
                }
            }
            finally
            {
                pakStream?.Dispose();
            }
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="QuakePakFile" /> class.
        /// </summary>
        /// <param name="fileName">Name of the PAK file.</param>
        public QuakePakFile(string fileName) : this(fileName, false)
        {
        }

        /// <summary>
        ///     The pak file list
        /// </summary>
        public List<string> PakFileList { get; } = new List<string>();

        /// <summary>
        ///     Gets the name of the file.
        /// </summary>
        /// <value>
        ///     The name of the file.
        /// </value>
        public string FileName { get; }

        /// <summary>
        ///     Gets or sets a value indicating whether class instance can write to a pak file.
        /// </summary>
        /// <value>
        ///     <c>true</c> if [write access]; otherwise, <c>false</c>.
        /// </value>
        public bool WriteAccess { get; set; }

        /// <summary>
        ///     Reads the PAK file header.
        /// </summary>
        /// <param name="pakStream">The pak stream.</param>
        /// <param name="pakReader">The pak reader.</param>
        /// <returns>
        ///     true or false
        /// </returns>
        private bool ReadHeader(Stream pakStream, BinaryReader pakReader)
        {
            pakStream.Position = 0;
            var pHeader = StreamHandling.PCharToString(pakReader.ReadChars(PakId.Length));
            if (pHeader == StreamHandling.PCharToString(PakId))
            {
                // hned za hlavickou je pozice zacatku fatky
                _pFatstart = pakReader.ReadInt32();
                // a pak je pocet souboru * 64
                _pFilecount = pakReader.ReadInt32() / 64;
                //
                // presuneme se na pozici fatky a nacteme ji
                pakStream.Position = _pFatstart;
                _pakFat = new PakFat[_pFilecount];
                // vymazneme filelist
                PakFileList.Clear();
                for (var i = 0; i < _pFilecount; i++)
                {
                    // my radi lowercase. v tom se lip hleda ;-)
                    var sfile =
                        StreamHandling.PCharToString(pakReader.ReadChars(56)).ToLower(CultureInfo.InvariantCulture);
                    sfile = sfile.Replace("/", "\\"); // unixovy lomitka my neradi.
                    // pridame soubor do filelistu a do PakFATky
                    PakFileList.Add(sfile);
                    _pakFat[i].FileName = sfile;
                    _pakFat[i].FileStart = pakReader.ReadInt32();
                    _pakFat[i].FileLength = pakReader.ReadInt32();
                }

                pakStream.Position = 0;
                //
                return true;
            }

            pakStream.Position = 0;
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
            for (var i = 0; i < _pakFat.Length; i++)
                if (string.Equals(_pakFat[i].FileName, strFileInPak, StringComparison.CurrentCultureIgnoreCase))
                    return i;
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
            var fIndex = GetFileIndex(strFileInPak);
            if (fIndex == -1) return; // soubor v paku neni, tudiz konec.
            sOutput.SetLength(0);
            using (var pakStream = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                pakStream.Seek(_pakFat[fIndex].FileStart, SeekOrigin.Begin);
                StreamHandling.StreamCopy(pakStream, sOutput, _pakFat[fIndex].FileLength);
            }
        }

        /// <summary>
        ///     Extracts the file to disk.
        /// </summary>
        /// <param name="strFileInPak">The file in a pak.</param>
        /// <param name="strOutputFile">The output file.</param>
        public void ExtractFile(string strFileInPak, string strOutputFile)
        {
            using (
                var fOutput = new FileStream(strOutputFile, FileMode.OpenOrCreate, FileAccess.ReadWrite,
                    FileShare.ReadWrite))
            {
                ExtractStream(strFileInPak, fOutput);
            }
        }

        /// <summary>
        ///     Creates a new PAK file.
        /// </summary>
        /// <param name="strFileName">Name of the file.</param>
        /// <returns>true or false</returns>
        public static bool CreateNewPak(string strFileName)
        {
            try
            {
                Stream fs = null;
                try
                {
                    fs = new FileStream(strFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                    fs.Position = 0;
                    using (var bw = new BinaryWriter(fs, Encoding.ASCII))
                    {
                        bw.Write(PakId);
                        var pFatstart = PakId.Length;
                        pFatstart += sizeof(int); // offset
                        pFatstart += sizeof(int); // length
                        const int pFilecount = 0;
                        bw.Write(pFatstart);
                        bw.Write(pFilecount);

                        return true;
                    }
                }
                finally
                {
                    fs?.Dispose();
                }
            }
            catch
            {
                return false;
            }
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
            var fName = filename.Replace("\\", "/");
            // kdyz to nekdo prepisk s nazvem souboru tak ho seriznout :-)
            if (fName.Length > 56)
                fName.CopyTo(0, result, 0, 55);
            else
                fName.CopyTo(0, result, 0, fName.Length);
            return result;
        }

        /// <summary>
        ///     Writes the FAT.
        /// </summary>
        private void WriteFat(Stream pakStream)
        {
            // naseekovat startovní pozici fatky
            pakStream.Seek(_pFatstart, SeekOrigin.Begin);
            using (var bw = new BinaryWriter(pakStream, Encoding.ASCII))
            {
                foreach (var item in _pakFat)
                {
                    // nazev souboru
                    bw.Write(PrepFileNameWrite(item.FileName));
                    bw.Write(item.FileStart);
                    bw.Write(item.FileLength);
                }

                // naseekovat na pocet souboru a zapsat
                pakStream.Seek(PakId.Length + sizeof(int), SeekOrigin.Begin);
                bw.Write(_pFilecount * 64); // krat 64. z neznamych duvodu
            }
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
            // mame zakazany zapis
            if (!WriteAccess) return false;
            // novy soubor zapisujeme na pozici fatky
            Stream pakStream = null;
            try
            {
                pakStream = new FileStream(FileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                pakStream.Seek(_pFatstart, SeekOrigin.Begin);
                // vytvorit novou fatku a zapsat do ni novy soubor
                var oldPakFat = _pakFat;
                _pFilecount = oldPakFat.Length + 1;
                _pakFat = new PakFat[_pFilecount];
                oldPakFat.CopyTo(_pakFat, 0);
                _pakFat[_pFilecount - 1].FileName = pakFileName;
                _pakFat[_pFilecount - 1].FileLength = (int) stream.Length;
                _pakFat[_pFilecount - 1].FileStart = _pFatstart;
                // zapsat soubor
                StreamHandling.StreamCopy(stream, pakStream, 0, pakStream.Position);
                // po dokonceni zapisovani zjistit pozici streamu
                _pFatstart = (int) pakStream.Position;
                // zapsat fatku
                if (writeFat)
                {
                    pakStream.Seek(PakId.Length, SeekOrigin.Begin);
                    using (var bw = new BinaryWriter(pakStream))
                    {
                        // zapsat startovni pozici fatky
                        bw.Write(_pFatstart);
                        WriteFat(pakStream);
                    }
                }
            }
            finally
            {
                pakStream?.Dispose();
            }

            return true;
        }

        /// <summary>
        ///     Adds the file to a PAK.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="pakFileName">Name of the file. in a pak</param>
        /// <returns></returns>
        public bool AddFile(string fileName, string pakFileName)
        {
            return AddFile(fileName, pakFileName, true);
        }

        /// <summary>
        ///     Adds the file to a PAK.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="pakFileName">Name of the file. in a pak</param>
        /// <param name="writeFat">if set to <c>true</c> [write fat].</param>
        /// <returns></returns>
        public bool AddFile(string fileName, string pakFileName, bool writeFat)
        {
            if (!File.Exists(fileName)) return false;
            if (!WriteAccess) return false;
            bool result;
            try
            {
                using (var fstream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    result = AddStream(fstream, pakFileName, writeFat);
                }
            }
            catch
            {
                result = false;
            }

            return result;
        }
    }
}