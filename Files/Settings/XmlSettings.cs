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

namespace ArachNGIN.Files.Settings
{
    /// <summary>
    ///     Třída pro ukládání nastavení do xml souboru
    /// </summary>
    [XmlRoot("xml_def")]
    public class XmlSettings
    {
        [XmlAttribute("AssemblyName")]
        private readonly string _mAsm;

        [XmlAttribute("CreationDate")]
        private readonly DateTime _mCreationdate = DateTime.Now;

        [XmlElement("Settings")]
        private readonly Hashtable _mSettingstable;

        [XmlAttribute("fileName")]
        private string _mFile; //= "conf.xml"

        /// <summary>
        ///     Konstruktor - bez jména souboru
        /// </summary>
        public XmlSettings()
        {
            _mSettingstable = new Hashtable();
            var asm = Assembly.GetExecutingAssembly();
            _mAsm = asm.GetName().ToString();

            var indexOf = _mAsm.IndexOf(",", StringComparison.Ordinal);
            _mAsm = _mAsm.Substring(0, indexOf);
            _mFile = /*Path.ChangeExtension(m_asm,".conf")*/ _mAsm + ".conf";

            var dir = new DirectoryInfo(".");
            foreach (var f in dir.GetFiles(_mFile))
            {
                _mFile = f.FullName;
                _mCreationdate = f.CreationTime;
                break;
            }
            _mFile = GetAppPath() + _mFile;
            LoadFromFile();
        }

        /// <summary>
        ///     Konstruktor, se specifikováním jména souboru
        /// </summary>
        /// <param name="strFileName">jméno souboru</param>
        public XmlSettings(string strFileName)
        {
            _mSettingstable = new Hashtable();
            var asm = Assembly.GetExecutingAssembly();
            _mAsm = asm.GetName().ToString();
            var indexOf = _mAsm.IndexOf(",", StringComparison.Ordinal);
            _mAsm = _mAsm.Substring(0, indexOf);

            _mFile = strFileName;
            //
            LoadFromFile();
        }

        /// <summary>
        ///     Vlastnost udávající jméno souboru
        /// </summary>
        public string FileName
        {
            get { return _mFile; }
            set { _mFile = value; }
        }

        /// <summary>
        ///     Načtě xml nastavení ze souboru
        /// </summary>
        public void LoadFromFile()
        {
            using (var reader = new XmlTextReader(_mFile))
            {
                try
                {
                    FormatXml(reader);
                }
                catch
                {
                    // ignored
                }
            }
        }

        /// <summary>
        ///     Uloží xml nastavení do souboru
        /// </summary>
        public void SaveToFile()
        {
            //var info = new FileInfo(_mFile)
            //if(!info.Exists) info.Create()
            try
            {
                using (var writer = new XmlTextWriter(_mFile, Encoding.UTF8))
                {
                    writer.WriteStartDocument();
                    //
                    writer.WriteStartElement("xml_def");
                    //
                    writer.WriteAttributeString("", "AssemblyName", "", _mAsm);
                    writer.WriteAttributeString("", "fileName", "", Path.GetFileName(_mFile));
                    writer.WriteAttributeString("", "CreationDate", "",
                        _mCreationdate.ToString(CultureInfo.InvariantCulture));
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
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void FormatXml(XmlReader reader)
        {
            while (reader.Read())
                if (reader.NodeType == XmlNodeType.Element)
                {
                    var sName = "";
                    var sValue = "";
                    if (reader.Name == "Item")
                    {
                        if (reader.HasAttributes)
                            while (reader.MoveToNextAttribute())
                                if (reader.Name == "Name")
                                    sName = reader.Value;
                                else if (reader.Name == "Value")
                                    sValue = reader.Value;
                        if (sName != "") _mSettingstable.Add(sName, sValue);
                    }
                }
        }

        /// <summary>
        ///     Načte nastavení zadaného jména
        /// </summary>
        /// <param name="mName">jméno nastavení</param>
        /// <param name="strDefault">defaultní hodnota</param>
        /// <returns>hodnota nastavení, nebo defaultní hodnota</returns>
        public string GetSetting(string mName, string strDefault)
        {
            if (_mSettingstable.ContainsKey(mName))
                return (string)_mSettingstable[mName];
            SetSetting(mName, strDefault);
            return strDefault;
        }

        /// <summary>
        ///     Načte nastavení zadaného jména
        /// </summary>
        /// <param name="mName">jméno nastavení</param>
        /// <returns>hodnota nastavení, nebo prázdný řetězec když není nalezeno</returns>
        public string GetSetting(string mName)
        {
            return GetSetting(mName, "");
        }

        /// <summary>
        ///     Uloží nastavení zadaného jména a hodnoty
        /// </summary>
        /// <param name="mName">jméno nastavení</param>
        /// <param name="mValue">hodnota nastavení</param>
        public void SetSetting(string mName, string mValue)
        {
            if (_mSettingstable.ContainsKey(mName))
                _mSettingstable[mName] = mValue;
            else
                _mSettingstable.Add(mName, mValue);
        }

        /// <summary>
        ///     Uloží řetězec zadaného jména a hodnoty
        /// </summary>
        /// <param name="mName">jméno nastavení</param>
        /// <param name="mValue">hodnota nastavení</param>
        public void SetString(string mName, string mValue)
        {
            SetSetting(mName, mValue);
        }

        /// <summary>
        ///     Uloží číslo zadaného jména a hodnoty
        /// </summary>
        /// <param name="mName">jméno nastavení</param>
        /// <param name="mValue">hodnota nastavení</param>
        public void SetInt(string mName, int mValue)
        {
            SetSetting(mName, mValue.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        ///     Uloží hodnotu ano/ne zadaného jména a hodnoty
        /// </summary>
        /// <param name="mName">jméno nastavení</param>
        /// <param name="mValue">hodnota nastavení</param>
        public void SetBool(string mName, bool mValue)
        {
            SetSetting(mName, mValue.ToString());
        }

        /// <summary>
        ///     Načte řetězec z nastavení
        /// </summary>
        /// <param name="mName">jméno nastavení</param>
        /// <param name="strDefault">defaultní hodnota</param>
        /// <returns>hodnota nebo defaultní hodnota</returns>
        public string GetString(string mName, string strDefault)
        {
            return GetSetting(mName, strDefault);
        }

        /// <summary>
        ///     Načte číslo z nastavení
        /// </summary>
        /// <param name="mName">jméno nastavení</param>
        /// <param name="intDefault">defaultní hodnota</param>
        /// <returns>hodnota nebo defaultní hodnota</returns>
        public int GetInt(string mName, int intDefault)
        {
            var str = GetSetting(mName, intDefault.ToString(CultureInfo.InvariantCulture));
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
        ///     Načte číslo z nastavení
        /// </summary>
        /// <param name="mName">jméno nastavení</param>
        /// <returns>hodnota</returns>
        public int GetInt(string mName)
        {
            return GetInt(mName, 0);
        }

        /// <summary>
        ///     Načte hodnotu ano/ne z nastavení
        /// </summary>
        /// <param name="mName">jméno nastavení</param>
        /// <param name="boolDefault">defaultní hodnota</param>
        /// <returns>hodnota nebo defaultní hodnota</returns>
        public bool GetBool(string mName, bool boolDefault)
        {
            var str = GetSetting(mName, boolDefault.ToString());
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
        ///     Načte hodnotu ano/ne z nastavení
        /// </summary>
        /// <param name="mName">jméno nastavení</param>
        /// <returns>hodnota</returns>
        public bool GetBool(string mName)
        {
            return GetBool(mName, true);
        }

        #region privátní podpůrné fce

        /// <summary>
        ///     fce na zjištění cesty k aplikaci
        /// </summary>
        /// <returns>cesta k aplikaci</returns>
        private string GetAppPath()
        {
            return StrAddSlash(Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName));
        }

        /// <summary>
        ///     Zjistí jestli cesta končí lomítkem, když ne, tak ho přidá
        /// </summary>
        /// <param name="strString">cesta</param>
        /// <returns>cesta s lomítkem</returns>
        private string StrAddSlash(string strString)
        {
            // zapamatovat si: lomítko je 0x5C!
            var s = strString;
            if (s[s.Length - 1] != (char)0x5C) return s + (char)0x5C;
            return s;
        }

        #endregion privátní podpůrné fce
    }
}