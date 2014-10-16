using System;
using System.Windows.Forms;
using ArachNGIN.Files.TempDir;

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
        public static FormMain frmMain;

        /// <summary>
        ///     Temporary Directory Manager
        /// </summary>
        public static TempManager ATemp = new TempManager();

        /// <summary>
        ///     Defines the entry point of the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(frmMain = new FormMain());
        }
    }
}