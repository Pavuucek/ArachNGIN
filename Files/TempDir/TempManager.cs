using System;
using System.IO;
using System.Windows.Forms;
using ArachNGIN.ClassExtensions;

namespace ArachNGIN.Files.TempDir
{
    /// <summary>
    ///     Class for handling Temp directory
    /// </summary>
    public class TempManager : IDisposable
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="TempManager" /> class.
        /// </summary>
        public TempManager()
        {
            var gGuid = Guid.NewGuid();
            TempDir = Environment.GetEnvironmentVariable("TEMP").AddSlash();
            var fileName = Path.GetFileName(Application.ExecutablePath);
            if (fileName != null)
            {
                var str = fileName.ToLower();
                str = str.Replace(@".", @"_");
                str = str + @"_" + gGuid;
                AppTempDir = (TempDir + str.ToLower()).AddSlash();
            }
            AppDir = Path.GetDirectoryName(Application.ExecutablePath).AddSlash();
            Directory.CreateDirectory(AppTempDir);
        }

        /// <summary>
        ///     Gets the application dir.
        /// </summary>
        /// <value>
        ///     The application dir.
        /// </value>
        public string AppDir { get; }

        /// <summary>
        ///     Gets the application temporary dir.
        /// </summary>
        /// <value>
        ///     The application temporary dir.
        /// </value>
        public string AppTempDir { get; }

        /// <summary>
        ///     Gets the default temporary dir.
        /// </summary>
        /// <value>
        ///     The temporary dir.
        /// </value>
        public string TempDir { get; }

        #region IDisposable Members

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Close();
        }

        #endregion IDisposable Members

        /// <summary>
        ///     Closes this instance.
        /// </summary>
        public void Close()
        {
            Directory.Delete(AppTempDir, true);
        }
    }
}