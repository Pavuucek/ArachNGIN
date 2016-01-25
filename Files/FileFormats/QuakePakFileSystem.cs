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

using ArachNGIN.ClassExtensions;
using ArachNGIN.Files.Streams;
using System;
using System.Collections.Specialized;
using System.IO;

namespace ArachNGIN.Files.FileFormats
{
    /// <summary>
    ///     Class representing a Quake PAK file system
    /// </summary>
    public class QuakePakFileSystem : IDisposable
    {
        private const string PakExtension = "pak"; // bez tecky
        private const string PakIndexFileName = "(pak-index)";
        private readonly string[] _aPathfiles;
        private readonly Int64 _iPakCount;
        private readonly string[] _lPakFiles;
        private readonly string _sDir;
        private readonly string _sTemp;
        private StringCollection[] _indexFat;
        private StringCollection[] _pakFat;

        /// <summary>
        ///     Initializes a new instance of the <see cref="QuakePakFileSystem" /> class.
        /// </summary>
        /// <param name="appDir">The application dir.</param>
        /// <param name="tempDir">The temporary dir.</param>
        public QuakePakFileSystem(string appDir, string tempDir)
        {
            _sDir = appDir.AddSlash();
            _sTemp = tempDir.AddSlash();
            var di = new DirectoryInfo(_sDir);
            FileInfo[] fi = di.GetFiles("*." + PakExtension);
            _iPakCount = fi.LongLength;
            _lPakFiles = new string[fi.LongLength];
            for (int i = 0; i < _lPakFiles.LongLength; i++)
            {
                _lPakFiles[i] = fi[i].Name;
            }
            FileInfo[] fi2 = di.GetFiles("*.*", SearchOption.AllDirectories);
            _aPathfiles = new string[fi2.LongLength];
            for (int i = 0; i < fi2.LongLength; i++)
            {
                _aPathfiles[i] = fi2[i].FullName.Replace(_sDir, "");
            }
            ReadPakFiles();
        }

        #region IDisposable Members

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            // nothing :-)
        }

        #endregion IDisposable Members

        /// <summary>
        ///     Reads the pak files.
        /// </summary>
        private void ReadPakFiles()
        {
            if (_iPakCount == 0) return;
            _pakFat = new StringCollection[_iPakCount];
            _indexFat = new StringCollection[_iPakCount];
            for (int i = 0; i < _iPakCount; i++)
            {
                var q = new QuakePakFile(_sDir + _lPakFiles[i], false);
                _pakFat[i] = q.PakFileList;
                _indexFat[i] = new StringCollection();
                if (q.PakFileExists(PakIndexFileName))
                {
                    Stream st = new MemoryStream();
                    q.ExtractStream(PakIndexFileName, st);
                    st.Position = 0;
                    var tr = new StreamReader(st);
                    string line;
                    while ((line = tr.ReadLine()) != null)
                    {
                        _indexFat[i].Add(line);
                    }
                    st.Close();
                }
            }
        }

        /// <summary>
        ///     Replaces slashes in a file name to unix format
        /// </summary>
        /// <param name="sInput">The input.</param>
        /// <returns></returns>
        private static string ReplaceSlashesIn(string sInput)
        {
            return sInput.Replace("\\", "/");
        }

        /// <summary>
        ///     Replaces slashes in a filename back to windows format.
        /// </summary>
        /// <param name="sInput">The input.</param>
        /// <returns></returns>
        private static string ReplaceSlashesOut(string sInput)
        {
            return sInput.Replace("/", "\\");
        }

        /// <summary>
        ///     Asks for a file and places it in a temp directory
        ///     - if it already is in temp dir it does nothing
        ///     - if it exists in program dir it gets copied to temp dir
        ///     - if it exists in one of PAK files it gets extracted to temp dir
        ///     In this order.
        /// </summary>
        /// <param name="sFile">The file name.</param>
        /// <returns>true or false</returns>
        public bool AskFile(string sFile)
        {
            bool r = false;
            string sIndexfile = string.Empty;
            sFile = ReplaceSlashesOut(sFile); // jen pro jistotu
            if (File.Exists(_sTemp + sFile))
            {
                // fajl uz je v tempu, tak ho tam nechame
                // obsah nas nezaujma
                r = true;
                return r;
            }
            // soubor v adresari ma prioritu
            if (File.Exists(_sDir + sFile))
            {
                string sFullpath = _sTemp + sFile;
                Directory.CreateDirectory(Path.GetDirectoryName(sFullpath));
                File.Copy(_sDir + sFile, sFullpath, true);
                r = true;
                return r;
            }

            // ted uz pracujeme s pakem, takze lomitka do unixovyho tvaru :-)
            sFile = ReplaceSlashesIn(sFile);
            // soubor musime najit v paku
            if ((_pakFat != null) && (_indexFat != null))
            {
                for (int i = 0; i < _indexFat.Length; i++)
                {
                    string sIndexline = "";
                    // prohledame fat index
                    foreach (string sLine in _indexFat[i])
                    {
                        if (sLine.ToLower().Contains(sFile.ToLower() + "="))
                        {
                            sIndexline = sLine;
                        }
                    }
                    //s_file=s_indexline.Substring(s_indexline.IndexOf("=")+1);
                    // rozdelit radku indexu na fajl jmeno souboru v paku a skutecne jmeno
                    string[] aIndexline = sIndexline.Split(new[] { '=' });
                    if (aIndexline.Length > 1)
                    {
                        sIndexfile = aIndexline[0];
                        sFile = aIndexline[1];
                    }
                }
                for (int i = 0; i < _pakFat.LongLength; i++)
                {
                    // nejdriv se podivame do indexu

                    if (_pakFat[i].Contains(sFile))
                    {
                        string sFullpath = _sTemp;
                        if (sIndexfile != string.Empty) sFullpath += sIndexfile;
                        else sFullpath += sFile;
                        // prevest lomitka :-)
                        sFullpath = ReplaceSlashesOut(sFullpath);
                        Directory.CreateDirectory(Path.GetDirectoryName(sFullpath));
                        var q = new QuakePakFile(_sDir + _lPakFiles[i], false);
                        q.ExtractFile(sFile, sFullpath);
                        if (File.Exists(sFullpath))
                        {
                            return true;
                        }
                    }
                }
            }
            return r;
        }
    }
}