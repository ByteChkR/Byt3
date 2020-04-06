using System.Collections.Generic;
using System.IO;
using Byt3.ExtPP.Base;
using Byt3.ExtPP.Plugins;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Byt3.ExtPP.Tests
{
    [TestClass]
    public class TextEncoderPluginTests
    {
        private static string ResourceFolder { get; } = TestHelper.ResourceFolder + "TENC_tests/";

        [TestInitialize]
        public void SetUp()
        {
            TestHelper.SetupPath();
        }

        [TestMethod]
        public void ExtPP_Plugins_TextEncoder_Base64BlockDecode_Test()
        {
            Directory.SetCurrentDirectory(ResourceFolder);
            var ret = TestHelper.SetUpAndProcess(new List<AbstractPlugin> { new TextEncoderPlugin() }, "decode_b64_test.txt");
            Assert.IsTrue(ret[0].GetSource()[0] == "HelloWASAAAAAAAAAAAABI");
        }



        [TestMethod]
        public void ExtPP_Plugins_TextEncoder_Base64BlockEncode_Test()
        {
            Directory.SetCurrentDirectory(ResourceFolder);
            var ret = TestHelper.SetUpAndProcess(new List<AbstractPlugin> { new TextEncoderPlugin() }, "encode_b64_test.txt");
            Assert.IsTrue(ret[0].GetSource()[0] == "SGVsbG9XQVNBQUFBQUFBQUFBQUFCSQ==");
        }

        [TestMethod]
        public void ExtPP_Plugins_TextEncoder_ROTBlockDecode_Test()
        {
            Directory.SetCurrentDirectory(ResourceFolder);
            var ret = TestHelper.SetUpAndProcess(new List<AbstractPlugin> { new TextEncoderPlugin() }, "decode_rot_test.txt");
            Assert.IsTrue(ret[0].GetSource()[0] == "Hello WASAAAAAAAAAAAABIZZ");
        }



        [TestMethod]
        public void ExtPP_Plugins_TextEncoder_ROTBlockEncode_Test()
        {
            Directory.SetCurrentDirectory(ResourceFolder);
            var ret = TestHelper.SetUpAndProcess(new List<AbstractPlugin> { new TextEncoderPlugin() }, "encode_rot_test.txt");
            Assert.IsTrue(ret[0].GetSource()[0] == "Ifmmp XBTBBBBBBBBBBBBCJAA");
        }
    }
}