using System.Collections;
using System.IO;

namespace ArachNGIN.Files.Streams
{
    /// <summary>
    ///     Multi-mask file searcher class
    /// </summary>
    public class MultimaskFileSearcher
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="MultimaskFileSearcher" /> class.
        /// </summary>
        public MultimaskFileSearcher()
        {
            SearchExtensions = ArrayList.Synchronized(new ArrayList());
            Recursive = true;
        }

        /// <summary>
        ///     List of extensions used for search.
        /// </summary>
        /// <value>
        ///     The search extensions.
        /// </value>
        public ArrayList SearchExtensions { get; }

        /// <summary>
        ///     Gets or sets a value indicating whether searching <see cref="MultimaskFileSearcher" /> is recursive.
        /// </summary>
        /// <value>
        ///     <c>true</c> if recursive; otherwise, <c>false</c>.
        /// </value>
        public bool Recursive { get; set; }

        /// <summary>
        ///     Searches the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public FileInfo[] Search(string path)
        {
            var root = new DirectoryInfo(path);
            var subFiles = new ArrayList();
            foreach (var file in root.GetFiles())
                if (SearchExtensions.Contains(file.Extension) || SearchExtensions.Contains("*.*"))
                    subFiles.Add(file);
            if (!Recursive) return (FileInfo[])subFiles.ToArray(typeof(FileInfo));
            foreach (var directory in root.GetDirectories())
                subFiles.AddRange(Search(directory.FullName));
            return (FileInfo[])subFiles.ToArray(typeof(FileInfo));
        }
    }
}