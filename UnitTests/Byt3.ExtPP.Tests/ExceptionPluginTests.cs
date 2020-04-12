using System;
using System.Collections.Generic;
using System.IO;
using Byt3.ExtPP.Base.Plugins;
using Byt3.ExtPP.Base.settings;
using Byt3.ExtPP.Plugins;
using Xunit;


namespace Byt3.ExtPP.Tests
{
    
    public  class ExceptionPluginTests
    {

        private static string ResourceFolder { get; } = TestHelper.ResourceFolder + "EX_tests/";


        
        public ExceptionPluginTests()
        {
            TestHelper.SetupPath();
        }

        [Fact]
        public  void ExtPP_Plugins_Exception_Warning_Test()
        {
            Directory.SetCurrentDirectory(ResourceFolder);
            Settings s = new Settings(new Dictionary<string, string[]>
            {
                {"-ex:tow", new string[0]}
            });
            try
            {
                TestHelper.SetUpAndProcess(new List<AbstractPlugin> { new ExceptionPlugin() }, s, "warning_test.txt");
                Assert.True(false);
            }
            catch (Exception)
            {
                //It will throw if it works.
            }
        }
        [Fact]
        public  void ExtPP_Plugins_Exception_Error_Test()
        {
            Directory.SetCurrentDirectory(ResourceFolder);
            try
            {
                TestHelper.SetUpAndProcess(new List<AbstractPlugin> { new ExceptionPlugin() }, "error_test.txt");
                Assert.True(false);
            }
            catch (Exception)
            {
                //It will throw if it works.
            }
        }
    }
}