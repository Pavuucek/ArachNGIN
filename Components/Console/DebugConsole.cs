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

using ArachNGIN.ClassExtensions;
using ArachNGIN.Components.Console.Forms;
using ArachNGIN.Components.Console.Misc;
using ArachNGIN.Files.Streams;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Security;
using System.Text;
using System.Windows.Forms;

namespace ArachNGIN.Components.Console
{
    /// <summary>
    ///     A Debug / Command console class
    /// </summary>
    public class DebugConsole : TraceListener
    {
        private readonly ConsoleForm _consoleForm;

        private readonly string _logName = Path.GetDirectoryName(Application.ExecutablePath).AddSlash() +
                                           DateTime.Now.ToString(CultureInfo.InvariantCulture)
                                               .Replace(@"/", "-")
                                               .Replace(":", "-") + ".log";

        private StringBuilder _buffer = new StringBuilder();
        private ListViewItem.ListViewSubItem _currentMsgItem;
        private int _eventCounter;
        private bool _echoCommands = true;
        private bool _processInternalCommands = true;

        /// <summary>
        ///     The automatic save
        /// </summary>
        public ConsoleAutoSave AutoSave = ConsoleAutoSave.ManualOnly;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DebugConsole" /> class.
        /// </summary>
        /// <param name="useDebug">if set to <c>true</c> [use debug].</param>
        public DebugConsole(bool useDebug)
        {
            _consoleForm = new ConsoleForm();
            // připíchneme na txtCommand event pro zpracování zmáčknutí klávesy
            _consoleForm.txtCommand.KeyPress += TxtCommandKeyPress;
            // připíchneme ještě event interních příkazů
            OnCommandEntered += InitInternalCommands;
            if (useDebug) Debug.Listeners.Add(this);
            else Trace.Listeners.Add(this);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DebugConsole" /> class.
        /// </summary>
        public DebugConsole() : this(true)
        {
        }

        /// <summary>
        ///     Writes the line.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <exception cref="UnauthorizedAccessException">Access to fileName is denied. </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Enlarging the value of this instance would exceed
        ///     <see cref="P:System.Text.StringBuilder.MaxCapacity" />.
        /// </exception>
        /// <exception cref="SecurityException">The caller does not have the required permission. </exception>
        /// <exception cref="IOException">The disk is read-only. </exception>
        public override void WriteLine(string message)
        {
            CreateEventRow();
            _buffer = new StringBuilder();
            _buffer.Append(message);
            UpdateCurrentRow(true);
            _buffer = new StringBuilder();
            if (AutoSave == ConsoleAutoSave.OnLineAdd)
            {
                SaveLog();
            }
            Application.DoEvents();
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
        public bool EchoCommands
        {
            get { return _echoCommands; }
            set { _echoCommands = value; }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether [process internal commands].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [process internal commands]; otherwise, <c>false</c>.
        /// </value>
        public bool ProcessInternalCommands
        {
            get { return _processInternalCommands; }
            set { _processInternalCommands = value; }
        }

        /// <summary>
        ///     Occurs when [on command entered].
        /// </summary>
        public event CommandEnteredEvent OnCommandEntered;

        #endregion Veřejné vlastnosti

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
        public override void Close()
        {
            _consoleForm.Close();
            base.Close();
        }

        /// <summary>
        ///     Writes the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <exception cref="UnauthorizedAccessException">Access to fileName is denied. </exception>
        /// <exception cref="SecurityException">The caller does not have the required permission. </exception>
        /// <exception cref="IOException">The disk is read-only. </exception>
        public override void Write(string message)
        {
            _buffer.Append(message);
            UpdateCurrentRow(false);
            if (AutoSave == ConsoleAutoSave.OnLineAdd)
            {
                SaveLog();
            }
            Application.DoEvents();
        }

        /// <summary>
        ///     Writes the line without date and number (does not increase line number).
        /// </summary>
        /// <param name="message">The message.</param>
        /// <exception cref="UnauthorizedAccessException">Access to fileName is denied. </exception>
        /// <exception cref="SecurityException">The caller does not have the required permission. </exception>
        /// <exception cref="IOException">The disk is read-only. </exception>
        public void WriteLinePlain(string message)
        {
            CreateEventRow(false, false);
            _buffer = new StringBuilder();
            _buffer.Append(message);
            UpdateCurrentRow(true);
            _buffer = new StringBuilder();
            if (AutoSave == ConsoleAutoSave.OnLineAdd)
            {
                SaveLog();
            }
            Application.DoEvents();
        }

        /// <summary>
        ///     Saves the log.
        /// </summary>
        /// <exception cref="SecurityException">The caller does not have the required permission. </exception>
        /// <exception cref="UnauthorizedAccessException">Access to fileName is denied. </exception>
        /// <exception cref="IOException">The disk is read-only. </exception>
        public void SaveLog()
        {
            var f = new FileInfo(_logName);
            using (var s = f.CreateText())
            {
                for (var i = 0; i < _consoleForm.lstLogSeparate.Items.Count; i++)
                {
                    var sb = new StringBuilder();
                    sb.Append(_consoleForm.lstLogSeparate.Items[i].SubItems[0].Text);
                    sb.Append("\t");
                    sb.Append(_consoleForm.lstLogSeparate.Items[i].SubItems[1].Text);
                    sb.Append("\t");
                    sb.Append(_consoleForm.lstLogSeparate.Items[i].SubItems[2].Text);
                    s.WriteLine(sb.ToString());
                }
            }
        }

        /// <summary>
        ///     Performs a console command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="UnauthorizedAccessException">Access to fileName is denied. </exception>
        /// <exception cref="SecurityException">The caller does not have the required permission. </exception>
        /// <exception cref="IOException">The disk is read-only. </exception>
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

        #endregion Veřejné procedury

        #region Eventy

        /// <summary>
        ///     Texts the command key press.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="KeyPressEventArgs" /> instance containing the event data.</param>
        private void TxtCommandKeyPress(object sender, KeyPressEventArgs e)
        {
            // kdyz user zmackne enter a prikaz neni prazdny...
            if ((e.KeyChar == (char)Keys.Enter) && (_consoleForm.txtCommand.Text.Length > 0))
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
                        break;

                    case "echop":
                        WriteLinePlain(e.ParamString);
                        break;

                    case "echo":
                        WriteLine(e.ParamString);
                        break;

                    case "savelog":
                        SaveLog();
                        break;
                }
            }
        }

        /// <summary>
        ///     Updates the current row.
        /// </summary>
        /// <param name="createRowNextTime">if set to <c>true</c> [creates row next time].</param>
        private void UpdateCurrentRow(bool createRowNextTime)
        {
            if (_currentMsgItem == null) CreateEventRow();
            _currentMsgItem.Text = _buffer.ToString();
            if (createRowNextTime) _currentMsgItem = null;
            if (_consoleForm.lstLogSeparate.Items.Count > 0)
                _consoleForm.lstLogSeparate.EnsureVisible(_consoleForm.lstLogSeparate.Items.Count - 1);
        }

        /// <summary>
        ///     Creates the event row.
        /// </summary>
        /// <param name="addEventNumber">if set to <c>true</c> [add event number].</param>
        /// <param name="addTimeStamp">if set to <c>true</c> [add time stamp].</param>
        private void CreateEventRow(bool addEventNumber = true, bool addTimeStamp = true)
        {
            var timestamp = string.Empty;
            var number = string.Empty;
            if (addEventNumber) number = (++_eventCounter).ToString();
            if (addTimeStamp) timestamp = DateTime.Now.ToLongTimeString();
            var elem = new ListViewItem(number);
            elem.SubItems.Add(timestamp);
            elem.SubItems.Add(string.Empty);
            _consoleForm.lstLogSeparate.Items.Add(elem);
            _currentMsgItem = elem.SubItems[2];
        }

        #endregion Eventy
    }
}