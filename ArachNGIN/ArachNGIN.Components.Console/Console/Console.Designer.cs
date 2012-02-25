/*
 * Created by SharpDevelop.
 * User: ${USER}
 * Date: ${DATE}
 * Time: ${TIME}
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace ArachNGIN.Components
{
	internal partial class DebugConsoleForm : System.Windows.Forms.Form
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
            this.lstLog = new System.Windows.Forms.ListView();
            this.colTime = new System.Windows.Forms.ColumnHeader();
            this.colMessage = new System.Windows.Forms.ColumnHeader();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkAutoSave = new System.Windows.Forms.CheckBox();
            this.txtCommand = new System.Windows.Forms.TextBox();
            this.lblCMD = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lstLog
            // 
            this.lstLog.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colTime,
            this.colMessage});
            this.lstLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstLog.Location = new System.Drawing.Point(0, 0);
            this.lstLog.MultiSelect = false;
            this.lstLog.Name = "lstLog";
            this.lstLog.Size = new System.Drawing.Size(392, 221);
            this.lstLog.TabIndex = 0;
            this.lstLog.UseCompatibleStateImageBehavior = false;
            this.lstLog.View = System.Windows.Forms.View.Details;
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
            this.panel1.Size = new System.Drawing.Size(392, 44);
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
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // DebugConsoleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 265);
            this.Controls.Add(this.lstLog);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "DebugConsoleForm";
            this.Text = "DebugConsoleForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DebugConsoleForm_FormClosing);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

		}
		internal System.Windows.Forms.ListView lstLog;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Label lblCMD;
		internal System.Windows.Forms.TextBox txtCommand;
		private System.Windows.Forms.CheckBox chkAutoSave;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.ColumnHeader colMessage;
		private System.Windows.Forms.ColumnHeader colTime;
	}
}
