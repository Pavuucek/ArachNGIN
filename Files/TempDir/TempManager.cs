using System;
using System.IO;
using System.Windows.Forms;
using ArachNGIN.Files.Streams;

namespace ArachNGIN.Files.TempDir
{
    /// <summary>
    /// Třída pro obstarávání temp adresáře a podobné věci
    /// </summary>
    public class TempManager : IDisposable
    {
        private readonly string _sAppDir;
        private readonly string _sAppTempDir;
        private readonly string _sTempDir;

        /// <summary>
        /// Konstruktor třídy
        /// vytvoří adresář v tempu
        /// </summary>
        /// <returns>instance třídy</returns>
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
        /// property vracející adresář aplikace
        /// </summary>
        public string AppDir
        {
            get { return _sAppDir; }
        }

        /// <summary>
        /// property vracející adresář aplikace v tempu
        /// (např. c:\windows\temp\aplikace_035521152515)
        /// poslední část je guid (aby se 2 instance aplikace/této třídy
        /// nehádaly o 1 adresář)
        /// </summary>
        public string AppTempDir
        {
            get { return _sAppTempDir; }
        }

        /// <summary>
        /// property vracející tempový adresář
        /// (např. c:\windows\temp)
        /// </summary>
        public string TempDir
        {
            get { return _sTempDir; }
        }

        #region IDisposable Members

        /// <summary>
        /// Destruktor třídy vyvolá fci Close(); a potlačuje výjimky
        /// </summary>
        public void Dispose()
        {
            Close();
        }

        #endregion

        /// <summary>
        /// Smaže adresář v tempu
        /// </summary>
        public void Close()
        {
            Directory.Delete(_sAppTempDir, true);
        }
    }
}