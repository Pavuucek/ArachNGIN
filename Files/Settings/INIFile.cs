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
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;

namespace ArachNGIN.Files.Settings
{
    /// <summary>
    ///     Class for accessing INI files
    /// </summary>
    public class IniFile
    {
        private string _mFileName = "";

        /// <summary>
        ///     Initializes a new instance of the <see cref="IniFile" /> class.
        /// </summary>
        /// <param name="iniFileName">Name of the ini file.</param>
        public IniFile(string iniFileName)
        {
            FileName = iniFileName;
        }

        /// <summary>
        ///     Gets or sets the name of the file.
        /// </summary>
        /// <value>
        ///     The name of the file.
        /// </value>
        public string FileName
        {
            get { return _mFileName; }
            set
            {
                if (value.Trim() == _mFileName) return;
                _mFileName = value;
                LoadIniToDataSet();
            }
        }

        /// <summary>
        ///     Gets the data storage.
        /// </summary>
        /// <value>
        ///     The data storage.
        /// </value>
        public DataSet DataStorage { get; private set; }

        /// <summary>
        ///     Reads the string.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public string ReadString(string section, string key)
        {
            return ReadString(section, key, string.Empty);
        }

        /// <summary>
        ///     Reads the string.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public string ReadString(string section, string key, string defaultValue)
        {
            return Read(section, key, defaultValue);
        }

        /// <summary>
        ///     Reads the string.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="iniFileName">Name of the ini file.</param>
        /// <returns></returns>
        public string ReadString(string section, string key, string defaultValue, string iniFileName)
        {
            FileName = iniFileName;
            return ReadString(section, key, defaultValue);
        }

        /// <summary>
        ///     Reads the bool.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public bool ReadBool(string section, string key)
        {
            return ReadBool(section, key, false);
        }

        /// <summary>
        ///     Reads the integer.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public int ReadInteger(string section, string key)
        {
            return ReadInteger(section, key, 0);
        }

        /// <summary>
        ///     Reads the bool.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">if set to <c>true</c> [default value].</param>
        /// <returns></returns>
        public bool ReadBool(string section, string key, bool defaultValue)
        {
            bool ret;
            var tmpRet = Read(section, key, defaultValue.ToString());
            try
            {
                ret = Convert.ToBoolean(tmpRet);
            }
            catch
            {
                ret = defaultValue;
            }
            return ret;
        }

        /// <summary>
        ///     Reads the integer.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public int ReadInteger(string section, string key, int defaultValue)
        {
            int ret;
            var tmpRet = Read(section, key, defaultValue.ToString(CultureInfo.InvariantCulture));
            try
            {
                ret = Convert.ToInt32(tmpRet);
            }
            catch
            {
                ret = defaultValue;
            }
            return ret;
        }

        /// <summary>
        ///     Reads the integer.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="iniFileName">Name of the ini file.</param>
        /// <returns></returns>
        public int ReadInteger(string section, string key, int defaultValue, string iniFileName)
        {
            FileName = iniFileName;
            return ReadInteger(section, key, defaultValue);
        }

        /// <summary>
        ///     Reads the bool.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">if set to <c>true</c> [default value].</param>
        /// <param name="iniFileName">Name of the ini file.</param>
        /// <returns></returns>
        public bool ReadBool(string section, string key, bool defaultValue, string iniFileName)
        {
            FileName = iniFileName;
            return ReadBool(section, key, defaultValue);
        }

        /// <summary>
        ///     Reads the color.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public Color ReadColor(string section, string key, Color defaultValue)
        {
            return ReadString(section, key, defaultValue.Name).FromString();
        }

        /// <summary>
        ///     Reads the color.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public Color ReadColor(string section, string key)
        {
            return ReadColor(section, key, Color.Black);
        }

        /// <summary>
        ///     Reads the color.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="iniFileName">Name of the ini file.</param>
        /// <returns></returns>
        public Color ReadColor(string section, string key, Color defaultValue, string iniFileName)
        {
            FileName = iniFileName;
            return ReadColor(section, key, defaultValue);
        }

        /// <summary>
        ///     Reads section names from ini file
        /// </summary>
        /// <returns></returns>
        public ArrayList SectionNames()
        {
            var ret = new ArrayList();
            foreach (DataTable table in DataStorage.Tables)
                ret.Add(table.TableName);
            return ret;
        }

        /// <summary>
        ///     Writes the string.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void WriteString(string section, string key, string value)
        {
            Write(section, key, value);
            DumpDatasetToIni();
        }

        /// <summary>
        ///     Writes the string.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="iniFileName">Name of the ini file.</param>
        public void WriteString(string section, string key, string value, string iniFileName)
        {
            FileName = iniFileName;
            WriteString(section, key, value);
        }

        /// <summary>
        ///     Writes the integer.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void WriteInteger(string section, string key, int value)
        {
            WriteString(section, key, value.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        ///     Writes the bool.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public void WriteBool(string section, string key, bool value)
        {
            WriteString(section, key, value.ToString());
        }

        /// <summary>
        ///     Writes the integer.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="iniFileName">Name of the ini file.</param>
        public void WriteInteger(string section, string key, int value, string iniFileName)
        {
            WriteString(section, key, value.ToString(CultureInfo.InvariantCulture), iniFileName);
        }

        /// <summary>
        ///     Writes the bool.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <param name="iniFileName">Name of the ini file.</param>
        public void WriteBool(string section, string key, bool value, string iniFileName)
        {
            WriteString(section, key, value.ToString(), iniFileName);
        }

        /// <summary>
        ///     Writes the color.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="iniFileName">Name of the ini file.</param>
        public void WriteColor(string section, string key, Color value, string iniFileName)
        {
            WriteString(section, key, value.Name, iniFileName);
        }

        /// <summary>
        ///     Reads a section to array.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <returns></returns>
        public string[] ReadSectionToArray(string section)
        {
            var result = new string[DataStorage.Tables[section].Columns.Count];
            for (var i = 0; i < DataStorage.Tables[section].Columns.Count; i++)
                result[i] = DataStorage.Tables[section].Columns[i].ColumnName;
            return result;
        }

        /// <summary>
        ///     Reads a whole section.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <returns></returns>
        public string ReadSection(string section)
        {
            var sc = ReadSectionToArray(section);
            var result = new StringBuilder();
            foreach (var s in sc)
                result.AppendLine(s);
            return result.ToString();
        }

        /// <summary>
        ///     Writes the color.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void WriteColor(string section, string key, Color value)
        {
            WriteString(section, key, value.Name);
        }

        /// <summary>
        ///     Deletes a section.
        /// </summary>
        /// <param name="section">The section.</param>
        public void DeleteSection(string section)
        {
            if (DataStorage.Tables[section] == null) return;
            DataStorage.Tables.Remove(section);
            DumpDatasetToIni();
        }

        /// <summary>
        ///     Deletes a section.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="iniFileName">Name of the ini file.</param>
        public void DeleteSection(string section, string iniFileName)
        {
            FileName = iniFileName;
            DeleteSection(section);
        }

        /// <summary>
        ///     Reads the specified section.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        private string Read(string section, string key, string defaultValue)
        {
            string ret;
            try
            {
                ret = DataStorage.Tables[section].Rows[0][key].ToString();
            }
            catch
            {
                ret = defaultValue;
            }
            return ret;
        }

        /// <summary>
        ///     Writes the specified section.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        private void Write(string section, string key, string value)
        {
            if (DataStorage.Tables[section] == null)
            {
                DataStorage.Tables.Add(section);
                DataStorage.Tables[section].Columns.Add(key);
                var row = DataStorage.Tables[section].NewRow();
                row[key] = value;
                DataStorage.Tables[section].Rows.Add(row);
            }
            else
            {
                if (DataStorage.Tables[section].Columns[key] == null)
                    DataStorage.Tables[section].Columns.Add(key);
                DataStorage.Tables[section].Rows[0][key] = value;
            }
        }

        /// <summary>
        ///     Loads the ini to data set.
        /// </summary>
        private void LoadIniToDataSet()
        {
            DataStorage = new DataSet();
            var file = new FileInfo(_mFileName);
            DataStorage.DataSetName = file.Name.Remove(file.Name.IndexOf(file.Extension, StringComparison.Ordinal),
                file.Extension.Length);
            if (!file.Exists) return;
            DataTable table = null;
            DataRow row = null;
            var addRow = false;
            var skipSection = false;
            using (var fileStream = new StreamReader(_mFileName))
            {
                var readLine = fileStream.ReadLine();
                while (readLine != null)
                {
                    readLine = readLine.Trim();
                    if (readLine != "" && !readLine.StartsWith(";"))
                        // section marker
                        if (readLine.StartsWith("[") && readLine.EndsWith("]"))
                        {
                            // table.rows.add moved from here
                            readLine = readLine.TrimStart('[');
                            readLine = readLine.TrimEnd(']');
                            skipSection = true;
                            table = DataStorage.Tables[readLine];
                            if (table == null)
                            {
                                table = new DataTable(readLine);
                                DataStorage.Tables.Add(table);
                                row = table.NewRow();
                                skipSection = false;
                            }
                            addRow = false;
                        }
                        else // normal line
                        {
                            if (!skipSection)
                            {
                                var splitLine = readLine.Split('=');
                                if (table != null && table.Columns[splitLine[0]] == null)
                                {
                                    table.Columns.Add(splitLine[0]);
                                    if (splitLine.Length == 2)
                                        row[splitLine[0]] = splitLine[1];
                                    else
                                        row[splitLine[0]] = "";
                                    addRow = true;
                                }
                            }
                        }
                    if (addRow) table.Rows.Add(row); //... to here
                    readLine = fileStream.ReadLine();
                }
                // ... and here deleted
            }
        }

        /// <summary>
        ///     Dumps the dataset to ini.
        /// </summary>
        private void DumpDatasetToIni()
        {
            if (File.Exists(_mFileName))
                File.Delete(_mFileName);
            using (var file = File.CreateText(_mFileName))
            {
                foreach (DataTable table in DataStorage.Tables)
                {
                    file.WriteLine("[" + table.TableName + "]");
                    foreach (DataColumn col in table.Columns)
                    {
                        var value = table.Rows[0][col].ToString();
                        file.WriteLine(col.ColumnName + "=" + value);
                    }
                    file.WriteLine("");
                }
            }
        }
    }
}