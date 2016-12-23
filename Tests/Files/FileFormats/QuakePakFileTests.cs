using ArachNGIN.Files.FileFormats;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ArachNGIN.Tests.Files.FileFormats
{
    [TestClass]
    public class QuakePakFileTests
    {
        private const string TestFile1 = "test.pak";
        private const string TestFile2 = "testwrite.pak";
        private const string SimpleFile = "simple.pak";

        [TestMethod]
        public void CreatePakFileAndVerifyFileExists()
        {
            QuakePakFile.CreateNewPak(TestFile1).ShouldBeTrue();
            File.Exists(TestFile1).ShouldBeTrue();
            File.Delete(TestFile1);
        }

        [TestMethod]
        public void CheckIfFileExistsInPakUsingPakFileExistsFunction()
        {
            var q = new QuakePakFile(SimpleFile, false);
            q.PakFileExists("simple.txt").ShouldBeTrue();
            q.PakFileExists("this_file_should_not_exist.txt").ShouldBeFalse();
        }

        [TestMethod]
        public void CheckIfFileExistsInPakUsingFileList()
        {
            var q = new QuakePakFile(SimpleFile, false);
            q.PakFileList.ShouldContain(f => f.Equals("simple.txt"));
            q.PakFileList.ShouldNotContain(f => f == "this_file_should_not_exist.txt");
        }

        [TestMethod]
        public void ReadTextFromFileInsidePakFile()
        {
            var q = new QuakePakFile(SimpleFile);
            using (var stream = new MemoryStream())
            {
                q.ExtractStream("simple.txt", stream);
                stream.Length.ShouldBeGreaterThan(0);
                var s = Encoding.Default.GetString(stream.ToArray());
                s.ShouldNotBeNullOrEmpty();
                (s == "simple pak test").ShouldBeTrue();
            }
        }

        [TestMethod]
        public void ExtractFileFromPak()
        {
            var q = new QuakePakFile(SimpleFile);
            q.ExtractFile("simple.txt", "simple_extracted.txt");
            File.Exists("simple_extracted.txt").ShouldBeTrue();
            using (var stream = new FileStream("simple_extracted.txt", FileMode.Open, FileAccess.Read))
            {
                using (var reader = new StreamReader(stream, Encoding.Default))
                {
                    var s = reader.ReadToEnd();
                    s.ShouldNotBeNullOrEmpty();
                    s.ShouldNotBeNullOrWhiteSpace();
                    (s == "simple pak test").ShouldBeTrue();
                }
            }
        }

        [TestMethod]
        public void AddFileToPakAndVerifyCrc()
        {
            if (File.Exists(TestFile2)) File.Delete(TestFile2);
            QuakePakFile.CreateNewPak(TestFile2);
            var q = new QuakePakFile(TestFile2, true);
            q.ShouldNotBeNull();
            q.AddFile("simple-crc-test.7763A7AE.cmd", "simple-crc-test.7763A7AE.cmd").ShouldBeTrue();
            q.PakFileExists("simple-crc-test.7763A7AE.cmd").ShouldBeTrue();
            using (var stream = new MemoryStream())
            {
                q.ExtractStream("simple-crc-test.7763A7AE.cmd", stream);
                ArachNGIN.Files.CRC.AnimeCrc.GetCrcFromStream(stream).ToUpperInvariant().ShouldBe("7763A7AE");
            }
        }

        [TestMethod]
        public void AttemptToCreatePakFileAndFail()
        {
            // open file handle
            using (var stream = new FileStream("file.pak", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
            {
                QuakePakFile.CreateNewPak("file.pak").ShouldBeFalse();
            }
            File.Delete("file.pak");
        }

        [TestMethod]
        public void TestProperties()
        {
            var q = new QuakePakFile(SimpleFile);
            q.FileName.ShouldBe(SimpleFile);
            q.PakFileList.ShouldNotBeNull();
            q.PakFileList.ShouldNotBeEmpty();
            q.WriteAccess.ShouldBeFalse();
        }
    }
}