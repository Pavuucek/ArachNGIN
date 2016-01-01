using System;

namespace ArachNGIN.Files.ClassExtensions
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