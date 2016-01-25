using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArachNGIN.ClassExtensions
{
    /// <summary>
    /// Class extensions for using on string type objects
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
            string s = strString;
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
            string r = strString;
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