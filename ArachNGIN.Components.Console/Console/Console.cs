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
/*
 * Created by SharpDevelop.
 * User: Takeru
 * Date: 19.3.2006
 * Time: 11:05
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using ArachNGIN.Files.Streams;

namespace ArachNGIN.Components.Console.Console
{
	/// <summary>
	/// Výčet použitý pro umístění konzole na obrazovku
	/// </summary>
	public enum ConsoleLocation
	{
		/// <summary>
		/// Levý horní roh
		/// </summary>
		TopLeft,
		/// <summary>
		/// Pravý horní roh
		/// </summary>
		TopRight,
		/// <summary>
		/// Spodní levý roh
		/// </summary>
		BottomLeft,
		/// <summary>
		/// Spodní pravý roh
		/// </summary>
		BottomRight,
		/// <summary>
		/// Prostředek obrazovky
		/// </summary>
		ScreenCenter,
		/// <summary>
		/// Někde jinde. Nastaví se na hodnoty uvedené
		/// v property Location
		/// </summary>
		SomeWhereElse
	}

    /// <summary>
    /// Výčet vlastností jak ukládat log
    /// </summary>
    public enum ConsoleAutoSave
    {
        /// <summary>
        /// Pouze manuální ukládání (default)
        /// </summary>
        ManualOnly,
        /// <summary>
        /// Uložit log při každém přidání textu
        /// </summary>
        OnLineAdd,
        /// <summary>
        /// Uložit log při ukončení programu
        /// </summary>
        OnProgramExit
    }

	/// <summary>
	/// Okno konzole
	/// </summary>
	internal partial class DebugConsoleForm
	{
		public DebugConsoleForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}

        private void BtnSaveClick(object sender, EventArgs e)
        {

        }

        private void DebugConsoleFormFormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }
	}
	/// <summary>
	/// Třída parametrů události OnCommandEntered
	/// </summary>
	public class CommandEnteredEventArgs : EventArgs
	{
		/// <summary>
		/// Konstruktor třídy události OnCommandEntered
		/// </summary>
		/// <param name="cmd">příkaz (1 slovo)</param>
		/// <param name="parArray">parametry (ostatní slova) jako pole</param>
		/// <param name="parString">parametry (ostatní slova) jako řetězec</param>
		/// <returns></returns>
		public CommandEnteredEventArgs(string cmd, string[] parArray, string parString )
		{
			parametry = parArray;
			prikaz = cmd;
			parametry_str = parString;
		}
		
		string prikaz;
		/// <summary>
		/// Příkaz konzole
		/// </summary>
		public string Command
		{
			get
			{
				return prikaz;
			}
		}
		
		string[] parametry;
		/// <summary>
		/// Parametry příkazu jako pole
		/// </summary>
		public string[] ParamArray
		{
			get
			{
				return parametry;
			}
		}
		
		string parametry_str;
		/// <summary>
		/// Parametry příkazu jako řetězec
		/// </summary>
		public string ParamString
		{
			get
			{
				return parametry_str;
			}
		}
	}
	
	/// <summary>
	/// Třída debugovací/příkazové konzole
	/// </summary>
	public class DebugConsole
	{
		/// <summary>
		/// Konstruktor konzole
		/// </summary>
		/// <returns>konzole</returns>
		public DebugConsole()
		{
			_consoleForm = new DebugConsoleForm();
			// připíchneme na txtCommand event pro zpracování zmáčknutí klávesy
			_consoleForm.txtCommand.KeyPress += new KeyPressEventHandler(this.TxtCommandKeyPress);
			// připíchneme ještě event interních příkazů
			OnCommandEntered += new CommandEnteredEvent(InternalCommands);
		}

        /// <summary>
        /// Určuje jestli má konzole automaticky
        /// </summary>
        public ConsoleAutoSave AutoSave = ConsoleAutoSave.ManualOnly;

		private bool _echoCommands = true;
		private bool _processInternalCommands = false;
		private DebugConsoleForm _consoleForm;
        private string logName = StringUtils.StrAddSlash(Path.GetDirectoryName(Application.ExecutablePath)) + DateTime.Now.ToString().Replace(":","-") + ".log";

		#region Veřejné vlastnosti
		
		/// <summary>
		/// Delegát události OnCommandEntered
		/// </summary>
		public delegate void CommandEnteredEvent(object sender, CommandEnteredEventArgs e);
		/// <summary>
		/// Událost OnCommandEntered
		/// </summary>
		public event CommandEnteredEvent OnCommandEntered;
		
		/// <summary>
		/// Textový popisek formuláře s konzolí
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
		/// Property umístění konzole (pouze zápis)
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
					default:
						break;
				}
			}
		}
		
		/// <summary>
		/// Výška formuláře konzole
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
		/// Šířka formuláře konzole
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
		/// Umístění formuláře konzole
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
		/// Určuje, jestli se při provádění příkazů budou i vypisovat do konzole
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
		/// Určuje, jestli se budou provádět i interní příkazy
		/// (např. cls - výmaz výpisu)
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
		
		#region Veřejné procedury
		/// <summary>
		/// Ukáže konzoli
		/// </summary>
		public void Show()
		{
			_consoleForm.Show();
		}
		
		/// <summary>
		/// Zavře konzoli
		/// </summary>
		public void Close()
		{
			_consoleForm.Close();
		}
		
		/// <summary>
		/// Zapíše událost do konzole
		/// </summary>
		/// <param name="t">čas události</param>
		/// <param name="message">název události</param>
		public void Write(DateTime t, string message)
		{
			ListViewItem item = new ListViewItem(t.ToLongTimeString());
			item.SubItems.Add(message);
			_consoleForm.lstLog.Items.Add(item);
			_consoleForm.lstLog.EnsureVisible(_consoleForm.lstLog.Items.Count-1);
            if (AutoSave == ConsoleAutoSave.OnLineAdd)
            {
                SaveLog();
            }
            Application.DoEvents();
		}
		
		/// <summary>
		/// Zapíše událost do konzole
		/// (čas je nastaven na teď)
		/// </summary>
		/// <param name="message">název události</param>
		public void Write(string message)
		{
			Write(DateTime.Now,message);
		}


        /// <summary>
        /// Zapíše soubor se záznamem
        /// </summary>
        public void SaveLog()
        {
            StringCollections.SaveToFile(logName,_consoleForm.lstLog.Items);
        }

		/// <summary>
		/// Zapíše událost do konzole
		/// (čas se nevyplňuje)
		/// </summary>
		/// <param name="message"></param>
		public void WriteNoTime(string message)
		{
			var item = new ListViewItem("");
			item.SubItems.Add(message);
			_consoleForm.lstLog.Items.Add(item);
			_consoleForm.lstLog.EnsureVisible(_consoleForm.lstLog.Items.Count-1);
            if (AutoSave == ConsoleAutoSave.OnLineAdd)
            {
                SaveLog();
            }
            Application.DoEvents();
		}
		
		/// <summary>
		/// Provede příkaz přes konzoli
		/// </summary>
		/// <param name="command">název příkazu + parametry</param>
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
				if(_echoCommands) Write("Command: "+command);
												
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
		/// handler události na _consoleForm.txtCommand.KeyPress
		/// </summary>
		/// <param name="sender">Odesílatel</param>
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
						_consoleForm.lstLog.Items.Clear();
						break;
                    case "savelog":
                        //_consoleForm.lstLog.Items.
                        StringCollections.SaveToFile(@"c:\aa.txt", _consoleForm.lstLog);
                        break;
				}
			}
		}
		
		#endregion
	}
	
}
