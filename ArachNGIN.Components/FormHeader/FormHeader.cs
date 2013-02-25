using System;
using System.Drawing;
using System.Windows.Forms;

namespace ArachNGIN.Components.FormHeader
{
    /*******************************************************************************************************************************

	*******************************************************************************************************************************/

    public class FormHeader : UserControl
    {
        /***************************************************************
			static properties
		***************************************************************/
        public const FontStyle DefaultMessageFontStyle = FontStyle.Regular;
        public const FontStyle DefaultTitleFontStyle = FontStyle.Bold;
        public const int DefaultBoundrySize = 15;
        private readonly string _drawTextWorkaroundAppendString = new string(' ', 10000) + ".";
        private int _iBoundrySize = DefaultBoundrySize;


        private Icon _icon;
        private Image _image;
        private Font _messageFont;
        private FontStyle _messageFontStyle = DefaultMessageFontStyle;
        private String _strMessage = String.Empty;
        private String _strTitle = String.Empty;
        private Point _textStartPoint = new Point(DefaultBoundrySize, DefaultBoundrySize);
        private Font _titleFont;
        private FontStyle _titleFontStyle = DefaultTitleFontStyle;

        public FormHeader()
        {
            Size = new Size(10, 70); //header height of 70 does not look bad
            Dock = DockStyle.Top;
            CreateTitleFont();
            CreateMessageFont();
        }


        /***************************************************************
			public properties
		***************************************************************/

        public String Message
        {
            get { return _strMessage; }
            set
            {
                _strMessage = value;
                Invalidate();
            }
        }

        public String Title
        {
            get { return _strTitle; }
            set
            {
                _strTitle = value;
                Invalidate();
            }
        }

        public Icon Icon
        {
            get { return _icon; }
            set
            {
                _icon = value;
                Invalidate();
            }
        }

        public Image Image
        {
            get { return _image; }
            set
            {
                _image = value;
                Invalidate();
            }
        }

        public FontStyle TitleFontStyle
        {
            get { return _titleFontStyle; }
            set
            {
                _titleFontStyle = value;
                CreateTitleFont();
                Invalidate();
            }
        }

        public FontStyle MessageFontStyle
        {
            get { return _messageFontStyle; }
            set
            {
                _messageFontStyle = value;
                CreateMessageFont();
                Invalidate();
            }
        }

        public int BoundrySize
        {
            get { return _iBoundrySize; }
            set
            {
                _iBoundrySize = value;
                Invalidate();
            }
        }

        public Point TextStartPosition
        {
            get { return _textStartPoint; }
            set
            {
                _textStartPoint = value;
                Invalidate();
            }
        }


        /***************************************************************
			newly implemented/overridden public properties
		***************************************************************/

        public new Image BackgroundImage
        {
            get { return null; }
        }

        public new AnchorStyles Anchor
        {
            get { return AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top; }
        }

        //only allow black foregound and white background
        public new Color ForeColor
        {
            get { return Color.Black; }
        }

        public new Color BackColor
        {
            get { return Color.White; }
        }

        /***************************************************************
			drawing stuff
		***************************************************************/

        protected void CreateTitleFont()
        {
            _titleFont = new Font(Font.FontFamily, Font.Size, _titleFontStyle);
        }

        protected void CreateMessageFont()
        {
            _messageFont = new Font(Font.FontFamily, Font.Size, _messageFontStyle);
        }

        protected void Draw3DLine(Graphics g)
        {
            ControlPaint.DrawBorder3D(g, 0, Height, Width, 0, Border3DStyle.RaisedInner);
        }

        protected void DrawTitle(Graphics g)
        {
            // Normally the next line should work fine
            // but the spacing of the characters at the end of the string is smaller than at the beginning
            // therefore we add _drawTextWorkaroundAppendString to the string to be drawn
            // this works fine
            //
            // i reported this behaviour to microsoft. they confirmed this is a bug in GDI+.
            //
            //			g.DrawString( this._strTitle, this._titleFont, new SolidBrush(Color.Black), BoundrySize, BoundrySize); //BoundrySize is used as the x & y coords
            g.DrawString(_strTitle + _drawTextWorkaroundAppendString, _titleFont, new SolidBrush(Color.Black),
                         TextStartPosition);
        }

        protected void DrawMessage(Graphics g)
        {
            //calculate the new startpoint
            int iNewPosY = TextStartPosition.Y + Font.Height*3/2;
            int iNewPosX = TextStartPosition.X + Font.Height*3/2;
            int iTextBoxWidth = Width - iNewPosX;
            int iTextBoxHeight = Height - iNewPosY;

            if (_icon != null)
                iTextBoxWidth -= (BoundrySize + _icon.Width);
                    // subtract the width of the icon and the boundry size again
            else if (_image != null)
                iTextBoxWidth -= (BoundrySize + _image.Width);
                    // subtract the width of the icon and the boundry size again

            var rect = new Rectangle(iNewPosX, iNewPosY, iTextBoxWidth, iTextBoxHeight);
            g.DrawString(_strMessage, _messageFont, new SolidBrush(Color.Black), rect);
        }

        protected void DrawImage(Graphics g)
        {
            if (_image == null)
                return;
            g.DrawImage(_image, Width - _image.Width - BoundrySize, (Height - _image.Height)/2);
        }

        protected void DrawIcon(Graphics g)
        {
            if (_icon == null)
                return;
            g.DrawIcon(_icon, Width - _icon.Width - BoundrySize, (Height - _icon.Height)/2);
        }

        protected virtual void DrawBackground(Graphics g)
        {
            g.FillRectangle(new SolidBrush(BackColor), 0, 0, Width, Height);
        }


        /***************************************************************
			overridden methods
		***************************************************************/

        protected override void OnFontChanged(EventArgs e)
        {
            CreateTitleFont();
            base.OnFontChanged(e);
        }


        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            DrawBackground(e.Graphics);
            Draw3DLine(e.Graphics);
            DrawTitle(e.Graphics);
            DrawMessage(e.Graphics);
            if (_icon != null)
                DrawIcon(e.Graphics);
            else if (_image != null)
                DrawImage(e.Graphics);
        }


        protected override void OnSizeChanged(EventArgs e)
        {
            Invalidate();
            base.OnSizeChanged(e);
        }
    }


    /*******************************************************************************************************************************
		ColorSlideFormHeader is an extended version of the FormHeader class
		It also provides the functionality of a color slide of the background image
	*******************************************************************************************************************************/
}