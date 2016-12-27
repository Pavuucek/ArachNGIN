using ArachNGIN.ClassExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Drawing;

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
                var s = Color.FromKnownColor(knownColor).Name;
                Console.Write(s);
                var c = s.FromString();
                Console.Write(" => ");
                Console.Write(c.ToString());
                Console.WriteLine();
                c.Name.ShouldBe(s);
            }
        }
    }
}