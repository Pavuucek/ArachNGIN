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
using ArachNGIN.Files.Streams;

namespace ArachNGIN.Files.QuakePak
{
    /// <summary>
    /// Souborový systém Quake pakù
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
        /// Konstruktor <see cref="QuakePakFileSystem"/> class.
        /// </summary>
        /// <param name="appDir">Startovní adresáø ze kterého se budou naèítat pak soubory, nejlépe ten s aplikací</param>
        /// <param name="tempDir">Adresáø aplikace v tempu</param>
        public QuakePakFileSystem(string appDir, string tempDir)
        {
            _sDir = StringUtils.StrAddSlash(appDir);
            _sTemp = StringUtils.StrAddSlash(tempDir);
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
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            // nothing :-)
        }

        #endregion

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
        /// prevede lomitka na unixovy tvar
        /// </summary>
        /// <param name="sInput">cesta</param>
        /// <returns></returns>
        private static string ReplaceSlashesIn(string sInput)
        {
            return sInput.Replace("\\", "/");
        }

        /// <summary>
        /// prevede lomitka na widelni tvar
        /// </summary>
        /// <param name="sInput"></param>
        /// <returns></returns>
        private static string ReplaceSlashesOut(string sInput)
        {
            return sInput.Replace("/", "\\");
        }

        /// <summary>
        /// Vyžádá soubor a umístí ho do tempu.
        /// Soubor mùže být buï:
        /// - už v tempu -> neudìlá se nic
        /// - v adresáøi programu -> zkopíruje se do tempu
        /// - v jednom z pak souborù -> rozbalí se do tempu.
        /// V tomto poøadí.
        /// </summary>
        /// <param name="sFile">název souboru</param>
        /// <returns>jestli se zadaøí tak true</returns>
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
                    string[] aIndexline = sIndexline.Split(new[] {'='});
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