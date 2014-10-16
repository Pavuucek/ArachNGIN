using System;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;

namespace ArachNGIN.Files.Torrents
{
    /// <summary>
    ///     Class for parsing torrent files
    /// </summary>
    public class TorrentParser
    {
        private StFile _infoFile;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TorrentParser" /> class.
        /// </summary>
        /// <param name="torrentFile">The torrent file.</param>
        /// <exception cref="System.Exception">Torrent File invalid (null)</exception>
        public TorrentParser(BinaryReader torrentFile)
        {
            if (torrentFile != null)
            {
                ProcessFile(torrentFile);
            }
            else
            {
                throw new Exception("Torrent File invalid (null)");
            }
        }

        /// <summary>
        ///     Processes the file.
        /// </summary>
        /// <param name="torrentFile">The torrent file.</param>
        /// <exception cref="System.Exception">Torrent file invalid (character 'd' expected)</exception>
        private void ProcessFile(BinaryReader torrentFile)
        {
            do
            {
                if (torrentFile.ReadChar().ToString(CultureInfo.InvariantCulture) == "d")
                {
                    ProcessDictionary(torrentFile, false, false);
                }
                else
                {
                    throw new Exception("Torrent file invalid (character 'd' expected)");
                }
            } while (torrentFile.ReadChar().ToString(CultureInfo.InvariantCulture) != "e");
        }

        /// <summary>
        ///     Gets the length of the string.
        /// </summary>
        /// <param name="torrentFile">The torrent file.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Invalid character. expecting ':'</exception>
        private int getStringLength(BinaryReader torrentFile)
        {
            int stringLength = 0;
            while (char.IsDigit((char) torrentFile.PeekChar()))
            {
                stringLength = stringLength*10;
                stringLength += Convert.ToInt32(torrentFile.ReadChar()) - Convert.ToInt32("0");
            }
            if (torrentFile.ReadChar().ToString(CultureInfo.InvariantCulture) == ":")
            {
                return stringLength;
            }
            throw new Exception("Invalid character. expecting ':'");
        }

        /// <summary>
        ///     Gets the item value.
        /// </summary>
        /// <param name="torrentFile">The torrent file.</param>
        /// <param name="stringLength">Length of the string.</param>
        /// <returns></returns>
        private string getItemValue(BinaryReader torrentFile, int stringLength)
        {
            return torrentFile.ReadChars(stringLength).ToString();
        }

        /// <summary>
        ///     Gets the item value byte.
        /// </summary>
        /// <param name="torrentFile">The torrent file.</param>
        /// <param name="stringLength">Length of the string.</param>
        /// <returns></returns>
        private byte[] getItemValueByte(BinaryReader torrentFile, int stringLength)
        {
            return torrentFile.ReadBytes(stringLength);
        }

        /// <summary>
        ///     Gets the name of the item.
        /// </summary>
        /// <param name="torrentFile">The torrent file.</param>
        /// <param name="stringLength">Length of the string.</param>
        /// <returns></returns>
        private string getItemName(BinaryReader torrentFile, int stringLength)
        {
            return torrentFile.ReadChars(stringLength).ToString();
        }

        /// <summary>
        ///     Gets the integer number.
        /// </summary>
        /// <param name="torrentFile">The torrent file.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">
        ///     -0 not allowed!
        ///     or
        ///     expected 'e'
        /// </exception>
        private long getIntegerNumber(BinaryReader torrentFile)
        {
            torrentFile.ReadChar();
            bool isNegative = (torrentFile.PeekChar().ToString() == "-");
            long integerNumber = 0;
            while (char.IsDigit((char) torrentFile.PeekChar()))
            {
                integerNumber *= 10;
                integerNumber = Convert.ToInt32(torrentFile.ReadChar()) - Convert.ToInt32("0");
            }
            if (torrentFile.ReadChar().ToString(CultureInfo.InvariantCulture) == "e")
            {
                if (isNegative)
                {
                    if (integerNumber > 0)
                    {
                        return -integerNumber;
                    }
                    throw new Exception("-0 not allowed!");
                }
                return integerNumber;
            }
            throw new Exception("expected 'e'");
        }

        /// <summary>
        ///     Gets the hash information.
        /// </summary>
        /// <param name="torrentFile">The torrent file.</param>
        /// <param name="infoStart">The information start.</param>
        /// <param name="infoLength">Length of the information.</param>
        /// <returns></returns>
        private string getHashInfo(BinaryReader torrentFile, int infoStart, int infoLength)
        {
            var sha1 = new SHA1Managed();
            byte[] infoValueBytes;
            torrentFile.BaseStream.Position = infoStart;
            infoValueBytes = torrentFile.ReadBytes(infoLength);
            return BitConverter.ToString(sha1.ComputeHash(infoValueBytes)).Replace("-", "").ToLower();
        }

        /// <summary>
        ///     Processes the dictionary.
        /// </summary>
        /// <param name="torrentFile">The torrent file.</param>
        /// <param name="isInfo">if set to <c>true</c> [is information].</param>
        /// <param name="isFiles">if set to <c>true</c> [is files].</param>
        /// <exception cref="System.Exception">
        ///     character invalid. expected 'd'
        ///     or
        ///     expected number, 'd' or 'l'
        /// </exception>
        private void ProcessDictionary(BinaryReader torrentFile, bool isInfo, bool isFiles)
        {
            int stringLength = 0;
            string itemName = "";
            string itemValueString = "";
            long itemValueInteger = 0;
            var itemValueByte = new byte[0];

            while (Convert.ToChar(torrentFile.PeekChar()).ToString(CultureInfo.InvariantCulture) != "e")
            {
                if (char.IsDigit(Convert.ToChar(torrentFile.PeekChar())))
                {
                    stringLength = getStringLength(torrentFile);
                    itemName = getItemName(torrentFile, stringLength);
                    if (itemName == "info")
                    {
                        var infoPositionStart = (int) torrentFile.BaseStream.Position;
                        if (torrentFile.ReadChar().ToString(CultureInfo.InvariantCulture) == "d")
                        {
                            ProcessDictionary(torrentFile, true, false);
                        }
                        else
                        {
                            throw new Exception("character invalid. expected 'd'");
                        }
                        var infoPositionEnd = (int) torrentFile.BaseStream.Position;
                        PInfoHash = getHashInfo(torrentFile, infoPositionStart, infoPositionEnd - infoPositionStart - 1);
                        if (PIsSingleFile)
                        {
                            InsertNewFile();
                        }
                    }
                    else
                    {
                        if (Convert.ToChar(torrentFile.PeekChar()).ToString(CultureInfo.InvariantCulture) == "i")
                        {
                            itemValueInteger = getIntegerNumber(torrentFile);
                        }
                        else if (Convert.ToChar(torrentFile.PeekChar()).ToString(CultureInfo.InvariantCulture) == "l")
                        {
                            ProcessList(torrentFile, itemName, itemName == "path");
                            torrentFile.ReadChar();
                        }
                        else if (Convert.ToChar(torrentFile.PeekChar()).ToString(CultureInfo.InvariantCulture) == "d")
                        {
                            ProcessDictionary(torrentFile, false, false);
                            torrentFile.ReadChar();
                        }
                        else
                        {
                            stringLength = getStringLength(torrentFile);
                            if ((itemName == "pieces") | (itemName == "ed2k") | (itemName == "ed2k"))
                            {
                                itemValueByte = getItemValueByte(torrentFile, stringLength);
                            }
                            else
                            {
                                itemValueString = getItemValue(torrentFile, stringLength);
                            }
                        }

                        if (isInfo || isFiles)
                        {
                            switch (itemName)
                            {
                                case "length":
                                    _infoFile.Length = itemValueInteger;
                                    break;
                                case "name":
                                    _infoFile.Name = itemValueString;
                                    break;
                                case "piece length":
                                    _infoFile.PieceLength = itemValueInteger;
                                    break;
                                case "pieces":
                                    _infoFile.PieceLength = itemValueInteger;
                                    break;
                                case "md5sum":
                                    _infoFile.Md5Sum = itemValueString;
                                    break;
                                case "ed2k":
                                    _infoFile.Ed2K = itemValueByte;
                                    break;
                                case "sha1":
                                    _infoFile.Sha1 = itemValueByte;
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            switch (itemName)
                            {
                                case "announce":
                                    _pAnounce = itemValueString;
                                    break;
                                case "comment":
                                    _pComment = itemValueString;
                                    break;
                                case "creation date":
                                    _pCreationDate = new DateTime(1970, 1, 1).AddSeconds(itemValueInteger);
                                    break;
                                case "encoding":
                                    _pEncoding = itemValueString;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
                else if (Convert.ToChar(torrentFile.PeekChar()).ToString(CultureInfo.InvariantCulture) == "d")
                {
                    torrentFile.ReadChar();
                    ProcessDictionary(torrentFile, isInfo, isFiles);
                }
                else if (Convert.ToChar(torrentFile.PeekChar()).ToString(CultureInfo.InvariantCulture) == "e")
                {
                    break;
                }
                else
                {
                    throw new Exception("expected number, 'd' or 'l'");
                }
            }
        }

        /// <summary>
        ///     Inserts the new file.
        /// </summary>
        private void InsertNewFile()
        {
            if (PFiles == null)
            {
                PFiles = new StFile[0];
            }
            else
            {
                var oldArray = new StFile[PFiles.Length - 1];
                PFiles.CopyTo(oldArray, 0);
                PFiles = new StFile[PFiles.Length];
                oldArray.CopyTo(PFiles, 0);
            }
            if (!PIsSingleFile)
            {
                _infoFile.Path = _infoFile.Path.Substring(1);
            }
            PFiles[PFiles.Length - 1] = _infoFile;
            //infoFile = null;
        }

        /// <summary>
        ///     Processes the list.
        /// </summary>
        /// <param name="torrentFile">The torrent file.</param>
        /// <param name="itemName">Name of the item.</param>
        /// <param name="isPath">if set to <c>true</c> [is path].</param>
        private void ProcessList(BinaryReader torrentFile, string itemName, bool isPath)
        {
            bool IsFiles = false;
            if (itemName == "files")
            {
                IsFiles = true;
                PIsSingleFile = false;
            }
            bool isFirstTime = true;
            while (Convert.ToChar(torrentFile.PeekChar()).ToString(CultureInfo.InvariantCulture) != "e")
            {
                if (isFirstTime &&
                    (Convert.ToChar(torrentFile.PeekChar()).ToString(CultureInfo.InvariantCulture) == "l"))
                {
                    torrentFile.ReadChar();
                }
                if (isPath)
                {
                    while (Convert.ToChar(torrentFile.PeekChar()).ToString(CultureInfo.InvariantCulture) != "e")
                    {
                        int stringLength = getStringLength(torrentFile);
                        string itemValue = getItemName(torrentFile, stringLength);
                        _infoFile.Path += "\\" + itemValue;
                    }
                    InsertNewFile();
                    break;
                }
                if (Convert.ToChar(torrentFile.PeekChar()).ToString(CultureInfo.InvariantCulture) == "d")
                {
                    torrentFile.ReadChar();
                    ProcessDictionary(torrentFile, true, true);
                    torrentFile.ReadChar();
                }
                else if (Convert.ToChar(torrentFile.PeekChar()).ToString(CultureInfo.InvariantCulture) == "l")
                {
                    ProcessList(torrentFile, itemName, isPath);
                }
                else
                {
                    int stringLength;
                    string itemValue = "";
                    while ((Convert.ToChar(torrentFile.PeekChar()).ToString(CultureInfo.InvariantCulture) != "e"))
                    {
                        stringLength = getStringLength(torrentFile);
                        itemValue = getItemValue(torrentFile, stringLength);
                    }
                    if (itemName == "announce-list")
                    {
                        InsertNewAnnounce(itemValue);
                    }
                    break;
                }
                isFirstTime = false;
            }
        }

        /// <summary>
        ///     Inserts the new announce.
        /// </summary>
        /// <param name="newAnnounce">The new announce.</param>
        private void InsertNewAnnounce(string newAnnounce)
        {
            if (PAnnounceList == null)
            {
                PAnnounceList = new string[0];
            }
            else
            {
                var oldArray = new string[PAnnounceList.Length - 1];
                PAnnounceList.CopyTo(oldArray, 0);
                PAnnounceList = new string[PAnnounceList.Length];
                oldArray.CopyTo(PAnnounceList, 0);
            }
            PAnnounceList[PAnnounceList.Length - 1] = newAnnounce;
        }

        #region Nested type: stFile

        /// <summary>
        ///     StFile structure
        /// </summary>
        public struct StFile
        {
            /// <summary>
            ///     The ed2k
            /// </summary>
            public byte[] Ed2K;

            /// <summary>
            ///     The length
            /// </summary>
            public long Length;

            /// <summary>
            ///     The md5sum
            /// </summary>
            public string Md5Sum;

            /// <summary>
            ///     The name
            /// </summary>
            public string Name;

            /// <summary>
            ///     The path
            /// </summary>
            public string Path;

            /// <summary>
            ///     The piece length
            /// </summary>
            public long PieceLength;

            /// <summary>
            ///     The pieces
            /// </summary>
            public byte[] Pieces;

            /// <summary>
            ///     The sha1
            /// </summary>
            public byte[] Sha1;
        }

        #endregion

        #region Privátní variábly

        /// <summary>
        ///     The announce list
        /// </summary>
        public string[] PAnnounceList;

        /// <summary>
        ///     The files
        /// </summary>
        public StFile[] PFiles;

        /// <summary>
        ///     The information hash
        /// </summary>
        public string PInfoHash;

        /// <summary>
        ///     Is single file
        /// </summary>
        public Boolean PIsSingleFile = true;

        /// <summary>
        ///     The anounce
        /// </summary>
        private string _pAnounce;

        /// <summary>
        ///     The comment
        /// </summary>
        private string _pComment;

        /// <summary>
        ///     The creation date
        /// </summary>
        private DateTime _pCreationDate;

        /// <summary>
        ///     The encoding
        /// </summary>
        private string _pEncoding;

        #endregion
    }
}