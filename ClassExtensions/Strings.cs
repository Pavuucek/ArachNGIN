/*
 * Copyright (c) 2006-2016 Michal Kuncl <michal.kuncl@gmail.com> http://www.pavucina.info
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

namespace ArachNGIN.ClassExtensions
{
    /// <summary>
    ///     Class extensions for using on string type objects
    /// </summary>
    public static class Strings
    {
        /// <summary>
        ///     Adds a slash to the end of a string (if it's not already there)
        /// </summary>
        /// <param name="strString">The string.</param>
        /// <returns></returns>
        public static string AddSlash(this string strString)
        {
            // zapamatovat si: lomítko je 0x5C!
            var s = strString;
            if (s[s.Length - 1] != (char)0x5C) return s + (char)0x5C;
            return s;
        }

        /// <summary>
        ///     Checks if a string starts with a slash. If it does it removes it.
        /// </summary>
        /// <param name="strString">The string.</param>
        /// <returns></returns>
        public static string NoStartingSlash(this string strString)
        {
            if (string.IsNullOrEmpty(strString)) return string.Empty;
            if (strString[0] == '\\') return strString.Substring(1);
            return strString;
        }

        /// <summary>
        ///     Checks if a string ends with a slash. if it does it removes it.
        /// </summary>
        /// <param name="strString">The string.</param>
        /// <returns></returns>
        public static string NoEndingSlash(this string strString)
        {
            if (string.IsNullOrEmpty(strString)) return string.Empty;
            var r = strString;
            if (strString[strString.Length - 1] == '\\') r = strString.Substring(0, strString.Length - 1);
            return r;
        }

        /// <summary>
        ///     Deletes slashes on both ends of a string
        /// </summary>
        /// <param name="strString">The string.</param>
        /// <returns></returns>
        public static string NoSlashesOnEnds(this string strString)
        {
            return strString.NoEndingSlash().NoStartingSlash();
        }
    }
}