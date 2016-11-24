using ArachNGIN.Files.CRC;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;

namespace ArachNGIN.Files.Tests
{
    [TestClass]
    public class AnimeCrcTest
    {
        [TestMethod]
        public void TestTestTextFileCrc()
        {
            AnimeCrc.GetCrcFromFile("simple-crc-test.7763A7AE.cmd").ToUpper().ShouldBe("7763A7AE");
        }

        [TestMethod]
        public void TestBinaryFileCrc()
        {
            AnimeCrc.GetCrcFromFile("MV_Root.3612.vp").ToUpper().ShouldBe("F8905EDF");
        }
    }
}