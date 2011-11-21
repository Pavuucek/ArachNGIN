using System;
using System.IO;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ArachNGIN.Files.Torrents
{
    public class TorrentParser
    {
        public struct stFile
        {
            public long Length;
            public string Name;
            public long PieceLength;
            public byte[] Pieces;
            public string md5sum;
            public byte[] ed2k;
            public byte[] sha1;
        }

        #region Privátní variábly
        private string p_Anounce;
        private string p_Comment;
        private DateTime p_CreationDate;
        private string p_Encoding;
        public stFile[] p_Files;
        public string p_InfoHash;
        public Boolean p_IsSingleFile = true;
        public string[] p_AnnounceList;
        #endregion

        private stFile infoFile;

        public TorrentParser(BinaryReader torrentFile)
        {
            if (torrentFile == null)
            {
                throw new Exception("Torrent File invalid (null)");
            }
            else
            {
                ProcessFile(torrentFile);
            }
        }

        private void ProcessFile(BinaryReader torrentFile)
        {
            while (torrentFile.ReadChar().ToString()!="e")
            {
                if (torrentFile.ReadChar().ToString() == "d")
                {
                }
                else
                {
                    throw new Exception("Torrent file invalid (character 'd' expected)");
                }
            }
        }

        private int getStringLength(BinaryReader torrentFile)
        {
            int stringLength = 0;
            while (char.IsDigit((char)torrentFile.PeekChar()))
            {
                stringLength = stringLength * 10;
                stringLength += Convert.ToInt32(torrentFile.ReadChar()) - Convert.ToInt32("0");

            }
            if (torrentFile.ReadChar().ToString() == ":")
            {
                return stringLength;
            }
            else
            {
                throw new Exception("Invalid character. expecting ':'");
            }
        }

        private string getItemValue(BinaryReader torrentFile, int stringLength)
        {
            return torrentFile.ReadChars(stringLength).ToString();
        }

        private byte[] getItemValueByte(BinaryReader torrentFile, int stringLength)
        {
            return torrentFile.ReadBytes(stringLength);
        }

        private string getItemName(BinaryReader torrentFile, int stringLength)
        {
            return torrentFile.ReadChars(stringLength).ToString();
        }

        private long getIntegerNumber(BinaryReader torrentFile)
        {
            torrentFile.ReadChar();
            bool IsNegative = (torrentFile.PeekChar().ToString() == "-");
            long IntegerNumber = 0;
            while (char.IsDigit((char)torrentFile.PeekChar()))
            {
                IntegerNumber *= 10;
                IntegerNumber = Convert.ToInt32(torrentFile.ReadChar()) - Convert.ToInt32("0");
            }
            if (torrentFile.ReadChar().ToString() == "e")
            {
                if (IsNegative)
                {
                    if (IntegerNumber > 0)
                    {
                        return -IntegerNumber;
                    }
                    else
                    {
                        throw new Exception("-0 not allowed!");
                    }
                }
                else
                {
                    return IntegerNumber;
                }
            }
            else
            {
                throw new Exception("expected 'e'");
            }
        }


    }
}
