﻿using ArachNGIN.Files.ClassExtensions;
using ArachNGIN.Files.Streams;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Security;
using System.Text;

namespace ArachNGIN.Files.FileFormats
{
    /// <summary>
    /// Class to read Volition VP files (used by Conflict Freespace and Freespace 2)
    ///
    /// File structure (copypasta from http://www.hard-light.net/wiki/index.php/*.VP)
    ///
    /// VP files are made up of three main components; the header, followed by the individual files, and finally the index for the entries.
    /// <b>The Header</b>
    /// char header[4]; //Always "VPVP"
    /// int version;    //As of this version, still 2.
    /// int diroffset;  //Offset to the file index
    /// int direntries; //Number of entries
    ///
    /// <b>The files</b>
    /// Files are simply stored in the VP, one right after the other.No spacing or null termination is necessary.
    ///
    /// <b>The index</b>
    /// The index is a series of "direntries"; each directory has the structure, as seen below.
    ///
    /// int offset; //Offset of the file data for this entry.
    /// int size; //Size of the file data for this entry
    /// char name[32]; //Null-terminated filename, directory name, or ".." for backdir
    /// int timestamp; //Time the file was last modified, in unix time.
    ///
    /// Each direntry may be a directory, a file, or a backdir.
    /// A directory entry signifies the start of a directory,
    /// and has the name entry set to the name of the directory;
    /// a backdir has the name of "..", and represents the end of a directory.
    /// Because there is no type descriptor inherent to the format,
    /// directories and backdirs are identified by the "size", and "timestamp" entries being set to 0.
    /// All valid VP files should start with the "data" directory as the toplevel.
    ///
    /// Note that it isn't necessary at all to add backdirs at the end of a VP file.
    /// </summary>
    public class VpFile
    {
        /// <summary>
        /// Standard VP header (VPVP)
        /// </summary>
        private readonly char[] _vpHeaderStandard = { 'V', 'P', 'V', 'P' };

        /// <summary>
        /// Non-Standard VP header (VPFS) reserved for further ArachNGIN versions
        /// </summary>
        private readonly char[] _vpHeaderCustom = { 'V', 'P', 'F', 'S' };

        /// <summary>
        /// Name of currently opened file including full path
        /// </summary>
        /// <value>
        /// When is empty file wasn't opened properly.
        /// </value>
        public string FileName { get; }

        /// <summary>
        /// The list of files inside VP archive
        /// </summary>
        public List<VpDirEntry> Files = new List<VpDirEntry>();

        /// <summary>
        /// Initializes a new instance of the <see cref="VpFile"/> class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="ObjectDisposedException">Methods were called after the stream was closed.</exception>
        /// <exception cref="OverflowException">The array is multidimensional and contains more than <see cref="F:System.Int32.MaxValue" /> elements.</exception>
        /// <exception cref="FileNotFoundException">The file cannot be found, such as when <paramref name="mode" /> is FileMode.Truncate or FileMode.Open, and the file specified by <paramref name="path" /> does not exist. The file must already exist in these modes.</exception>
        /// <exception cref="SecurityException">The caller does not have the required permission.</exception>
        /// <exception cref="DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
        /// <exception cref="UnauthorizedAccessException">The <paramref name="access" /> requested is not permitted by the operating system for the specified <paramref name="path" />, such as when <paramref name="access" /> is Write or ReadWrite and the file or directory is set for read-only access.</exception>
        /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
        public VpFile(string fileName)
        {
            FileName = string.Empty;
            var result = true;
            if (string.IsNullOrEmpty(fileName) || File.Exists(fileName) == false)
            {
                result = false;
                return;
            }

            using (var readStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                // read header
                using (var reader = new BinaryReader(readStream, Encoding.ASCII))
                {
                    var header = reader.ReadBytes(4);
                    if (!CompareHeader(header, _vpHeaderStandard))
                    {
                        result = false;
                        return;
                    }
                    if (reader.ReadInt32() != 2)
                    {
                        result = false;
                        return;
                    }
                    var headerOffset = reader.ReadInt32();
                    var headerEntries = reader.ReadInt32();
                    reader.BaseStream.Position = headerOffset;
                    var currentPath = string.Empty;
                    for (int i = 0; i < headerEntries; i++)
                    {
                        VpDirEntry entry;
                        entry.Offset = reader.ReadInt32();
                        entry.Size = reader.ReadInt32();
                        entry.FileName = StreamHandling.PCharToString(reader.ReadChars(32));
                        entry.Timestamp = reader.ReadInt32();
                        entry.Date = entry.Timestamp.UnixTimestampToDateTime();
                        var isDir = (entry.Timestamp == 0) && (entry.Size == 0);
                        if (isDir)
                        {
                            if (entry.FileName != "..") currentPath += entry.FileName + Path.DirectorySeparatorChar;
                            else currentPath = RemoveLastPartOfPath(currentPath);
                        }
                        else
                        {
                            entry.FileName = currentPath + entry.FileName;
                            Files.Add(entry);
                        }
                    }
                    if (Files.Count <= 0) result = false;
                }
                if (result) FileName = Path.GetFullPath(fileName);
            }
        }

        private string RemoveLastPartOfPath(string path)
        {
            var result = string.Empty;
            var separated = path.Split(Path.DirectorySeparatorChar);
            for (int i = 0; i < separated.Length - 2; i++)
            {
                result += separated[i] + Path.DirectorySeparatorChar;
            }
            return result;
        }

        private bool CompareHeader(byte[] fileHeader, char[] desiredHeader)
        {
            var result = true;
            if (fileHeader.Length != desiredHeader.Length) return false;
            for (int i = 0; i < fileHeader.Length; i++)
            {
                if (Convert.ToChar(fileHeader[i]) != desiredHeader[i]) result = false;
            }
            return result;
        }

        /// <summary>
        /// Checks if file exists in archive
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public bool Exists(string fileName)
        {
            return Files.Any(entry => entry.FileName == fileName);
        }

        /// <summary>
        /// Structure of directory entries in VP archive
        /// </summary>
        public struct VpDirEntry
        {
            /// <summary>
            /// Starting position
            /// </summary>
            public int Offset;

            /// <summary>
            /// Size of the file
            /// </summary>
            public int Size;

            /// <summary>
            /// Unix timestamp
            /// </summary>
            public int Timestamp;

            /// <summary>
            /// Timestamp converted to datetime
            /// </summary>
            public DateTime Date;

            /// <summary>
            /// Full name of the file with path
            /// </summary>
            public string FileName;

            public override string ToString()
            {
                return string.Format("[FileName: {0}, Offset: {1}, Size: {2}, Timestamp: {3}, Date: {4}]", FileName,
                    Offset, Size, Timestamp, Date);
            }
        }
    }
}