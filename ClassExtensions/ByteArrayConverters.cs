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

using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace ArachNGIN.ClassExtensions
{
    /// <summary>
    ///     Converts images from byte arrays and back
    /// </summary>
    public static class ByteArrayConverters
    {
        /// <summary>
        ///     Converts image to a byte array
        /// </summary>
        /// <param name="imageIn">input image</param>
        /// <param name="imageFormat">output image format</param>
        /// <returns>byte array</returns>
        public static byte[] ImageToByteArray(this Image imageIn, ImageFormat imageFormat)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, imageFormat);
                return ms.ToArray();
            }
        }

        /// <summary>
        ///     Converts byte array back to image
        /// </summary>
        /// <param name="byteArrayIn">byte array</param>
        /// <returns>image</returns>
        public static Image ByteArrayToImage(this byte[] byteArrayIn)
        {
            Image returnImage;
            using (var ms = new MemoryStream(byteArrayIn))
            {
                returnImage = Image.FromStream(ms);
            }
            return returnImage;
        }
    }
}