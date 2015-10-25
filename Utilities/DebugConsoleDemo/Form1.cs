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
            DC.ScreenLocation = ConsoleLocation.TopRight;
            Debug.WriteLine("Debug start");
            Trace.WriteLine("Trace Start");
            DC.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Trace.WriteLine(textBox1.Text.ToString());
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            Trace.Write(trackBar1.Value.ToString());
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Trace.WriteLine("Event : textBox1_TextChanged ");
        }
    }
}
