using System.Collections.Generic;
using System.IO;
using Byt3.ExtPP.Base;
using Byt3.ExtPP.Plugins;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Byt3.ExtPP.Tests
{
    [TestClass]
    public class BlankLineRemoverTests
    {
        private static string ResourceFolder { get; } = TestHelper.ResourceFolder + "BLR_tests/";

        [TestInitialize]
        public void SetUp()
        {
            TestHelper.SetupPath();
        }

        [TestMethod]
        public void ExtPP_Plugins_BlankLineRemover_Test()
        {
            Directory.SetCurrentDirectory(ResourceFolder);
            string[] ret = TestHelper.SetUpAndCompile(new List<AbstractPlugin> { new BlankLineRemover() }, "blankline_test.txt");
            Assert.IsTrue(ret.Length == 0);
        }

    }
}