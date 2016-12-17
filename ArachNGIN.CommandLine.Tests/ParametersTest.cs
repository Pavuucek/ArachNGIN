using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections;

namespace ArachNGIN.CommandLine.Tests
{
    [TestClass]
    public class ParametersTest
    {
        private string[] testParameters = { "action", "/debug", "--size=100", "/color:blue" };

        [TestMethod]
        public void TestParameters()
        {
            var args = new Parameters(testParameters);
            args["debug"].ShouldBe("true");
            args["size"].ShouldBe("100");
            args["color"].ShouldBe("blue");
            args["action"].ShouldBe("true");
        }
    }
}