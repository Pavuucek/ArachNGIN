/**************************************************************************************************************/
/*                                                                                                            */
/*  Versioner.cs                                                                                              */
/*                                                                                                            */
/*  Increments an assembly's version by one                                                                   */
/*                                                                                                            */
/*  This is free code, use it as you require. It was a good learning exercise for me and I hope it will be    */
/*  for you too. If you modify it please use your own namespace.                                              */
/*                                                                                                            */
/*  If you like it or have suggestions for improvements please let me know at: PIEBALDconsult@aol.com         */
/*                                                                                                            */
/*  Modification history:                                                                                     */
/*  2006/01/25          Sir John E. Boucher     Created                                                       */
/*                                                                                                            */
/**************************************************************************************************************/

using System;
using System.IO;
using System.Reflection;
using System.Security.Permissions;
using System.Text.RegularExpressions;

[assembly: FileIOPermission(SecurityAction.RequestMinimum)]

namespace PIEBALD.Versioner
{
    internal static class Versioner
    {
        private static void
            Reversion
            (
            FileInfo fi
            )
        {
            FileStream fs = null;

            var reg = new Regex
                (
                "^(?'Part1'\\s*\\[\\s*assembly\\s*:.*AssemblyVersion\\s*\\(\\s*\")" +
                "(?'Major'\\d+)\\.(?'Minor'\\d+)\\.(?'Build'\\d+)\\.(?'Rever'\\d+)" +
                "(?'Part2'\"\\s*\\)\\s*].*)"
                );

            MatchCollection mat;

            string[] lines;
            var vvals = new decimal[4];

            try
            {
                fs = fi.Open
                    (
                        FileMode.Open
                        ,
                        FileAccess.Read
                        ,
                        FileShare.None
                    );

                lines = (new StreamReader(fs)).ReadToEnd().Split
                    (
                        new[] {'\n'}
                        ,
                        StringSplitOptions.RemoveEmptyEntries
                    );
            }
            catch (Exception err)
            {
                throw (new Exception("Could not read file", err));
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fs = null;
                }
            }

            for (int runner = 0; runner < lines.Length; runner++)
            {
                mat = reg.Matches(lines[runner]);

                if (mat.Count == 1)
                {
                    try
                    {
                        checked
                        {
                            vvals[0] = decimal.Parse(mat[0].Groups["Major"].Value);
                            vvals[1] = decimal.Parse(mat[0].Groups["Minor"].Value);
                            vvals[2] = decimal.Parse(mat[0].Groups["Build"].Value);
                            vvals[3] = decimal.Parse(mat[0].Groups["Rever"].Value);
                        }

                        if ((vvals[0] < ushort.MaxValue) &&
                            (vvals[1] < ushort.MaxValue) &&
                            (vvals[2] < ushort.MaxValue) &&
                            (vvals[3] < ushort.MaxValue))
                        {
                            vvals[3] += 1;

                            if (vvals[3] >= ushort.MaxValue)
                            {
                                vvals[3] = 0;
                                vvals[2] += 1;

                                if (vvals[2] >= ushort.MaxValue)
                                {
                                    vvals[2] = 0;
                                    vvals[1] += 1;

                                    if (vvals[1] >= ushort.MaxValue)
                                    {
                                        vvals[1] = 0;
                                        vvals[0] += 1;

                                        if (vvals[0] >= ushort.MaxValue)
                                        {
                                            Console.WriteLine
                                                (
                                                    "AssemblyVersion hit the max: " + lines[runner]
                                                );

                                            continue;
                                        }
                                    }
                                }
                            }

                            lines[runner] = string.Format
                                (
                                    "{0}{1}.{2}.{3}.{4}{5}"
                                    ,
                                    mat[0].Groups["Part1"].Value
                                    ,
                                    vvals[0]
                                    ,
                                    vvals[1]
                                    ,
                                    vvals[2]
                                    ,
                                    vvals[3]
                                    ,
                                    mat[0].Groups["Part2"].Value
                                );
                        }
                        else
                        {
                            Console.WriteLine("Invalid AssemblyVersion entry: " + lines[runner]);
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Invalid AssemblyVersion entry: " + lines[runner]);
                    }
                }
            }

            try
            {
                fs = fi.Open
                    (
                        FileMode.Create
                        ,
                        FileAccess.Write
                        ,
                        FileShare.None
                    );

                using (var sw = new StreamWriter(fs))
                {
                    for (int runner = 0; runner < lines.Length; runner++)
                    {
                        sw.WriteLine(lines[runner].Trim(new[] {'\r'}));
                    }
                }
            }
            catch (Exception err)
            {
                throw (new Exception("Could not write file", err));
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fs = null;
                }
            }
        }

        [STAThread]
        private static void
            Main
            (
            string[] args
            )
        {
            try
            {
                if (args.Length > 0)
                {
                    var fi = new FileInfo
                        (
                        Environment.ExpandEnvironmentVariables(args[0])
                        );

                    if (fi.Exists)
                    {
                        Reversion(fi);
                    }
                    else
                    {
                        throw (new Exception("Did not find file: " + fi.FullName));
                    }
                }
                else
                {
                    Console.WriteLine("Syntax: Versioner assemblyinfo.cs");
                }
            }
            catch (Exception err)
            {
                while (err != null)
                {
                    Console.Write(err.Message);
                    err = err.InnerException;
                }
            }
        }
    }
}