using System.Collections.Generic;
using System.IO;
using Byt3.ExtPP.Base;
using Byt3.ExtPP.Base.Interfaces;
using Byt3.ExtPP.Plugins;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Byt3.ExtPP.Tests
{
    [TestClass]
    public class IncludeFakeGenericsTests
    {
        private static string ResourceFolder { get; } = TestHelper.ResourceFolder + "compiler_tests/";
        [TestInitialize]
        public void SetUp()
        {
            TestHelper.SetupPath();
        }

        [TestMethod]
        public void ExtPP_Plugins_Include_Circular_Test()
        {
            Directory.SetCurrentDirectory(ResourceFolder);
            ISourceScript[] ret = TestHelper.SetUpAndProcess(new List<AbstractPlugin> { new IncludePlugin() }, new[] { "includecircular.cl" });

            Assert.AreEqual(
                ret.Length,
                3);
        }

        [TestMethod]
        public void ExtPP_Plugins_Include_FakeGenerics_GenericCircular_Test()
        {
            Directory.SetCurrentDirectory(ResourceFolder);
            ISourceScript[] ret = TestHelper.SetUpAndProcess(new List<AbstractPlugin> { new FakeGenericsPlugin(), new IncludePlugin(), }, new[] { "genericincludepassthrough.cl" });
            Assert.AreEqual(
                ret.Length,
                5);
        }

        [TestMethod]
        public void ExtPP_Plugins_Include_FakeGenerics_TypePassing_Test()
        {
            Directory.SetCurrentDirectory(ResourceFolder);
            ISourceScript[] ret = TestHelper.SetUpAndProcess(new List<AbstractPlugin> { new FakeGenericsPlugin(), new IncludePlugin(), }, new[] { "typePassing.cl" });

            Assert.AreEqual(
                ret.Length,
                4);
        }
    }
}