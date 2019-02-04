using ArachNGIN.CommandLine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace ArachNGIN.Tests.CommandLine
{
    [TestClass]
    public class ParametersTest
    {
        private readonly string[] _testParameters = { "debug", "test", "-aaa", "\"bbb\"", "/action", "do_action", "--size=100", "/color:blue" };

        private readonly string[] _testParametersWithPath =
        {
            @"/file1:d:\debug.log1", @"--file2=d:\debug.log2", @"-file3",
            @"d:\debug.log3"
        };

        private readonly string[] _testParametersWithPathWithSpaces =
        {
            "/file1:\"d:\\de bug.log1\"", "--file2=\"d:\\de bug.log2\"",
            "-file3", @"d:\de bug.log 3"
        };

        private readonly string[] _testGitVersionParameters =
        {
            "\"d:\\dev\\gitversioner\\GitVersioner.bat\"", "-a",
            @"""d:\dev\gitversioner\Properties\AssemblyInfo.cs"""
        };

        [TestMethod]
        public void TestParameters()
        {
            var args = new Parameters(_testParameters);
            args["debug"].ShouldBe("false");
            args["test"].ShouldBe("false");
            args["size"].ShouldBe("100");
            args["color"].ShouldBe("blue");
            args["action"].ShouldBe("do_action");
            args["aaa"].ShouldBe("bbb");
        }

        [TestMethod]
        public void TestParametersWithPaths()
        {
            var args = new Parameters(_testParametersWithPath);
            args["file1"].ShouldBe(@"d:\debug.log1");
            args["file2"].ShouldBe(@"d:\debug.log2");
            args["file3"].ShouldBe(@"d:\debug.log3");
        }

        [TestMethod]
        public void TestParametersWithPathsWithSpaces()
        {
            var args = new Parameters(_testParametersWithPathWithSpaces);
            args["file1"].ShouldBe(@"d:\de bug.log1");
            args["file2"].ShouldBe(@"d:\de bug.log2");
            args["file3"].ShouldBe(@"d:\de bug.log 3");
            args["d:\\de bug.log 3"].ShouldBeNull();
        }

        [TestMethod]
        public void TestGitVersionerParameters()
        {
            var args = new Parameters(_testGitVersionParameters);
            args["a"].ToLower().ShouldBe(@"d:\dev\gitversioner\properties\assemblyinfo.cs");
        }
    }
}