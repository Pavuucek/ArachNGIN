using ArachNGIN.ClassExtensions;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ArachNGIN.Components.ColorComboBox
{
    /// <summary>
    ///     A ComboBox component for selecting a color
    /// </summary>
    public class ColorComboBox : ComboBox
    {
        private readonly bool _bHideText;
        private readonly SolidBrush _blackBrush;
        private readonly SolidBrush _whiteBrush;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ColorComboBox" /> class.
        /// </summary>
        public ColorComboBox()
        {
            _blackBrush = new SolidBrush(Color.Black);
            _whiteBrush = new SolidBrush(Color.White);

            DrawMode = DrawMode.OwnerDrawFixed;
            Items.Clear();

            DrawItem += OnDrawItem;
            SelectedIndexChanged += OnSelectedIndexChanged;
            DropDown += OnDropDown;

            _bHideText = true;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ColorComboBox" /> class.
        /// </summary>
        /// <param name="hideText">if set to <c>true</c> [hide text].</param>
        public ColorComboBox(bool hideText) : this()
        {
            _bHideText = hideText;
        }

        /// <summary>
        ///     Gets or sets the color of the selected.
        /// </summary>
        /// <value>
        ///     The color of the selected.
        /// </value>
        public Color SelectedColor
        {
            get { return BackColor; }
            set { BackColor = value; }
        }

        /// <summary>
        ///     Initializes the items.
        /// </summary>
        public void InitItems()
        {
            Items.Add(Color.AliceBlue.Name);
            Items.Add(Color.AntiqueWhite.Name);
            Items.Add(Color.Aqua.Name);
            Items.Add(Color.Aquamarine.Name);
            Items.Add(Color.Azure.Name);
            Items.Add(Color.Black.Name);
            Items.Add(Color.BlanchedAlmond.Name);
            Items.Add(Color.Blue.Name);
            Items.Add(Color.BlueViolet.Name);
            Items.Add(Color.Brown.Name);
            Items.Add(Color.BurlyWood.Name);
            Items.Add(Color.CadetBlue.Name);
            Items.Add(Color.Chartreuse.Name);
            Items.Add(Color.Chocolate.Name);
            Items.Add(Color.Coral.Name);
            Items.Add(Color.CornflowerBlue.Name);
            Items.Add(Color.Cornsilk.Name);
            Items.Add(Color.Crimson.Name);
            Items.Add(Color.Cyan.Name);
            Items.Add(Color.DarkBlue.Name);
            Items.Add(Color.DarkCyan.Name);
            Items.Add(Color.DarkGoldenrod.Name);
            Items.Add(Color.DarkGray.Name);
            Items.Add(Color.DarkGreen.Name);
            Items.Add(Color.DarkKhaki.Name);
            Items.Add(Color.DarkMagenta.Name);
            Items.Add(Color.DarkOliveGreen.Name);
            Items.Add(Color.DarkOrange.Name);
            Items.Add(Color.DarkOrchid.Name);
            Items.Add(Color.DarkRed.Name);
            Items.Add(Color.DarkSalmon.Name);
            Items.Add(Color.DarkSeaGreen.Name);
            Items.Add(Color.DarkSlateBlue.Name);
            Items.Add(Color.DarkSlateGray.Name);
            Items.Add(Color.DarkTurquoise.Name);
            Items.Add(Color.DarkViolet.Name);
            Items.Add(Color.DeepSkyBlue.Name);
            Items.Add(Color.DimGray.Name);
            Items.Add(Color.DodgerBlue.Name);
            Items.Add(Color.Firebrick.Name);
            Items.Add(Color.FloralWhite.Name);
            Items.Add(Color.ForestGreen.Name);
            Items.Add(Color.Fuchsia.Name);
            Items.Add(Color.Gainsboro.Name);
            Items.Add(Color.GhostWhite.Name);
            Items.Add(Color.Gold.Name);
            Items.Add(Color.Goldenrod.Name);
            Items.Add(Color.Gray.Name);
            Items.Add(Color.Green.Name);
            Items.Add(Color.GreenYellow.Name);
            Items.Add(Color.Honeydew.Name);
            Items.Add(Color.HotPink.Name);
            Items.Add(Color.IndianRed.Name);
            Items.Add(Color.Indigo.Name);
            Items.Add(Color.Ivory.Name);
            Items.Add(Color.Khaki.Name);
            Items.Add(Color.Lavender.Name);
            Items.Add(Color.LavenderBlush.Name);
            Items.Add(Color.LawnGreen.Name);
            Items.Add(Color.LemonChiffon.Name);
            Items.Add(Color.LightBlue.Name);
            Items.Add(Color.LightCoral.Name);
            Items.Add(Color.LightCyan.Name);
            Items.Add(Color.LightGoldenrodYellow.Name);
            Items.Add(Color.LightGray.Name);
            Items.Add(Color.LightGreen.Name);
            Items.Add(Color.LightPink.Name);
            Items.Add(Color.LightSalmon.Name);
            Items.Add(Color.LightSeaGreen.Name);
            Items.Add(Color.LightSkyBlue.Name);
            Items.Add(Color.LightSlateGray.Name);
            Items.Add(Color.LightSteelBlue.Name);
            Items.Add(Color.LightYellow.Name);
            Items.Add(Color.Lime.Name);
            Items.Add(Color.LimeGreen.Name);
            Items.Add(Color.Linen.Name);
            Items.Add(Color.Magenta.Name);
            Items.Add(Color.Maroon.Name);
            Items.Add(Color.MediumAquamarine.Name);
            Items.Add(Color.MediumBlue.Name);
            Items.Add(Color.MediumOrchid.Name);
            Items.Add(Color.MediumPurple.Name);
            Items.Add(Color.MediumSeaGreen.Name);
            Items.Add(Color.MediumSlateBlue.Name);
            Items.Add(Color.MediumSpringGreen.Name);
            Items.Add(Color.MediumTurquoise.Name);
            Items.Add(Color.MediumVioletRed.Name);
            Items.Add(Color.MidnightBlue.Name);
            Items.Add(Color.MintCream.Name);
            Items.Add(Color.MistyRose.Name);
            Items.Add(Color.Moccasin.Name);
            Items.Add(Color.NavajoWhite.Name);
            Items.Add(Color.Navy.Name);
            Items.Add(Color.OldLace.Name);
            Items.Add(Color.Olive.Name);
            Items.Add(Color.OliveDrab.Name);
            Items.Add(Color.Orange.Name);
            Items.Add(Color.OrangeRed.Name);
            Items.Add(Color.Orchid.Name);
            Items.Add(Color.PaleGoldenrod.Name);
            Items.Add(Color.PaleGreen.Name);
            Items.Add(Color.PaleTurquoise.Name);
            Items.Add(Color.PaleVioletRed.Name);
            Items.Add(Color.PapayaWhip.Name);
            Items.Add(Color.PeachPuff.Name);
            Items.Add(Color.Peru.Name);
            Items.Add(Color.Pink.Name);
            Items.Add(Color.Plum.Name);
            Items.Add(Color.PowderBlue.Name);
            Items.Add(Color.Purple.Name);
            Items.Add(Color.Red.Name);
            Items.Add(Color.RosyBrown.Name);
            Items.Add(Color.RoyalBlue.Name);
            Items.Add(Color.SaddleBrown.Name);
            Items.Add(Color.Salmon.Name);
            Items.Add(Color.SandyBrown.Name);
            Items.Add(Color.SeaGreen.Name);
            Items.Add(Color.SeaShell.Name);
            Items.Add(Color.Sienna.Name);
            Items.Add(Color.Silver.Name);
            Items.Add(Color.SkyBlue.Name);
            Items.Add(Color.SlateBlue.Name);
            Items.Add(Color.SlateGray.Name);
            Items.Add(Color.Snow.Name);
            Items.Add(Color.SpringGreen.Name);
            Items.Add(Color.SteelBlue.Name);
            Items.Add(Color.Tan.Name);
            Items.Add(Color.Teal.Name);
            Items.Add(Color.Thistle.Name);
            Items.Add(Color.Tomato.Name);
            Items.Add(Color.Transparent.Name);
            Items.Add(Color.Turquoise.Name);
            Items.Add(Color.Violet.Name);
            Items.Add(Color.Wheat.Name);
            Items.Add(Color.White.Name);
            Items.Add(Color.WhiteSmoke.Name);
            Items.Add(Color.Yellow.Name);
            Items.Add(Color.YellowGreen.Name);
        }

        /// <summary>
        ///     Called when [draw item].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DrawItemEventArgs" /> instance containing the event data.</param>
        private void OnDrawItem(object sender, DrawItemEventArgs e)
        {
            var grfx = e.Graphics;
            var brushColor = (Items[e.Index] as string).FromString();
            var brush = new SolidBrush(brushColor);

            grfx.FillRectangle(brush, e.Bounds);

            if (!_bHideText)
            {
                if (brushColor == Color.Black || brushColor == Color.MidnightBlue
                    || brushColor == Color.DarkBlue || brushColor == Color.Indigo
                    || brushColor == Color.MediumBlue || brushColor == Color.Maroon
                    || brushColor == Color.Navy || brushColor == Color.Purple)
                    grfx.DrawString((string)Items[e.Index], e.Font, _whiteBrush, e.Bounds);
                else
                    grfx.DrawString((string)Items[e.Index], e.Font, _blackBrush, e.Bounds);

                SelectionStart = 0;
                SelectionLength = 0;
            }
            else
            {
                grfx.DrawString((string)Items[e.Index], e.Font,
                    new SolidBrush((Items[e.Index] as string).FromString()), e.Bounds);
            }
        }

        /// <summary>
        ///     Called when [selected index changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            BackColor = (SelectedItem as string).FromString();

            if (!_bHideText) return;
            ForeColor = BackColor;
            SelectionStart = 0;
            SelectionLength = 0;
        }

        /// <summary>
        ///     Called when [drop down].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void OnDropDown(object sender, EventArgs e)
        {
            BackColor = (SelectedItem as string).FromString();

            if (!_bHideText) return;
            ForeColor = BackColor;
            SelectionStart = 0;
            SelectionLength = 0;
        }
    }
}