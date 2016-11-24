using System.Drawing;
using System.Windows.Forms;

namespace ArachNGIN.Components.FormHeader
{
    /// <summary>
    ///     Class for drawing Form Header Image
    /// </summary>
    public class ImageFormHeader : FormHeader
    {
        private Image _backgroundImage;

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
            get { return _backgroundImage; }
            set
            {
                _backgroundImage = value;
                Invalidate();
            }
        }

        /// <summary>
        ///     Draws the background image.
        /// </summary>
        /// <param name="g">The g.</param>
        protected void DrawBackgroundImage(Graphics g)
        {
            if (_backgroundImage == null)
                return;
            g.DrawImage(_backgroundImage, 0, 0);
        }

        /// <summary>
        ///     Raises the <see cref="E:System.Windows.Forms.Control.Paint" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data.</param>
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
            base.OnPaint(e);
        }

        /// <summary>
        ///     Initializes the component.
        /// </summary>
        private void InitializeComponent()
        {
            SuspendLayout();
            //
            // ImageFormHeader
            //
            Name = "ImageFormHeader";
            Size = new Size(1502, 70);
            ResumeLayout(false);
        }
    }
}