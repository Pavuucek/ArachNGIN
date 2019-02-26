﻿using ArachNGIN.ClassExtensions;
using ArachNGIN.Files.FileFormats;
using ArachNGIN.Files.Streams;
using PakCreator.Properties;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Windows.Forms;

namespace PakCreator
{
    /// <summary>
    ///     Main Form class
    /// </summary>
    public partial class FormMain : Form
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="FormMain" /> class.
        /// </summary>
        public FormMain()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            var starttime = DateTime.Now;
            if (txtPak.Text == string.Empty || txtDir.Text == string.Empty)
            {
                MessageBox.Show("Musíte zadat adresář pro zabalení i název výstupního souboru!");
                return;
            }
            if (!Directory.Exists(txtDir.Text))
            {
                MessageBox.Show("Zvolený adresář neexistuje!");
                return;
            }
            if (!Directory.Exists(Path.GetDirectoryName(txtPak.Text)))
            {
                MessageBox.Show("Zadaný název PAK souboru není platný!");
                return;
            }
            txtDir.Text = txtDir.Text.AddSlash();
            var startpath = txtDir.Text;
            var pakFile = txtPak.Text;
            // mru
            if (!txtDir.Items.Contains(startpath)) txtDir.Items.Add(startpath);
            if (!txtPak.Items.Contains(pakFile)) txtPak.Items.Add(pakFile);
            //
            DisableControls(this, false);
            Log("Searching " + txtDir.Text);
            var srch = new MultimaskFileSearcher();
            srch.Recursive = true;
            srch.SearchExtensions.Add("*.*");
            var pathFiles = srch.Search(txtDir.Text);
            if (pathFiles.Length < 1)
            {
                MessageBox.Show("Zadaný adresář je prázdný!");
                return;
            }
            Log("Found " + pathFiles.Length + " files");

            var fileIndex = new StringCollection
            {
                "; PakFile Index! Autogenerated on " + DateTime.Now,
                "; Generator: " + Application.ProductName + " " + Application.ProductVersion,
                "",
                "[Index]",
                "FileCount=" + pathFiles.Length,
                ""
            };
            //
            //
            QuakePakFile.CreateNewPak(pakFile);
            var newPak = new QuakePakFile(pakFile, true);
            Log("Created a new PAK: " + pakFile);
            //
            foreach (var fi in pathFiles)
            {
                // guidy jsou lepsi nez jen cisla.

                var g = Guid.NewGuid();
                var tf = g.ToString();
                var fn = fi.FullName;
                //
                fn = fn.Replace(startpath, "");
                fn = fn.Replace("\\", "/"); // prevest normalni lomitka na unixovy
                fileIndex.Add(fn + "=" + Path.GetFileName(tf));
                Log("Adding to PAK: " + Path.GetFileName(fi.FullName));
                newPak.AddFile(fi.FullName, tf, false);
            }
            Log("Writing Index file");
            Stream idx = new MemoryStream();
            fileIndex.SaveToStream(idx);
            newPak.AddStream(idx, "(pak-index)", true);
            idx.Close();
            Log("Closing PAK file");
            var endtime = DateTime.Now;
            Log("Start: " + starttime);
            Log("End: " + endtime);
            var t = endtime - starttime;
            Log("Time: " + t);
            Log("ALL DONE !!!");
            DisableControls(this, true);
        }

        private void btnOpenDir_Click(object sender, EventArgs e)
        {
            if (browseDir.ShowDialog() == DialogResult.OK)
                txtDir.Text = browseDir.SelectedPath.AddSlash();
        }

        private void btnSavePak_Click(object sender, EventArgs e)
        {
            if (savePak.ShowDialog() == DialogResult.OK)
                txtPak.Text = savePak.FileName;
        }

        private void Log(string text)
        {
            Application.DoEvents();
            var i = listOutput.Items.Add(text);
            listOutput.SelectedIndex = i;
            Application.DoEvents();
        }

        private void DisableControls(Control parent, bool enabled)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is ListBox)
                    c.Enabled = true;
                else c.Enabled = enabled;
                DisableControls(c, enabled);
            }
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.Save();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // na otestovani pak filesystemu
            var qfs = new QuakePakFileSystem(Program.ATemp.AppDir, Program.ATemp.AppTempDir);
            MessageBox.Show("created");
            MessageBox.Show(qfs.AskFile("Project v1.6/MP4Box/TODO").ToString());
            MessageBox.Show(qfs.AskFile("delphi-webp\\.svn\\entries").ToString());
            MessageBox.Show(qfs.AskFile("0000000083").ToString());
            MessageBox.Show(qfs.AskFile("arachngin.files.dll").ToString());
        }
    }
}