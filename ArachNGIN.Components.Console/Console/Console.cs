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
using System.Collections.Specialized;
using ArachNGIN.Files.Strings;

namespace ArachNGIN.Components
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

        private void btnSave_Click(object sender, EventArgs e)
        {

        }

        private void DebugConsoleForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
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
		/// <param name="par_array">parametry (ostatní slova) jako pole</param>
		/// <param name="par_string">parametry (ostatní slova) jako řetězec</param>
		/// <returns></returns>
		public CommandEnteredEventArgs(string cmd, string[] par_array, string par_string )
		{
			this.parametry = par_array;
			this.prikaz = cmd;
			this.parametry_str = par_string;
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
			ConsoleFrm = new DebugConsoleForm();
			// připíchneme na txtCommand event pro zpracování zmáčknutí klávesy
			ConsoleFrm.txtCommand.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtCommandKeyPress);
			// připíchneme ještě event interních příkazů
			OnCommandEntered += new CommandEnteredEvent(InternalCommands);
		}

        /// <summary>
        /// Určuje jestli má konzole automaticky
        /// </summary>
        public ConsoleAutoSave AutoSave = ConsoleAutoSave.ManualOnly;

		private bool echoCommands = true;
		private bool processInternalCommands = false;
		private DebugConsoleForm ConsoleFrm;
        private string logName = StringUtils.strAddSlash(Path.GetDirectoryName(Application.ExecutablePath)) + DateTime.Now.ToString().Replace(":","-") + ".log";

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
				return ConsoleFrm.Text;
			}
			set
			{
				ConsoleFrm.Text = value;
			}
		}
		
		/// <summary>
		/// Property umístění konzole (pouze zápis)
		/// </summary>
		public ConsoleLocation ScreenLocation
		{
			set
			{
				ConsoleFrm.StartPosition = FormStartPosition.Manual;
				switch(value)
				{
					case ConsoleLocation.TopLeft:
						ConsoleFrm.Left = 0;
						ConsoleFrm.Top = 0;
						break;
					case ConsoleLocation.TopRight:
						ConsoleFrm.Left = Screen.PrimaryScreen.WorkingArea.Width-ConsoleFrm.Width;
						ConsoleFrm.Top = 0;
						break;
					case ConsoleLocation.BottomLeft:
						ConsoleFrm.Left = 0;
						ConsoleFrm.Top = Screen.PrimaryScreen.WorkingArea.Height-ConsoleFrm.Height;
						break;
					case ConsoleLocation.BottomRight:
						ConsoleFrm.Top = Screen.PrimaryScreen.WorkingArea.Height-ConsoleFrm.Height;
						ConsoleFrm.Left = Screen.PrimaryScreen.WorkingArea.Width-ConsoleFrm.Width;
						break;
					case ConsoleLocation.ScreenCenter:
						ConsoleFrm.StartPosition = FormStartPosition.CenterScreen;
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
				return ConsoleFrm.Height;
			}
			set
			{
				ConsoleFrm.Height = value;
			}
		}
		
		/// <summary>
		/// Šířka formuláře konzole
		/// </summary>
		public int Width
		{
			get
			{
				return ConsoleFrm.Width;
			}
			set
			{
				ConsoleFrm.Width = value;
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
				return ConsoleFrm.Location;
			}
			set
			{
				ConsoleFrm.Location = value;
			}
		}
		
		/// <summary>
		/// Určuje, jestli se při provádění příkazů budou i vypisovat do konzole
		/// </summary>
		public bool EchoCommands
		{
			get
			{
				return echoCommands;
			}
			set
			{
				echoCommands = value;
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
				return processInternalCommands;
			}
			set
			{
				processInternalCommands = value;
			}
		}
		
		#endregion
		
		#region Veřejné procedury
		/// <summary>
		/// Ukáže konzoli
		/// </summary>
		public void Show()
		{
			ConsoleFrm.Show();
		}
		
		/// <summary>
		/// Zavře konzoli
		/// </summary>
		public void Close()
		{
			ConsoleFrm.Close();
		}
		
		/// <summary>
		/// Zapíše událost do konzole
		/// </summary>
		/// <param name="t">čas události</param>
		/// <param name="Message">název události</param>
		public void Write(DateTime t, string Message)
		{
			ListViewItem item = new ListViewItem(t.ToLongTimeString());
			item.SubItems.Add(Message);
			ConsoleFrm.lstLog.Items.Add(item);
			ConsoleFrm.lstLog.EnsureVisible(ConsoleFrm.lstLog.Items.Count-1);
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
		/// <param name="Message">název události</param>
		public void Write(string Message)
		{
			Write(DateTime.Now,Message);
		}

        public void SaveLog()
        {
            ArachNGIN.Files.Strings.StringCollections.SaveToFile(logName,ConsoleFrm.lstLog.Items);
        }

		/// <summary>
		/// Zapíše událost do konzole
		/// (čas se nevyplňuje)
		/// </summary>
		/// <param name="Message"></param>
		public void WriteNoTime(string Message)
		{
			ListViewItem item = new ListViewItem("");
			item.SubItems.Add(Message);
			ConsoleFrm.lstLog.Items.Add(item);
			ConsoleFrm.lstLog.EnsureVisible(ConsoleFrm.lstLog.Items.Count-1);
            if (AutoSave == ConsoleAutoSave.OnLineAdd)
            {
                SaveLog();
            }
            Application.DoEvents();
		}
		
		/// <summary>
		/// Provede příkaz přes konzoli
		/// </summary>
		/// <param name="Command">název příkazu + parametry</param>
		public void DoCommand(string Command)
		{
			string[] strCmdLine = StringUtils.StringSplit(Command, " "); // cely prikaz
				string cmd = strCmdLine[0];
				string[] par_array;
				string par_str;
				if(strCmdLine.Length == 1)
				{
					// pouze 1 slovo = prikaz bez parametru
					par_array = new string[0];
					par_str = "";
				}
				else
				{
					par_array = new string[strCmdLine.Length-1];
					par_str = "";
					for (int i = 1; i <= strCmdLine.Length-1 ;i++ )
					{
						par_array[i-1] = strCmdLine[i];
						par_str += strCmdLine[i]+" ";
					}
				}
				
				// zapiseme do outputu
				if(echoCommands) Write("Command: "+Command);
												
				// vyvolame event
				if(OnCommandEntered != null)
				{
					CommandEnteredEventArgs ea = new CommandEnteredEventArgs(cmd, par_array, par_str.Trim());
					OnCommandEntered(this,ea);
				}
		}
		
		#endregion
		
		#region Eventy
		
		/// <summary>
		/// handler události na ConsoleFrm.txtCommand.KeyPress
		/// </summary>
		/// <param name="sender">Odesílatel</param>
		/// <param name="e">Parametry (System.Windows.Forms.KeyPressEventArgs)</param>
		private void TxtCommandKeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			// kdyz user zmackne enter a prikaz neni prazdny...
			if((e.KeyChar == (char)Keys.Enter) && (ConsoleFrm.txtCommand.Text.Length >0))
			{
				//... poklada se obsah textboxu za prikaz
				e.Handled = true;
				DoCommand(ConsoleFrm.txtCommand.Text);
				// smazeme txtCommand
				ConsoleFrm.txtCommand.Text = "";
			}
		}
		
		private void InternalCommands(object sender, CommandEnteredEventArgs e)
		{
			if(processInternalCommands)
			{
				switch(e.Command.ToLower())
				{
					case "cls":
						ConsoleFrm.lstLog.Items.Clear();
						break;
                    case "savelog":
                        //ConsoleFrm.lstLog.Items.
                        StringCollections.SaveToFile(@"c:\aa.txt", ConsoleFrm.lstLog);
                        break;
				}
			}
		}
		
		#endregion
	}
	
}
