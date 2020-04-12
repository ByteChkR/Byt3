using System.Collections.Generic;
using System.IO;
using Byt3.ExtPP.Base.Interfaces;
using Byt3.ExtPP.Base.Plugins;
using Byt3.ExtPP.Plugins;
using Xunit;

namespace Byt3.ExtPP.Tests
{
    public class TextEncoderPluginTests
    {
        private static string ResourceFolder { get; } = TestHelper.ResF + "TENC_tests/";


        [Fact]
        public void ExtPP_Plugins_TextEncoder_Base64BlockDecode_Test()
        {
            //Directory.SetCurrentDirectory(ResourceFolder);
            string file = Path.Combine(ResourceFolder, "decode_b64_test.txt");
            ISourceScript[] ret = TestHelper.SetUpAndProcess(new List<AbstractPlugin> {new TextEncoderPlugin()}, file);
            Assert.True(ret[0].GetSource()[0] == "HelloWASAAAAAAAAAAAABI");
        }


        [Fact]
        public void ExtPP_Plugins_TextEncoder_Base64BlockEncode_Test()
        {
            //Directory.SetCurrentDirectory(ResourceFolder);
            string file = Path.Combine(ResourceFolder, "encode_b64_test.txt");
            ISourceScript[] ret = TestHelper.SetUpAndProcess(new List<AbstractPlugin> {new TextEncoderPlugin()}, file);
            Assert.True(ret[0].GetSource()[0] == "SGVsbG9XQVNBQUFBQUFBQUFBQUFCSQ==");
        }

        [Fact]
        public void ExtPP_Plugins_TextEncoder_ROTBlockDecode_Test()
        {
            //Directory.SetCurrentDirectory(ResourceFolder);
            string file = Path.Combine(ResourceFolder, "decode_rot_test.txt");
            ISourceScript[] ret = TestHelper.SetUpAndProcess(new List<AbstractPlugin> {new TextEncoderPlugin()}, file);
            Assert.True(ret[0].GetSource()[0] == "Hello WASAAAAAAAAAAAABIZZ");
        }


        [Fact]
        public void ExtPP_Plugins_TextEncoder_ROTBlockEncode_Test()
        {
            //Directory.SetCurrentDirectory(ResourceFolder);
            string file = Path.Combine(ResourceFolder, "encode_rot_test.txt");
            ISourceScript[] ret = TestHelper.SetUpAndProcess(new List<AbstractPlugin> {new TextEncoderPlugin()}, file);
            Assert.True(ret[0].GetSource()[0] == "Ifmmp XBTBBBBBBBBBBBBCJAA");
        }
    }
}