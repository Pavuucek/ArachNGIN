using ArachNGIN.ClassExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace ArachNGIN.Tests.ClassExtensions
{
    [TestClass]
    public class StringsTests
    {
        [TestMethod]
        public void TestAppendString()
        {
            var s = "this";
            s = s.Append("string");
            s.ShouldBe("thisstring");
        }

        [TestMethod]
        public void TestAppendChar()
        {
            var s = "ab";
            s = s.Append('c').Append('d');
            s.ShouldBe("abcd");
        }
    }
}