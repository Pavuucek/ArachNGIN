namespace ArachNGIN.Files.FileFormats
{
    /// <summary>
    ///     PAK File Allocation Table
    /// </summary>
    internal struct PakFat
    {
        /// <summary>
        ///     The file length
        /// </summary>
        public int FileLength;

        /// <summary>
        ///     The file name
        /// </summary>
        public string FileName;

        /// <summary>
        ///     The starting offset of a file
        /// </summary>
        public int FileStart;
    }
}