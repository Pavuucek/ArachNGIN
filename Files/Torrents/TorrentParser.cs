using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

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
                ProcessFile(torrentFile);
            else
                throw new Exception("Torrent File invalid (null)");
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
                if (torrentFile.ReadChar() == 'd')
                    ProcessDictionary(torrentFile, false, false);
                else
                    throw new Exception("Torrent file invalid (character 'd' expected)");
            } while (torrentFile.ReadChar() != 'e');
        }

        /// <summary>
        ///     Gets the length of the string.
        /// </summary>
        /// <param name="torrentFile">The torrent file.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Invalid character. expecting ':'</exception>
        private static int GetStringLength(BinaryReader torrentFile)
        {
            var stringLength = 0;
            while (char.IsDigit((char) torrentFile.PeekChar()))
            {
                stringLength = stringLength * 10;
                stringLength += Convert.ToInt32(torrentFile.ReadChar()) - Convert.ToInt32("0");
            }
            if (torrentFile.ReadChar() == ':')
                return stringLength;
            throw new Exception("Invalid character. expecting ':'");
        }

        /// <summary>
        ///     Gets the item value.
        /// </summary>
        /// <param name="torrentFile">The torrent file.</param>
        /// <param name="stringLength">Length of the string.</param>
        /// <returns></returns>
        private static string GetItemValue(BinaryReader torrentFile, int stringLength)
        {
            return torrentFile.ReadChars(stringLength).ToString();
        }

        /// <summary>
        ///     Gets the item value byte.
        /// </summary>
        /// <param name="torrentFile">The torrent file.</param>
        /// <param name="stringLength">Length of the string.</param>
        /// <returns></returns>
        private static byte[] GetItemValueByte(BinaryReader torrentFile, int stringLength)
        {
            return torrentFile.ReadBytes(stringLength);
        }

        /// <summary>
        ///     Gets the name of the item.
        /// </summary>
        /// <param name="torrentFile">The torrent file.</param>
        /// <param name="stringLength">Length of the string.</param>
        /// <returns></returns>
        private static string GetItemName(BinaryReader torrentFile, int stringLength)
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
        private static long GetIntegerNumber(BinaryReader torrentFile)
        {
            torrentFile.ReadChar();
            var isNegative = torrentFile.PeekChar().ToString() == "-";
            long integerNumber = 0;
            while (char.IsDigit((char) torrentFile.PeekChar()))
                integerNumber = Convert.ToInt32(torrentFile.ReadChar()) - Convert.ToInt32("0");
            if (torrentFile.ReadChar() != 'e')
                throw new Exception("expected 'e'");
            if (!isNegative) return integerNumber;
            if (integerNumber > 0)
                return -integerNumber;
            throw new Exception("-0 not allowed!");
        }

        /// <summary>
        ///     Gets the hash information.
        /// </summary>
        /// <param name="torrentFile">The torrent file.</param>
        /// <param name="infoStart">The information start.</param>
        /// <param name="infoLength">Length of the information.</param>
        /// <returns></returns>
        private static string GetHashInfo(BinaryReader torrentFile, int infoStart, int infoLength)
        {
            var sha1 = new SHA1Managed();
            torrentFile.BaseStream.Position = infoStart;
            var infoValueBytes = torrentFile.ReadBytes(infoLength);
            return BitConverter.ToString(sha1.ComputeHash(infoValueBytes)).Replace("-", string.Empty)
                .ToLowerInvariant();
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
            var itemValueString = string.Empty;
            long itemValueInteger = 0;
            var itemValueByte = new byte[0];

            while (Convert.ToChar(torrentFile.PeekChar()) != 'e')
                if (char.IsDigit(Convert.ToChar(torrentFile.PeekChar())))
                {
                    var stringLength = GetStringLength(torrentFile);
                    var itemName = GetItemName(torrentFile, stringLength);
                    if (itemName == "info")
                    {
                        var infoPositionStart = (int) torrentFile.BaseStream.Position;
                        if (torrentFile.ReadChar() == 'd')
                            ProcessDictionary(torrentFile, true, false);
                        else
                            throw new Exception("character invalid. expected 'd'");
                        var infoPositionEnd = (int) torrentFile.BaseStream.Position;
                        _infoHash = GetHashInfo(torrentFile, infoPositionStart,
                            infoPositionEnd - infoPositionStart - 1);
                        if (_isSingleFile)
                            InsertNewFile();
                    }
                    else
                    {
                        if (Convert.ToChar(torrentFile.PeekChar()) == 'i')
                        {
                            itemValueInteger = GetIntegerNumber(torrentFile);
                        }
                        else if (Convert.ToChar(torrentFile.PeekChar()) == 'l')
                        {
                            ProcessList(torrentFile, itemName, itemName == "path");
                            torrentFile.ReadChar();
                        }
                        else if (Convert.ToChar(torrentFile.PeekChar()) == 'd')
                        {
                            ProcessDictionary(torrentFile, false, false);
                            torrentFile.ReadChar();
                        }
                        else
                        {
                            stringLength = GetStringLength(torrentFile);
                            if (itemName == "pieces" || itemName == "ed2k" || itemName == "ed2k")
                                itemValueByte = GetItemValueByte(torrentFile, stringLength);
                            else
                                itemValueString = GetItemValue(torrentFile, stringLength);
                        }

                        if (isInfo || isFiles)
                            switch (itemName)
                            {
                                case "length":
                                    _infoFile.Length = itemValueInteger;
                                    break;

                                case "name":
                                    _infoFile.Name = itemValueString;
                                    break;

                                case "pieces":
                                case "piece length":
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
                            }
                        else
                            switch (itemName)
                            {
                                case "announce":
                                    _anounce = itemValueString;
                                    break;

                                case "comment":
                                    _comment = itemValueString;
                                    break;

                                case "creation date":
                                    _creationDate = new DateTime(1970, 1, 1).AddSeconds(itemValueInteger);
                                    break;

                                case "encoding":
                                    _encoding = itemValueString;
                                    break;
                            }
                    }
                }
                else if (Convert.ToChar(torrentFile.PeekChar()) == 'd')
                {
                    torrentFile.ReadChar();
                    ProcessDictionary(torrentFile, isInfo, isFiles);
                }
                else if (Convert.ToChar(torrentFile.PeekChar()) == 'e')
                {
                    break;
                }
                else
                {
                    throw new Exception("expected number, 'd' or 'l'");
                }
        }

        /// <summary>
        ///     Inserts the new file.
        /// </summary>
        private void InsertNewFile()
        {
            if (_files == null)
            {
                _files = new StFile[0];
            }
            else
            {
                var oldArray = new StFile[_files.Length - 1];
                _files.CopyTo(oldArray, 0);
                _files = new StFile[_files.Length];
                oldArray.CopyTo(_files, 0);
            }
            if (!_isSingleFile)
                _infoFile.Path = _infoFile.Path.Substring(1);
            _files[_files.Length - 1] = _infoFile;
        }

        /// <summary>
        ///     Processes the list.
        /// </summary>
        /// <param name="torrentFile">The torrent file.</param>
        /// <param name="itemName">Name of the item.</param>
        /// <param name="isPath">if set to <c>true</c> [is path].</param>
        private void ProcessList(BinaryReader torrentFile, string itemName, bool isPath)
        {
            if (itemName == "files")
                _isSingleFile = false;
            var isFirstTime = true;
            while (Convert.ToChar(torrentFile.PeekChar()) != 'e')
            {
                if (isFirstTime &&
                    Convert.ToChar(torrentFile.PeekChar()) == 'l')
                    torrentFile.ReadChar();
                if (isPath)
                {
                    while (Convert.ToChar(torrentFile.PeekChar()) != 'e')
                    {
                        var stringLength = GetStringLength(torrentFile);
                        var itemValue = GetItemName(torrentFile, stringLength);
                        var s = new StringBuilder(_infoFile.Path);
                        s.Append(@"\");
                        s.Append(itemValue);
                        _infoFile.Path = s.ToString();
                    }
                    InsertNewFile();
                    break;
                }
                switch (Convert.ToChar(torrentFile.PeekChar()))
                {
                    case 'd':
                        torrentFile.ReadChar();
                        ProcessDictionary(torrentFile, true, true);
                        torrentFile.ReadChar();
                        break;

                    case 'l':
                        ProcessList(torrentFile, itemName, isPath);
                        break;

                    default:
                        var itemValue = "";
                        while (Convert.ToChar(torrentFile.PeekChar()) != 'e')
                        {
                            var stringLength = GetStringLength(torrentFile);
                            itemValue = GetItemValue(torrentFile, stringLength);
                        }
                        if (itemName == "announce-list")
                            InsertNewAnnounce(itemValue);
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
            if (_announceList == null)
            {
                _announceList = new string[0];
            }
            else
            {
                var oldArray = new string[_announceList.Length - 1];
                _announceList.CopyTo(oldArray, 0);
                _announceList = new string[_announceList.Length];
                oldArray.CopyTo(_announceList, 0);
            }
            _announceList[_announceList.Length - 1] = newAnnounce;
        }

        #region Privátní variábly

        /// <summary>
        ///     The announce list
        /// </summary>
        private string[] _announceList;

        /// <summary>
        ///     The files
        /// </summary>
        private StFile[] _files;

        /// <summary>
        ///     The information hash
        /// </summary>
        private string _infoHash;

        /// <summary>
        ///     Is single file
        /// </summary>
        private bool _isSingleFile = true;

        /// <summary>
        ///     The anounce
        /// </summary>
        private string _anounce;

        /// <summary>
        ///     The comment
        /// </summary>
        private string _comment;

        /// <summary>
        ///     The creation date
        /// </summary>
        private DateTime _creationDate;

        /// <summary>
        ///     The encoding
        /// </summary>
        private string _encoding;

        #endregion Privátní variábly
    }
}