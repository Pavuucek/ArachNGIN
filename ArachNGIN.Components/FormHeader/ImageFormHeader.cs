using System.Drawing;
using System.Windows.Forms;

namespace ArachNGIN.Components.FormHeader
{
    public class ImageFormHeader : FormHeader
    {
        private Image _backgroundImage;


        public new Image BackgroundImage
        {
            get { return _backgroundImage; }
            set
            {
                _backgroundImage = value;
                Invalidate();
            }
        }

        protected void DrawBackgroundImage(Graphics g)
        {
            if (_backgroundImage == null)
                return;
            g.DrawImage(_backgroundImage, 0, 0);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            DrawBackground(e.Graphics);

            DrawBackgroundImage(e.Graphics);
            Draw3DLine(e.Graphics);
            DrawTitle(e.Graphics);
            DrawMessage(e.Graphics);
            if (Icon != null)
                DrawIcon(e.Graphics);
            else if (Image != null)
                DrawImage(e.Graphics);
        }
    }
}