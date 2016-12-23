namespace ArachNGIN.Files.FileFormats
{
    public partial class QuakePakFile
    {
        /// <summary>
        ///     PAK File Allocation Table
        /// </summary>
        private struct PakFat
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
}