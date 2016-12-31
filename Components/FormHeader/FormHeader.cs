using System;
using System.Drawing;
using System.Windows.Forms;

namespace ArachNGIN.Components.FormHeader
{
    /// <summary>
    ///     Class for drawing Form Header
    /// </summary>
    public class FormHeader : UserControl
    {
        /***************************************************************
            static properties
        ***************************************************************/

        /// <summary>
        ///     The default message font style
        /// </summary>
        public const FontStyle DefaultMessageFontStyle = FontStyle.Regular;

        /// <summary>
        ///     The default title font style
        /// </summary>
        public const FontStyle DefaultTitleFontStyle = FontStyle.Bold;

        /// <summary>
        ///     The default boundry size
        /// </summary>
        public const int DefaultBoundrySize = 15;

        private readonly string _drawTextWorkaroundAppendString = new string(' ', 10000) + ".";
        private int _iBoundrySize = DefaultBoundrySize;

        private Icon _icon;
        private Image _image;
        private Font _messageFont;
        private FontStyle _messageFontStyle = DefaultMessageFontStyle;
        private string _strMessage = string.Empty;
        private string _strTitle = string.Empty;
        private Point _textStartPoint = new Point(DefaultBoundrySize, DefaultBoundrySize);
        private Font _titleFont;
        private FontStyle _titleFontStyle = DefaultTitleFontStyle;

        /// <summary>
        ///     Initializes a new instance of the <see cref="FormHeader" /> class.
        /// </summary>
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

        /// <summary>
        ///     Gets or sets the message.
        /// </summary>
        /// <value>
        ///     The message.
        /// </value>
        public string Message
        {
            get { return _strMessage; }
            set
            {
                _strMessage = value;
                Invalidate();
            }
        }

        /// <summary>
        ///     Gets or sets the title.
        /// </summary>
        /// <value>
        ///     The title.
        /// </value>
        public string Title
        {
            get { return _strTitle; }
            set
            {
                _strTitle = value;
                Invalidate();
            }
        }

        /// <summary>
        ///     Gets or sets the icon.
        /// </summary>
        /// <value>
        ///     The icon.
        /// </value>
        public Icon Icon
        {
            get { return _icon; }
            set
            {
                _icon = value;
                Invalidate();
            }
        }

        /// <summary>
        ///     Gets or sets the image.
        /// </summary>
        /// <value>
        ///     The image.
        /// </value>
        public Image Image
        {
            get { return _image; }
            set
            {
                _image = value;
                Invalidate();
            }
        }

        /// <summary>
        ///     Gets or sets the title font style.
        /// </summary>
        /// <value>
        ///     The title font style.
        /// </value>
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

        /// <summary>
        ///     Gets or sets the message font style.
        /// </summary>
        /// <value>
        ///     The message font style.
        /// </value>
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

        /// <summary>
        ///     Gets or sets the size of the boundry.
        /// </summary>
        /// <value>
        ///     The size of the boundry.
        /// </value>
        public int BoundrySize
        {
            get { return _iBoundrySize; }
            set
            {
                _iBoundrySize = value;
                Invalidate();
            }
        }

        /// <summary>
        ///     Gets or sets the text start position.
        /// </summary>
        /// <value>
        ///     The text start position.
        /// </value>
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

        /// <summary>
        ///     Gets or sets the background image displayed in the control.
        /// </summary>
        /// <PermissionSet>
        ///     <IPermission
        ///         class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///         version="1" Unrestricted="true" />
        /// </PermissionSet>
        public new Image BackgroundImage
        {
            get { return null; }
        }

        /// <summary>
        ///     Gets or sets the edges of the container to which a control is bound and determines how a control is resized with
        ///     its parent.
        /// </summary>
        public new AnchorStyles Anchor
        {
            get { return AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top; }
        }

        //only allow black foregound and white background
        /// <summary>
        ///     Gets or sets the foreground color of the control.
        /// </summary>
        /// <PermissionSet>
        ///     <IPermission
        ///         class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///         version="1" Unrestricted="true" />
        /// </PermissionSet>
        public new Color ForeColor
        {
            get { return Color.Black; }
        }

        /// <summary>
        ///     Gets or sets the background color for the control.
        /// </summary>
        /// <PermissionSet>
        ///     <IPermission
        ///         class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///         version="1" Unrestricted="true" />
        /// </PermissionSet>
        public new Color BackColor
        {
            get { return Color.White; }
        }

        /***************************************************************
            drawing stuff
        ***************************************************************/

        /// <summary>
        ///     Creates the title font.
        /// </summary>
        protected void CreateTitleFont()
        {
            _titleFont = new Font(Font.FontFamily, Font.Size, _titleFontStyle);
        }

        /// <summary>
        ///     Creates the message font.
        /// </summary>
        protected void CreateMessageFont()
        {
            _messageFont = new Font(Font.FontFamily, Font.Size, _messageFontStyle);
        }

        /// <summary>
        ///     Draws the 3d line.
        /// </summary>
        /// <param name="g">The g.</param>
        protected void Draw3DLine(Graphics g)
        {
            ControlPaint.DrawBorder3D(g, 0, Height, Width, 0, Border3DStyle.RaisedInner);
        }

        /// <summary>
        ///     Draws the title.
        /// </summary>
        /// <param name="g">The g.</param>
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

        /// <summary>
        ///     Draws the message.
        /// </summary>
        /// <param name="g">The g.</param>
        protected void DrawMessage(Graphics g)
        {
            //calculate the new startpoint
            var iNewPosY = TextStartPosition.Y + Font.Height * 3 / 2;
            var iNewPosX = TextStartPosition.X + Font.Height * 3 / 2;
            var iTextBoxWidth = Width - iNewPosX;
            var iTextBoxHeight = Height - iNewPosY;

            if (_icon != null)
                iTextBoxWidth -= BoundrySize + _icon.Width;
            // subtract the width of the icon and the boundry size again
            else if (_image != null)
                iTextBoxWidth -= BoundrySize + _image.Width;
            // subtract the width of the icon and the boundry size again

            var rect = new Rectangle(iNewPosX, iNewPosY, iTextBoxWidth, iTextBoxHeight);
            g.DrawString(_strMessage, _messageFont, new SolidBrush(Color.Black), rect);
        }

        /// <summary>
        ///     Draws the image.
        /// </summary>
        /// <param name="g">The g.</param>
        protected void DrawImage(Graphics g)
        {
            if (_image == null)
                return;
            g.DrawImage(_image, Width - _image.Width - BoundrySize, (Height - _image.Height) / 2);
        }

        /// <summary>
        ///     Draws the icon.
        /// </summary>
        /// <param name="g">The g.</param>
        protected void DrawIcon(Graphics g)
        {
            if (_icon == null)
                return;
            g.DrawIcon(_icon, Width - _icon.Width - BoundrySize, (Height - _icon.Height) / 2);
        }

        /// <summary>
        ///     Draws the background.
        /// </summary>
        /// <param name="g">The g.</param>
        protected virtual void DrawBackground(Graphics g)
        {
            g.FillRectangle(new SolidBrush(BackColor), 0, 0, Width, Height);
        }

        /***************************************************************
            overridden methods
        ***************************************************************/

        /// <summary>
        ///     Raises the <see cref="E:System.Windows.Forms.Control.FontChanged" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnFontChanged(EventArgs e)
        {
            CreateTitleFont();
            base.OnFontChanged(e);
        }

        /// <summary>
        ///     Raises the <see cref="E:System.Windows.Forms.Control.Paint" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data.</param>
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
            base.OnPaint(e);
        }

        /// <summary>
        ///     Raises the <see cref="E:System.Windows.Forms.Control.SizeChanged" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
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