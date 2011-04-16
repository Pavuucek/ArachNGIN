using System;
using System.IO;
using ArachNGIN.Files.Streams;
using System.Collections.Specialized;

namespace ArachNGIN.Files
{
	/// <summary>
	/// Tøída na ètení z PAK souborù Quaka
	/// </summary>
	public class QuakePAK
	{
		private struct T_PakFAT
		{
			public string FileName;
			public int FileStart;
			public int FileLength;
		}

		private T_PakFAT[] PakFAT;
		private FileStream PakStream;
		private BinaryReader PakReader;
		
		/// <summary>
		/// Seznam souborù v PAKu
		/// </summary>
		public StringCollection PakFileList = new StringCollection();

		/// <summary>
		/// Konstruktor - otevøe pak soubor a naète z nìj hlavièku.
		/// </summary>
		/// <param name="strFileName">jméno pak souboru</param>
		public QuakePAK(string strFileName)
		{
			FileInfo info = new FileInfo(strFileName);
			if (info.Exists == false)
			{
				throw new FileNotFoundException("Can''t open "+strFileName);
			}
			// soubor existuje
			PakStream = new FileStream(strFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
			PakReader = new BinaryReader(PakStream,System.Text.Encoding.GetEncoding("Windows-1250"));
			//
			if (!ReadHeader())
			{
				PakStream.Close();
				throw new FileNotFoundException("File "+strFileName+" has unsupported format");
			}

		}

		/// <summary>
		/// Neoficiální destruktor
		/// </summary>
		public void Close()
		{
			PakReader.Close();
			PakStream.Close();
		}

		/// <summary>
		/// Oficiální Destruktor
		/// </summary>
		~QuakePAK()
		{
			try
			{ 
				Close();
			}
			catch
			{
				//
			}
		}

		private bool ReadHeader()
		{
			string p_header;
			PakStream.Position = 0;
			p_header = StreamHandling.PCharToString(PakReader.ReadChars(4));
			if ((string)p_header == "PACK")
			{
				// hned za hlavickou je pozice zacatku fatky
				int p_fatstart = PakReader.ReadInt32();
				// a pak je pocet souboru * 64
				int p_filecount = PakReader.ReadInt32() / 64;
				//
				// presuneme se na pozici fatky a nacteme ji
				PakStream.Position = p_fatstart;
				PakFAT = new T_PakFAT[p_filecount];
				// vymazneme filelist
				PakFileList.Clear();
				for (int i = 0; i < p_filecount; i++)
				{
					// my radi lowercase. v tom se lip hleda ;-)
					string sfile = StreamHandling.PCharToString(PakReader.ReadChars(56)).ToLower();
					sfile = sfile.Replace("/","\\"); // unixovy lomitka my neradi.
					// pridame soubor do filelistu a do PakFATky
					PakFileList.Add(sfile);
					PakFAT[i].FileName = sfile;
					PakFAT[i].FileStart = PakReader.ReadInt32();
					PakFAT[i].FileLength = PakReader.ReadInt32();
				}
				PakStream.Position = 0;
				//
				return true;
			}
			else
			{
				PakStream.Position = 0;
				return false;
			}
		}

		/// <summary>
		/// Zkontroluje, jestli je soubor zadaného jména v pak souboru
		/// </summary>
		/// <param name="strFileInPak">jméno hledaného souboru</param>
		/// <returns>je/není</returns>
		public bool PakFileExists(string strFileInPak)
		{
			return GetFileIndex(strFileInPak) != -1;
		}

		private int GetFileIndex(string strFileInPak)
		{
			for (int i = 0; i < PakFAT.Length; i++)
			{
				if (PakFAT[i].FileName.ToLower() == strFileInPak.ToLower())
				{
					// soubor nalezen, vracime jeho cislo
					return i;
				}
			}
			// soubor nenalezen
			return -1;
		}

		/// <summary>
		/// Rozbalí soubor z paku do proudu
		/// </summary>
		/// <param name="strFileInPak">jméno souboru v paku</param>
		/// <param name="s_Output">výstupní proud</param>
		public void ExtractStream(string strFileInPak, Stream s_Output)
		{
			int f_index = GetFileIndex(strFileInPak);
			if (f_index == -1) return; // soubor v paku neni, tudiz konec.
			s_Output.SetLength(0);
			PakStream.Seek((long)PakFAT[f_index].FileStart, SeekOrigin.Begin);
			Streams.StreamHandling.StreamCopy(PakStream,s_Output,(long)PakFAT[f_index].FileLength);
		}

		/// <summary>
		/// Rozbalí soubor z paku na disk
		/// </summary>
		/// <param name="strFileInPak">jméno souboru v paku</param>
		/// <param name="strOutputFile">cesta k výstupnímu souboru</param>
		public void ExtractFile(string strFileInPak, string strOutputFile)
		{
			Stream f_output = new FileStream(strOutputFile,FileMode.OpenOrCreate,FileAccess.ReadWrite, FileShare.ReadWrite);
			ExtractStream(strFileInPak,f_output);
			f_output.Close();
		}

        public static bool CreateNewPak(string strFileName)
        {
            bool result = false;
            FileStream FS = new FileStream(strFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            FS.Position = 0;
            BinaryWriter BW = new BinaryWriter(FS, System.Text.Encoding.GetEncoding("Windows-1250"));
            char[] hdr = { 'P', 'A', 'C', 'K' };
            BW.Write(hdr);
            Int32 p_fatstart = sizeof(char);
            p_fatstart += sizeof(char);
            p_fatstart += sizeof(char);
            p_fatstart += sizeof(char);
            p_fatstart += sizeof(Int32);
            p_fatstart += sizeof(Int32);
            Int32 p_filecount=0;
            BW.Write(p_fatstart);
            BW.Write(p_filecount);
            BW.Close();
            FS.Close();
            return result;
        }
	}
}
 

 
