// <copyright file="VpFileTest.cs" company="X-C Soft ltd.">Copyright © X-C Soft ltd. 2007 - 2014</copyright>

using ArachNGIN.Files.FileFormats;
using ArachNGIN.Files.Streams;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;

namespace ArachNGIN.Tests.Files.FileFormats
{
    /// <summary>Tato třída obsahuje parametrizované testy částí pro VpFile.</summary>
    [TestClass]
    public class VpFileTest
    {
        private const string TestVpFile = "MV_Root.3612.vp";
        private const string TestTextFile = "data\\tables\\post_processing.tbl";
        private VpFile _vp;

        [TestInitialize]
        public void SetupTests()
        {
            _vp = new VpFile(TestVpFile);
        }

        [TestMethod]
        public void TestVpFileCreation()
        {
            _vp.ShouldNotBeNull();
            _vp.Files.ShouldNotBeNull();
            _vp.Files.ShouldNotBeEmpty();
            _vp.FileName.ShouldNotBeNullOrWhiteSpace();
            _vp.ValidFile.ShouldBeTrue();
            Console.WriteLine("File {0} contains:", _vp.FileName);
            foreach (var vpDirEntry in _vp.Files)
                Console.WriteLine(vpDirEntry.ToString());
        }

        [TestMethod]
        public void TestVpFileLoadedProperly()
        {
            _vp.FileName.ShouldNotBeNullOrWhiteSpace();
            _vp.Files.ShouldNotBeEmpty();
            _vp.ValidFile.ShouldBeTrue();
            _vp.Files.ShouldContain(entry => entry.FileName == "data\\effects\\blur-f.sdr");
        }

        [TestMethod]
        public void TestFileExists()
        {
            _vp.ValidFile.ShouldBeTrue();
            _vp.Exists("data\\effects\\blur-f.sdr").ShouldBeTrue();
            _vp.Exists("nonsential\\file\\name.extension").ShouldBeFalse();
        }

        [TestMethod]
        public void TestStreamOutput()
        {
            _vp.ValidFile.ShouldBeTrue();
            var stream = _vp.GetStream(TestTextFile);
            stream.ShouldNotBeNull();
            stream.Length.ShouldNotBe(0);
            Console.WriteLine("Contents of file {0} follows:", TestTextFile);
            Console.WriteLine(StreamHandling.StreamToString(stream));
        }
    }
}