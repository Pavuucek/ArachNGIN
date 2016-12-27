using ArachNGIN.ClassExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ArachNGIN.Tests.ClassExtensions
{
    [TestClass]
    public class StringColorsTests
    {
        [TestMethod]
        public void ColorToNamedStringAndBack()
        {
            foreach (KnownColor knownColor in Enum.GetValues(typeof(KnownColor)))
            {
                var s = Color.FromKnownColor(knownColor).NamedColorToString();
                Console.Write(s);
                var c = s.NamedColorStringToColor();
                Console.Write(" => ");
                Console.Write(c.ToString());
                Console.WriteLine();
            }
        }
    }
}