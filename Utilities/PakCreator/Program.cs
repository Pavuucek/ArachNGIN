using System;
using System.Windows.Forms;
using ArachNGIN.Files.TempDir;

namespace PakCreator
{
    internal static class Program
    {
        public static FormMain frmMain;
        public static TempManager ATemp = new TempManager();

        /// <summary>
        /// Hlavní vstupní bod aplikace.
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