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
using System.Security;
using System.Text;
using System.Windows.Forms;

namespace ArachNGIN.ClassExtensions
{
    /// <summary>
    ///     Class for working with StringCollections
    /// </summary>
    public static class StringCollections
    {
        /// <summary>
        ///     Converts a StringCollection to string
        /// </summary>
        /// <param name="stringCollection">The string collection.</param>
        /// <returns></returns>
        public static string StringCollectionToString(this StringCollection stringCollection)
        {
            var outputBuilder = new StringBuilder();
            var outputString = string.Empty;
            if (stringCollection == null || stringCollection.Count <= 0) return outputString;
            foreach (var inputString in stringCollection)
                outputBuilder.Append(inputString + "\r\n");
            outputString = outputBuilder.ToString();
            return outputString.Substring(0, outputString.Length - 1);
        }

        /// <summary>
        ///     Saves StringCollection to a file
        /// </summary>
        /// <param name="sFile">The file.</param>
        /// <param name="sCollection">The StringCollection.</param>
        /// <exception cref="SecurityException">The caller does not have the required permission. </exception>
        /// <exception cref="UnauthorizedAccessException">Access to fileName is denied. </exception>
        /// <exception cref="IOException">The disk is read-only. </exception>
        public static void SaveToFile(this StringCollection sCollection, string sFile)
        {
            var fi = new FileInfo(sFile);
            using (var writer = fi.CreateText())
            {
                var enu = sCollection.GetEnumerator();
                while (enu.MoveNext())
                    writer.WriteLine(enu.Current);
            }
        }

        /// <summary>
        ///     Saves a StringCollection to stream.
        /// </summary>
        /// <param name="sOutput">The output stream.</param>
        /// <param name="sCollection">The StringCollection.</param>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        public static void SaveToStream(this StringCollection sCollection, Stream sOutput)
        {
            using (var writer = new StreamWriter(sOutput))
            {
                writer.AutoFlush = true;
                var enu = sCollection.GetEnumerator();
                while (enu.MoveNext())
                    writer.WriteLine(enu.Current);
                writer.Flush();
            }
            //writer.Close(); // nezavirat!
        }

        /// <summary>
        ///     Saves StringCollection to a file.
        /// </summary>
        /// <param name="sCollection">The StringCollection.</param>
        /// <param name="sFile">The file.</param>
        /// <exception cref="SecurityException">The caller does not have the required permission. </exception>
        /// <exception cref="UnauthorizedAccessException">Access to fileName is denied. </exception>
        /// <exception cref="IOException">The disk is read-only. </exception>
        public static void SaveToFile(this ListView.ListViewItemCollection sCollection, string sFile)
        {
            var fi = new FileInfo(sFile);
            using (var writer = fi.CreateText())
            {
                foreach (ListViewItem item in sCollection)
                {
                    var s = new StringBuilder();
                    for (var i = 0; i < item.SubItems.Count; i++)
                    {
                        s.Append(item.SubItems[i]);
                        if (i < item.SubItems.Count - 1) s.Append(" -> ");
                        s = s.Replace("ListViewSubItem:", "");
                        s = s.Replace("}", "");
                        s = s.Replace("{", "");
                    }
                    //ListViewItemConverter k = new ListViewItemConverter()
                    //string s = k.ConvertToString(item)
                    writer.WriteLine(s.ToString());
                }
            }
        }

        /// <summary>
        ///     Loads StringCollection from file.
        /// </summary>
        /// <param name="sCollection">The StringCollection.</param>
        /// <param name="sFile">The file.</param>
        /// <param name="bAppend">if set to <c>true</c> [b append].</param>
        /// <exception cref="SecurityException">The caller does not have the required permission. </exception>
        /// <exception cref="UnauthorizedAccessException">Access to fileName is denied. </exception>
        /// <exception cref="FileNotFoundException">The file is not found. </exception>
        /// <exception cref="DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive. </exception>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        /// <exception cref="OutOfMemoryException">There is insufficient memory to allocate a buffer for the returned string. </exception>
        public static void LoadFromFile(StringCollection sCollection, string sFile, bool bAppend)
        {
            var souborInfo = new FileInfo(sFile);
            string s;
            if (!souborInfo.Exists) return;
            // načíty načíty :-)
            using (var reader = souborInfo.OpenText())
            {
                if (!bAppend) sCollection.Clear();
                while ((s = reader.ReadLine()) != null)
                    sCollection.Add(s);
                // načteno
            }
        }

        /// <summary>
        ///     Loads a StringCollection from a stream.
        /// </summary>
        /// <param name="sCollection">The StringCollection.</param>
        /// <param name="sInput">The input stream.</param>
        /// <param name="bAppend">if set to <c>true</c> [b append].</param>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        /// <exception cref="OutOfMemoryException">There is insufficient memory to allocate a buffer for the returned string. </exception>
        public static void LoadFromStream(this StringCollection sCollection, Stream sInput, bool bAppend)
        {
            using (var reader = new StreamReader(sInput))
            {
                string s;
                if (!bAppend) sCollection.Clear();
                while ((s = reader.ReadLine()) != null)
                    sCollection.Add(s);
            }
            // reader.Close(); // nezavirat!
        }

        /// <summary>
        ///     Loads a StringCollection from a file.
        /// </summary>
        /// <param name="sCollection">The StringCollection.</param>
        /// <param name="sFile">The input file.</param>
        /// <exception cref="SecurityException">The caller does not have the required permission. </exception>
        /// <exception cref="UnauthorizedAccessException">Access to fileName is denied. </exception>
        /// <exception cref="FileNotFoundException">The file is not found. </exception>
        /// <exception cref="DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive. </exception>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        /// <exception cref="OutOfMemoryException">There is insufficient memory to allocate a buffer for the returned string. </exception>
        public static void LoadFromFile(this StringCollection sCollection, string sFile)
        {
            LoadFromFile(sCollection, sFile, false);
        }

        /// <summary>
        ///     Saves a ListView to a file
        /// </summary>
        /// <param name="list">The listview.</param>
        /// <param name="sFile">The file.</param>
        /// <exception cref="UnauthorizedAccessException">Access is denied. </exception>
        /// <exception cref="DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive. </exception>
        /// <exception cref="IOException">
        ///     path includes an incorrect or invalid syntax for file name, directory name, or volume
        ///     label syntax.
        /// </exception>
        /// <exception cref="SecurityException">The caller does not have the required permission. </exception>
        public static void SaveToFile(this ListView list, string sFile)
        {
            var listViewContent = new StringBuilder();

            foreach (ListViewItem item in list.Items)
            {
                listViewContent.Append(item.Text);
                listViewContent.Append(Environment.NewLine);
            }
            using (var tw = new StreamWriter(sFile))
            {
                tw.WriteLine(listViewContent.ToString());
            }
        }
    }
}