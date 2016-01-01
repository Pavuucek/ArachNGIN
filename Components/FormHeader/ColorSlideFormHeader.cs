using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace ArachNGIN.Components.FormHeader
{
    /// <summary>
    ///     Class for drawing Form Header as Color slide
    /// </summary>
    public class ColorSlideFormHeader : FormHeader
    {
        /// <summary>
        ///     The default color1
        /// </summary>
        public static readonly Color DefaultColor1 = Color.White;

        /// <summary>
        ///     The default color2
        /// </summary>
        public static readonly Color DefaultColor2 = Color.White;

        private Color _color1 = DefaultColor1;
        private Color _color2 = DefaultColor2;
        private Image _image;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ColorSlideFormHeader" /> class.
        /// </summary>
        /// <exception cref="Exception">image has an indexed pixel format or its format is undefined.</exception>
        /// <exception cref="ArgumentNullException">image is null.</exception>
        public ColorSlideFormHeader()
        {
            CreateBackgroundPicture();
        }

        /// <summary>
        ///     Gets or sets the color1.
        /// </summary>
        /// <value>
        ///     The color1.
        /// </value>
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

        /// <summary>
        ///     Gets or sets the color2.
        /// </summary>
        /// <value>
        ///     The color2.
        /// </value>
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

        /// <summary>
        ///     Creates the background picture.
        /// </summary>
        /// <exception cref="ArgumentNullException">image is null.</exception>
        /// <exception cref="Exception">image has an indexed pixel format or its format is undefined.</exception>
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

            var gfx = Graphics.FromImage(_image);

            if (_color1.Equals(_color2)) //check if we need to calc the color slide
            {
                gfx.FillRectangle(new SolidBrush(_color1), 0, 0, Width, Height);
            }
            else
            {
                for (var i = 0; i < _image.Width; i++)
                {
                    //
                    // calculate the new color to use (linear color mix)
                    //
                    var colorR = (Color2.R - Color1.R) * i / _image.Width;
                    var colorG = (Color2.G - Color1.G) * i / _image.Width;
                    var colorB = (Color2.B - Color1.B) * i / _image.Width;
                    var color = Color.FromArgb(Color1.R + colorR, Color1.G + colorG, Color1.B + colorB);

                    gfx.DrawLine(new Pen(new SolidBrush(color)), i, 0, i, Height);
                }
            }
        }

        /// <summary>
        ///     Draws the background.
        /// </summary>
        /// <param name="g">The g.</param>
        protected override void DrawBackground(Graphics g)
        {
            g.DrawImage(_image, 0, 0);
        }

        /// <summary>
        ///     Raises the <see cref="E:System.Windows.Forms.Control.SizeChanged" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        /// <exception cref="ArgumentNullException">image is null.</exception>
        /// <exception cref="Exception">image has an indexed pixel format or its format is undefined.</exception>
        protected override void OnSizeChanged(EventArgs e)
        {
            CreateBackgroundPicture();
            base.OnSizeChanged(e);
            Invalidate();
        }

        /// <summary>
        ///     Initializes the component.
        /// </summary>
        private void InitializeComponent()
        {
            SuspendLayout();
            //
            // ColorSlideFormHeader
            //
            Name = "ColorSlideFormHeader";
            Size = new Size(1502, 70);
            ResumeLayout(false);
        }
    }
}