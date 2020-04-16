using System.Collections.Generic;
using System.IO;
using Byt3.ExtPP.Base.Plugins;
using Byt3.ExtPP.Plugins;
using NUnit.Framework;

namespace Byt3.ExtPP.Tests
{
    public class BlankLineRemoverTests
    {
        private static string ResourceFolder { get; } = TestHelper.ResF + "BLR_tests/";

        public BlankLineRemoverTests()
        {
        }


        [Test]
        public void ExtPP_Plugins_BlankLineRemover_Test()
        {
            // Directory.SetCurrentDirectory(ResourceFolder);
            string[] ret = TestHelper.SetUpAndCompile(new List<AbstractPlugin> {new BlankLineRemover()},
                Path.Combine(ResourceFolder, "blankline_test.txt"));
            Assert.True(ret.Length == 0);
        }
    }
}