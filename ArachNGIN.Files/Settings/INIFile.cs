/*
 * Copyright (c) 2006-2013 Michal Kuncl <michal.kuncl@gmail.com> http://www.pavucina.info
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
 * associated documentation files (the "Software"), to deal in the Software without restriction,
 * including without limitation the rights to use, copy, modify, merge, publish, distribute,
 * sublicense, and/or sell copies of the Software, and to permit persons to whom the Software
 * is furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all copies or
 * substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
 * PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
 * FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
 * ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;

namespace ArachNGIN.Files.Settings
{
    /// <summary>
    /// třída pro přístup k ini souborům
    /// </summary>
    public class IniFile
    {
        private DataSet _mDs;
        private string _mFileName = "";

        #region Color fce

        /// <summary>
        /// konvertne barvu na řetězec
        /// </summary>
        /// <param name="color">barva</param>
        /// <returns>výsledný řetězec</returns>
        public string GetStringFromColor(Color color)
        {
            string strColorName;

            if (color == Color.AliceBlue)
            {
                strColorName = "Alice Blue";
            }
            else if (color == Color.AntiqueWhite)
            {
                strColorName = "Antique White";
            }
            else if (color == Color.Aqua)
            {
                strColorName = "Aqua";
            }
            else if (color == Color.Aquamarine)
            {
                strColorName = "Aquamarine";
            }
            else if (color == Color.Azure)
            {
                strColorName = "Azure";
            }
            else if (color == Color.Beige)
            {
                strColorName = "Biege";
            }
            else if (color == Color.Bisque)
            {
                strColorName = "Bisque";
            }
            else if (color == Color.Black)
            {
                strColorName = "Black";
            }
            else if (color == Color.BlanchedAlmond)
            {
                strColorName = "Blanched Almond";
            }
            else if (color == Color.Blue)
            {
                strColorName = "Blue";
            }
            else if (color == Color.BlueViolet)
            {
                strColorName = "Blue Violet";
            }
            else if (color == Color.Brown)
            {
                strColorName = "Brown";
            }
            else if (color == Color.BurlyWood)
            {
                strColorName = "Burly Wood";
            }
            else if (color == Color.CadetBlue)
            {
                strColorName = "Cadet Blue";
            }
            else if (color == Color.Chartreuse)
            {
                strColorName = "Chartreuse";
            }
            else if (color == Color.Chocolate)
            {
                strColorName = "Chocolate";
            }
            else if (color == Color.Coral)
            {
                strColorName = "Coral";
            }
            else if (color == Color.CornflowerBlue)
            {
                strColorName = "Cornflower Blue";
            }
            else if (color == Color.Cornsilk)
            {
                strColorName = "Cornsilk";
            }
            else if (color == Color.Crimson)
            {
                strColorName = "Crimson";
            }
            else if (color == Color.Cyan)
            {
                strColorName = "Cyan";
            }
            else if (color == Color.DarkBlue)
            {
                strColorName = "Dark Blue";
            }
            else if (color == Color.DarkCyan)
            {
                strColorName = "Dark Cyan";
            }
            else if (color == Color.DarkGoldenrod)
            {
                strColorName = "Dark Goldenrod";
            }
            else if (color == Color.DarkGray)
            {
                strColorName = "Dark Gray";
            }
            else if (color == Color.DarkGreen)
            {
                strColorName = "Dark Green";
            }
            else if (color == Color.DarkKhaki)
            {
                strColorName = "Dark Khaki";
            }
            else if (color == Color.DarkMagenta)
            {
                strColorName = "Dark Magenta";
            }
            else if (color == Color.DarkOliveGreen)
            {
                strColorName = "Dark Olive Green";
            }
            else if (color == Color.DarkOrange)
            {
                strColorName = "Dark Orange";
            }
            else if (color == Color.DarkOrchid)
            {
                strColorName = "Dark Orchid";
            }
            else if (color == Color.DarkRed)
            {
                strColorName = "Dark Red";
            }
            else if (color == Color.DarkSalmon)
            {
                strColorName = "Dark Salmon";
            }
            else if (color == Color.DarkSeaGreen)
            {
                strColorName = "Dark Sea Green";
            }
            else if (color == Color.DarkSlateBlue)
            {
                strColorName = "Dark Slate Blue";
            }
            else if (color == Color.DarkSlateGray)
            {
                strColorName = "Dark Slate Gray";
            }
            else if (color == Color.DarkTurquoise)
            {
                strColorName = "Dark Turquoise";
            }
            else if (color == Color.DarkViolet)
            {
                strColorName = "Dark Violet";
            }
            else if (color == Color.DeepPink)
            {
                strColorName = "Deep Pink";
            }
            else if (color == Color.DeepSkyBlue)
            {
                strColorName = "Deep Sky Blue";
            }
            else if (color == Color.DimGray)
            {
                strColorName = "Dim Gray";
            }
            else if (color == Color.DodgerBlue)
            {
                strColorName = "Dodger Blue";
            }
            else if (color == Color.Firebrick)
            {
                strColorName = "Fire Brick";
            }
            else if (color == Color.FloralWhite)
            {
                strColorName = "Floral White";
            }
            else if (color == Color.ForestGreen)
            {
                strColorName = "Forest Green";
            }
            else if (color == Color.Fuchsia)
            {
                strColorName = "Fuschia";
            }
            else if (color == Color.Gainsboro)
            {
                strColorName = "Gainsboro";
            }
            else if (color == Color.GhostWhite)
            {
                strColorName = "Ghost White";
            }
            else if (color == Color.Gold)
            {
                strColorName = "Gold";
            }
            else if (color == Color.Goldenrod)
            {
                strColorName = "Goldenrod";
            }
            else if (color == Color.Gray)
            {
                strColorName = "Gray";
            }
            else if (color == Color.Green)
            {
                strColorName = "Green";
            }
            else if (color == Color.GreenYellow)
            {
                strColorName = "Green Yellow";
            }
            else if (color == Color.Honeydew)
            {
                strColorName = "Honeydew";
            }
            else if (color == Color.HotPink)
            {
                strColorName = "Hot Pink";
            }
            else if (color == Color.IndianRed)
            {
                strColorName = "Indian Red";
            }
            else if (color == Color.Indigo)
            {
                strColorName = "Indigo";
            }
            else if (color == Color.Ivory)
            {
                strColorName = "Ivory";
            }
            else if (color == Color.Khaki)
            {
                strColorName = "Khaki";
            }
            else if (color == Color.Lavender)
            {
                strColorName = "Lavender";
            }
            else if (color == Color.LavenderBlush)
            {
                strColorName = "Lavender Blush";
            }
            else if (color == Color.LawnGreen)
            {
                strColorName = "Lawn Green";
            }
            else if (color == Color.LemonChiffon)
            {
                strColorName = "Lemon Chiffon";
            }
            else if (color == Color.LightBlue)
            {
                strColorName = "Light Blue";
            }
            else if (color == Color.LightCoral)
            {
                strColorName = "Light Coral";
            }
            else if (color == Color.LightCyan)
            {
                strColorName = "Light Cyan";
            }
            else if (color == Color.LightGoldenrodYellow)
            {
                strColorName = "Light Goldenrod Yellow";
            }
            else if (color == Color.LightGray)
            {
                strColorName = "Light Gray";
            }
            else if (color == Color.LightGreen)
            {
                strColorName = "Light Green";
            }
            else if (color == Color.LightPink)
            {
                strColorName = "Light Pink";
            }
            else if (color == Color.LightSalmon)
            {
                strColorName = "Light Salmon";
            }
            else if (color == Color.LightSeaGreen)
            {
                strColorName = "Light Sea Green";
            }
            else if (color == Color.LightSkyBlue)
            {
                strColorName = "Light Sky Blue";
            }
            else if (color == Color.LightSlateGray)
            {
                strColorName = "Light Slate Gray";
            }
            else if (color == Color.LightSteelBlue)
            {
                strColorName = "Light Steel Blue";
            }
            else if (color == Color.LightYellow)
            {
                strColorName = "Light Yellow";
            }
            else if (color == Color.Lime)
            {
                strColorName = "Lime";
            }
            else if (color == Color.LimeGreen)
            {
                strColorName = "Lime Green";
            }
            else if (color == Color.Linen)
            {
                strColorName = "Linen";
            }
            else if (color == Color.Magenta)
            {
                strColorName = "Magenta";
            }
            else if (color == Color.Maroon)
            {
                strColorName = "Maroon";
            }
            else if (color == Color.MediumAquamarine)
            {
                strColorName = "Medium Aquamarine";
            }
            else if (color == Color.MediumBlue)
            {
                strColorName = "Medium Blue";
            }
            else if (color == Color.MediumOrchid)
            {
                strColorName = "Medium Orchid";
            }
            else if (color == Color.MediumPurple)
            {
                strColorName = "Medium Purple";
            }
            else if (color == Color.MediumSeaGreen)
            {
                strColorName = "Medium Sea Green";
            }
            else if (color == Color.MediumSlateBlue)
            {
                strColorName = "Medium Slate Blue";
            }
            else if (color == Color.MediumSpringGreen)
            {
                strColorName = "Medium Spring Green";
            }
            else if (color == Color.MediumTurquoise)
            {
                strColorName = "Medium Turquoise";
            }
            else if (color == Color.MediumVioletRed)
            {
                strColorName = "Medium Violet Red";
            }
            else if (color == Color.MidnightBlue)
            {
                strColorName = "Midnight Blue";
            }
            else if (color == Color.MintCream)
            {
                strColorName = "Mint Cream";
            }
            else if (color == Color.MistyRose)
            {
                strColorName = "Misty Rose";
            }
            else if (color == Color.Moccasin)
            {
                strColorName = "Moccasin";
            }
            else if (color == Color.NavajoWhite)
            {
                strColorName = "Navajo White";
            }
            else if (color == Color.Navy)
            {
                strColorName = "Navy";
            }
            else if (color == Color.OldLace)
            {
                strColorName = "Old Lace";
            }
            else if (color == Color.Olive)
            {
                strColorName = "Olive";
            }
            else if (color == Color.OliveDrab)
            {
                strColorName = "Olive Drab";
            }
            else if (color == Color.Orange)
            {
                strColorName = "Orange";
            }
            else if (color == Color.OrangeRed)
            {
                strColorName = "Orange Red";
            }
            else if (color == Color.Orchid)
            {
                strColorName = "Orchid";
            }
            else if (color == Color.PaleGoldenrod)
            {
                strColorName = "Pale Goldenrod";
            }
            else if (color == Color.PaleGreen)
            {
                strColorName = "Pale Green";
            }
            else if (color == Color.PaleTurquoise)
            {
                strColorName = "Pale Turquoise";
            }
            else if (color == Color.PaleVioletRed)
            {
                strColorName = "Pale Violet Red";
            }
            else if (color == Color.PapayaWhip)
            {
                strColorName = "Papaya Whip";
            }
            else if (color == Color.PeachPuff)
            {
                strColorName = "Peach Puff";
            }
            else if (color == Color.Peru)
            {
                strColorName = "Peru";
            }
            else if (color == Color.Pink)
            {
                strColorName = "Pink";
            }
            else if (color == Color.Plum)
            {
                strColorName = "Plum";
            }
            else if (color == Color.PowderBlue)
            {
                strColorName = "Powder Blue";
            }
            else if (color == Color.Purple)
            {
                strColorName = "Purple";
            }
            else if (color == Color.Red)
            {
                strColorName = "Red";
            }
            else if (color == Color.RosyBrown)
            {
                strColorName = "Rosy Brown";
            }
            else if (color == Color.RoyalBlue)
            {
                strColorName = "Royal Blue";
            }
            else if (color == Color.SaddleBrown)
            {
                strColorName = "Saddle Brown";
            }
            else if (color == Color.Salmon)
            {
                strColorName = "Salmon";
            }
            else if (color == Color.SandyBrown)
            {
                strColorName = "Sandy Brown";
            }
            else if (color == Color.SeaGreen)
            {
                strColorName = "Sea Green";
            }
            else if (color == Color.SeaShell)
            {
                strColorName = "Sea Shell";
            }
            else if (color == Color.Sienna)
            {
                strColorName = "Sienna";
            }
            else if (color == Color.Silver)
            {
                strColorName = "Silver";
            }
            else if (color == Color.SkyBlue)
            {
                strColorName = "Sky Blue";
            }
            else if (color == Color.SlateBlue)
            {
                strColorName = "Slate Blue";
            }
            else if (color == Color.SlateGray)
            {
                strColorName = "Slate Gray";
            }
            else if (color == Color.Snow)
            {
                strColorName = "Snow";
            }
            else if (color == Color.SpringGreen)
            {
                strColorName = "Spring Green";
            }
            else if (color == Color.SteelBlue)
            {
                strColorName = "Steel Blue";
            }
            else if (color == Color.Tan)
            {
                strColorName = "Tan";
            }
            else if (color == Color.Teal)
            {
                strColorName = "Teal";
            }
            else if (color == Color.Thistle)
            {
                strColorName = "Thistle";
            }
            else if (color == Color.Tomato)
            {
                strColorName = "Tomato";
            }
            else if (color == Color.Transparent)
            {
                strColorName = "Transparent";
            }
            else if (color == Color.Turquoise)
            {
                strColorName = "Turquoise";
            }
            else if (color == Color.Violet)
            {
                strColorName = "Violet";
            }
            else if (color == Color.Wheat)
            {
                strColorName = "Wheat";
            }
            else if (color == Color.White)
            {
                strColorName = "White";
            }
            else if (color == Color.WhiteSmoke)
            {
                strColorName = "White Smoke";
            }
            else if (color == Color.Yellow)
            {
                strColorName = "Yellow";
            }
            else if (color == Color.YellowGreen)
            {
                strColorName = "Yellow Green";
            }
            else
            {
                strColorName = "none";
            }


            return strColorName;
        }

        /// <summary>
        /// konvertne řetězec na barvu
        /// </summary>
        /// <param name="strColorName">řetězec</param>
        /// <returns>výsledná barva</returns>
        public Color GetColorFromString(string strColorName)
        {
            Color color;

            if (strColorName == "Alice Blue")
            {
                color = Color.AliceBlue;
            }
            else if (strColorName == "Antique White")
            {
                color = Color.AntiqueWhite;
            }
            else if (strColorName == "Aqua")
            {
                color = Color.Aqua;
            }
            else if (strColorName == "Aquamarine")
            {
                color = Color.Aquamarine;
            }
            else if (strColorName == "Azure")
            {
                color = Color.Azure;
            }
            else if (strColorName == "Biege")
            {
                color = Color.Beige;
            }
            else if (strColorName == "Bisque")
            {
                color = Color.Bisque;
            }
            else if (strColorName == "Black")
            {
                color = Color.Black;
            }
            else if (strColorName == "Blanched Almond")
            {
                color = Color.BlanchedAlmond;
            }
            else if (strColorName == "Blue")
            {
                color = Color.Blue;
            }
            else if (strColorName == "Blue Violet")
            {
                color = Color.BlueViolet;
            }
            else if (strColorName == "Brown")
            {
                color = Color.Brown;
            }
            else if (strColorName == "Burly Wood")
            {
                color = Color.BurlyWood;
            }
            else if (strColorName == "Cadet Blue")
            {
                color = Color.CadetBlue;
            }
            else if (strColorName == "Chartreuse")
            {
                color = Color.Chartreuse;
            }
            else if (strColorName == "Chocolate")
            {
                color = Color.Chocolate;
            }
            else if (strColorName == "Coral")
            {
                color = Color.Coral;
            }
            else if (strColorName == "Cornflower Blue")
            {
                color = Color.CornflowerBlue;
            }
            else if (strColorName == "Cornsilk")
            {
                color = Color.Cornsilk;
            }
            else if (strColorName == "Crimson")
            {
                color = Color.Crimson;
            }
            else if (strColorName == "Cyan")
            {
                color = Color.Cyan;
            }
            else if (strColorName == "Dark Blue")
            {
                color = Color.DarkBlue;
            }
            else if (strColorName == "Dark Cyan")
            {
                color = Color.DarkCyan;
            }
            else if (strColorName == "Dark Goldenrod")
            {
                color = Color.DarkGoldenrod;
            }
            else if (strColorName == "Dark Gray")
            {
                color = Color.DarkGray;
            }
            else if (strColorName == "Dark Green")
            {
                color = Color.DarkGreen;
            }
            else if (strColorName == "Dark Khaki")
            {
                color = Color.DarkKhaki;
            }
            else if (strColorName == "Dark Magenta")
            {
                color = Color.DarkMagenta;
            }
            else if (strColorName == "Dark Olive Green")
            {
                color = Color.DarkOliveGreen;
            }
            else if (strColorName == "Dark Orange")
            {
                color = Color.DarkOrange;
            }
            else if (strColorName == "Dark Orchid")
            {
                color = Color.DarkOrchid;
            }
            else if (strColorName == "Dark Red")
            {
                color = Color.DarkRed;
            }
            else if (strColorName == "Dark Salmon")
            {
                color = Color.DarkSalmon;
            }
            else if (strColorName == "Dark Sea Green")
            {
                color = Color.DarkSeaGreen;
            }
            else if (strColorName == "Dark Slate Blue")
            {
                color = Color.DarkSlateBlue;
            }
            else if (strColorName == "Dark Slate Gray")
            {
                color = Color.DarkSlateGray;
            }
            else if (strColorName == "Dark Turquoise")
            {
                color = Color.DarkTurquoise;
            }
            else if (strColorName == "Dark Violet")
            {
                color = Color.DarkViolet;
            }
            else if (strColorName == "Deep Pink")
            {
                color = Color.DeepPink;
            }
            else if (strColorName == "Deep Sky Blue")
            {
                color = Color.DeepSkyBlue;
            }
            else if (strColorName == "Dim Gray")
            {
                color = Color.DimGray;
            }
            else if (strColorName == "Dodger Blue")
            {
                color = Color.DodgerBlue;
            }
            else if (strColorName == "Fire Brick")
            {
                color = Color.Firebrick;
            }
            else if (strColorName == "Floral White")
            {
                color = Color.FloralWhite;
            }
            else if (strColorName == "Forest Green")
            {
                color = Color.ForestGreen;
            }
            else if (strColorName == "Fuschia")
            {
                color = Color.Fuchsia;
            }
            else if (strColorName == "Gainsboro")
            {
                color = Color.Gainsboro;
            }
            else if (strColorName == "Ghost White")
            {
                color = Color.GhostWhite;
            }
            else if (strColorName == "Gold")
            {
                color = Color.Gold;
            }
            else if (strColorName == "Goldenrod")
            {
                color = Color.Goldenrod;
            }
            else if (strColorName == "Gray")
            {
                color = Color.Gray;
            }
            else if (strColorName == "Green")
            {
                color = Color.Green;
            }
            else if (strColorName == "Green Yellow")
            {
                color = Color.GreenYellow;
            }
            else if (strColorName == "Honeydew")
            {
                color = Color.Honeydew;
            }
            else if (strColorName == "Hot Pink")
            {
                color = Color.HotPink;
            }
            else if (strColorName == "Indian Red")
            {
                color = Color.IndianRed;
            }
            else if (strColorName == "Indigo")
            {
                color = Color.Indigo;
            }
            else if (strColorName == "Ivory")
            {
                color = Color.Ivory;
            }
            else if (strColorName == "Khaki")
            {
                color = Color.Khaki;
            }
            else if (strColorName == "Lavender")
            {
                color = Color.Lavender;
            }
            else if (strColorName == "Lavender Blush")
            {
                color = Color.LavenderBlush;
            }
            else if (strColorName == "Lawn Green")
            {
                color = Color.LawnGreen;
            }
            else if (strColorName == "Lemon Chiffon")
            {
                color = Color.LemonChiffon;
            }
            else if (strColorName == "Light Blue")
            {
                color = Color.LightBlue;
            }
            else if (strColorName == "Light Coral")
            {
                color = Color.LightCoral;
            }
            else if (strColorName == "Light Cyan")
            {
                color = Color.LightCyan;
            }
            else if (strColorName == "Light Goldenrod Yellow")
            {
                color = Color.LightGoldenrodYellow;
            }
            else if (strColorName == "Light Gray")
            {
                color = Color.LightGray;
            }
            else if (strColorName == "Light Green")
            {
                color = Color.LightGreen;
            }
            else if (strColorName == "Light Pink")
            {
                color = Color.LightPink;
            }
            else if (strColorName == "Light Salmon")
            {
                color = Color.LightSalmon;
            }
            else if (strColorName == "Light Sea Green")
            {
                color = Color.LightSeaGreen;
            }
            else if (strColorName == "Light Sky Blue")
            {
                color = Color.LightSkyBlue;
            }
            else if (strColorName == "Light Slate Gray")
            {
                color = Color.LightSlateGray;
            }
            else if (strColorName == "Light Steel Blue")
            {
                color = Color.LightSteelBlue;
            }
            else if (strColorName == "Light Yellow")
            {
                color = Color.LightYellow;
            }
            else if (strColorName == "Lime")
            {
                color = Color.Lime;
            }
            else if (strColorName == "Lime Green")
            {
                color = Color.LimeGreen;
            }
            else if (strColorName == "Linen")
            {
                color = Color.Linen;
            }
            else if (strColorName == "Magenta")
            {
                color = Color.Magenta;
            }
            else if (strColorName == "Maroon")
            {
                color = Color.Maroon;
            }
            else if (strColorName == "Medium Aquamarine")
            {
                color = Color.MediumAquamarine;
            }
            else if (strColorName == "Medium Blue")
            {
                color = Color.MediumBlue;
            }
            else if (strColorName == "Medium Orchid")
            {
                color = Color.MediumOrchid;
            }
            else if (strColorName == "Medium Purple")
            {
                color = Color.MediumPurple;
            }
            else if (strColorName == "Medium Sea Green")
            {
                color = Color.MediumSeaGreen;
            }
            else if (strColorName == "Medium Slate Blue")
            {
                color = Color.MediumSlateBlue;
            }
            else if (strColorName == "Medium Spring Green")
            {
                color = Color.MediumSpringGreen;
            }
            else if (strColorName == "Medium Turquoise")
            {
                color = Color.MediumTurquoise;
            }
            else if (strColorName == "Medium Violet Red")
            {
                color = Color.MediumVioletRed;
            }
            else if (strColorName == "Midnight Blue")
            {
                color = Color.MidnightBlue;
            }
            else if (strColorName == "Mint Cream")
            {
                color = Color.MintCream;
            }
            else if (strColorName == "Misty Rose")
            {
                color = Color.MistyRose;
            }
            else if (strColorName == "Moccasin")
            {
                color = Color.Moccasin;
            }
            else if (strColorName == "Navajo White")
            {
                color = Color.NavajoWhite;
            }
            else if (strColorName == "Navy")
            {
                color = Color.Navy;
            }
            else if (strColorName == "Old Lace")
            {
                color = Color.OldLace;
            }
            else if (strColorName == "Olive")
            {
                color = Color.Olive;
            }
            else if (strColorName == "Olive Drab")
            {
                color = Color.OliveDrab;
            }
            else if (strColorName == "Orange")
            {
                color = Color.Orange;
            }
            else if (strColorName == "Orange Red")
            {
                color = Color.OrangeRed;
            }
            else if (strColorName == "Orchid")
            {
                color = Color.Orchid;
            }
            else if (strColorName == "Pale Goldenrod")
            {
                color = Color.PaleGoldenrod;
            }
            else if (strColorName == "Pale Green")
            {
                color = Color.PaleGreen;
            }
            else if (strColorName == "Pale Turquoise")
            {
                color = Color.PaleTurquoise;
            }
            else if (strColorName == "Pale Violet Red")
            {
                color = Color.PaleVioletRed;
            }
            else if (strColorName == "Papaya Whip")
            {
                color = Color.PapayaWhip;
            }
            else if (strColorName == "Peach Puff")
            {
                color = Color.PeachPuff;
            }
            else if (strColorName == "Peru")
            {
                color = Color.Peru;
            }
            else if (strColorName == "Pink")
            {
                color = Color.Pink;
            }
            else if (strColorName == "Plum")
            {
                color = Color.Plum;
            }
            else if (strColorName == "Powder Blue")
            {
                color = Color.PowderBlue;
            }
            else if (strColorName == "Purple")
            {
                color = Color.Purple;
            }
            else if (strColorName == "Red")
            {
                color = Color.Red;
            }
            else if (strColorName == "Rosy Brown")
            {
                color = Color.RosyBrown;
            }
            else if (strColorName == "Royal Blue")
            {
                color = Color.RoyalBlue;
            }
            else if (strColorName == "Saddle Brown")
            {
                color = Color.SaddleBrown;
            }
            else if (strColorName == "Salmon")
            {
                color = Color.Salmon;
            }
            else if (strColorName == "Sandy Brown")
            {
                color = Color.SandyBrown;
            }
            else if (strColorName == "Sea Green")
            {
                color = Color.SeaGreen;
            }
            else if (strColorName == "Sea Shell")
            {
                color = Color.SeaShell;
            }
            else if (strColorName == "Sienna")
            {
                color = Color.Sienna;
            }
            else if (strColorName == "Silver")
            {
                color = Color.Silver;
            }
            else if (strColorName == "Sky Blue")
            {
                color = Color.SkyBlue;
            }
            else if (strColorName == "Slate Blue")
            {
                color = Color.SlateBlue;
            }
            else if (strColorName == "Slate Gray")
            {
                color = Color.SlateGray;
            }
            else if (strColorName == "Snow")
            {
                color = Color.Snow;
            }
            else if (strColorName == "Spring Green")
            {
                color = Color.SpringGreen;
            }
            else if (strColorName == "Steel Blue")
            {
                color = Color.SteelBlue;
            }
            else if (strColorName == "Tan")
            {
                color = Color.Tan;
            }
            else if (strColorName == "Teal")
            {
                color = Color.Teal;
            }
            else if (strColorName == "Thistle")
            {
                color = Color.Thistle;
            }
            else if (strColorName == "Tomato")
            {
                color = Color.Tomato;
            }
            else if (strColorName == "Transplant")
            {
                color = Color.Transparent;
            }
            else if (strColorName == "Turquoise")
            {
                color = Color.Turquoise;
            }
            else if (strColorName == "Violet")
            {
                color = Color.Violet;
            }
            else if (strColorName == "Wheat")
            {
                color = Color.Wheat;
            }
            else if (strColorName == "White")
            {
                color = Color.White;
            }
            else if (strColorName == "White Smoke")
            {
                color = Color.WhiteSmoke;
            }
            else if (strColorName == "Yellow")
            {
                color = Color.Yellow;
            }
            else if (strColorName == "Yellow Green")
            {
                color = Color.YellowGreen;
            }
            else
            {
                color = Color.Black;
            }

            return color;
        }

        #endregion

        /// <summary>
        /// konstruktor třídy
        /// </summary>
        /// <param name="iniFileName">název ini souboru</param>
        public IniFile(string iniFileName)
        {
            FileName = iniFileName;
        }

        /// <summary>
        /// název ini souboru
        /// </summary>
        public string FileName
        {
            get { return _mFileName; }
            set
            {
                if (value.Trim() == _mFileName) return;
                _mFileName = value;
                LoadIniToDataSet();
            }
        }

        /// <summary>
        /// dataset do kterého se budou ukládat načtené hodnoty
        /// </summary>
        public DataSet DataSet
        {
            get { return _mDs; }
        }

        /// <summary>
        /// načte řetězec z ini souboru
        /// </summary>
        /// <param name="section">název sekce</param>
        /// <param name="key">název klíče</param>
        /// <returns>hodnota</returns>
        public string ReadString(string section, string key)
        {
            return ReadString(section, key, "");
        }

        /// <summary>
        /// načte řetězec z ini souboru
        /// </summary>
        /// <param name="section">sekce</param>
        /// <param name="key">klíč</param>
        /// <param name="defaultValue">defaultní hodnota</param>
        /// <returns>hodnota</returns>
        public string ReadString(string section, string key, string defaultValue)
        {
            return Read(section, key, defaultValue);
        }

        /// <summary>
        /// načte řetězec z ini souboru poskytnutého jako parametr
        /// </summary>
        /// <param name="section">sekce</param>
        /// <param name="key">klíč</param>
        /// <param name="defaultValue">defaultní hodnota</param>
        /// <param name="iniFileName">ini soubor</param>
        /// <returns>hodnota</returns>
        public string ReadString(string section, string key, string defaultValue, string iniFileName)
        {
            FileName = iniFileName;
            return ReadString(section, key, defaultValue);
        }

        /// <summary>
        /// načte boolean (true nebo false) hodnotu z ini souboru
        /// </summary>
        /// <param name="section">sekce</param>
        /// <param name="key">klíč</param>
        /// <returns>hodnota</returns>
        public bool ReadBool(string section, string key)
        {
            return ReadBool(section, key, false);
        }

        /// <summary>
        /// načte číslo z ini souboru
        /// </summary>
        /// <param name="section">sekce</param>
        /// <param name="key">klíč</param>
        /// <returns>hodnota</returns>
        public int ReadInteger(string section, string key)
        {
            return ReadInteger(section, key, 0);
        }

        /// <summary>
        /// načte boolean (true nebo false) hodnotu z ini souboru
        /// </summary>
        /// <param name="section">sekce</param>
        /// <param name="key">klíč</param>
        /// <param name="defaultValue">defaultní hodnota</param>
        /// <returns>hodnota</returns>
        public bool ReadBool(string section, string key, bool defaultValue)
        {
            bool ret;
            string tmpRet;
            tmpRet = Read(section, key, defaultValue.ToString());
            try
            {
                ret = Convert.ToBoolean(tmpRet);
            }
            catch
            {
                ret = defaultValue;
            }
            return ret;
        }

        /// <summary>
        /// načte číslo z ini souboru
        /// </summary>
        /// <param name="section">sekce</param>
        /// <param name="key">klíč</param>
        /// <param name="defaultValue">defaultní hodnota</param>
        /// <returns>hodnota</returns>
        public int ReadInteger(string section, string key, int defaultValue)
        {
            int ret;
            string tmpRet = Read(section, key, defaultValue.ToString(CultureInfo.InvariantCulture));
            try
            {
                ret = (Convert.ToInt32(tmpRet));
            }
            catch
            {
                ret = defaultValue;
            }
            return ret;
        }

        /// <summary>
        /// načte číslo z ini souboru poskytnutého jako parametr
        /// </summary>
        /// <param name="section">sekce</param>
        /// <param name="key">klíč</param>
        /// <param name="defaultValue">defaultní hodnota</param>
        /// <param name="iniFileName">ini soubor</param>
        /// <returns>hodnota</returns>
        public int ReadInteger(string section, string key, int defaultValue, string iniFileName)
        {
            FileName = iniFileName;
            return ReadInteger(section, key, defaultValue);
        }

        /// <summary>
        /// načte boolean (true nebo false) hodnotu z ini souboru
        /// </summary>
        /// <param name="section">sekce</param>
        /// <param name="key">klíč</param>
        /// <param name="defaultValue">defaultní hodnota</param>
        /// <param name="iniFileName">ini soubor</param>
        /// <returns>hodnota</returns>
        public bool ReadBool(string section, string key, bool defaultValue, string iniFileName)
        {
            FileName = iniFileName;
            return ReadBool(section, key, defaultValue);
        }

        /// <summary>
        /// načte barvu z ini souboru
        /// </summary>
        /// <param name="section">sekce</param>
        /// <param name="key">klíč</param>
        /// <param name="defaultValue">defaultní hodnota</param>
        /// <returns>barva</returns>
        public Color ReadColor(string section, string key, Color defaultValue)
        {
            return GetColorFromString(ReadString(section, key, GetStringFromColor(defaultValue)));
        }

        /// <summary>
        /// načte barvu z ini souboru
        /// </summary>
        /// <param name="section">sekce</param>
        /// <param name="key">klíč</param>
        /// <returns>barva</returns>
        public Color ReadColor(string section, string key)
        {
            return ReadColor(section, key, Color.Black);
        }

        /// <summary>
        /// načte barvu z ini souboru
        /// </summary>
        /// <param name="section">sekce</param>
        /// <param name="key">klíč</param>
        /// <param name="defaultValue">standartní hodnota</param>
        /// <param name="iniFileName">jméno ini souboru</param>
        /// <returns>barva</returns>
        public Color ReadColor(string section, string key, Color defaultValue, string iniFileName)
        {
            FileName = iniFileName;
            return ReadColor(section, key, defaultValue);
        }

        /// <summary>
        /// získá z ini souboru jména sekcí
        /// </summary>
        /// <returns>pole s názvy sekcí</returns>
        public ArrayList SectionNames()
        {
            var ret = new ArrayList();
            //System.Data.DataTable table;
            foreach (DataTable table in _mDs.Tables)
            {
                ret.Add(table.TableName);
            }
            return ret;
        }

        /// <summary>
        /// zapíše do ini souboru řetězec
        /// </summary>
        /// <param name="section">sekce</param>
        /// <param name="key">klíč</param>
        /// <param name="value">hodnota</param>
        public void WriteString(string section, string key, string value)
        {
            Write(section, key, value);
            DumpDatasetToIni();
        }

        /// <summary>
        /// zapíše řetězec do ini souboru poskytnutého jako parametr
        /// </summary>
        /// <param name="section">sekce</param>
        /// <param name="key">klíč</param>
        /// <param name="value">hodnota</param>
        /// <param name="iniFileName">ini soubor</param>
        public void WriteString(string section, string key, string value, string iniFileName)
        {
            FileName = iniFileName;
            WriteString(section, key, value);
        }

        /// <summary>
        /// zapíše do ini souboru číslo
        /// </summary>
        /// <param name="section">sekce</param>
        /// <param name="key">klíč</param>
        /// <param name="value">hodnota</param>
        public void WriteInteger(string section, string key, int value)
        {
            WriteString(section, key, value.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// zapíše bool hodnotu do ini souboru
        /// </summary>
        /// <param name="section">sekce</param>
        /// <param name="key">klíč</param>
        /// <param name="value">hodnota</param>
        public void WriteBool(string section, string key, bool value)
        {
            WriteString(section, key, value.ToString());
        }

        /// <summary>
        /// zapíše do ini souboru číslo. název ini souboru poskytnut jako parametr
        /// </summary>
        /// <param name="section">sekce</param>
        /// <param name="key">klíč</param>
        /// <param name="value">hodnota</param>
        /// <param name="iniFileName">ini soubor</param>
        public void WriteInteger(string section, string key, int value, string iniFileName)
        {
            WriteString(section, key, value.ToString(CultureInfo.InvariantCulture), iniFileName);
        }

        /// <summary>
        /// zapíše bool hodnotu do ini souboru, název souboru jako parametr
        /// </summary>
        /// <param name="section">sekce</param>
        /// <param name="key">klíč</param>
        /// <param name="value">hodnota</param>
        /// <param name="iniFileName">ini soubor</param>
        public void WriteBool(string section, string key, bool value, string iniFileName)
        {
            WriteString(section, key, value.ToString(), iniFileName);
        }

        /// <summary>
        /// zapíše barvu do ini souboru, název jako parametr
        /// </summary>
        /// <param name="section">sekce</param>
        /// <param name="key">klíč</param>
        /// <param name="value">hodnota</param>
        /// <param name="iniFileName">ini soubor</param>
        public void WriteColor(string section, string key, Color value, string iniFileName)
        {
            WriteString(section, key, GetStringFromColor(value), iniFileName);
        }

        /// <summary>
        /// Načte celou sekci do pole řetězců
        /// v případě, že se v sekci vyskytují standartní ini záznamy
        /// (key=value) se načtou pouze názvy klíčů, nikoliv jejich hodnoty
        /// </summary>
        /// <param name="section">název sekce</param>
        /// <returns>názvy klíčů sekce</returns>
        public string[] ReadSectionToArray(string section)
        {
            var result = new string[_mDs.Tables[section].Columns.Count];
            for (int i = 0; i < _mDs.Tables[section].Columns.Count; i++)
            {
                result[i] = _mDs.Tables[section].Columns[i].ColumnName;
            }
            return result;
        }

        /// <summary>
        /// to samé jako ReadSectionToArray
        /// </summary>
        /// <param name="section">název sekce</param>
        /// <returns>názvy klíčů sekce jako jeden řetězec</returns>
        public string ReadSection(string section)
        {
            string[] sc = ReadSectionToArray(section);
            string result = "";
            foreach (string s in sc)
            {
                result += s + "\n";
            }
            return result;
        }

        /// <summary>
        /// zapíše barvu do teď otevřeného ini souboru
        /// </summary>
        /// <param name="section">sekce</param>
        /// <param name="key">klíč</param>
        /// <param name="value">hodnota</param>
        public void WriteColor(string section, string key, Color value)
        {
            WriteString(section, key, GetStringFromColor(value));
        }

        /// <summary>
        /// smaže z ini souboru jednu sekci
        /// </summary>
        /// <param name="section">název sekce</param>
        public void DeleteSection(string section)
        {
            if (!((_mDs.Tables[section] == null)))
            {
                _mDs.Tables.Remove(section);
                DumpDatasetToIni();
            }
        }

        /// <summary>
        /// smaže z ini souboru jednu sekci
        /// </summary>
        /// <param name="section">sekce</param>
        /// <param name="iniFileName">ini soubor</param>
        public void DeleteSection(string section, string iniFileName)
        {
            FileName = iniFileName;
            DeleteSection(section);
        }

        private string Read(string section, string key, string defaultValue)
        {
            string ret;
            try
            {
                ret = _mDs.Tables[section].Rows[0][key].ToString();
            }
            catch
            {
                ret = defaultValue;
            }
            return ret;
        }

        private void Write(string section, string key, string value)
        {
            if ((_mDs.Tables[section] == null))
            {
                _mDs.Tables.Add(section);
                _mDs.Tables[section].Columns.Add(key);
                DataRow row;
                row = _mDs.Tables[section].NewRow();
                row[key] = value;
                _mDs.Tables[section].Rows.Add(row);
            }
            else
            {
                if ((_mDs.Tables[section].Columns[key] == null))
                {
                    _mDs.Tables[section].Columns.Add(key);
                }
                _mDs.Tables[section].Rows[0][key] = value;
            }
        }

        private void LoadIniToDataSet()
        {
            _mDs = new DataSet();
            var file = new FileInfo(_mFileName);
            _mDs.DataSetName = file.Name.Remove(file.Name.IndexOf(file.Extension), file.Extension.Length);
            if (file.Exists)
            {
                DataTable table = null;
                DataRow row = null;
                bool addRow = false;
                bool skipSection = false;
                var fileStream = new StreamReader(_mFileName);
                string readLine;
                readLine = fileStream.ReadLine();
                while (readLine != null)
                {
                    readLine = readLine.Trim();
                    if (readLine != "" & !(readLine.StartsWith(";")))
                    {
                        if (readLine.StartsWith("[") && readLine.EndsWith("]"))
                        {
                            if (addRow)
                            {
                                table.Rows.Add(row);
                            }

                            readLine = readLine.TrimStart('[');
                            readLine = readLine.TrimEnd(']');
                            skipSection = true;
                            table = _mDs.Tables[readLine];
                            if ((table == null))
                            {
                                table = new DataTable(readLine);
                                _mDs.Tables.Add(table);
                                row = table.NewRow();
                                skipSection = false;
                            }
                            addRow = false;
                        }
                        else
                        {
                            if (!(skipSection))
                            {
                                string[] splitLine = readLine.Split('=');
                                if (table != null && (table.Columns[splitLine[0]] == null))
                                {
                                    table.Columns.Add(splitLine[0]);
                                    if (splitLine.Length == 2)
                                    {
                                        row[splitLine[0]] = splitLine[1];
                                    }
                                    else
                                    {
                                        row[splitLine[0]] = "";
                                    }
                                    addRow = true;
                                }
                            }
                        }
                    }
                    readLine = fileStream.ReadLine();
                }
                if (addRow)
                {
                    table.Rows.Add(row);
                }
                fileStream.Close();
            }
        }

        private void DumpDatasetToIni()
        {
            if (File.Exists(_mFileName))
            {
                File.Delete(_mFileName);
            }
            StreamWriter file = File.CreateText(_mFileName);
            //System.Data.DataTable table;
            //System.Data.DataColumn col;
            string value;
            foreach (DataTable table in _mDs.Tables)
            {
                file.WriteLine("[" + table.TableName + "]");
                foreach (DataColumn col in table.Columns)
                {
                    value = table.Rows[0][col].ToString();
                    file.WriteLine(col.ColumnName + "=" + value);
                }
                file.WriteLine("");
            }
            file.Close();
        }
    }
}