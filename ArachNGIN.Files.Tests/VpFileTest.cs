// <copyright file="VpFileTest.cs" company="X-C Soft ltd.">Copyright © X-C Soft ltd. 2007 - 2014</copyright>
using ArachNGIN.Files.FileFormats;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;

namespace ArachNGIN.Files.FileFormats.Tests
{
    /// <summary>Tato třída obsahuje parametrizované testy částí pro VpFile.</summary>
    [PexClass(typeof(VpFile))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class VpFileTest
    {
        private const string _testVpFile = "MV_Root.3612.vp";
        private VpFile _vp;

        [TestInitialize]
        public void SetupTests()
        {
            _vp = new VpFile(_testVpFile);
        }

        [TestMethod]
        public void TestVpFileCreation()
        {
            _vp.ShouldNotBeNull();
            _vp.Files.ShouldNotBeNull();
            _vp.Files.ShouldNotBeEmpty();
            _vp.FileName.ShouldNotBeNullOrWhiteSpace();
            Console.WriteLine("file {0} contains:", _vp.FileName);
            foreach (var vpDirEntry in _vp.Files)
            {
                Console.WriteLine(vpDirEntry.ToString());
            }
        }

        [TestMethod]
        public void TestVpFileLoadedProperly()
        {
            _vp.FileName.ShouldNotBeNullOrWhiteSpace();
            _vp.Files.ShouldNotBeEmpty();
            _vp.Files.ShouldContain(entry => entry.FileName == "data\\effects\\blur-f.sdr");
        }

        [TestMethod]
        public void TestFileExists()
        {
            _vp.Exists("data\\effects\\blur-f.sdr").ShouldBeTrue();
            _vp.Exists("nonsential\\file\\name.extension").ShouldBeFalse();
        }
    }
}