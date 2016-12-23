using ArachNGIN.Files.Crc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.IO;

namespace ArachNGIN.Tests.Files.Crc
{
    [TestClass]
    public class AnimeCrcTest
    {
        [TestMethod]
        public void CalculateTextFileCrcFromFile()
        {
            AnimeCrc.GetCrcFromFile("simple-crc-test.7763A7AE.cmd").ToUpperInvariant().ShouldBe("7763A7AE");
        }

        [TestMethod]
        public void CalculateTextFileCrcFromStream()
        {
            using (var stream = new FileStream("simple-crc-test.7763A7AE.cmd", FileMode.Open, FileAccess.Read))
            {
                AnimeCrc.GetCrcFromStream(stream).ToUpperInvariant().ShouldBe("7763A7AE");
            }
        }

        [TestMethod]
        public void CalculateBinaryFileCrcFromFile()
        {
            AnimeCrc.GetCrcFromFile("MV_Root.3612.vp").ToUpperInvariant().ShouldBe("F8905EDF");
        }

        [TestMethod]
        public void CalculateBinaryFileCrcFromStream()
        {
            using (var stream = new FileStream("MV_Root.3612.vp", FileMode.Open, FileAccess.Read))
            {
                AnimeCrc.GetCrcFromStream(stream).ToUpperInvariant().ShouldBe("F8905EDF");
            }
        }
    }
}