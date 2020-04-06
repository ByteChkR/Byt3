using System;
using System.Collections.Generic;
using System.IO;
using Byt3.ExtPP.Base;
using Byt3.ExtPP.Base.settings;
using Byt3.ExtPP.Plugins;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Byt3.ExtPP.Tests
{
    [TestClass]
    public  class ExceptionPluginTests
    {

        private static string ResourceFolder { get; } = TestHelper.ResourceFolder + "EX_tests/";


        [TestInitialize]
        public  void SetUp()
        {
            TestHelper.SetupPath();
        }

        [TestMethod]
        public  void ExtPP_Plugins_Exception_Warning_Test()
        {
            Directory.SetCurrentDirectory(ResourceFolder);
            PPLogger.Instance.ThrowOnWarning = true;
            Settings s = new Settings(new Dictionary<string, string[]>
            {
                {"-ex:tow", new string[0]}
            });
            try
            {
                TestHelper.SetUpAndProcess(new List<AbstractPlugin> { new ExceptionPlugin() }, s, "warning_test.txt");
                Assert.Fail();
            }
            catch (Exception)
            {
                //It will throw if it works.
            }
        }
        [TestMethod]
        public  void ExtPP_Plugins_Exception_Error_Test()
        {
            Directory.SetCurrentDirectory(ResourceFolder);
            PPLogger.Instance.ThrowOnError = true;
            try
            {
                TestHelper.SetUpAndProcess(new List<AbstractPlugin> { new ExceptionPlugin() }, "error_test.txt");
                Assert.Fail();
            }
            catch (Exception)
            {
                //It will throw if it works.
            }
        }
    }
}