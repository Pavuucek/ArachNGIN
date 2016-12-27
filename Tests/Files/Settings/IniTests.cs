using ArachNGIN.Files.Settings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ArachNGIN.Tests.Files.Settings
{
    [TestClass]
    public class IniTests
    {
        private Ini ini = new Ini("inifile.ini");

        [TestMethod]
        public void WriteAndReadString()
        {
            ini.WriteString("section 1", "string 1", "value 1");
            ini.ReadString("section 1", "string 1", "This is incorrect default value").ShouldBe("value 1");
            ini.ReadString("section 1", "string 1").ShouldBe("value 1");
        }

        [TestMethod]
        public void WriteAndReadBool()
        {
            ini.WriteBool("section 2", "bool 1", false);
            ini.WriteBool("section 2", "bool 2", true);
            ini.ReadBool("section 2", "bool 2").ShouldBeTrue();
            ini.ReadBool("section 2", "bool 1", true).ShouldBeFalse();
        }

        [TestMethod]
        public void WriteAndReadInteger()
        {
            for (int i = 0; i < 20; i++)
            {
                ini.WriteInteger("section 3", "integer " + i, i);
            }
            for (int i = 20 - 1; i >= 0; i--)
            {
                ini.ReadInteger("section 3", "integer " + i).ShouldBe(i);
            }
        }

        [TestMethod]
        public void WriteAndReadColor()
        {
            var i = 1;
            foreach (KnownColor knownColor in Enum.GetValues(typeof(KnownColor)))
            {
                ini.WriteColor("section 4", "color " + i, Color.FromKnownColor(knownColor));
                ini.ReadColor("section 4", "color " + i).ShouldBe(Color.FromKnownColor(knownColor));
                i++;
            }
            var c = Color.FromArgb(250, 252, 253, 255);
            ini.WriteColor("section 4", "color 0", c);
            Console.WriteLine(ColorTranslator.ToHtml(c));
            ini.ReadColor("section 4", "color 0", Color.Black).ShouldBe(c);
        }

        [TestMethod]
        public void ReadSection()
        {
            ini.WriteString("section 5", "string 1", "valueeeee");
            ini.ReadSection("section 5").ShouldNotBeNullOrWhiteSpace();
            var s = ini.ReadSectionToArray("section 5");
            s.ShouldNotBeNull();
            s.ShouldNotBeEmpty();
        }

        [TestMethod]
        public void SectionNamesShouldContainASectionName()
        {
            ini.WriteString("section 6", "string 1", "valueeeee");
            ini.SectionNames().Contains("section 6").ShouldBeTrue();
            ini.SectionNames().Contains("section 6 a").ShouldBeFalse();
        }

        [TestMethod]
        public void DeleteSectionShouldWork()
        {
            ini.WriteString("section 7", "string 1", "valueeeee");
            ini.SectionNames().Contains("section 7").ShouldBeTrue();
            ini.DeleteSection("section 7");
            ini.SectionNames().Contains("section 7").ShouldBeFalse();
        }
    }
}