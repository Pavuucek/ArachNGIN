namespace PakCreator
{
    partial class FormMain
    {
        /// <summary>
        /// Vyžadovaná proměnná návrháře.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Uvolnit všechny používané prostředky.
        /// </summary>
        /// <param name="disposing">hodnota true, když by měl být spravovaný prostředek odstraněn; jinak false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kód generovaný Návrhářem formulářů

        /// <summary>
        /// Metoda vyžadovaná pro podporu Návrháře - neupravovat
        /// obsah této metody s editorem kódu.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblDir = new System.Windows.Forms.Label();
            this.lblPak = new System.Windows.Forms.Label();
            this.listOutput = new System.Windows.Forms.ListBox();
            this.btnOpenDir = new System.Windows.Forms.Button();
            this.btnSavePak = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.lblOutput = new System.Windows.Forms.Label();
            this.browseDir = new System.Windows.Forms.FolderBrowserDialog();
            this.savePak = new System.Windows.Forms.SaveFileDialog();
            this.txtDir = new System.Windows.Forms.ComboBox();
            this.txtPak = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblDir
            // 
            this.lblDir.AutoSize = true;
            this.lblDir.Location = new System.Drawing.Point(9, 9);
            this.lblDir.Name = "lblDir";
            this.lblDir.Size = new System.Drawing.Size(109, 13);
            this.lblDir.TabIndex = 4;
            this.lblDir.Text = "Adresář pro zabalení:";
            // 
            // lblPak
            // 
            this.lblPak.AutoSize = true;
            this.lblPak.Location = new System.Drawing.Point(9, 58);
            this.lblPak.Name = "lblPak";
            this.lblPak.Size = new System.Drawing.Size(87, 13);
            this.lblPak.TabIndex = 1;
            this.lblPak.Text = "Výstupní soubor:";
            // 
            // listOutput
            // 
            this.listOutput.FormattingEnabled = true;
            this.listOutput.Location = new System.Drawing.Point(12, 123);
            this.listOutput.Name = "listOutput";
            this.listOutput.Size = new System.Drawing.Size(356, 121);
            this.listOutput.TabIndex = 5;
            // 
            // btnOpenDir
            // 
            this.btnOpenDir.Location = new System.Drawing.Point(340, 23);
            this.btnOpenDir.Name = "btnOpenDir";
            this.btnOpenDir.Size = new System.Drawing.Size(28, 23);
            this.btnOpenDir.TabIndex = 1;
            this.btnOpenDir.Text = "...";
            this.btnOpenDir.UseVisualStyleBackColor = true;
            this.btnOpenDir.Click += new System.EventHandler(this.btnOpenDir_Click);
            // 
            // btnSavePak
            // 
            this.btnSavePak.Location = new System.Drawing.Point(340, 72);
            this.btnSavePak.Name = "btnSavePak";
            this.btnSavePak.Size = new System.Drawing.Size(28, 23);
            this.btnSavePak.TabIndex = 3;
            this.btnSavePak.Text = "...";
            this.btnSavePak.UseVisualStyleBackColor = true;
            this.btnSavePak.Click += new System.EventHandler(this.btnSavePak_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(147, 250);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 4;
            this.btnStart.Text = "Start!";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // lblOutput
            // 
            this.lblOutput.AutoSize = true;
            this.lblOutput.Location = new System.Drawing.Point(9, 107);
            this.lblOutput.Name = "lblOutput";
            this.lblOutput.Size = new System.Drawing.Size(90, 13);
            this.lblOutput.TabIndex = 6;
            this.lblOutput.Text = "Výstupní hlášení:";
            // 
            // browseDir
            // 
            this.browseDir.ShowNewFolderButton = false;
            // 
            // savePak
            // 
            this.savePak.DefaultExt = "pak";
            this.savePak.Filter = "Standardní PAK (*.pak)|*.pak|Všechny soubory (*.*)|*.*";
            // 
            // txtDir
            // 
            this.txtDir.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::PakCreator.Properties.Settings.Default, "mruDirLast", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtDir.FormattingEnabled = true;
            this.txtDir.Location = new System.Drawing.Point(12, 25);
            this.txtDir.Name = "txtDir";
            this.txtDir.Size = new System.Drawing.Size(318, 21);
            this.txtDir.TabIndex = 8;
            this.txtDir.Text = global::PakCreator.Properties.Settings.Default.mruDirLast;
            // 
            // txtPak
            // 
            this.txtPak.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::PakCreator.Properties.Settings.Default, "mruPakLast", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtPak.FormattingEnabled = true;
            this.txtPak.Location = new System.Drawing.Point(12, 74);
            this.txtPak.Name = "txtPak";
            this.txtPak.Size = new System.Drawing.Size(318, 21);
            this.txtPak.TabIndex = 7;
            this.txtPak.Text = global::PakCreator.Properties.Settings.Default.mruPakLast;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(249, 250);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 9;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(380, 285);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtDir);
            this.Controls.Add(this.txtPak);
            this.Controls.Add(this.lblOutput);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnSavePak);
            this.Controls.Add(this.btnOpenDir);
            this.Controls.Add(this.listOutput);
            this.Controls.Add(this.lblPak);
            this.Controls.Add(this.lblDir);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ArachNGIN PakCreator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDir;
        private System.Windows.Forms.Label lblPak;
        private System.Windows.Forms.ListBox listOutput;
        private System.Windows.Forms.Button btnOpenDir;
        private System.Windows.Forms.Button btnSavePak;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label lblOutput;
        private System.Windows.Forms.FolderBrowserDialog browseDir;
        private System.Windows.Forms.SaveFileDialog savePak;
        private System.Windows.Forms.ComboBox txtPak;
        private System.Windows.Forms.ComboBox txtDir;
        private System.Windows.Forms.Button button1;
    }
}

