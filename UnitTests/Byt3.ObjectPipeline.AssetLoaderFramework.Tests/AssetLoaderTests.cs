using System;
using Xunit;

namespace Byt3.ObjectPipeline.AssetLoaderFramework.Tests
{
    
    public class AssetLoaderTests
    {
        [Fact]
        public void AssetLoader_ExceptionsTest()
        {

            AssetLoader<object> loader = new AssetLoader<object>();

            Assert.Throws<InvalidOperationException>(() => loader.SetFileLoader(null));
            Assert.Throws<UnknownFileFormatException>(() => loader.Process("File.unknown_extension"));

        }
    }
}
