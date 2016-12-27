using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ArachNGIN.Tests.ClassExtensions
{
    public class StringColors
    {
        [TestMethod]
        public void ColorToNamedStringAndBack()
        {
            foreach (var knownColor in Enum.GetValues(typeof(KnownColor)))
            {
                var s = knownColor.NamedColorToString();
            }
        }
    }
}