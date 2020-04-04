using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Byt3.ObjectPipeline.AssetLoaderFramework.Tests
{
    [TestClass]
    public class AssetLoaderTests
    {
        [TestMethod]
        public void AssetLoader_ExceptionsTest()
        {

            AssetLoader<object> loader = new AssetLoader<object>();

            Assert.ThrowsException<InvalidOperationException>(() => loader.SetFileLoader(null));
            Assert.ThrowsException<UnknownFileFormatException>(() => loader.Process("File.unknown_extension"));

        }
    }
}
