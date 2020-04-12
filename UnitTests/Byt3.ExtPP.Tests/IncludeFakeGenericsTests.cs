using System.Collections.Generic;
using System.IO;
using Byt3.ExtPP.Base.Interfaces;
using Byt3.ExtPP.Base.Plugins;
using Byt3.ExtPP.Plugins;
using Xunit;

namespace Byt3.ExtPP.Tests
{
    public class IncludeFakeGenericsTests
    {
        private static string ResourceFolder { get; } = TestHelper.ResF + "compiler_tests/";


        [Fact]
        public void ExtPP_Plugins_Include_Circular_Test()
        {

            //Directory.SetCurrentDirectory(ResourceFolder);
            string file = Path.Combine(ResourceFolder, "includecircular.cl");
            ISourceScript[] ret =
                TestHelper.SetUpAndProcess(new List<AbstractPlugin> {new IncludePlugin()}, new[] {file});

            Assert.Equal(
                3,
                ret.Length);
        }

        [Fact]
        public void ExtPP_Plugins_Include_FakeGenerics_GenericCircular_Test()
        {
            //Directory.SetCurrentDirectory(ResourceFolder);
            string file = Path.Combine(ResourceFolder, "genericincludepassthrough.cl");
            ISourceScript[] ret =
                TestHelper.SetUpAndProcess(new List<AbstractPlugin> {new FakeGenericsPlugin(), new IncludePlugin(),},
                    new[] {file});
            Assert.Equal(
                5,
                ret.Length);
        }

        [Fact]
        public void ExtPP_Plugins_Include_FakeGenerics_TypePassing_Test()
        {
            //Directory.SetCurrentDirectory(ResourceFolder);
            string file = Path.Combine(ResourceFolder, "typePassing.cl");
            ISourceScript[] ret =
                TestHelper.SetUpAndProcess(new List<AbstractPlugin> {new FakeGenericsPlugin(), new IncludePlugin(),},
                    new[] {file});

            Assert.Equal(
                4,
                ret.Length);
        }
    }
}