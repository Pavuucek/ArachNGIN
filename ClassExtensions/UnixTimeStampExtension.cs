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

using System;

namespace ArachNGIN.ClassExtensions
{
    /// <summary>
    ///     Extension class for converting Unix Timestamps to double and back
    /// </summary>
    public static class UnixTimeStampExtension
    {
        /// <summary>
        ///     Convert any date to unix timestamp (double).
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns></returns>
        public static double ToUnixTimestampDouble(this DateTime dateTime)
        {
            return (TimeZoneInfo.ConvertTimeToUtc(dateTime) -
                    new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        }

        /// <summary>
        ///     Converts unix timestamp (double) to unix timestamp
        /// </summary>
        /// <param name="unixTimeStamp">The unix time stamp.</param>
        /// <returns></returns>
        public static DateTime UnixTimestampToDateTime(this double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        /// <summary>
        ///     Converts unix timestamp (int) to unix timestamp
        /// </summary>
        /// <param name="unixTimeStamp">The unix time stamp.</param>
        /// <returns></returns>
        public static DateTime UnixTimestampToDateTime(this int unixTimeStamp)
        {
            return UnixTimestampToDateTime(Convert.ToDouble(unixTimeStamp));
        }

        /// <summary>
        ///     Convert any date to unix timestamp (int).
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns></returns>
        public static int ToUnixTimestampInt(this DateTime dateTime)
        {
            return Convert.ToInt32(ToUnixTimestampDouble(dateTime));
        }
    }
}