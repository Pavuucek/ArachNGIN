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
    /// T��da debugovac�/p��kazov� konzole
    /// </summary>
    public class DebugConsole
    {
        /// <summary>
        /// Konstruktor konzole
        /// </summary>
        /// <returns>konzole</returns>
        public DebugConsole()
        {
            _consoleForm = new ConsoleForm();
            _consoleForm.lstLogPlain.Size = _consoleForm.lstLogSeparate.Size;
            _consoleForm.lstLogPlain.Location = _consoleForm.lstLogSeparate.Location;
            _consoleForm.lstLogPlain.Dock = _consoleForm.lstLogSeparate.Dock;
            // p�ip�chneme na txtCommand event pro zpracov�n� zm��knut� kl�vesy
            _consoleForm.txtCommand.KeyPress += TxtCommandKeyPress;
            // p�ip�chneme je�t� event intern�ch p��kaz�
            OnCommandEntered += InternalCommands;
        }

        /// <summary>
        /// Ur�uje jestli m� konzole automaticky
        /// </summary>
        public ConsoleAutoSave AutoSave = ConsoleAutoSave.ManualOnly;

        private bool _echoCommands = true;
        private bool _processInternalCommands = true;
        private readonly ConsoleForm _consoleForm;
        private bool _usePlainView = true;
        private readonly string _logName = StringUtils.StrAddSlash(Path.GetDirectoryName(Application.ExecutablePath)) + DateTime.Now.ToString(CultureInfo.InvariantCulture).Replace(@"/","-").Replace(":","-") + ".log";

        #region Ve�ejn� vlastnosti
        
        /// <summary>
        /// Deleg�t ud�losti OnCommandEntered
        /// </summary>
        public delegate void CommandEnteredEvent(object sender, CommandEnteredEventArgs e);
        /// <summary>
        /// Ud�lost OnCommandEntered
        /// </summary>
        public event CommandEnteredEvent OnCommandEntered;
        
        /// <summary>
        /// Textov� popisek formul��e s konzol�
        /// </summary>
        public string Caption
        {
            get
            {
                return _consoleForm.Text;
            }
            set
            {
                _consoleForm.Text = value;
            }
        }
        
        /// <summary>
        /// Pou��t hol� v�stup nebo rozd�len� na datum a zpr�vu?
        /// </summary>
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
        /// Property um�st�n� konzole (pouze z�pis)
        /// </summary>
        public ConsoleLocation ScreenLocation
        {
            set
            {
                _consoleForm.StartPosition = FormStartPosition.Manual;
                switch(value)
                {
                    case ConsoleLocation.TopLeft:
                        _consoleForm.Left = 0;
                        _consoleForm.Top = 0;
                        break;
                    case ConsoleLocation.TopRight:
                        _consoleForm.Left = Screen.PrimaryScreen.WorkingArea.Width-_consoleForm.Width;
                        _consoleForm.Top = 0;
                        break;
                    case ConsoleLocation.BottomLeft:
                        _consoleForm.Left = 0;
                        _consoleForm.Top = Screen.PrimaryScreen.WorkingArea.Height-_consoleForm.Height;
                        break;
                    case ConsoleLocation.BottomRight:
                        _consoleForm.Top = Screen.PrimaryScreen.WorkingArea.Height-_consoleForm.Height;
                        _consoleForm.Left = Screen.PrimaryScreen.WorkingArea.Width-_consoleForm.Width;
                        break;
                    case ConsoleLocation.ScreenCenter:
                        _consoleForm.StartPosition = FormStartPosition.CenterScreen;
                        break;
                }
            }
        }
        
        /// <summary>
        /// V��ka formul��e konzole
        /// </summary>
        public int Height
        {
            get
            {
                return _consoleForm.Height;
            }
            set
            {
                _consoleForm.Height = value;
            }
        }
        
        /// <summary>
        /// ���ka formul��e konzole
        /// </summary>
        public int Width
        {
            get
            {
                return _consoleForm.Width;
            }
            set
            {
                _consoleForm.Width = value;
            }
        }
        
        /// <summary>
        /// Um�st�n� formul��e konzole
        /// (pokud je v <seealso cref="ScreenLocation">ScreenLocation</seealso>
        /// nastaveno SomeWhereElse)
        /// </summary>
        public Point Location
        {
            get
            {
                return _consoleForm.Location;
            }
            set
            {
                _consoleForm.Location = value;
            }
        }
        
        /// <summary>
        /// Ur�uje, jestli se p�i prov�d�n� p��kaz� budou i vypisovat do konzole
        /// </summary>
        public bool EchoCommands
        {
            get
            {
                return _echoCommands;
            }
            set
            {
                _echoCommands = value;
            }
        }
        
        /// <summary>
        /// Ur�uje, jestli se budou prov�d�t i intern� p��kazy
        /// (nap�. cls - v�maz v�pisu)
        /// </summary>
        public bool ProcessInternalCommands
        {
            get
            {
                return _processInternalCommands;
            }
            set
            {
                _processInternalCommands = value;
            }
        }
        
        #endregion
        
        #region Ve�ejn� procedury
        /// <summary>
        /// Uk�e konzoli
        /// </summary>
        public void Show()
        {
            _consoleForm.Show();
        }
        
        /// <summary>
        /// Zav�e konzoli
        /// </summary>
        public void Close()
        {
            _consoleForm.Close();
        }
        
        /// <summary>
        /// Zap�e ud�lost do konzole
        /// </summary>
        /// <param name="t">�as ud�losti</param>
        /// <param name="message">n�zev ud�losti</param>
        public void Write(DateTime t, string message)
        {
            var item = new ListViewItem(t.ToLongTimeString());
            item.SubItems.Add(message);
            _consoleForm.lstLogSeparate.Items.Add(item);
            _consoleForm.lstLogPlain.Items.Add(t.ToLongTimeString() + " --> " + message);
            _consoleForm.lstLogSeparate.EnsureVisible(_consoleForm.lstLogSeparate.Items.Count-1);
            _consoleForm.lstLogPlain.SelectedIndex = _consoleForm.lstLogPlain.Items.Count - 1;
            if (AutoSave == ConsoleAutoSave.OnLineAdd)
            {
                SaveLog();
            }
            Application.DoEvents();
        }
        
        /// <summary>
        /// Zap�e ud�lost do konzole
        /// (�as je nastaven na te�)
        /// </summary>
        /// <param name="message">n�zev ud�losti</param>
        public void Write(string message)
        {
            Write(DateTime.Now,message);
        }


        /// <summary>
        /// Zap�e soubor se z�znamem
        /// </summary>
        public void SaveLog()
        {
            StringCollections.SaveToFile(_logName,_consoleForm.lstLogSeparate.Items);
        }

        /// <summary>
        /// Zap�e ud�lost do konzole
        /// (�as se nevypl�uje)
        /// </summary>
        /// <param name="message"></param>
        public void WriteNoTime(string message)
        {
            var item = new ListViewItem("");
            item.SubItems.Add(message);
            _consoleForm.lstLogSeparate.Items.Add(item);
            _consoleForm.lstLogPlain.Items.Add(message);
            _consoleForm.lstLogSeparate.EnsureVisible(_consoleForm.lstLogSeparate.Items.Count-1);
            _consoleForm.lstLogPlain.SelectedIndex = _consoleForm.lstLogPlain.Items.Count - 1;
            if (AutoSave == ConsoleAutoSave.OnLineAdd)
            {
                SaveLog();
            }
            Application.DoEvents();
        }
        
        /// <summary>
        /// Provede p��kaz p�es konzoli
        /// </summary>
        /// <param name="command">n�zev p��kazu + parametry</param>
        public void DoCommand(string command)
        {
            string[] strCmdLine = StringUtils.StringSplit(command, " "); // cely prikaz
            string cmd = strCmdLine[0];
            string[] parArray;
            string parStr;
            if(strCmdLine.Length == 1)
            {
                // pouze 1 slovo = prikaz bez parametru
                parArray = new string[0];
                parStr = "";
            }
            else
            {
                parArray = new string[strCmdLine.Length-1];
                parStr = "";
                for (int i = 1; i <= strCmdLine.Length-1 ;i++ )
                {
                    parArray[i-1] = strCmdLine[i];
                    parStr += strCmdLine[i]+" ";
                }
            }
                
            // zapiseme do outputu
            if (_echoCommands) Write("Command: " + command);
                                                
            // vyvolame event
            if(OnCommandEntered != null)
            {
                var ea = new CommandEnteredEventArgs(cmd, parArray, parStr.Trim());
                OnCommandEntered(this,ea);
            }
        }
        
        #endregion
        
        #region Eventy
        
        /// <summary>
        /// handler ud�losti na _consoleForm.txtCommand.KeyPress
        /// </summary>
        /// <param name="sender">Odes�latel</param>
        /// <param name="e">Parametry (System.Windows.Forms.KeyPressEventArgs)</param>
        private void TxtCommandKeyPress(object sender, KeyPressEventArgs e)
        {
            // kdyz user zmackne enter a prikaz neni prazdny...
            if((e.KeyChar == (char)Keys.Enter) && (_consoleForm.txtCommand.Text.Length >0))
            {
                //... poklada se obsah textboxu za prikaz
                e.Handled = true;
                DoCommand(_consoleForm.txtCommand.Text);
                // smazeme txtCommand
                _consoleForm.txtCommand.Text = "";
            }
        }
        
        private void InternalCommands(object sender, CommandEnteredEventArgs e)
        {
            if(_processInternalCommands)
            {
                switch(e.Command.ToLower())
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