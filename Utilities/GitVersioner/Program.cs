﻿using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.Win32;

namespace GitVersioner
{
    internal static class Program
    {
        private static readonly string GitExeName = Environment.OSVersion.Platform == PlatformID.Unix
            ? "git"
            : "git.exe";

        private static bool Is64Bit
        {
            get
            {
                return IntPtr.Size == 8 ||
                       !String.IsNullOrEmpty(Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432"));
            }
        }

        private static string ProgramFilesX86()
        {
            if (Is64Bit)
            {
                return Environment.GetEnvironmentVariable("ProgramFiles(x86)");
            }
            return Environment.GetEnvironmentVariable("ProgramFiles");
        }

        private static string FindGitBinary()
        {
            string git = null;
            RegistryKey key;

            // Try the PATH environment variable

            string pathEnv = Environment.GetEnvironmentVariable("PATH");
            if (pathEnv != null)
                foreach (string dir in pathEnv.Split(Path.PathSeparator))
                {
                    string sdir = dir;
                    if (sdir.StartsWith("\"") && sdir.EndsWith("\""))
                    {
                        // Strip quotes (no Path.PathSeparator supported in quoted directories though)
                        sdir = sdir.Substring(1, sdir.Length - 2);
                    }
                    git = Path.Combine(sdir, GitExeName);
                    if (File.Exists(git)) break;
                }
            if (!File.Exists(git)) git = null;


            // Read registry uninstaller key
            if (git == null)
            {
                key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Git_is1");
                if (key != null)
                {
                    object loc = key.GetValue("InstallLocation");
                    if (loc is string)
                    {
                        git = Path.Combine((string) loc, Path.Combine("bin", GitExeName));
                        if (!File.Exists(git)) git = null;
                    }
                }
            }

            // Try 64-bit registry key
            if (git == null && Is64Bit)
            {
                key =
                    Registry.LocalMachine.OpenSubKey(
                        @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\Git_is1");
                if (key != null)
                {
                    object loc = key.GetValue("InstallLocation");
                    if (loc is string)
                    {
                        git = Path.Combine((string) loc, Path.Combine("bin", GitExeName));
                        if (!File.Exists(git)) git = null;
                    }
                }
            }

            // Search program files directory
            if (git == null)
            {
                foreach (
                    string dir in
                        Directory.GetDirectories(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                            "git*"))
                {
                    git = Path.Combine(dir, Path.Combine("bin", GitExeName));
                    if (!File.Exists(git)) git = null;
                }
            }

            // Try 32-bit program files directory
            if (git != null || !Is64Bit) return git;
            foreach (string dir in Directory.GetDirectories(ProgramFilesX86(), "git*"))
            {
                git = Path.Combine(dir, Path.Combine("bin", GitExeName));
                if (!File.Exists(git)) git = null;
            }

            return git;
        }


        private static void WriteInfo(string sFile, bool backup = true)
        {
            if (!File.Exists(sFile))
            {
                Console.WriteLine("Unable to find file {0}", sFile);
                return;
            }
            string bkp = sFile + ".gwbackup";
            //if (File.Exists(bkp)) return false;
            try
            {
                // zapis
                if (backup) File.Copy(sFile, bkp, true);
                GitResult git = GetVersionInfo(Path.GetDirectoryName(Path.GetFullPath(sFile)));
                using (var infile = new StreamReader(bkp, Encoding.Default, true))
                {
                    Console.WriteLine("Reading {0}...", sFile);
                    bool append = true;
                    using (var outfile = new StreamWriter(sFile, false, infile.CurrentEncoding))
                    {
                        Console.WriteLine("Writing {0}", sFile);
                        while (!infile.EndOfStream)
                        {
                            string l = infile.ReadLine();
                            if (l != null && l.Contains("AssemblyInformationalVersion")) append = false;
                            outfile.WriteLine(DoReplace(l, git));
                        }
                        // kdyz neni pritomno AssemblyInformationalVersion tak vlozit defaultni
                        if (append)
                        {
                            Console.WriteLine("Appending AssemblyInformationalVersion...");
                            outfile.WriteLine(
                                DoReplace(
                                    "[assembly: AssemblyInformationalVersion(\"$Branch$:$MajorVersion$.$MinorVersion$.$Revision$-$Commit$-$ShortHash$\")]",
                                    git));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: '{0}' in '{1}'", e.Message, e.Source);
            }
        }

        private static GitResult GetVersionInfo(string workDir)
        {
            Console.WriteLine("Getting version info for {0}", workDir);
            string lines = ExecGit(workDir, "describe --long --tags --always");
            GitResult r;
            r.MajorVersion = 0;
            r.MinorVersion = 0;
            r.Revision = 0;
            r.Commit = 0;
            r.ShortHash = "";
            // ocekavany retezec ve formatu: 1.7.6-235-g0a52e4b
            //lines = "g0a52e4b";
            string[] part1 = lines.Split('-');
            if (part1.Length >= 3)
            {
                // druhou cast rozdelit po teckach
                string[] part2 = part1[0].Split('.');
                if (part2.Length > 1)
                {
                    // delsi nez 1: mame major a minor verzi
                    if (part2.Length > 2)
                    {
                        // mame i revizi
                        try
                        {
                            r.Revision = Convert.ToInt32(part2[2]);
                        }
                        catch
                        {
                            r.Revision = 0;
                        }
                    }
                    try
                    {
                        r.MinorVersion = Convert.ToInt32(part2[1]);
                    }
                    catch
                    {
                        r.MinorVersion = 0;
                    }
                }
                // mame jen major verzi
                try
                {
                    string s = part2[0].ToLower();
                    // kdyby nahodou nekdo chtel pojmenovavat git tagy v1.0.0 atd (tj zacinajci ne cislem ale v)
                    if (s[0] == 'v')
                    {
                        s = s.Remove(0, 1);
                    }
                    r.MajorVersion = Convert.ToInt32(s);
                }
                catch
                {
                    r.MajorVersion = 0;
                }
            }
            try
            {
                r.Commit = Convert.ToInt32(part1[1]);
            }
            catch
            {
                r.Commit = 0;
            }
            try
            {
                r.ShortHash = part1[2];
            }
            catch
            {
                r.ShortHash = lines;
            }
            r.ShortHash = r.ShortHash.Trim();
            // 
            r.Branch = ExecGit(workDir, "rev-parse --abbrev-ref HEAD").Trim();
            r.LongHash = ExecGit(workDir, "rev-parse HEAD").Trim();
            Console.WriteLine("Version info: {0}", GitResultToString(r));
            if (string.IsNullOrEmpty(lines))
            {
                Console.WriteLine("Possible error, git output follows:\n {0}", lines);
            }
            return r;
        }

        private static string GitResultToString(GitResult gr)
        {
            string s = gr.MajorVersion + ".";
            s += gr.MinorVersion + ".";
            s += gr.Revision + "-";
            s += gr.Commit + "-";
            s += gr.ShortHash;
            return s;
        }

        private static string ExecGit(string workDir, string parameters)
        {
            var psi = new ProcessStartInfo(FindGitBinary(), parameters)
            {
                WorkingDirectory = workDir,
                RedirectStandardOutput = true,
                UseShellExecute = false
            };
            Process p = Process.Start(psi);
            string r = string.Empty;
            while (p != null && !p.StandardOutput.EndOfStream)
            {
                r += p.StandardOutput.ReadLine() + "\n";
            }
            if (p != null && !p.WaitForExit(1000))
            {
                p.Kill();
            }
            return r;
        }

        private static string DoReplace(string inString, GitResult gr)
        {
            string r = inString.Replace("$MajorVersion$", gr.MajorVersion.ToString(CultureInfo.InvariantCulture));
            r = r.Replace("$MinorVersion$", gr.MinorVersion.ToString(CultureInfo.InvariantCulture));
            r = r.Replace("$Revision$", gr.Revision.ToString(CultureInfo.InvariantCulture));
            r = r.Replace("$Commit$", gr.Commit.ToString(CultureInfo.InvariantCulture));
            r = r.Replace("$ShortHash$", gr.ShortHash);
            r = r.Replace("$LongHash$", gr.LongHash);
            r = r.Replace("$Branch$", gr.Branch);
            return r;
        }

        private static void RestoreBackup(string sFile)
        {
            Console.WriteLine("Restoring {0}...", sFile);
            string bkp = sFile + ".gwbackup";
            if (!File.Exists(bkp)) return;
            try
            {
                File.Copy(bkp, sFile, true);
                File.Delete(bkp);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to restore backup {0}", bkp);
                Console.WriteLine("Error: '{0}' in '{1}'", e.Message, e.Source);
            }
        }

        private static void Main(string[] args)
        {
            Console.WriteLine("GitVersioner");
            if (string.IsNullOrEmpty(FindGitBinary()))
            {
                NoGit();
                return;
            }
            if (args.Length < 2)
            {
                ShowHelp();
                return;
            }
            switch (args[0].ToLower())
            {
                case "w":
                    WriteInfo(args[1]);
                    break;
                case "r":
                    RestoreBackup(args[1]);
                    break;
                default:
                    ShowHelp();
                    return;
            }
            Console.WriteLine("Finished!");
        }

        private static void NoGit()
        {
            Console.WriteLine("Unable to find Git binary!");
        }

        private static void ShowHelp()
        {
            string exename = Path.GetFileName(Assembly.GetExecutingAssembly().ManifestModule.ToString());
            Console.WriteLine();
            Console.WriteLine("Usage: {0} [parameter] [file]", exename);
            Console.WriteLine("Supported parameters:");
            Console.WriteLine("W: write version information to file and do a backup");
            Console.WriteLine("R: restore file from backup");
            Console.WriteLine();
            Console.WriteLine("for example {0} w Properties\\AssemblyInfo.cs", exename);
            Console.WriteLine("or {0} r Properties\\AssemblyInfo.cs", exename);
            Console.WriteLine();
            Console.WriteLine("Supported replacement strings:");
            Console.WriteLine("$MajorVersion$");
            Console.WriteLine("$MinorVersion$");
            Console.WriteLine("$Revision$");
            Console.WriteLine("$Commit$");
            Console.WriteLine("$ShortHash$");
            Console.WriteLine("$LongHash$");
            Console.WriteLine("$Branch$");
        }

        private struct GitResult
        {
            public string Branch;
            public int Commit;
            public string LongHash;
            public int MajorVersion;
            public int MinorVersion;
            public int Revision;
            public string ShortHash;
        }
    }
}