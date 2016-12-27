using System;
using System.Drawing;

namespace ArachNGIN.ClassExtensions
{
    /// <summary>
    ///     Extension class to convert strings to named colors and vice versa.
    /// </summary>
    public static class StringColors
    {
        /// <summary>
        ///     Gets color from string. Defaults to black
        /// </summary>
        /// <param name="strColorName">String containing color name.</param>
        /// <returns>Color</returns>
        public static Color FromString(this string strColorName)
        {
            if (string.IsNullOrWhiteSpace(strColorName))
                strColorName = "Black";

            KnownColor knownColor;

            if (Enum.TryParse(strColorName, out knownColor))
                return Color.FromKnownColor(knownColor);

            return ColorTranslator.FromHtml(strColorName);
        }
    }
}