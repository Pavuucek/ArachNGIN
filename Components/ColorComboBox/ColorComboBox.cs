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
            Items.Add(Color.AliceBlue.NamedColorToString());
            Items.Add(Color.AntiqueWhite.NamedColorToString());
            Items.Add(Color.Aqua.NamedColorToString());
            Items.Add(Color.Aquamarine.NamedColorToString());
            Items.Add(Color.Azure.NamedColorToString());
            Items.Add(Color.Black.NamedColorToString());
            Items.Add(Color.BlanchedAlmond.NamedColorToString());
            Items.Add(Color.Blue.NamedColorToString());
            Items.Add(Color.BlueViolet.NamedColorToString());
            Items.Add(Color.Brown.NamedColorToString());
            Items.Add(Color.BurlyWood.NamedColorToString());
            Items.Add(Color.CadetBlue.NamedColorToString());
            Items.Add(Color.Chartreuse.NamedColorToString());
            Items.Add(Color.Chocolate.NamedColorToString());
            Items.Add(Color.Coral.NamedColorToString());
            Items.Add(Color.CornflowerBlue.NamedColorToString());
            Items.Add(Color.Cornsilk.NamedColorToString());
            Items.Add(Color.Crimson.NamedColorToString());
            Items.Add(Color.Cyan.NamedColorToString());
            Items.Add(Color.DarkBlue.NamedColorToString());
            Items.Add(Color.DarkCyan.NamedColorToString());
            Items.Add(Color.DarkGoldenrod.NamedColorToString());
            Items.Add(Color.DarkGray.NamedColorToString());
            Items.Add(Color.DarkGreen.NamedColorToString());
            Items.Add(Color.DarkKhaki.NamedColorToString());
            Items.Add(Color.DarkMagenta.NamedColorToString());
            Items.Add(Color.DarkOliveGreen.NamedColorToString());
            Items.Add(Color.DarkOrange.NamedColorToString());
            Items.Add(Color.DarkOrchid.NamedColorToString());
            Items.Add(Color.DarkRed.NamedColorToString());
            Items.Add(Color.DarkSalmon.NamedColorToString());
            Items.Add(Color.DarkSeaGreen.NamedColorToString());
            Items.Add(Color.DarkSlateBlue.NamedColorToString());
            Items.Add(Color.DarkSlateGray.NamedColorToString());
            Items.Add(Color.DarkTurquoise.NamedColorToString());
            Items.Add(Color.DarkViolet.NamedColorToString());
            Items.Add(Color.DeepSkyBlue.NamedColorToString());
            Items.Add(Color.DimGray.NamedColorToString());
            Items.Add(Color.DodgerBlue.NamedColorToString());
            Items.Add(Color.Firebrick.NamedColorToString());
            Items.Add(Color.FloralWhite.NamedColorToString());
            Items.Add(Color.ForestGreen.NamedColorToString());
            Items.Add(Color.Fuchsia.NamedColorToString());
            Items.Add(Color.Gainsboro.NamedColorToString());
            Items.Add(Color.GhostWhite.NamedColorToString());
            Items.Add(Color.Gold.NamedColorToString());
            Items.Add(Color.Goldenrod.NamedColorToString());
            Items.Add(Color.Gray.NamedColorToString());
            Items.Add(Color.Green.NamedColorToString());
            Items.Add(Color.GreenYellow.NamedColorToString());
            Items.Add(Color.Honeydew.NamedColorToString());
            Items.Add(Color.HotPink.NamedColorToString());
            Items.Add(Color.IndianRed.NamedColorToString());
            Items.Add(Color.Indigo.NamedColorToString());
            Items.Add(Color.Ivory.NamedColorToString());
            Items.Add(Color.Khaki.NamedColorToString());
            Items.Add(Color.Lavender.NamedColorToString());
            Items.Add(Color.LavenderBlush.NamedColorToString());
            Items.Add(Color.LawnGreen.NamedColorToString());
            Items.Add(Color.LemonChiffon.NamedColorToString());
            Items.Add(Color.LightBlue.NamedColorToString());
            Items.Add(Color.LightCoral.NamedColorToString());
            Items.Add(Color.LightCyan.NamedColorToString());
            Items.Add(Color.LightGoldenrodYellow.NamedColorToString());
            Items.Add(Color.LightGray.NamedColorToString());
            Items.Add(Color.LightGreen.NamedColorToString());
            Items.Add(Color.LightPink.NamedColorToString());
            Items.Add(Color.LightSalmon.NamedColorToString());
            Items.Add(Color.LightSeaGreen.NamedColorToString());
            Items.Add(Color.LightSkyBlue.NamedColorToString());
            Items.Add(Color.LightSlateGray.NamedColorToString());
            Items.Add(Color.LightSteelBlue.NamedColorToString());
            Items.Add(Color.LightYellow.NamedColorToString());
            Items.Add(Color.Lime.NamedColorToString());
            Items.Add(Color.LimeGreen.NamedColorToString());
            Items.Add(Color.Linen.NamedColorToString());
            Items.Add(Color.Magenta.NamedColorToString());
            Items.Add(Color.Maroon.NamedColorToString());
            Items.Add(Color.MediumAquamarine.NamedColorToString());
            Items.Add(Color.MediumBlue.NamedColorToString());
            Items.Add(Color.MediumOrchid.NamedColorToString());
            Items.Add(Color.MediumPurple.NamedColorToString());
            Items.Add(Color.MediumSeaGreen.NamedColorToString());
            Items.Add(Color.MediumSlateBlue.NamedColorToString());
            Items.Add(Color.MediumSpringGreen.NamedColorToString());
            Items.Add(Color.MediumTurquoise.NamedColorToString());
            Items.Add(Color.MediumVioletRed.NamedColorToString());
            Items.Add(Color.MidnightBlue.NamedColorToString());
            Items.Add(Color.MintCream.NamedColorToString());
            Items.Add(Color.MistyRose.NamedColorToString());
            Items.Add(Color.Moccasin.NamedColorToString());
            Items.Add(Color.NavajoWhite.NamedColorToString());
            Items.Add(Color.Navy.NamedColorToString());
            Items.Add(Color.OldLace.NamedColorToString());
            Items.Add(Color.Olive.NamedColorToString());
            Items.Add(Color.OliveDrab.NamedColorToString());
            Items.Add(Color.Orange.NamedColorToString());
            Items.Add(Color.OrangeRed.NamedColorToString());
            Items.Add(Color.Orchid.NamedColorToString());
            Items.Add(Color.PaleGoldenrod.NamedColorToString());
            Items.Add(Color.PaleGreen.NamedColorToString());
            Items.Add(Color.PaleTurquoise.NamedColorToString());
            Items.Add(Color.PaleVioletRed.NamedColorToString());
            Items.Add(Color.PapayaWhip.NamedColorToString());
            Items.Add(Color.PeachPuff.NamedColorToString());
            Items.Add(Color.Peru.NamedColorToString());
            Items.Add(Color.Pink.NamedColorToString());
            Items.Add(Color.Plum.NamedColorToString());
            Items.Add(Color.PowderBlue.NamedColorToString());
            Items.Add(Color.Purple.NamedColorToString());
            Items.Add(Color.Red.NamedColorToString());
            Items.Add(Color.RosyBrown.NamedColorToString());
            Items.Add(Color.RoyalBlue.NamedColorToString());
            Items.Add(Color.SaddleBrown.NamedColorToString());
            Items.Add(Color.Salmon.NamedColorToString());
            Items.Add(Color.SandyBrown.NamedColorToString());
            Items.Add(Color.SeaGreen.NamedColorToString());
            Items.Add(Color.SeaShell.NamedColorToString());
            Items.Add(Color.Sienna.NamedColorToString());
            Items.Add(Color.Silver.NamedColorToString());
            Items.Add(Color.SkyBlue.NamedColorToString());
            Items.Add(Color.SlateBlue.NamedColorToString());
            Items.Add(Color.SlateGray.NamedColorToString());
            Items.Add(Color.Snow.NamedColorToString());
            Items.Add(Color.SpringGreen.NamedColorToString());
            Items.Add(Color.SteelBlue.NamedColorToString());
            Items.Add(Color.Tan.NamedColorToString());
            Items.Add(Color.Teal.NamedColorToString());
            Items.Add(Color.Thistle.NamedColorToString());
            Items.Add(Color.Tomato.NamedColorToString());
            Items.Add(Color.Transparent.NamedColorToString());
            Items.Add(Color.Turquoise.NamedColorToString());
            Items.Add(Color.Violet.NamedColorToString());
            Items.Add(Color.Wheat.NamedColorToString());
            Items.Add(Color.White.NamedColorToString());
            Items.Add(Color.WhiteSmoke.NamedColorToString());
            Items.Add(Color.Yellow.NamedColorToString());
            Items.Add(Color.YellowGreen.NamedColorToString());
        }

        /// <summary>
        ///     Called when [draw item].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DrawItemEventArgs" /> instance containing the event data.</param>
        private void OnDrawItem(object sender, DrawItemEventArgs e)
        {
            var grfx = e.Graphics;
            var brushColor = (Items[e.Index] as string).NamedColorToString();
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
                    new SolidBrush((Items[e.Index] as string).NamedColorToString()), e.Bounds);
            }
        }

        /// <summary>
        ///     Called when [selected index changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            BackColor = (SelectedItem as string).NamedColorToString();

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
            BackColor = (SelectedItem as string).NamedColorToString();

            if (!_bHideText) return;
            ForeColor = BackColor;
            SelectionStart = 0;
            SelectionLength = 0;
        }
    }
}