using System;
using System.Drawing;
using System.Drawing.Imaging;
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
		public static readonly FontStyle DefaultMessageFontStyle = FontStyle.Regular;
		public static readonly FontStyle DefaultTitleFontStyle = FontStyle.Bold;
		public static readonly int DefaultBoundrySize = 15;


		private String _strTitle = String.Empty;
		private String _strMessage = String.Empty;
		private Font _messageFont;
		private Font _titleFont;
		private Icon _icon = null;
		private Image _image = null;
		private int _iBoundrySize = DefaultBoundrySize;
		private FontStyle _titleFontStyle = DefaultTitleFontStyle;
		private FontStyle _messageFontStyle = DefaultMessageFontStyle;
		private string _drawTextWorkaroundAppendString = new string(' ', 10000) + ".";
		private Point _textStartPoint = new Point(DefaultBoundrySize, DefaultBoundrySize);

		public FormHeader()
		{
			this.Size = new Size(10, 70); //header height of 70 does not look bad
			this.Dock = DockStyle.Top;
			this.CreateTitleFont();
			this.CreateMessageFont();
		}


		/***************************************************************
			public properties
		***************************************************************/

		public String Message
		{
			get
			{
				return _strMessage;
			}
			set
			{
				_strMessage = value;
				Invalidate();
			}
		}

		public String Title
		{
			get
			{
				return _strTitle;
			}
			set
			{
				_strTitle = value;
				Invalidate();
			}
		}

		public Icon Icon
		{
			get
			{
				return this._icon;
			}
			set
			{
				this._icon = value;
				Invalidate();
			}
		}

		public Image Image
		{
			get
			{
				return this._image;
			}
			set
			{
				this._image = value;
				Invalidate();
			}
		}

		public FontStyle TitleFontStyle
		{
			get
			{
				return this._titleFontStyle;
			}
			set
			{
				this._titleFontStyle = value;
				this.CreateTitleFont();
				Invalidate();
			}
		}

		public FontStyle MessageFontStyle
		{
			get
			{
				return this._messageFontStyle;
			}
			set
			{
				this._messageFontStyle = value;
				this.CreateMessageFont();
				Invalidate();
			}
		}

		public int BoundrySize
		{
			get
			{
				return _iBoundrySize;
			}
			set
			{
				_iBoundrySize = value;
				Invalidate();
			}
		}

		public Point TextStartPosition
		{
			get
			{
				return _textStartPoint;
			}
			set
			{
				_textStartPoint = value;
				Invalidate();
			}
		}


		/***************************************************************
			newly implemented/overridden public properties
		***************************************************************/

		new public Image BackgroundImage
		{
			get
			{
				return null;
			}
		}

		new public AnchorStyles Anchor
		{
			get
			{
				return AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
			}
		}

		//only allow black foregound and white background
		new public Color ForeColor
		{
			get
			{
				return Color.Black;
			}
		}

		new public Color BackColor
		{
			get
			{
				return Color.White;
			}
		}

		/***************************************************************
			drawing stuff
		***************************************************************/

		protected void CreateTitleFont()
		{
			this._titleFont = new Font(this.Font.FontFamily, this.Font.Size, this._titleFontStyle);
		}

		protected void CreateMessageFont()
		{
			this._messageFont = new Font(this.Font.FontFamily, this.Font.Size, this._messageFontStyle);
		}

		protected void Draw3dLine(Graphics g)
		{
			ControlPaint.DrawBorder3D(g, 0, this.Height, this.Width, 0, Border3DStyle.RaisedInner);
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
			g.DrawString(this._strTitle + _drawTextWorkaroundAppendString, this._titleFont, new SolidBrush(Color.Black), this.TextStartPosition);
		}

		protected void DrawMessage(Graphics g)
		{
			//calculate the new startpoint
			int iNewPosY = this.TextStartPosition.Y + this.Font.Height*3/2;
			int iNewPosX = this.TextStartPosition.X + this.Font.Height*3/2;
			int iTextBoxWidth = this.Width - iNewPosX;
			int iTextBoxHeight = this.Height - iNewPosY;

			if (this._icon != null)
				iTextBoxWidth -= (BoundrySize + _icon.Width); // subtract the width of the icon and the boundry size again
			else if (this._image != null)
				iTextBoxWidth -= (BoundrySize + _image.Width); // subtract the width of the icon and the boundry size again

			Rectangle rect = new Rectangle(iNewPosX, iNewPosY, iTextBoxWidth, iTextBoxHeight);
			g.DrawString(this._strMessage, this._messageFont, new SolidBrush(Color.Black), rect);
		}

		protected void DrawImage(Graphics g)
		{
			if (this._image == null)
				return;
			g.DrawImage(this._image, this.Width - this._image.Width - BoundrySize, (this.Height - this._image.Height)/2);
		}

		protected void DrawIcon(Graphics g)
		{
			if (this._icon == null)
				return;
			g.DrawIcon(this._icon, this.Width - this._icon.Width - BoundrySize, (this.Height - this._icon.Height)/2);
		}

		protected virtual void DrawBackground(Graphics g)
		{
			g.FillRectangle(new SolidBrush(this.BackColor), 0, 0, this.Width, this.Height);
		}


		/***************************************************************
			overridden methods
		***************************************************************/

		protected override void OnFontChanged(EventArgs e)
		{
			this.CreateTitleFont();
			base.OnFontChanged(e);
		}


		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
		}


		protected override void OnPaint(PaintEventArgs e)
		{
			this.DrawBackground(e.Graphics);
			this.Draw3dLine(e.Graphics);
			this.DrawTitle(e.Graphics);
			this.DrawMessage(e.Graphics);
			if (this._icon != null)
				this.DrawIcon(e.Graphics);
			else if (this._image != null)
				this.DrawImage(e.Graphics);
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

	public class ColorSlideFormHeader : FormHeader
	{
		public static readonly Color DefaultColor1 = Color.White;
		public static readonly Color DefaultColor2 = Color.White;

		private Color _color1 = DefaultColor1;
		private Color _color2 = DefaultColor2;
		private Image _image = null;

		public ColorSlideFormHeader()
		{
			CreateBackgroundPicture();
		}


		protected virtual void CreateBackgroundPicture()
		{
			try
			{
				_image = new Bitmap(this.Width, this.Height, PixelFormat.Format24bppRgb);
			}
			catch
			{
				return;
			}

			Graphics gfx = Graphics.FromImage(_image);

			if (this._color1.Equals(this._color2)) //check if we need to calc the color slide
			{
				gfx.FillRectangle(new SolidBrush(this._color1), 0, 0, this.Width, this.Height);
			}
			else
			{
				for (int i = 0; i < _image.Width; i++)
				{
					//
					// calculate the new color to use (linear color mix)
					//
					int colorR = ((int) (this.Color2.R - this.Color1.R))*i/_image.Width;
					int colorG = ((int) (this.Color2.G - this.Color1.G))*i/_image.Width;
					int colorB = ((int) (this.Color2.B - this.Color1.B))*i/_image.Width;
					Color color = Color.FromArgb(this.Color1.R + colorR, this.Color1.G + colorG, this.Color1.B + colorB);

					gfx.DrawLine(new Pen(new SolidBrush(color)), i, 0, i, this.Height);
				}
			}
		}


		public Color Color1
		{
			get
			{
				return this._color1;
			}
			set
			{
				this._color1 = value;
				CreateBackgroundPicture();
				Invalidate();
			}
		}


		public Color Color2
		{
			get
			{
				return this._color2;
			}
			set
			{
				this._color2 = value;
				CreateBackgroundPicture();
				Invalidate();
			}
		}


		protected override void DrawBackground(Graphics g)
		{
			g.DrawImage(this._image, 0, 0);
		}


		protected override void OnSizeChanged(EventArgs e)
		{
			CreateBackgroundPicture();
			base.OnSizeChanged(e);
			Invalidate();
		}
	}

	public class ImageFormHeader : FormHeader
	{
		private Image _backgroundImage;


		new public Image BackgroundImage
		{
			get
			{
				return this._backgroundImage;
			}
			set
			{
				this._backgroundImage = value;
				Invalidate();
			}
		}

		protected void DrawBackgroundImage(Graphics g)
		{
			if (this._backgroundImage == null)
				return;
			g.DrawImage(this._backgroundImage, 0, 0);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			this.DrawBackground(e.Graphics);

			this.DrawBackgroundImage(e.Graphics);
			this.Draw3dLine(e.Graphics);
			this.DrawTitle(e.Graphics);
			this.DrawMessage(e.Graphics);
			if (this.Icon != null)
				this.DrawIcon(e.Graphics);
			else if (this.Image != null)
				this.DrawImage(e.Graphics);
		}
	}
}