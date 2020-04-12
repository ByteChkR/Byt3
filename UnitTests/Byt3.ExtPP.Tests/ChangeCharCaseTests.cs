using System.Collections.Generic;
using System.IO;
using Byt3.ExtPP.Base.Plugins;
using Byt3.ExtPP.Base.settings;
using Byt3.ExtPP.Plugins;
using Xunit;


namespace Byt3.ExtPP.Tests
{
    
    public  class ChangeCharCaseTests
    {

        private static string ResourceFolder { get; } = TestHelper.ResF + "CCC_tests/";

        [Fact]
        public  void ExtPP_Plugins_ChangeCharCase_ToLower_Test()
        {
            string file = Path.Combine(ResourceFolder, "tolower_test.txt");
            //Directory.SetCurrentDirectory(ResourceFolder);
            string[] ret = TestHelper.SetUpAndCompile(new List<AbstractPlugin> { new ChangeCharCase() }, file);
            Assert.True(ret[0]== "hello_this_works right?");
        }

        [Fact]
        public  void ExtPP_Plugins_ChangeCharCase_ToUpper_Test()
        {

            string file = Path.Combine(ResourceFolder, "toupper_test.txt");

            //Directory.SetCurrentDirectory(ResourceFolder);
            string[] ret = TestHelper.SetUpAndCompile(
                new List<AbstractPlugin> { new ChangeCharCase() }, 
                new Settings(new Dictionary<string, string[]>
                {
                    {"-ccc:sc", new []{"toupper"} }
                }),
                file);
            Assert.True(ret[0] == "HELLO_THIS_WORKS RIGHT?");
        }
    }
}