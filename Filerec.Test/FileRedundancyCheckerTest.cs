using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Filerec.RedundancyChecker;
using System.Diagnostics;

namespace Filerec.Test
{
    [TestClass]
    public class FileRedundancyCheckerTest
    {
        private TestContext testContextInstance;
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        [TestMethod]
        public void TestFileRedundancyChecker()
        {
            FileRedundancyChecker checker = new FileRedundancyChecker(compareNames: true, compareSize: true, compareStream: true);
            Assert.IsTrue(checker.FileEquals
                (
                    @"C:\path\to\your\testfile",
                    @"C:\path\to\your\second\testfile"
                ));

            Assert.IsFalse(checker.FileEquals
                (
                    @"C:\path\to\your\testfile",
                    @"C:\path\to\your\testfile"
                ));
        }
    }
}
