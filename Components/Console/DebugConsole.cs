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

using System;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using ArachNGIN.Components.Console.Forms;
using ArachNGIN.Components.Console.Misc;
using ArachNGIN.Files.Streams;

namespace ArachNGIN.Components.Console
{
    /// <summary>
    ///     A Debug / Command console class
    /// </summary>
    public class DebugConsole : TraceListener
    {
        private readonly ConsoleForm _consoleForm;

        private readonly string _logName = StringUtils.StrAddSlash(Path.GetDirectoryName(Application.ExecutablePath)) +
                                           DateTime.Now.ToString(CultureInfo.InvariantCulture)
                                               .Replace(@"/", "-")
                                               .Replace(":", "-") + ".log";

        private bool _usePlainView = true;

        /// <summary>
        ///     The automatic save
        /// </summary>
        public ConsoleAutoSave AutoSave = ConsoleAutoSave.ManualOnly;

        /// <summary>
        /// Initializes a new instance of the <see cref="DebugConsole" /> class.
        /// </summary>
        /// <param name="useDebug">if set to <c>true</c> [use debug].</param>
        public DebugConsole(bool useDebug)
        {
            _consoleForm = new ConsoleForm();
            _consoleForm.lstLogPlain.Size = _consoleForm.lstLogSeparate.Size;
            _consoleForm.lstLogPlain.Location = _consoleForm.lstLogSeparate.Location;
            _consoleForm.lstLogPlain.Dock = _consoleForm.lstLogSeparate.Dock;
            // připíchneme na txtCommand event pro zpracování zmáčknutí klávesy
            _consoleForm.txtCommand.KeyPress += TxtCommandKeyPress;
            // připíchneme ještě event interních příkazů
            OnCommandEntered += InitInternalCommands;
            if (useDebug) Debug.Listeners.Add(this);
            else Trace.Listeners.Add(this);
        }

        public DebugConsole() : this(true)
        {
            
        }
        

        public override void WriteLine(string message)
        {
        }

        #region Veřejné vlastnosti

        /// <summary>
        ///     Delegate for CommandEntered event
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="CommandEnteredEventArgs" /> instance containing the event data.</param>
        public delegate void CommandEnteredEvent(object sender, CommandEnteredEventArgs e);

        /// <summary>
        ///     Gets or sets the caption.
        /// </summary>
        /// <value>
        ///     The caption.
        /// </value>
        public string Caption
        {
            get { return _consoleForm.Text; }
            set { _consoleForm.Text = value; }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether [use plain view].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [use plain view]; otherwise, <c>false</c>.
        /// </value>
        public bool UsePlainView
        {
            get { return _usePlainView; }
            set
            {
                _usePlainView = value;
                _consoleForm.lstLogPlain.Visible = value;
                _consoleForm.lstLogSeparate.Visible = !value;
            }
        }

        /// <summary>
        ///     Sets the screen location.
        /// </summary>
        /// <value>
        ///     The screen location.
        /// </value>
        public ConsoleLocation ScreenLocation
        {
            set
            {
                _consoleForm.StartPosition = FormStartPosition.Manual;
                switch (value)
                {
                    case ConsoleLocation.TopLeft:
                        _consoleForm.Left = 0;
                        _consoleForm.Top = 0;
                        break;
                    case ConsoleLocation.TopRight:
                        _consoleForm.Left = Screen.PrimaryScreen.WorkingArea.Width - _consoleForm.Width;
                        _consoleForm.Top = 0;
                        break;
                    case ConsoleLocation.BottomLeft:
                        _consoleForm.Left = 0;
                        _consoleForm.Top = Screen.PrimaryScreen.WorkingArea.Height - _consoleForm.Height;
                        break;
                    case ConsoleLocation.BottomRight:
                        _consoleForm.Top = Screen.PrimaryScreen.WorkingArea.Height - _consoleForm.Height;
                        _consoleForm.Left = Screen.PrimaryScreen.WorkingArea.Width - _consoleForm.Width;
                        break;
                    case ConsoleLocation.ScreenCenter:
                        _consoleForm.StartPosition = FormStartPosition.CenterScreen;
                        break;
                }
            }
        }

        /// <summary>
        ///     Gets or sets the height.
        /// </summary>
        /// <value>
        ///     The height.
        /// </value>
        public int Height
        {
            get { return _consoleForm.Height; }
            set { _consoleForm.Height = value; }
        }

        /// <summary>
        ///     Gets or sets the width.
        /// </summary>
        /// <value>
        ///     The width.
        /// </value>
        public int Width
        {
            get { return _consoleForm.Width; }
            set { _consoleForm.Width = value; }
        }

        /// <summary>
        ///     Gets or sets the location.
        /// </summary>
        /// <value>
        ///     The location.
        /// </value>
        public Point Location
        {
            get { return _consoleForm.Location; }
            set { _consoleForm.Location = value; }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether [echo commands].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [echo commands]; otherwise, <c>false</c>.
        /// </value>
        public bool EchoCommands { get; set; } = true;

        /// <summary>
        ///     Gets or sets a value indicating whether [process internal commands].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [process internal commands]; otherwise, <c>false</c>.
        /// </value>
        public bool ProcessInternalCommands { get; set; } = true;

        /// <summary>
        ///     Occurs when [on command entered].
        /// </summary>
        public event CommandEnteredEvent OnCommandEntered;

        #endregion

        #region Veřejné procedury        

        /// <summary>
        ///     Shows the console.
        /// </summary>
        public void Show()
        {
            _consoleForm.Show();
        }

        /// <summary>
        ///     Closes the console.
        /// </summary>
        public void Close()
        {
            _consoleForm.Close();
        }

        /// <summary>
        ///     Writes a message to console.
        /// </summary>
        /// <param name="t">The time.</param>
        /// <param name="message">The message.</param>
        /// <exception cref="SystemException">There is insufficient space available to add the new item to the list. </exception>
        public void Write(DateTime t, string message)
        {
            var item = new ListViewItem(t.ToLongTimeString());
            item.SubItems.Add(message);
            _consoleForm.lstLogSeparate.Items.Add(item);
            _consoleForm.lstLogPlain.Items.Add(t.ToLongTimeString() + " --> " + message);
            _consoleForm.lstLogSeparate.EnsureVisible(_consoleForm.lstLogSeparate.Items.Count - 1);
            _consoleForm.lstLogPlain.SelectedIndex = _consoleForm.lstLogPlain.Items.Count - 1;
            if (AutoSave == ConsoleAutoSave.OnLineAdd)
            {
                SaveLog();
            }
            Application.DoEvents();
        }

        /// <summary>
        ///     Writes the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <exception cref="SystemException">There is insufficient space available to add the new item to the list. </exception>
        public override void Write(string message)
        {
            Write(DateTime.Now, message);
        }


        /// <summary>
        ///     Saves the log.
        /// </summary>
        public void SaveLog()
        {
            StringCollections.SaveToFile(_logName, _consoleForm.lstLogSeparate.Items);
        }

        /// <summary>
        ///     Writes a message to console without time.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <exception cref="SystemException">There is insufficient space available to add the new item to the list. </exception>
        public void WriteNoTime(string message)
        {
            var item = new ListViewItem("");
            item.SubItems.Add(message);
            _consoleForm.lstLogSeparate.Items.Add(item);
            _consoleForm.lstLogPlain.Items.Add(message);
            _consoleForm.lstLogSeparate.EnsureVisible(_consoleForm.lstLogSeparate.Items.Count - 1);
            _consoleForm.lstLogPlain.SelectedIndex = _consoleForm.lstLogPlain.Items.Count - 1;
            if (AutoSave == ConsoleAutoSave.OnLineAdd)
            {
                SaveLog();
            }
            Application.DoEvents();
        }

        /// <summary>
        ///     Performs a console command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <exception cref="SystemException">There is insufficient space available to add the new item to the list. </exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public void DoCommand(string command)
        {
            var strCmdLine = StringUtils.StringSplit(command, " "); // cely prikaz
            var cmd = strCmdLine[0];
            string[] parArray;
            string parStr;
            if (strCmdLine.Length == 1)
            {
                // pouze 1 slovo = prikaz bez parametru
                parArray = new string[0];
                parStr = "";
            }
            else
            {
                parArray = new string[strCmdLine.Length - 1];
                parStr = "";
                for (var i = 1; i <= strCmdLine.Length - 1; i++)
                {
                    parArray[i - 1] = strCmdLine[i];
                    parStr += strCmdLine[i] + " ";
                }
            }

            // zapiseme do outputu
            if (EchoCommands) Write("Command: " + command);

            // vyvolame event
            if (OnCommandEntered != null)
            {
                var ea = new CommandEnteredEventArgs(cmd, parArray, parStr.Trim());
                OnCommandEntered(this, ea);
            }
        }

        #endregion

        #region Eventy

        /// <summary>
        ///     Texts the command key press.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="KeyPressEventArgs" /> instance containing the event data.</param>
        private void TxtCommandKeyPress(object sender, KeyPressEventArgs e)
        {
            // kdyz user zmackne enter a prikaz neni prazdny...
            if ((e.KeyChar == (char) Keys.Enter) && (_consoleForm.txtCommand.Text.Length > 0))
            {
                //... poklada se obsah textboxu za prikaz
                e.Handled = true;
                DoCommand(_consoleForm.txtCommand.Text);
                // smazeme txtCommand
                _consoleForm.txtCommand.Text = "";
            }
        }

        /// <summary>
        ///     Initializes the internal commands.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="CommandEnteredEventArgs" /> instance containing the event data.</param>
        private void InitInternalCommands(object sender, CommandEnteredEventArgs e)
        {
            if (ProcessInternalCommands)
            {
                switch (e.Command.ToLower())
                {
                    case "cls":
                        _consoleForm.lstLogSeparate.Items.Clear();
                        _consoleForm.lstLogPlain.Items.Clear();
                        break;
                    case "echot":
                        Write(DateTime.Now, e.ParamString);
                        break;
                    case "echo":
                        WriteNoTime(e.ParamString);
                        break;
                    case "savelog":
                        if (string.IsNullOrEmpty(e.ParamString)) break;
                        try
                        {
                            StringCollections.SaveToFile(e.ParamString, _consoleForm.lstLogSeparate.Items);
                        }
                        catch (Exception)
                        {
                            Write("Unable to save to file! " + e.ParamString);
                        }
                        break;
                }
            }
        }

        #endregion
    }
}