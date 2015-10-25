/*
 * Copyright (c) 2006-2013 Michal Kuncl <michal.kuncl@gmail.com> http://www.pavucina.info
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
 * associated documentation files (the "Software"), to deal in the Software without restriction,
 * including without limitation the rights to use, copy, modify, merge, publish, distribute,
 * sublicense, and/or sell copies of the Software, and to permit persons to whom the Software
 * is furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all copies or
 * substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
 * PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
 * FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
 * ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */
namespace ArachNGIN.Components.Console.Forms
{
	internal partial class ConsoleForm : System.Windows.Forms.Form
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
            this.lstLogSeparate = new System.Windows.Forms.ListView();
            this.colTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colMessage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkAutoSave = new System.Windows.Forms.CheckBox();
            this.txtCommand = new System.Windows.Forms.TextBox();
            this.lblCMD = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.colNumber = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lstLogSeparate
            // 
            this.lstLogSeparate.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colNumber,
            this.colTime,
            this.colMessage});
            this.lstLogSeparate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstLogSeparate.Location = new System.Drawing.Point(0, 0);
            this.lstLogSeparate.MultiSelect = false;
            this.lstLogSeparate.Name = "lstLogSeparate";
            this.lstLogSeparate.Size = new System.Drawing.Size(411, 221);
            this.lstLogSeparate.TabIndex = 0;
            this.lstLogSeparate.UseCompatibleStateImageBehavior = false;
            this.lstLogSeparate.View = System.Windows.Forms.View.Details;
            this.lstLogSeparate.SizeChanged += new System.EventHandler(this.LstLogSeparateSizeChanged);
            // 
            // colTime
            // 
            this.colTime.Text = "Time";
            // 
            // colMessage
            // 
            this.colMessage.Text = "Message";
            this.colMessage.Width = 300;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chkAutoSave);
            this.panel1.Controls.Add(this.txtCommand);
            this.panel1.Controls.Add(this.lblCMD);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 221);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(411, 44);
            this.panel1.TabIndex = 1;
            // 
            // chkAutoSave
            // 
            this.chkAutoSave.Location = new System.Drawing.Point(222, 9);
            this.chkAutoSave.Name = "chkAutoSave";
            this.chkAutoSave.Size = new System.Drawing.Size(77, 21);
            this.chkAutoSave.TabIndex = 3;
            this.chkAutoSave.Text = "Autosave";
            this.chkAutoSave.UseVisualStyleBackColor = true;
            // 
            // txtCommand
            // 
            this.txtCommand.Location = new System.Drawing.Point(52, 9);
            this.txtCommand.Name = "txtCommand";
            this.txtCommand.Size = new System.Drawing.Size(164, 20);
            this.txtCommand.TabIndex = 2;
            // 
            // lblCMD
            // 
            this.lblCMD.BackColor = System.Drawing.SystemColors.Window;
            this.lblCMD.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblCMD.Location = new System.Drawing.Point(7, 9);
            this.lblCMD.Name = "lblCMD";
            this.lblCMD.Size = new System.Drawing.Size(49, 21);
            this.lblCMD.TabIndex = 1;
            this.lblCMD.Text = "CMD:\\>";
            this.lblCMD.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(305, 9);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // colNumber
            // 
            this.colNumber.Text = "#";
            this.colNumber.Width = 30;
            // 
            // ConsoleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(411, 265);
            this.Controls.Add(this.lstLogSeparate);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ConsoleForm";
            this.Text = "ConsoleForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DebugConsoleFormFormClosing);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

		}
		internal System.Windows.Forms.ListView lstLogSeparate;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Label lblCMD;
		internal System.Windows.Forms.TextBox txtCommand;
		private System.Windows.Forms.CheckBox chkAutoSave;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.ColumnHeader colMessage;
        private System.Windows.Forms.ColumnHeader colTime;
        private System.Windows.Forms.ColumnHeader colNumber;
    }
}
