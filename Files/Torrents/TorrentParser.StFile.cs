namespace ArachNGIN.Files.Torrents
{
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
}