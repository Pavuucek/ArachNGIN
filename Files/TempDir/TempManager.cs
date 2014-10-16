using System;
using System.IO;
using System.Windows.Forms;
using ArachNGIN.Files.Streams;

namespace ArachNGIN.Files.TempDir
{
    /// <summary>
    ///     Class for handling Temp directory
    /// </summary>
    public class TempManager : IDisposable
    {
        private readonly string _sAppDir;
        private readonly string _sAppTempDir;
        private readonly string _sTempDir;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TempManager" /> class.
        /// </summary>
        public TempManager()
        {
            Guid gGuid = Guid.NewGuid();
            _sTempDir = StringUtils.StrAddSlash(Environment.GetEnvironmentVariable("TEMP"));
            string fileName = Path.GetFileName(Application.ExecutablePath);
            if (fileName != null)
            {
                string str = fileName.ToLower();
                str = str.Replace(@".", @"_");
                str = str + @"_" + gGuid;
                _sAppTempDir = StringUtils.StrAddSlash(_sTempDir + str.ToLower());
            }
            _sAppDir = StringUtils.StrAddSlash(Path.GetDirectoryName(Application.ExecutablePath));
            Directory.CreateDirectory(_sAppTempDir);
        }

        /// <summary>
        ///     Gets the application dir.
        /// </summary>
        /// <value>
        ///     The application dir.
        /// </value>
        public string AppDir
        {
            get { return _sAppDir; }
        }

        /// <summary>
        ///     Gets the application temporary dir.
        /// </summary>
        /// <value>
        ///     The application temporary dir.
        /// </value>
        public string AppTempDir
        {
            get { return _sAppTempDir; }
        }

        /// <summary>
        ///     Gets the default temporary dir.
        /// </summary>
        /// <value>
        ///     The temporary dir.
        /// </value>
        public string TempDir
        {
            get { return _sTempDir; }
        }

        #region IDisposable Members

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Close();
        }

        #endregion

        /// <summary>
        ///     Closes this instance.
        /// </summary>
        public void Close()
        {
            Directory.Delete(_sAppTempDir, true);
        }
    }
}