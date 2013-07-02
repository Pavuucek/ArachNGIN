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
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ArachNGIN.Files.Streams
{
    /// <summary>
    /// T��da pro pr�ci se StringCollection (aka TStrings)
    /// </summary>
    public static class StringCollections
    {
        /// <summary>
        /// P�evede StringCollection na jeden string
        /// </summary>
        /// <param name="stringCollection">Vstupn� StringCollection</param>
        /// <returns>V�stupn� string</returns>
        public static string StringCollectionToString(StringCollection stringCollection)
        {
            string outputString = "";
            if (stringCollection != null && stringCollection.Count > 0)
            {
                foreach (string inputString in stringCollection)
                    outputString += inputString + "\r\n";
                outputString = outputString.Substring(0, outputString.Length - 1);
            }
            return outputString;
        }


        /// <summary>
        /// Ulo�� obsah StringCollection do souboru
        /// </summary>
        /// <param name="sFile">n�zev souboru</param>
        /// <param name="sCollection">StringCollection</param>
        public static void SaveToFile(string sFile, StringCollection sCollection)
        {
            var fi = new FileInfo(sFile);
            TextWriter writer = fi.CreateText();
            StringEnumerator enu = sCollection.GetEnumerator();
            while (enu.MoveNext())
            {
                writer.WriteLine(enu.Current);
            }
            writer.Close();
        }

        /// <summary>
        /// Ulo�� obsah StringCollection do proudu
        /// </summary>
        /// <param name="sOutput">proud</param>
        /// <param name="sCollection">obsah</param>
        public static void SaveToStream(Stream sOutput, StringCollection sCollection)
        {
            var writer = new StreamWriter(sOutput);
            writer.AutoFlush = true;
            StringEnumerator enu = sCollection.GetEnumerator();
            while (enu.MoveNext())
            {
                writer.WriteLine(enu.Current);
            }
            writer.Flush();
            //writer.Close(); // nezavirat!
        }

        /// <summary>
        /// Ulo�� StringCollection do souboru.
        /// </summary>
        /// <param name="sFile">soubor</param>
        /// <param name="sCollection">kolekce</param>
        public static void SaveToFile(string sFile, ListView.ListViewItemCollection sCollection)
        {
            var fi = new FileInfo(sFile);
            TextWriter writer = fi.CreateText();
            foreach (ListViewItem item in sCollection)
            {
                string s = "";
                for (int i = 0; i < item.SubItems.Count; i++)
                {
                    s = s + item.SubItems[i];
                    if (i < item.SubItems.Count - 1) s = s + " -> ";
                    s = s.Replace("ListViewSubItem:", "");
                    s = s.Replace("}", "");
                    s = s.Replace("{", "");
                }
                //ListViewItemConverter k = new ListViewItemConverter();
                //string s = k.ConvertToString(item);
                writer.WriteLine(s);
            }
            writer.Close();
        }

        /// <summary>
        /// Na�te soubor (textov�) do StringCollection
        /// </summary>
        /// <param name="sFile">n�zev souboru</param>
        /// <param name="sCollection">StringCollection do kter� se bude na��tat</param>
        /// <param name="bAppend">p�ipojit k existuj�c�m prvk�m v kolekci</param>
        public static void LoadFromFile(string sFile, StringCollection sCollection, bool bAppend)
        {
            var souborInfo = new FileInfo(sFile);
            string s;
            if (!souborInfo.Exists) return;
            // na��ty na��ty :-)
            TextReader reader = souborInfo.OpenText();
            if (!bAppend) sCollection.Clear();
            while ((s = reader.ReadLine()) != null)
            {
                sCollection.Add(s);
            }
            // na�teno	
            reader.Close();
        }

        /// <summary>
        /// Na�te proud do StringCollection
        /// </summary>
        /// <param name="sInput">proud</param>
        /// <param name="sCollection">v�stup</param>
        /// <param name="bAppend">p�ipojit k existuj�c�m prvk�m v kolekci</param>
        public static void LoadFromStream(Stream sInput, StringCollection sCollection, bool bAppend)
        {
            var reader = new StreamReader(sInput);
            string s;
            if (!bAppend) sCollection.Clear();
            while ((s = reader.ReadLine()) != null)
            {
                sCollection.Add(s);
            }
            // reader.Close(); // nezavirat!
        }

        /// <summary>
        /// Na�te proud do StringCollection
        /// </summary>
        /// <param name="sInput">proud</param>
        /// <param name="sCollection">v�stup</param>
        public static void LoadFromStream(Stream sInput, StringCollection sCollection)
        {
            LoadFromStream(sInput, sCollection, false);
        }

        /// <summary>
        /// Na�te soubor (textov�) do StringCollection
        /// </summary>
        /// <param name="sFile">n�zev souboru</param>
        /// <param name="sCollection">StringCollection do kter� se bude na��tat</param>
        public static void LoadFromFile(string sFile, StringCollection sCollection)
        {
            LoadFromFile(sFile, sCollection, false);
        }

        /// <summary>
        /// Ulo�� ListView do souboru.
        /// </summary>
        /// <param name="sFile">soubor</param>
        /// <param name="list">ListView</param>
        public static void SaveToFile(string sFile, ListView list)
        {
            var listViewContent = new StringBuilder();

            foreach (ListViewItem item in list.Items)
            {
                listViewContent.Append(item.Text);
                listViewContent.Append(Environment.NewLine);
            }

            TextWriter tw = new StreamWriter(sFile);

            tw.WriteLine(listViewContent.ToString());

            tw.Close();
        }
    }
}