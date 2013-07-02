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
 * Time: 15:11
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ArachNGIN.Files.Streams
{
    /// <summary>
    /// Třída plná statických funkcí pro práci s řetězci
    /// </summary>
    public static class StringUtils
    {
        /// <summary>
        /// Funkce pro rozdělení řetězce na jednotlivá slova
        /// </summary>
        /// <param name="wholeString">celý řetězec</param>
        /// <param name="delimiter">oddělovač (nejspíš mezera)</param>
        /// <returns></returns>
        public static string[] StringSplit(string wholeString, string delimiter)
        {
            var r = new Regex("(" + delimiter + ")");
            string[] s = r.Split(wholeString);
            int iHalf = Convert.ToInt16((s.GetUpperBound(0)/2) + 1);
            var res = new string[iHalf];
            int j = 0;
            for (int i = 0; i <= s.GetUpperBound(0); i++)
            {
                if (s[i] != delimiter)
                {
                    res[j] = s[i];
                    j++;
                }
            }
            return res;
        }

        /// <summary>
        /// Přidá na konec stringu lomítko, když už tam není
        /// </summary>
        /// <param name="strString">nejlépe cesta např. c:\\abcd</param>
        /// <returns>cesta s lomítkem na konci např. c:\\abcd\\</returns>
        public static string StrAddSlash(string strString)
        {
            // zapamatovat si: lomítko je 0x5C!
            var s = strString;
            if (s[s.Length - 1] != (char) 0x5C) return s + (char) 0x5C;
            return s;
        }

        /// <summary>
        /// Zkontroluje jestli řetězec začíná lomítkem a případně ho odřízne
        /// </summary>
        /// <param name="strString">řetězec</param>
        /// <returns>řetězec bez lomítka na začátku</returns>
        public static string NoStartingSlash(string strString)
        {
            if(string.IsNullOrEmpty(strString)) return string.Empty;
            if (strString[0] == '\\') return strString.Substring(1);
            return strString;
        }

        /// <summary>
        /// Zkontroluje jestli řetězec končí lomítkem a případně ho odřízne
        /// </summary>
        /// <param name="strString">řetězec</param>
        /// <returns>řetězec bez lomítka na konci</returns>
        public static string NoEndingSlash(string strString)
        {
            if(string.IsNullOrEmpty(strString)) return string.Empty;
            var r = strString;
            if (strString[strString.Length - 1] == '\\') r = strString.Substring(0, strString.Length - 1);
            return r;
        }

        /// <summary>
        /// Odřízne lomítka na obou koncích řetězce
        /// </summary>
        /// <param name="strString">retězec</param>
        /// <returns>řetězec bez lomítek</returns>
        public static string NoSlashesOnEnds(string strString)
        {
            return NoEndingSlash(NoStartingSlash(strString));
        }

    /// <summary>
        /// Převede číslo na pole bytů
        /// </summary>
        /// <param name="x">číslo.</param>
        /// <returns>pole bytů</returns>
        public static byte[] UInt32ToBigEndianBytes(UInt32 x)
        {
            return new[]
                       {
                           (byte) ((x >> 24) & 0xff),
                           (byte) ((x >> 16) & 0xff),
                           (byte) ((x >> 8) & 0xff),
                           (byte) (x & 0xff)
                       };
        }

        /// <summary>
        /// Převede číslo na řetězec v hex formátu
        /// </summary>
        /// <param name="x">číslo</param>
        /// <returns>řetězec v hex formátu</returns>
        public static string UInt32ToByteString(UInt32 x)
        {
            byte[] tmp = UInt32ToBigEndianBytes(x);
            string s = string.Empty;
            foreach (byte b in tmp) s += b.ToString("x2");
            return s;
        }


        /// <summary>
        /// Převede pole bytů na řetězec v hex formátu
        /// </summary>
        /// <param name="x">číslo</param>
        /// <returns>řetězec v hex formátu</returns>
        public static string ByteArrayToString(byte[] x)
        {
            string s = string.Empty;
            if ((x != null) && (x.Length > 0))
            {
                foreach (byte b in x) s += b.ToString("x2");
            }
            return s;
        }

        /// <summary>
        /// Převede číslo na řetězec s příslušnou délkou,
        /// případně doplní na začátek nuly.
        /// </summary>
        /// <param name="number">číslo</param>
        /// <param name="length">délka požadovaného řetězce</param>
        /// <returns>řetězec, např. 000000123</returns>
        public static string PadNumToLength(int number, int length)
        {
            var result = number.ToString(CultureInfo.InvariantCulture);
            while (result.Length < length)
            {
                result = "0" + result;
            }
            return result;
        }

        /// <summary>
        /// Vloží do Treeview názvy souborů strukturované podle adresářů
        /// </summary>
        /// <param name="treeView">komponenta TreeView.</param>
        /// <param name="paths">pole s cestama. 
        /// new List&lt;string&gt; {"jedna cesta", "druha cesta"}
        /// </param>
        /// <param name="pathSeparator">The path separator.</param>
        public static void PopulateTreeViewByFiles(TreeView treeView, IEnumerable<string> paths, char pathSeparator)
        {
            TreeNode lastNode = null;
            foreach (string path in paths)
            {
                lastNode = LastNode(treeView, pathSeparator, path, null);
            }
        }

        private static TreeNode LastNode(TreeView treeView, char pathSeparator, string path, TreeNode lastNode)
        {
            string subPathAgg = string.Empty;
            foreach (string subPath in path.Split(pathSeparator))
            {
                subPathAgg += subPath + pathSeparator;
                TreeNode[] nodes = treeView.Nodes.Find(subPathAgg, true);
                if (nodes.Length == 0)
                    if (lastNode == null)
                        lastNode = treeView.Nodes.Add(subPathAgg, subPath);
                    else
                        lastNode = lastNode.Nodes.Add(subPathAgg, subPath);
                else
                    lastNode = nodes[0];
            }
            return lastNode;
        }
    }
}