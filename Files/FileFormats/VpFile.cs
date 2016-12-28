using ArachNGIN.ClassExtensions;
using ArachNGIN.Files.Streams;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;

namespace ArachNGIN.Files.FileFormats
{
    /// <summary>
    ///     Class to read Volition VP files (used by Conflict Freespace and Freespace 2)
    ///     File structure (copypasta from http://www.hard-light.net/wiki/index.php/*.VP)
    ///     VP files are made up of three main components; the header, followed by the individual files, and finally the index
    ///     for the entries.
    ///     <b>The Header</b>
    ///     char header[4]; //Always "VPVP"
    ///     int version;    //As of this version, still 2.
    ///     int diroffset;  //Offset to the file index
    ///     int direntries; //Number of entries
    ///     <b>The files</b>
    ///     Files are simply stored in the VP, one right after the other.No spacing or null termination is necessary.
    ///     <b>The index</b>
    ///     The index is a series of "direntries"; each directory has the structure, as seen below.
    ///     int offset; //Offset of the file data for this entry.
    ///     int size; //Size of the file data for this entry
    ///     char name[32]; //Null-terminated filename, directory name, or ".." for backdir
    ///     int timestamp; //Time the file was last modified, in unix time.
    ///     Each direntry may be a directory, a file, or a backdir.
    ///     A directory entry signifies the start of a directory,
    ///     and has the name entry set to the name of the directory;
    ///     a backdir has the name of "..", and represents the end of a directory.
    ///     Because there is no type descriptor inherent to the format,
    ///     directories and backdirs are identified by the "size", and "timestamp" entries being set to 0.
    ///     All valid VP files should start with the "data" directory as the toplevel.
    ///     Note that it isn't necessary at all to add backdirs at the end of a VP file.
    /// </summary>
    public class VpFile
    {
        /// <summary>
        ///     Non-Standard VP header (VPFS) reserved for further ArachNGIN versions
        /// </summary>
        private readonly char[] _vpHeaderCustom = { 'V', 'P', 'F', 'S' };

        /// <summary>
        ///     Standard VP header (VPVP)
        /// </summary>
        private readonly char[] _vpHeaderStandard = { 'V', 'P', 'V', 'P' };

        /// <summary>
        ///     The list of files inside VP archive
        /// </summary>
        public readonly List<VpDirEntry> Files = new List<VpDirEntry>();

        /// <summary>
        ///     Initializes a new instance of the <see cref="VpFile" /> class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public VpFile(string fileName)
        {
            FileName = string.Empty;
            if (string.IsNullOrEmpty(fileName) || !File.Exists(fileName)) return;
            ValidFile = ReadHeader(fileName);
        }

        /// <summary>
        ///     Indicates successfully opened non-empty VP file.
        /// </summary>
        /// <value>
        ///     <c>true</c> if file is valid and not empty otherwise, <c>false</c>.
        /// </value>
        public bool ValidFile { get; private set; }

        /// <summary>
        ///     Name of currently opened file including full path
        /// </summary>
        /// <value>
        ///     When is empty file wasn't opened properly.
        /// </value>
        public string FileName { get; private set; }

        /// <summary>
        ///     Reads file header.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        private bool ReadHeader(string fileName)
        {
            var result = true;
            using (var readStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                // read header
                using (var reader = new BinaryReader(readStream, Encoding.ASCII))
                {
                    var header = reader.ReadBytes(4);
                    if (!CompareHeader(header, _vpHeaderStandard) || !CompareHeader(header, _vpHeaderCustom))
                        return false;
                    if (reader.ReadInt32() != 2) return false;
                    var headerOffset = reader.ReadInt32();
                    var headerEntries = reader.ReadInt32();
                    reader.BaseStream.Position = headerOffset;
                    var currentPath = string.Empty;
                    for (var i = 0; i < headerEntries; i++)
                    {
                        VpDirEntry entry;
                        entry.Offset = reader.ReadInt32();
                        entry.Size = reader.ReadInt32();
                        entry.FileName = StreamHandling.PCharToString(reader.ReadChars(32));
                        entry.Timestamp = reader.ReadInt32();
                        entry.Date = entry.Timestamp.UnixTimestampToDateTime();
                        var isDir = entry.Timestamp == 0 && entry.Size == 0;
                        if (isDir)
                        {
                            if (entry.FileName != "..")
                                currentPath = currentPath.Append(entry.FileName).Append(Path.DirectorySeparatorChar);
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
            return result;
        }

        private static string RemoveLastPartOfPath(string path)
        {
            var result = string.Empty;
            var separated = path.Split(Path.DirectorySeparatorChar);
            for (var i = 0; i < separated.Length - 2; i++)
                result = result.Append(separated[i]).Append(Path.DirectorySeparatorChar);
            return result;
        }

        private static bool CompareHeader(byte[] fileHeader, IList<char> desiredHeader)
        {
            if (fileHeader == null) return false;
            var result = true;
            if (fileHeader.Length != desiredHeader.Count) return false;
            for (var i = 0; i < fileHeader.Length; i++)
                if (Convert.ToChar(fileHeader[i]) != desiredHeader[i]) result = false;
            return result;
        }

        /// <summary>
        ///     Checks if file exists in archive
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public bool Exists(string fileName)
        {
            return Files.Any(entry => entry.FileName == fileName);
        }

        /// <summary>
        ///     Extracts stream from VP file
        /// </summary>
        /// <param name="fileName">Name of the file to extract</param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedAccessException">
        ///     The access requested is not permitted by the operating system for the
        ///     specified path, such as when access is Write or ReadWrite and the file or directory is set for read-only access.
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive. </exception>
        /// <exception cref="SecurityException">The caller does not have the required permission. </exception>
        /// <exception cref="IOException">
        ///     An I/O error, such as specifying FileMode.CreateNew when the file specified by path
        ///     already exists, occurred. -or-The stream has been closed.
        /// </exception>
        /// <exception cref="FileNotFoundException">
        ///     The file cannot be found, such as when mode is FileMode.Truncate or
        ///     FileMode.Open, and the file specified by path does not exist. The file must already exist in these modes.
        /// </exception>
        public Stream GetStream(string fileName)
        {
            var result = new MemoryStream();
            if (!Exists(fileName)) return result;
            var entry = Files.FirstOrDefault(dirEntry => dirEntry.FileName == fileName);
            using (var stream = new FileStream(FileName, FileMode.Open, FileAccess.Read))
            {
                stream.Seek(entry.Offset, SeekOrigin.Begin);
                StreamHandling.StreamCopy(stream, result, entry.Size);
            }
            return result;
        }
    }
}