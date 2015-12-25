// <copyright file="VpFileTest.cs" company="X-C Soft ltd.">Copyright © X-C Soft ltd. 2007 - 2014</copyright>
using ArachNGIN.Files.FileFormats;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        [TestMethod]
        public void TestVpFileCreation()
        {
            var vp = new VpFile("MV_Root.3612.vp");
            Assert.IsNotNull(vp);
        }
    }
}