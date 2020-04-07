using System.IO;
using Byt3.ADL.Configs;
using Byt3.ADL.Streams;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Byt3.ADL.Tests
{
    [TestClass]
    public class DebugOperations
    {
        [TestMethod]
        public void ADL_Debug_AddOutputStream_Test()
        {
            Debug.AdlEnabled = false;

            Debug.AddOutputStream(null);

            Assert.AreEqual(0, Debug.LogStreamCount);

            Debug.RemoveAllOutputStreams();
            Debug.AdlEnabled = false;
            LogStream ls = new LogStream(new MemoryStream());
            Debug.AddOutputStream(ls);
            Debug.AddOutputStream(ls);
            Assert.AreEqual(1, Debug.LogStreamCount);

            Debug.RemoveAllOutputStreams();
            
            Debug.AdlEnabled = true;
        }


        [TestMethod]
        public void ADL_Debug_AddPrefixForMask_Test()
        {
            //Flag is required to find tags made up of unique masks(example: 2|8)
            Debug.PrefixLookupMode =
                PrefixLookupSettings.Addprefixifavailable | PrefixLookupSettings.Deconstructmasktofind;

            ADLLogger logger = new ADLLogger("UnitTest");

            BitMask bm = new BitMask(2 | 8);

            Debug.AdlEnabled = false;
            logger.AddPrefixForMask(bm | 4, "Test");
            logger.AddPrefixForMask(bm | 4, "Test");
            logger.RemoveAllPrefixes();

            logger.AddPrefixForMask(bm, "HELLO");
            Debug.AdlEnabled = true;
            bool ret = logger.GetMaskPrefix(bm) == "HELLO";
            Assert.IsTrue(ret);
        }

        [TestMethod]
        public void ADL_Debug_RemovePrefixForMask_Test()
        {
            ADLLogger logger = new ADLLogger("UnitTest");
            BitMask bm = new BitMask(2 | 8);
            logger.AddPrefixForMask(bm, "HELLO");
            logger.RemovePrefixForMask(bm);


            Debug.AdlEnabled = false;

            logger.AddPrefixForMask(bm, "AAA");
            logger.RemovePrefixForMask(bm);
            Debug.AdlEnabled = true;
            
            Assert.IsTrue(logger.GetAllPrefixes().Count == 0);
        }

        [TestMethod]
        public void ADL_Debug_RemoveAllPrefixes_Test()
        {
            ADLLogger logger = new ADLLogger("UnitTest");
            BitMask bm = new BitMask(2 | 8);
            logger.AddPrefixForMask(bm, "HELLO");
            logger.RemoveAllPrefixes();
            Assert.IsTrue(logger.GetAllPrefixes().Count == 0);
        }

        [TestMethod]
        public void ADL_Debug_SetAllPrefixes_Test()
        {
            //Flag is required to find tags made up of unique masks(example: 2|8)
            Debug.PrefixLookupMode = PrefixLookupSettings.Addprefixifavailable;

            ADLLogger logger = new ADLLogger("UnitTest");

            logger.SetAllPrefixes("Hello", "HELLO1", "HOLA2");

            Assert.IsTrue(logger.GetMaskPrefix(1) == "Hello");
            Assert.IsTrue(logger.GetMaskPrefix(2) == "HELLO1");
            Assert.IsTrue(logger.GetMaskPrefix(4) == "HOLA2");

            logger.RemoveAllPrefixes();

            Debug.AdlEnabled = false;
            logger.SetAllPrefixes("Hello", "HELLO1", "HOLA2");

            Assert.IsTrue(logger.GetMaskPrefix(1) == "Hello");
            Assert.IsTrue(logger.GetMaskPrefix(2) == "HELLO1");
            Assert.IsTrue(logger.GetMaskPrefix(4) == "HOLA2");
            Debug.AdlEnabled = true;
        }

        [TestMethod]
        public void ADL_Debug_GetAllPrefixes_Test()
        {
            ADLLogger logger = new ADLLogger("UnitTest");
            //Flag is required to find tags made up of unique masks(example: 2|8)
            Debug.PrefixLookupMode = PrefixLookupSettings.Addprefixifavailable;


            logger.SetAllPrefixes("Hello", "HELLO1", "HOLA2");

            Assert.IsTrue(logger.GetAllPrefixes().Count == 3);

            logger.RemoveAllPrefixes();

            Debug.AdlEnabled = false;
            logger.SetAllPrefixes("Hello", "HELLO1", "HOLA2");

            Assert.IsTrue(logger.GetAllPrefixes().Count == 3);
            Debug.AdlEnabled = true;
        }


        [TestMethod]
        public void ADL_Debug_GetPrefixMask_Test()
        {
            ADLLogger logger = new ADLLogger("UnitTest");
            logger.SetAllPrefixes("Hello", "HELLO1", "HOLA2");
            if (logger.GetPrefixMask("Hello", out BitMask bm)) Assert.IsTrue(bm == 1);
            if (logger.GetPrefixMask("HELLO1", out bm)) Assert.IsTrue(bm == 2);
            if (logger.GetPrefixMask("HOLA2", out bm)) Assert.IsTrue(bm == 4);
        }

        [TestMethod]
        public void ADL_Debug_GetMaskPrefix_Test()
        {
            ADLLogger logger = new ADLLogger("UnitTest");
            //Flag is required to find tags made up of unique masks(example: 2|8)
            Debug.PrefixLookupMode = PrefixLookupSettings.Addprefixifavailable;

            logger.SetAllPrefixes("Hello", "HELLO1", "HOLA2");
            BitMask bm = 2;
            Assert.IsTrue(logger.GetMaskPrefix(bm) == "HELLO1");

            Debug.PrefixLookupMode = PrefixLookupSettings.Addprefixifavailable | PrefixLookupSettings.Onlyoneprefix;

            bm = 32;
            Assert.IsFalse(logger.GetMaskPrefix(bm) == "HELLO1");

            Debug.PrefixLookupMode = PrefixLookupSettings.Addprefixifavailable | PrefixLookupSettings.Bakeprefixes |
                                     PrefixLookupSettings.Deconstructmasktofind;

            bm = 32;
            Assert.IsFalse(logger.GetMaskPrefix(bm) == "HELLO1");
        }

        [TestMethod]
        public void ADL_Debug_LogSerialization_Test()
        {
            Log l = new Log(-1, "Hello");
            byte[] buf = l.Serialize();
            MemoryStream ms = new MemoryStream(buf);
            Log ltest = Log.Deserialize(buf, 0, out _);
            LogPackage lp = LogPackage.ReadBlock(ms, 13);

            Assert.IsTrue(buf.Length == lp.GetSerialized(false).Length);

            Assert.IsTrue(lp.Logs.Count == 1 && lp.Logs[0].Mask == ltest.Mask && lp.Logs[0].Message == ltest.Message);

            Assert.IsTrue(ltest.Mask == l.Mask && ltest.Message == l.Message);
        }


        [TestMethod]
        public void ADL_Debug_Log_Test()
        {
            LogTextStream lts = new LogTextStream(new PipeStream())
            {
                AddTimeStamp = false
            };


            Debug.PrefixLookupMode = PrefixLookupSettings.Noprefix;
            Assert.IsTrue(Debug.PrefixLookupMode == PrefixLookupSettings.Noprefix);


            ADLLogger logger = new ADLLogger("UnitTest");


            Debug.AddOutputStream(lts);
            logger.Log(1, "ffffffffff");

            byte[] buf = new byte[lts.Length];
            lts.Read(buf, 0, buf.Length);
            string s = Debug.TextEncoding.GetString(buf);

            Assert.IsTrue(s.EndsWith("ffffffffff\n")); //ADL is appending the \n when using LogTextStreams
            


            logger.Log(1, "ffffffffff");
            Debug.AdlEnabled = false;
            logger.Log(1, "ffffffffff");
            Debug.AdlEnabled = true;
            buf = new byte[lts.Length];
            lts.Read(buf, 0, buf.Length);
            s = Debug.TextEncoding.GetString(buf);

            Assert.IsTrue(s.EndsWith("ffffffffff\n"));


            Debug.PrefixLookupMode = PrefixLookupSettings.Addprefixifavailable | PrefixLookupSettings.Bakeprefixes;

            logger.Log(2 | 4, "CODE COVERAGE");
            logger.Log(2 | 4, "CODE COVERAGE");
        }

        [TestMethod]
        public void ADL_Debug_RemoveOutputStream_Test()
        {
            LogTextStream lts = new LogTextStream(new PipeStream());
            Debug.AddOutputStream(lts);
            int newCount = Debug.LogStreamCount;

            Debug.AdlEnabled = false;
            Debug.RemoveOutputStream(lts);

            Debug.AdlEnabled = true;
            Debug.RemoveOutputStream(lts);
            Debug.RemoveOutputStream(lts);

            Assert.IsTrue(Debug.LogStreamCount == newCount - 1);
        }

        [TestMethod]
        public void ADL_Debug_LoadConfig_Test()
        {
            ADLConfig config = ConfigManager.GetDefault<ADLConfig>();
            Debug.LoadConfig(config);
            Assert.IsTrue(Debug.AdlEnabled == config.AdlEnabled);
        }
    }
}