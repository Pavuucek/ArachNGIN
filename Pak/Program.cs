using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.IO;
using ArachNGIN.Files;
using ArachNGIN.Files.QuakePak;
using ArachNGIN.Files.Streams;

namespace Pak
{
    class Program
    {
        private static string ExeName = Path.GetFileName(Assembly.GetExecutingAssembly().CodeBase);
        static void DisplayHeader()
        {
            Console.WriteLine("ArachNGIN PAK file Creator/Extractor\n(c) 2013 Michal Kuncl (michal.kuncl@gmail.com)");
            Console.WriteLine();
            Console.WriteLine();
        }

        static void DisplayHelp()
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
            Console.WriteLine("{0} e [název_archivu] [název_souboru]", ExeName);
            Console.WriteLine();
            Console.WriteLine("Rozbalení celého archivu:");
            Console.WriteLine("{0} x [název_archivu] [výstupní_cesta]", ExeName);
        }
        
        static void NotEnoughArgs()
        {
            Console.WriteLine("Chyba: nedostatečný počet parametrů!");
            Console.WriteLine();
            DisplayHelp();
        }

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
                case "a":
                    if (args.Length < 3)
                    {
                        NotEnoughArgs();
                        pak.Close();
                        return;
                    }
                    Console.WriteLine("Přidávám {0} do {1}", args[1], args[2]);
                    pak.AddFile(args[2], Path.GetFileName(args[2]));
                    break;
                case "d":
                    if (args.Length < 3)
                    {
                        NotEnoughArgs();
                        pak.Close();
                        return;
                    }
                    var path = StringUtils.StrAddSlash(args[2]);
                    var filenames = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
                    for (int i = 0; i < filenames.Length; i++)
                    {
                        var lastfile = (i == filenames.Length - 1); // posledni po sobe zavre fatku.
                        var plainfilename = filenames[i].Replace(path, "");
                        Console.WriteLine("Přidávám soubor {0}", plainfilename);
                        pak.AddFile(filenames[i], plainfilename, lastfile);
                    }
                    break;
            }
            Console.WriteLine();
            Console.WriteLine("Hotovo!");
            pak.Close();
        }
    }
}
