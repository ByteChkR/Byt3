using System.Collections.Generic;
using System.IO;
using Byt3.ExtPP.Base.Plugins;
using Byt3.ExtPP.Base.settings;
using Byt3.ExtPP.Plugins;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Byt3.ExtPP.Tests
{
    [TestClass]
    public  class ChangeCharCaseTests
    {

        private static string ResourceFolder { get; } = TestHelper.ResourceFolder + "CCC_tests/";

        [TestInitialize]
        public  void SetUp()
        {
            TestHelper.SetupPath();
        }

        [TestMethod]
        public  void ExtPP_Plugins_ChangeCharCase_ToLower_Test()
        {
            Directory.SetCurrentDirectory(ResourceFolder);
            string[] ret = TestHelper.SetUpAndCompile(new List<AbstractPlugin> { new ChangeCharCase() }, "tolower_test.txt");
            Assert.IsTrue(ret[0]== "hello_this_works right?");
        }

        [TestMethod]
        public  void ExtPP_Plugins_ChangeCharCase_ToUpper_Test()
        {
            Directory.SetCurrentDirectory(ResourceFolder);
            string[] ret = TestHelper.SetUpAndCompile(
                new List<AbstractPlugin> { new ChangeCharCase() }, 
                new Settings(new Dictionary<string, string[]>
                {
                    {"-ccc:sc", new []{"toupper"} }
                }),
                "toupper_test.txt");
            Assert.IsTrue(ret[0] == "HELLO_THIS_WORKS RIGHT?");
        }
    }
}