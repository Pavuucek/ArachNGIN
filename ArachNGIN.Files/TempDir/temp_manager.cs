using System;
using System.IO;
using System.Windows.Forms;
using ArachNGIN.Files.Streams;

namespace ArachNGIN.Files.TempDir
{
    /// <summary>
    /// Tøída pro obstarávání temp adresáøe a podobné vìci
    /// </summary>
    public class TempManager : IDisposable
    {
        private readonly string _sAppDir;
        private readonly string _sAppTempDir;
        private readonly string _sTempDir;

        /// <summary>
        /// Konstruktor tøídy
        /// vytvoøí adresáø v tempu
        /// </summary>
        /// <returns>instance tøídy</returns>
        public TempManager()
        {
            Guid gGuid = Guid.NewGuid();
            _sTempDir = StringUtils.StrAddSlash(Environment.GetEnvironmentVariable("TEMP"));
            string fileName = Path.GetFileName(Application.ExecutablePath);
            if (fileName != null)
            {
                string str = fileName.ToLower();
                str = str.Replace(@".", @"_");
                str = str + @"_" + gGuid.ToString();
                _sAppTempDir = StringUtils.StrAddSlash(_sTempDir + str.ToLower());
            }
            _sAppDir = StringUtils.StrAddSlash(Path.GetDirectoryName(Application.ExecutablePath));
            Directory.CreateDirectory(_sAppTempDir);
        }

        /// <summary>
        /// property vracející adresáø aplikace
        /// </summary>
        public string AppDir
        {
            get { return _sAppDir; }
        }

        /// <summary>
        /// property vracející adresáø aplikace v tempu
        /// (napø. c:\windows\temp\aplikace_035521152515)
        /// poslední èást je guid (aby se 2 instance aplikace/této tøídy
        /// nehádaly o 1 adresáø)
        /// </summary>
        public string AppTempDir
        {
            get { return _sAppTempDir; }
        }

        /// <summary>
        /// property vracející tempový adresáø
        /// (napø. c:\windows\temp)
        /// </summary>
        public string TempDir
        {
            get { return _sTempDir; }
        }

        #region IDisposable Members

        /// <summary>
        /// Destruktor tøídy vyvolá fci Close(); a potlaèuje výjimky
        /// </summary>
        public void Dispose()
        {
            Close();
        }

        #endregion

        /// <summary>
        /// Smaže adresáø v tempu
        /// </summary>
        public void Close()
        {
            Directory.Delete(_sAppTempDir, true);
        }
    }
}