/*
 * Copyright (c) 2006-2014 Michal Kuncl <michal.kuncl@gmail.com> http://www.pavucina.info
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
using ArachNGIN.Files.FileFormats;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace Pak
{
    /// <summary>
    ///     Main class of Pak
    /// </summary>
    internal static class Program
    {
        /// <summary>
        ///     The pak index
        /// </summary>
        private static readonly StringCollection PakIndex = new StringCollection();

        /// <summary>
        ///     The executable name
        /// </summary>
        private static readonly string ExeName = Path.GetFileName(Assembly.GetExecutingAssembly().CodeBase);

        /// <summary>
        ///     Displays the header.
        /// </summary>
        private static void DisplayHeader()
        {
            Console.WriteLine("ArachNGIN PAK file Creator/Extractor\n(c) 2013 Michal Kuncl (michal.kuncl@gmail.com)");
            Console.WriteLine();
            Console.WriteLine();
        }

        /// <summary>
        ///     Displays the help.
        /// </summary>
        private static void DisplayHelp()
        {
            Console.WriteLine("Použití:");
            Console.WriteLine("--------");
            Console.WriteLine();
            Console.WriteLine("Přidání jednoho souboru do archivu:");
            Console.WriteLine("{0} a [název_archivu] [název_souboru]", ExeName);
            Console.WriteLine();
            Console.WriteLine("Přidání celého adresáře (včetně podadresářů) do archivu:");
            Console.WriteLine("{0} d [název_archivu] [název_souboru]", ExeName);
            Console.WriteLine();
            Console.WriteLine("Rozbalení jednoho souboru z archivu:");
            Console.WriteLine("{0} e [název_archivu] [název_souboru] [výstupní_cesta]", ExeName);
            Console.WriteLine();
            Console.WriteLine("Rozbalení celého archivu:");
            Console.WriteLine("{0} x [název_archivu] [výstupní_cesta]", ExeName);
        }

        /// <summary>
        ///     Prints a message when more command line arguments are expected.
        /// </summary>
        private static void NotEnoughArgs()
        {
            Console.WriteLine("Chyba: nedostatečný počet parametrů!");
            Console.WriteLine();
            DisplayHelp();
        }

        /// <summary>
        ///     Main function of Pak program.
        /// </summary>
        /// <param name="args">The arguments.</param>
        private static void Main(string[] args)
        {
            DisplayHeader();
            if (args == null || args.Length < 2)
            {
                DisplayHelp();
                Console.ReadKey();
                return;
            }

            if (!File.Exists(args[1])) QuakePakFile.CreateNewPak(args[1]);
            var pak = new QuakePakFile(args[1], true);

            switch (args[0])
            {
                default:
                    return;
                case "a":
                    if (!AddSingleFile(args, pak)) return;
                    break;

                case "d":
                    if (!AddDirectory(args, pak)) return;
                    break;

                case "e":
                    if (!ExtractSingleFile(args, pak)) return;
                    break;

                case "x":
                    if (!ExtractAllFiles(args, pak)) return;
                    break;
                // pridat a indexovat
                case "ia":
                    if (args.Length < 3)
                    {
                        NotEnoughArgs();
                        return;
                    }
                    LoadIndex(pak);

                    if (ExistsInIndex(args[2]))
                    {
                        var g = Guid.NewGuid();
                        ReplaceInIndex(args[2], g.ToString());
                        pak.AddFile(args[2], g.ToString());
                    }
                    else
                    {
                        var s = AddIndexedFile(args[2]);
                        // TODO: tohle neudělá nic. AddFile neumí nahrazovat :-)
                        pak.AddFile(args[2], s);
                    }
                    //
                    FinishIndex();
                    using (Stream ms = new MemoryStream())
                    {
                        PakIndex.SaveToStream(ms);
                        ms.Seek(0, SeekOrigin.Begin);
                        pak.AddStream(ms, "(pak-index)", true);
                    }
                    break;
            }
            Console.WriteLine();
            Console.WriteLine("Hotovo!");
        }

        /// <summary>
        ///     Extracts all files.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        /// <param name="pak">The pak file.</param>
        /// <returns></returns>
        private static bool ExtractAllFiles(string[] args, QuakePakFile pak)
        {
            if (args.Length < 3)
            {
                NotEnoughArgs();
                return false;
            }

            if (!Directory.Exists(args[2]))
                Console.WriteLine("Chyba: Výstupní adresář neexistuje!");

            var path = args[2].AddSlash(); // +Path.GetDirectoryName(args[1])
            foreach (var file in pak.PakFileList)
            {
                Console.WriteLine("Rozbaluji {0}", file);
                var localpath = path + file;
                Directory.CreateDirectory(Path.GetDirectoryName(localpath));
                pak.ExtractFile(file, localpath);
            }
            return true;
        }

        /// <summary>
        ///     Extracts a single file.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        /// <param name="pak">The pak file.</param>
        /// <returns></returns>
        private static bool ExtractSingleFile(IList<string> args, QuakePakFile pak)
        {
            if (args.Count < 4)
            {
                NotEnoughArgs();
                return false;
            }
            if (!pak.PakFileExists(args[2]))
            {
                Console.WriteLine("Chyba: Soubor {0} v archivu {1} neexistuje!", args[2], args[3]);
                return false;
            }
            if (!Directory.Exists(args[3]))
                Console.WriteLine("Chyba: Výstupní adresář neexistuje!");
            Console.WriteLine("Rozbaluji {0}", args[2]);
            var path = args[3].AddSlash() + Path.GetDirectoryName(args[2]);
            Directory.CreateDirectory(path);
            path = path.AddSlash() + Path.GetFileName(args[2]);
            pak.ExtractFile(args[2], path);

            return true;
        }

        /// <summary>
        ///     Adds a directory to pak file.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        /// <param name="pak">The pak file.</param>
        /// <returns></returns>
        private static bool AddDirectory(IList<string> args, QuakePakFile pak)
        {
            if (args.Count < 3)
            {
                NotEnoughArgs();
                return false;
            }
            if (!Directory.Exists(args[2]))
            {
                Console.WriteLine("Chyba: Adresář {0} neexistuje!", args[2]);
                return false;
            }
            var path = args[2].AddSlash();
            var filenames = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
            if (filenames.Length == 0)
            {
                Console.WriteLine("Chyba: Adresář {0} je prázdný!", args[2]);
                return false;
            }
            for (var i = 0; i < filenames.Length; i++)
            {
                var lastfile = i == filenames.Length - 1; // posledni po sobe zavre fatku.
                var plainfilename = filenames[i].Replace(path, "");
                Console.WriteLine("Přidávám soubor {0}", plainfilename);
                pak.AddFile(filenames[i], plainfilename, lastfile);
            }
            return true;
        }

        /// <summary>
        ///     Adds a single file.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        /// <param name="pak">The pak file.</param>
        /// <returns></returns>
        private static bool AddSingleFile(IList<string> args, QuakePakFile pak)
        {
            if (args.Count < 3)
            {
                NotEnoughArgs();
                return false;
            }
            Console.WriteLine("Přidávám {0} do {1}", args[1], args[2]);
            if (File.Exists(args[2]))
            {
                pak.AddFile(args[2], Path.GetFileName(args[2]));
            }
            else
            {
                Console.WriteLine("Chyba: Soubor {0} neexistuje!", args[2]);
                return false;
            }
            return true;
        }

        /// <summary>
        ///     Adds an indexed file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>The file name under which file is stored in pak.</returns>
        private static string AddIndexedFile(string file)
        {
            var g = Guid.NewGuid();
            PakIndex.Add(file + "=" + g.ToString().ToLowerInvariant());
            return g.ToString().ToLowerInvariant();
        }

        /// <summary>
        ///     Finishes the index.
        /// </summary>
        private static void FinishIndex()
        {
            var tempIndex = new string[PakIndex.Count];
            PakIndex.CopyTo(tempIndex, 0);
            PakIndex.Clear();
            PakIndex.Add("; ArachNGIN PAK File Index");
            PakIndex.Add("; Autogenerated on " + DateTime.Now.ToString(CultureInfo.InvariantCulture));
            PakIndex.Add("; Generator: " + ExeName + " " + Assembly.GetExecutingAssembly().GetName().Version);
            PakIndex.Add("");
            PakIndex.Add("[Index]");
            PakIndex.Add("FileCount=" + tempIndex.Length.ToString(CultureInfo.InvariantCulture));
            PakIndex.Add("");
            PakIndex.AddRange(tempIndex);
            // TODO: dopsat nahrazovani souboru do quakepakfile.
        }

        /// <summary>
        ///     Cleans the index.
        /// </summary>
        private static void CleanIndex()
        {
            var tempIndex = new string[PakIndex.Count];
            PakIndex.CopyTo(tempIndex, 0);
            PakIndex.Clear();
            foreach (var line in tempIndex)
            {
                var split = line.Split('=');
                // vic jak 1 -> radka je v ini formatu, tj ok a neobsahuje 'filecount' a neni prazdny
                if (split.Length > 1 && !line.ToLowerInvariant().Contains("filecount") && !string.IsNullOrEmpty(line))
                    PakIndex.Add(line.ToLowerInvariant());
            }
        }

        /// <summary>
        ///     Replaces a file in index.
        /// </summary>
        /// <param name="oldFile">The old file.</param>
        /// <param name="newFile">The new file.</param>
        private static void ReplaceInIndex(string oldFile, string newFile)
        {
            var tempIndex = new string[PakIndex.Count];
            PakIndex.CopyTo(tempIndex, 0);
            PakIndex.Clear();
            foreach (var line in tempIndex)
                if (line.Contains(oldFile.ToLowerInvariant()))
                {
                    var linesplit = line.Split('=');
                    if (linesplit.Length != 2) PakIndex.Add(line);
                    else PakIndex.Add(linesplit[0] + "=" + newFile.ToLowerInvariant());
                }
                else
                {
                    PakIndex.Add(line);
                }
        }

        /// <summary>
        ///     Loads the index.
        /// </summary>
        /// <param name="pak">The pak file.</param>
        private static void LoadIndex(QuakePakFile pak)
        {
            if (!pak.PakFileExists("(pak-index)")) return;
            var ms = new MemoryStream();
            pak.ExtractStream("(pak-index)", ms);
            ms.Seek(0, SeekOrigin.Begin);
            PakIndex.LoadFromStream(ms);
            ms.Close();
            //
            CleanIndex();
        }

        /// <summary>
        ///     Checks if file exists in index.
        /// </summary>
        /// <param name="filename">The file name.</param>
        /// <returns><c>true</c> if found</returns>
        private static bool ExistsInIndex(string filename)
        {
            var sd = new StringDictionary();
            foreach (var s in PakIndex)
            {
                var split = s.Split('=');
                if (split.Length > 1) sd.Add(split[0], split[1]);
            }
            return sd.ContainsKey(filename.ToLowerInvariant());
        }
    }
}