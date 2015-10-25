using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ArachNGIN.Components.Console;
using ArachNGIN.Components.Console.Misc;

namespace DebugConsoleDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public DebugConsole DC = new DebugConsole();

        private void Form1_Load(object sender, EventArgs e)
        {
            DC.AutoSave = ConsoleAutoSave.OnLineAdd;
            Debug.WriteLine("Debug start");
            Trace.WriteLine("Trace Start");
            DC.Show();
        }
    }
}
