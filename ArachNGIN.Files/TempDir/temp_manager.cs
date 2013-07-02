using System;
using System.IO;
using System.Windows.Forms;
using ArachNGIN.Files.Streams;

namespace ArachNGIN.Files.TempDir
{
    /// <summary>
    /// T��da pro obstar�v�n� temp adres��e a podobn� v�ci
    /// </summary>
    public class TempManager : IDisposable
    {
        private readonly string _sAppDir;
        private readonly string _sAppTempDir;
        private readonly string _sTempDir;

        /// <summary>
        /// Konstruktor t��dy
        /// vytvo�� adres�� v tempu
        /// </summary>
        /// <returns>instance t��dy</returns>
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
        /// property vracej�c� adres�� aplikace
        /// </summary>
        public string AppDir
        {
            get { return _sAppDir; }
        }

        /// <summary>
        /// property vracej�c� adres�� aplikace v tempu
        /// (nap�. c:\windows\temp\aplikace_035521152515)
        /// posledn� ��st je guid (aby se 2 instance aplikace/t�to t��dy
        /// neh�daly o 1 adres��)
        /// </summary>
        public string AppTempDir
        {
            get { return _sAppTempDir; }
        }

        /// <summary>
        /// property vracej�c� tempov� adres��
        /// (nap�. c:\windows\temp)
        /// </summary>
        public string TempDir
        {
            get { return _sTempDir; }
        }

        #region IDisposable Members

        /// <summary>
        /// Destruktor t��dy vyvol� fci Close(); a potla�uje v�jimky
        /// </summary>
        public void Dispose()
        {
            Close();
        }

        #endregion

        /// <summary>
        /// Sma�e adres�� v tempu
        /// </summary>
        public void Close()
        {
            Directory.Delete(_sAppTempDir, true);
        }
    }
}