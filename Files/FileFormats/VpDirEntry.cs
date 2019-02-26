using System;

namespace ArachNGIN.Files.FileFormats
{
    /// <summary>
    ///     Structure of directory entries in VP archive
    /// </summary>
    public struct VpDirEntry
    {
        /// <summary>
        ///     Starting position
        /// </summary>
        public int Offset;

        /// <summary>
        ///     Size of the file
        /// </summary>
        public int Size;

        /// <summary>
        ///     Unix timestamp
        /// </summary>
        public int Timestamp;

        /// <summary>
        ///     Timestamp converted to datetime
        /// </summary>
        public DateTime Date;

        /// <summary>
        ///     Full name of the file with path
        /// </summary>
        public string FileName;

        /// <summary>
        ///     Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        ///     A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("[FileName: {0}, Offset: {1}, Size: {2}, Timestamp: {3}, Date: {4}]", FileName,
                Offset, Size, Timestamp, Date);
        }
    }
}