using System.Drawing;

namespace ArachNGIN.Files.Graphics
{
    /// <summary>
    /// Třída na měnění velikosti obrázků
    /// </summary>
    public static class ImageResizer
    {
        /// <summary>
        /// Změní velikost obrázku a dodrží poměr stran
        /// </summary>
        /// <param name="img">obrázek</param>
        /// <param name="maxWidth">max šířka</param>
        /// <param name="maxHeight">max výška</param>
        public static void ResizeImage(Image img, double maxWidth, double maxHeight)
        {
            double srcWidth = img.Width;
            double srcHeight = img.Height;

            double resizeWidth = srcWidth;
            double resizeHeight = srcHeight;

            double aspect = resizeWidth / resizeHeight;

            if (resizeWidth > maxWidth)
            {
                resizeWidth = maxWidth;
                resizeHeight = resizeWidth / aspect;
            }
            if (resizeHeight > maxHeight)
            {
                aspect = resizeWidth / resizeHeight;
                resizeHeight = maxHeight;
                resizeWidth = resizeHeight * aspect;
            }

            img = new Bitmap(img, (int) resizeWidth, (int) resizeHeight);
        }
    }
}
