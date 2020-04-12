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

        private static string ResourceFolder { get; } = TestHelper.ResF + "EX_tests/";



        [Fact]
        public  void ExtPP_Plugins_Exception_Warning_Test()
        {
            string file = Path.Combine(ResourceFolder, "warning_test.txt");
            //Directory.SetCurrentDirectory(ResourceFolder);
            Settings s = new Settings(new Dictionary<string, string[]>
            {
                {"-ex:tow", new string[0]}
            });
            try
            {
                TestHelper.SetUpAndProcess(new List<AbstractPlugin> { new ExceptionPlugin() }, s,file);
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
            string file = Path.Combine(ResourceFolder, "error_test.txt");
            //Directory.SetCurrentDirectory(ResourceFolder);
            try
            {
                TestHelper.SetUpAndProcess(new List<AbstractPlugin> { new ExceptionPlugin() }, file);
                Assert.True(false);
            }
            catch (Exception)
            {
                //It will throw if it works.
            }
        }
    }
}