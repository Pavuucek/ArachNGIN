
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

[assembly: System.Security.Permissions.FileIOPermission(System.Security.Permissions.SecurityAction.RequestMinimum)]
[assembly: System.Reflection.AssemblyVersion("1.0.0.0")]

namespace PIEBALD.Versioner
{
    static class Versioner
    {
        private static void
        Reversion
        (
            System.IO.FileInfo fi
        )
        {
            System.IO.FileStream fs = null;

            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex
            (
                "^(?'Part1'\\s*\\[\\s*assembly\\s*:.*AssemblyVersion\\s*\\(\\s*\")" +
                "(?'Major'\\d+)\\.(?'Minor'\\d+)\\.(?'Build'\\d+)\\.(?'Rever'\\d+)" +
                "(?'Part2'\"\\s*\\)\\s*].*)"
            );

            System.Text.RegularExpressions.MatchCollection mat;

            string[] lines;
            decimal[] vvals = new decimal[4];

            try
            {
                fs = fi.Open
                (
                    System.IO.FileMode.Open
                ,
                    System.IO.FileAccess.Read
                ,
                    System.IO.FileShare.None
                );

                lines = (new System.IO.StreamReader(fs)).ReadToEnd().Split
                (
                    new char[] { '\n' }
                ,
                    System.StringSplitOptions.RemoveEmptyEntries
                );
            }
            catch (System.Exception err)
            {
                throw (new System.Exception("Could not read file", err));
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
                                            System.Console.WriteLine
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
                            System.Console.WriteLine("Invalid AssemblyVersion entry: " + lines[runner]);
                        }
                    }
                    catch
                    {
                        System.Console.WriteLine("Invalid AssemblyVersion entry: " + lines[runner]);
                    }
                }
            }

            try
            {
                fs = fi.Open
                (
                    System.IO.FileMode.Create
                ,
                    System.IO.FileAccess.Write
                ,
                    System.IO.FileShare.None
                );

                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(fs))
                {
                    for (int runner = 0; runner < lines.Length; runner++)
                    {
                        sw.WriteLine(lines[runner].Trim(new char[] { '\r' }));
                    }
                }
            }
            catch (System.Exception err)
            {
                throw (new System.Exception("Could not write file", err));
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

        [System.STAThread]
        static void
        Main
        (
            string[] args
        )
        {
            try
            {
                if (args.Length > 0)
                {
                    System.IO.FileInfo fi = new System.IO.FileInfo
                    (
                        System.Environment.ExpandEnvironmentVariables(args[0])
                    );

                    if (fi.Exists)
                    {
                        Reversion(fi);
                    }
                    else
                    {
                        throw (new System.Exception("Did not find file: " + fi.FullName));
                    }
                }
                else
                {
                    System.Console.WriteLine("Syntax: Versioner assemblyinfo.cs");
                }
            }
            catch (System.Exception err)
            {
                while (err != null)
                {
                    System.Console.Write(err.Message);
                    err = err.InnerException;
                }
            }

            return;
        }
    }
}
