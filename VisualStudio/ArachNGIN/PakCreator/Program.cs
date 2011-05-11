using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ArachNGIN.Files;

namespace PakCreator
{
    static class Program
    {

        public static FormMain frmMain;
        public static TempManager ATemp = new TempManager();

        /// <summary>
        /// Hlavní vstupní bod aplikace.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(frmMain = new FormMain());
        }
    }
}
