using System.Collections;
using System.IO;

namespace ArachNGIN.Files.Streams
{
    /// <summary>
    /// Multimaskový prohledávač souborů
    /// </summary>
    public class MultimaskFileSearcher
    {
        private readonly ArrayList _extensions;
        private bool _recursive;

        /// <summary>
        /// Konstruktor třídy <see cref="MultimaskFileSearcher"/>.
        /// </summary>
        public MultimaskFileSearcher()
        {
            _extensions = ArrayList.Synchronized(new ArrayList());
            _recursive = true;
        }

        /// <summary>
        /// Seznam přípon k vyhledání
        /// </summary>
        /// <value>
        /// Seznam přípon
        /// </value>
        public ArrayList SearchExtensions
        {
            get { return _extensions; }
        }


        /// <summary>
        /// Má být vyhledávání rekurzivní (tj. včetně podadresářů) viz <see cref="MultimaskFileSearcher"/>
        /// </summary>
        /// <value>
        ///   <c>true</c> když je rekurzivní, jinak <c>false</c>.
        /// </value>
        public bool Recursive
        {
            get { return _recursive; }
            set { _recursive = value; }
        }

        /// <summary>
        /// Prohledá uvedenou cestu
        /// </summary>
        /// <param name="path">cesta</param>
        /// <returns>seznam souborů s příslušnýma příponama</returns>
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