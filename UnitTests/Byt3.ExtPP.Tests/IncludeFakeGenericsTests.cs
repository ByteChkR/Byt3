using System.Collections.Generic;
using System.IO;
using Byt3.ExtPP.Base.Interfaces;
using Byt3.ExtPP.Base.Plugins;
using Byt3.ExtPP.Plugins;
using NUnit.Framework;

namespace Byt3.ExtPP.Tests
{
    public class IncludeFakeGenericsTests
    {
        private static string ResourceFolder { get; } = TestHelper.ResourceFolder + "compiler_tests/";


        [Test]
        public void ExtPP_Plugins_Include_Circular_Test()
        {
            string file = Path.Combine(ResourceFolder, "includecircular.cl");
            ISourceScript[] ret =
                TestHelper.SetUpAndProcess(new List<AbstractPlugin> {new IncludePlugin()}, new[] {file});

            Assert.AreEqual(
                3,
                ret.Length);
        }

        [Test]
        public void ExtPP_Plugins_Include_FakeGenerics_GenericCircular_Test()
        {
            string file = Path.Combine(ResourceFolder, "genericincludepassthrough.cl");
            ISourceScript[] ret =
                TestHelper.SetUpAndProcess(new List<AbstractPlugin> {new FakeGenericsPlugin(), new IncludePlugin(),},
                    new[] {file});
            Assert.AreEqual(
                5,
                ret.Length);
        }

        [Test]
        public void ExtPP_Plugins_Include_FakeGenerics_TypePassing_Test()
        {
            string file = Path.Combine(ResourceFolder, "typePassing.cl");
            ISourceScript[] ret =
                TestHelper.SetUpAndProcess(new List<AbstractPlugin> {new FakeGenericsPlugin(), new IncludePlugin(),},
                    new[] {file});

            Assert.AreEqual(
                4,
                ret.Length);
        }
    }
}