using System.Collections;
using System.IO;

namespace ArachNGIN.Files.Streams
{
    /// <summary>
    ///     Multi-mask file searcher class
    /// </summary>
    public class MultimaskFileSearcher
    {
        private readonly ArrayList _extensions;
        private bool _recursive;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MultimaskFileSearcher" /> class.
        /// </summary>
        public MultimaskFileSearcher()
        {
            _extensions = ArrayList.Synchronized(new ArrayList());
            _recursive = true;
        }

        /// <summary>
        ///     List of extensions used for search.
        /// </summary>
        /// <value>
        ///     The search extensions.
        /// </value>
        public ArrayList SearchExtensions
        {
            get { return _extensions; }
        }


        /// <summary>
        ///     Gets or sets a value indicating whether searching <see cref="MultimaskFileSearcher" /> is recursive.
        /// </summary>
        /// <value>
        ///     <c>true</c> if recursive; otherwise, <c>false</c>.
        /// </value>
        public bool Recursive
        {
            get { return _recursive; }
            set { _recursive = value; }
        }

        /// <summary>
        ///     Searches the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public FileInfo[] Search(string path)
        {
            var root = new DirectoryInfo(path);
            var subFiles = new ArrayList();
            foreach (FileInfo file in root.GetFiles())
            {
                // kdyz chceme vsechno (*.*) tak pridavame vsechno :-)
                if ((_extensions.Contains(file.Extension)) || (_extensions.Contains("*.*")))
                {
                    subFiles.Add(file);
                }
            }
            if (_recursive)
            {
                foreach (DirectoryInfo directory in root.GetDirectories())
                {
                    subFiles.AddRange(Search(directory.FullName));
                }
            }
            return (FileInfo[]) subFiles.ToArray(typeof (FileInfo));
        }
    }
}