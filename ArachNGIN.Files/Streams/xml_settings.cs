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
using System.Collections;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace ArachNGIN.Files.Streams
{
    /// <summary>
    /// Tøída pro ukládání nastavení do xml souboru
    /// </summary>
    [XmlRoot("xml_def")]
    public class XmlSettings
    {
        [XmlAttribute("AssemblyName")] private string _mAsm = string.Empty;
        [XmlAttribute("CreationDate")] private DateTime _mCreationdate = DateTime.Now;
        [XmlAttribute("fileName")] private string _mFile; //= "conf.xml";
        [XmlElement("Settings")] private Hashtable _mSettingstable;

        #region  privátní podpùrné fce 

        /// <summary>
        /// fce na zjištìní cesty k aplikaci
        /// </summary>
        /// <returns>cesta k aplikaci</returns>
        private string GetAppPath()
        {
            return StrAddSlash(Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName));
        }

        /// <summary>
        /// Zjistí jestli cesta konèí lomítkem, když ne, tak ho pøidá
        /// </summary>
        /// <param name="strString">cesta</param>
        /// <returns>cesta s lomítkem</returns>
        private string StrAddSlash(string strString)
        {
            // zapamatovat si: lomítko je 0x5C!
            string s = strString;
            if (s[s.Length - 1] != (char) 0x5C) return s + (char) 0x5C;
            return s;
        }

        #endregion

        /// <summary>
        /// Konstruktor - bez jména souboru
        /// </summary>
        public XmlSettings()
        {
            _mSettingstable = new Hashtable();
            Assembly asm = Assembly.GetExecutingAssembly();
            _mAsm = asm.GetName().ToString();

            int indexOf = _mAsm.IndexOf(",", StringComparison.Ordinal);
            _mAsm = _mAsm.Substring(0, indexOf);
            _mFile = /*Path.ChangeExtension(m_asm,".conf")*/ _mAsm + ".conf";

            var dir = new DirectoryInfo(".");
            foreach (FileInfo f in dir.GetFiles(_mFile))
            {
                _mFile = f.FullName;
                _mCreationdate = f.CreationTime;
                break;
            }
            _mFile = GetAppPath() + _mFile;
            LoadFromFile();
        }

        /// <summary>
        /// Konstruktor, se specifikováním jména souboru
        /// </summary>
        /// <param name="strFileName">jméno souboru</param>
        public XmlSettings(string strFileName)
        {
            _mSettingstable = new Hashtable();
            Assembly asm = Assembly.GetExecutingAssembly();
            _mAsm = asm.GetName().ToString();
            int indexOf = _mAsm.IndexOf(",", StringComparison.Ordinal);
            _mAsm = _mAsm.Substring(0, indexOf);

            _mFile = strFileName;
            //
            LoadFromFile();
        }

        /// <summary>
        /// Vlastnost udávající jméno souboru
        /// </summary>
        public string FileName
        {
            get { return _mFile; }
            set { _mFile = value; }
        }

        /// <summary>
        /// Naètì xml nastavení ze souboru
        /// </summary>
        public void LoadFromFile()
        {
            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(_mFile);
                FormatXml(reader, _mFile);
            }
            catch (Exception e)
            {
                string str = e.Message;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        /// <summary>
        /// Uloží xml nastavení do souboru
        /// </summary>
        public void SaveToFile()
        {
            var info = new FileInfo(_mFile);
            //if(!info.Exists) info.Create();
            try
            {
                var writer = new XmlTextWriter(_mFile, Encoding.UTF8);
                writer.WriteStartDocument();
                //
                writer.WriteStartElement("xml_def");
                //
                writer.WriteAttributeString("", "AssemblyName", "", _mAsm);
                writer.WriteAttributeString("", "fileName", "", Path.GetFileName(_mFile));
                writer.WriteAttributeString("", "CreationDate", "", _mCreationdate.ToString(CultureInfo.InvariantCulture));
                //
                foreach (string line in _mSettingstable.Keys)
                {
                    writer.WriteStartElement("Item");
                    writer.WriteAttributeString("", "Name", "", line);
                    writer.WriteAttributeString("", "Value", "", _mSettingstable[line].ToString());
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Flush();
                writer.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void FormatXml(XmlReader reader, string fileName)
        {
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    string sName = "";
                    string sValue = "";
                    if (reader.Name == "Item")
                    {
                        if (reader.HasAttributes)
                        {
                            while (reader.MoveToNextAttribute())
                            {
                                if (reader.Name == "Name")
                                {
                                    sName = reader.Value;
                                }
                                else if (reader.Name == "Value")
                                {
                                    sValue = reader.Value;
                                }
                            }
                        }
                        if (sName != "") _mSettingstable.Add(sName, sValue);
                    }
                }
            }
        }

        /// <summary>
        /// Naète nastavení zadaného jména
        /// </summary>
        /// <param name="mName">jméno nastavení</param>
        /// <param name="strDefault">defaultní hodnota</param>
        /// <returns>hodnota nastavení, nebo defaultní hodnota</returns>
        public string GetSetting(string mName, string strDefault)
        {
            if (_mSettingstable.ContainsKey(mName))
            {
                return (string) _mSettingstable[mName];
            }
            SetSetting(mName, strDefault);
            return strDefault;
        }

        /// <summary>
        /// Naète nastavení zadaného jména
        /// </summary>
        /// <param name="mName">jméno nastavení</param>
        /// <returns>hodnota nastavení, nebo prázdný øetìzec když není nalezeno</returns>
        public string GetSetting(string mName)
        {
            return GetSetting(mName, "");
        }

        /// <summary>
        /// Uloží nastavení zadaného jména a hodnoty
        /// </summary>
        /// <param name="mName">jméno nastavení</param>
        /// <param name="mValue">hodnota nastavení</param>
        public void SetSetting(string mName, string mValue)
        {
            if (_mSettingstable.ContainsKey(mName))
            {
                _mSettingstable[mName] = mValue;
            }
            else
            {
                _mSettingstable.Add(mName, mValue);
            }
        }

        /// <summary>
        /// Uloží øetìzec zadaného jména a hodnoty
        /// </summary>
        /// <param name="mName">jméno nastavení</param>
        /// <param name="mValue">hodnota nastavení</param>
        public void SetString(string mName, string mValue)
        {
            SetSetting(mName, mValue);
        }

        /// <summary>
        /// Uloží èíslo zadaného jména a hodnoty
        /// </summary>
        /// <param name="mName">jméno nastavení</param>
        /// <param name="mValue">hodnota nastavení</param>
        public void SetInt(string mName, int mValue)
        {
            SetSetting(mName, mValue.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Uloží hodnotu ano/ne zadaného jména a hodnoty
        /// </summary>
        /// <param name="mName">jméno nastavení</param>
        /// <param name="mValue">hodnota nastavení</param>
        public void SetBool(string mName, bool mValue)
        {
            SetSetting(mName, mValue.ToString());
        }

        /// <summary>
        /// Naète øetìzec z nastavení
        /// </summary>
        /// <param name="mName">jméno nastavení</param>
        /// <param name="strDefault">defaultní hodnota</param>
        /// <returns>hodnota nebo defaultní hodnota</returns>
        public string GetString(string mName, string strDefault)
        {
            return GetSetting(mName, strDefault);
        }

        /// <summary>
        /// Naète èíslo z nastavení
        /// </summary>
        /// <param name="mName">jméno nastavení</param>
        /// <param name="intDefault">defaultní hodnota</param>
        /// <returns>hodnota nebo defaultní hodnota</returns>
        public int GetInt(string mName, int intDefault)
        {
            string str = GetSetting(mName, intDefault.ToString(CultureInfo.InvariantCulture));
            int r;
            try
            {
                r = Convert.ToInt32(str);
            }
            catch
            {
                r = intDefault;
            }
            return r;
        }

        /// <summary>
        /// Naète èíslo z nastavení
        /// </summary>
        /// <param name="mName">jméno nastavení</param>
        /// <returns>hodnota</returns>
        public int GetInt(string mName)
        {
            return GetInt(mName, 0);
        }

        /// <summary>
        /// Naète hodnotu ano/ne z nastavení
        /// </summary>
        /// <param name="mName">jméno nastavení</param>
        /// <param name="boolDefault">defaultní hodnota</param>
        /// <returns>hodnota nebo defaultní hodnota</returns>
        public bool GetBool(string mName, bool boolDefault)
        {
            string str = GetSetting(mName, boolDefault.ToString());
            bool r;
            try
            {
                r = Convert.ToBoolean(str);
            }
            catch
            {
                r = boolDefault;
            }
            return r;
        }

        /// <summary>
        /// Naète hodnotu ano/ne z nastavení
        /// </summary>
        /// <param name="mName">jméno nastavení</param>
        /// <returns>hodnota</returns>
        public bool GetBool(string mName)
        {
            return GetBool(mName, true);
        }
    }
}