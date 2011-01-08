using System;
using System.IO;
using ArachNGIN.Files.Strings;
using System.Collections.Specialized;

namespace ArachNGIN.Files
{
	/// <summary>
	/// Summary description for quake_pak_filesystem.
	/// </summary>
	public class QuakePAKFilesystem
	{
		/// <summary>
		/// 
		/// </summary>
		private QuakePAK q_onepak;
        private string[] l_pakfiles;
        private StringCollection[] a_fat;
        private string[] a_pathfiles;
        private Int64 i_pakcount = 0;
        private const string PakExtension = "pak"; // bez tecky
        private string s_dir;
        private string s_temp;
		

		public QuakePAKFilesystem(string Dir, string TempDir)
		{
            s_dir = StringUtils.strAddSlash(Dir);
            s_temp = StringUtils.strAddSlash(TempDir);
            DirectoryInfo di = new DirectoryInfo(s_dir);
            FileInfo[] fi = di.GetFiles("*." + PakExtension);
            i_pakcount = fi.LongLength;
            l_pakfiles = new string[fi.LongLength];
            for (int i = 0; i < l_pakfiles.LongLength; i++)
            {
                l_pakfiles[i] = fi[i].Name;
            }
            FileInfo[] fi2 = di.GetFiles("*.*",SearchOption.AllDirectories);
            a_pathfiles = new string[fi2.LongLength];
            for (int i = 0; i < fi2.LongLength; i++)
            {
                a_pathfiles[i] = fi2[i].FullName.Replace(s_dir, "");
            }
            ReadPAKFiles();
        }

        private void ReadPAKFiles()
        {
            if (i_pakcount == 0) return;
            a_fat = new StringCollection[i_pakcount];
            for (int i = 0; i < i_pakcount; i++)
            {
                QuakePAK q = new QuakePAK(s_dir + l_pakfiles[i]);
                a_fat[i] = q.PakFileList;
            }
        }

        public bool AskFile(string s_file)
        {
            bool r = false;
            if(File.Exists(s_temp+s_file))
            {
                // fajl uz je v tempu, tak ho tam nechame
                // obsah nas nezaujma
                r = true;
                return r;
            }
            // soubor v adresari ma prioritu
            if (File.Exists(s_dir + s_file))
            {
                string s_fullpath = s_temp + s_file;
                Directory.CreateDirectory(Path.GetDirectoryName(s_fullpath));
                File.Copy(s_dir + s_file, s_fullpath, true);
                r = true;
                return r;
            }
            // soubor musime najit v paku
            for (int i = 0; i < a_fat.LongLength; i++)
            {
                if (a_fat[i].Contains(s_file))
                {
                    string s_fullpath = s_temp + s_file;
                    Directory.CreateDirectory(Path.GetDirectoryName(s_fullpath));
                    QuakePAK q = new QuakePAK(s_dir + l_pakfiles[i]);
                    q.ExtractFile(s_file, s_fullpath);
                    if (File.Exists(s_fullpath))
                    {
                        r = true;
                        return r;
                    }
                }
            }
            return r;
        }


	}
}
