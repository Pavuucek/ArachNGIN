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
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ArachNGIN.Files.Streams
{
    /// <summary>
    ///     Class for working with Strings
    /// </summary>
    public static class StringUtils
    {
        /// <summary>
        ///     Splits a string.
        /// </summary>
        /// <param name="wholeString">The whole string.</param>
        /// <param name="delimiter">The delimiter.</param>
        /// <returns></returns>
        public static string[] StringSplit(string wholeString, string delimiter)
        {
            var r = new Regex("(" + delimiter + ")");
            string[] s = r.Split(wholeString);
            int iHalf = Convert.ToInt16((s.GetUpperBound(0) / 2) + 1);
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
        ///     Converts an integer to big endian byte array
        /// </summary>
        /// <param name="x">The integer</param>
        /// <returns></returns>
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
        ///     Converts an integer to byte string
        /// </summary>
        /// <param name="x">The integer</param>
        /// <returns></returns>
        public static string UInt32ToByteString(UInt32 x)
        {
            byte[] tmp = UInt32ToBigEndianBytes(x);
            string s = string.Empty;
            foreach (byte b in tmp) s += b.ToString("x2");
            return s;
        }

        /// <summary>
        ///     Converts a number to a string of specified length. Pads the rest with zeroes.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public static string PadNumToLength(int number, int length)
        {
            string result = number.ToString(CultureInfo.InvariantCulture);
            while (result.Length < length)
            {
                result = "0" + result;
            }
            return result;
        }

        /// <summary>
        ///     Populates the TreeView by files.
        /// </summary>
        /// <param name="treeView">The tree view.</param>
        /// <param name="paths">The paths.</param>
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