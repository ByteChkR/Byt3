using System;
using System.Collections.Generic;
using System.IO;
using Byt3.ExtPP.Base.Plugins;
using Byt3.ExtPP.Base.settings;
using Byt3.ExtPP.Plugins;
using NUnit.Framework;

namespace Byt3.ExtPP.Tests
{
    public class ExceptionPluginTests
    {
        private static string ResourceFolder { get; } = TestHelper.ResourceFolder + "EX_tests/";


        [Test]
        public void ExtPP_Plugins_Exception_Warning_Test()
        {
            string file = Path.Combine(ResourceFolder, "warning_test.txt");

            Settings s = new Settings(new Dictionary<string, string[]>
            {
                {"-ex:tow", new string[0]}
            });
            try
            {
                TestHelper.SetUpAndProcess(new List<AbstractPlugin> {new ExceptionPlugin()}, s, file);
                Assert.True(false);
            }
            catch (Exception)
            {
                //It will throw if it works.
            }
        }

        [Test]
        public void ExtPP_Plugins_Exception_Error_Test()
        {
            string file = Path.Combine(ResourceFolder, "error_test.txt");

            try
            {
                TestHelper.SetUpAndProcess(new List<AbstractPlugin> {new ExceptionPlugin()}, file);
                Assert.True(false);
            }
            catch (Exception)
            {
                //It will throw if it works.
            }
        }
    }
}