using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace ArachNGIN.Components.FormHeader
{
    public class ColorSlideFormHeader : FormHeader
    {
        public static readonly Color DefaultColor1 = Color.White;
        public static readonly Color DefaultColor2 = Color.White;

        private Color _color1 = DefaultColor1;
        private Color _color2 = DefaultColor2;
        private Image _image;

        public ColorSlideFormHeader()
        {
            CreateBackgroundPicture();
        }


        public Color Color1
        {
            get { return _color1; }
            set
            {
                _color1 = value;
                CreateBackgroundPicture();
                Invalidate();
            }
        }


        public Color Color2
        {
            get { return _color2; }
            set
            {
                _color2 = value;
                CreateBackgroundPicture();
                Invalidate();
            }
        }

        protected virtual void CreateBackgroundPicture()
        {
            try
            {
                _image = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);
            }
            catch
            {
                return;
            }

            Graphics gfx = Graphics.FromImage(_image);

            if (_color1.Equals(_color2)) //check if we need to calc the color slide
            {
                gfx.FillRectangle(new SolidBrush(_color1), 0, 0, Width, Height);
            }
            else
            {
                for (int i = 0; i < _image.Width; i++)
                {
                    //
                    // calculate the new color to use (linear color mix)
                    //
                    int colorR = ((Color2.R - Color1.R))*i/_image.Width;
                    int colorG = ((Color2.G - Color1.G))*i/_image.Width;
                    int colorB = ((Color2.B - Color1.B))*i/_image.Width;
                    Color color = Color.FromArgb(Color1.R + colorR, Color1.G + colorG, Color1.B + colorB);

                    gfx.DrawLine(new Pen(new SolidBrush(color)), i, 0, i, Height);
                }
            }
        }


        protected override void DrawBackground(Graphics g)
        {
            g.DrawImage(_image, 0, 0);
        }


        protected override void OnSizeChanged(EventArgs e)
        {
            CreateBackgroundPicture();
            base.OnSizeChanged(e);
            Invalidate();
        }
    }
}