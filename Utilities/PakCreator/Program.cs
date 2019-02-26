using ArachNGIN.Files.TempDir;
using System;
using System.Windows.Forms;

namespace PakCreator
{
    /// <summary>
    ///     PakCreator Main window class
    /// </summary>
    internal static class Program
    {
        /// <summary>
        ///     The main form
        /// </summary>
        private static FormMain _frmMain;

        /// <summary>
        ///     Temporary Directory Manager
        /// </summary>
        public static readonly TempManager ATemp = new TempManager();

        /// <summary>
        ///     Defines the entry point of the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(_frmMain = new FormMain());
        }
    }
}