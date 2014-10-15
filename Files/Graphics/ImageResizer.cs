/*
 * Copyright (c) 2006-2014 Michal Kuncl <michal.kuncl@gmail.com> http://www.pavucina.info
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
 * 
 */

using System.Drawing;

namespace ArachNGIN.Files.Graphics
{
    /// <summary>
    ///     Image resizing class
    /// </summary>
    public static class ImageResizer
    {
        /// <summary>
        ///     Resizes an image and preserves size ratios
        /// </summary>
        /// <param name="img">an image</param>
        /// <param name="maxWidth">max width</param>
        /// <param name="maxHeight">max height</param>
        public static void ResizeImage(ref Image img, double maxWidth, double maxHeight)
        {
            double srcWidth = img.Width;
            double srcHeight = img.Height;

            double resizeWidth = srcWidth;
            double resizeHeight = srcHeight;

            double aspect = resizeWidth/resizeHeight;

            if (resizeWidth > maxWidth)
            {
                resizeWidth = maxWidth;
                resizeHeight = resizeWidth/aspect;
            }
            if (resizeHeight > maxHeight)
            {
                aspect = resizeWidth/resizeHeight;
                resizeHeight = maxHeight;
                resizeWidth = resizeHeight*aspect;
            }

            img = new Bitmap(img, (int) resizeWidth, (int) resizeHeight);
        }
    }
}