using System.IO;
using Byt3.ADL.Configs;
using Byt3.ADL.Streams;
using NUnit.Framework;

namespace Byt3.ADL.Tests
{
    public class DebugOperations
    {
        [Test]
        public void ADL_Debug_AddOutputStream_Test()
        {
            Debug.RemoveAllOutputStreams(); //Remove streams because unit tests may leave streams attached.


            Debug.RemoveAllOutputStreams();
            LogStream ls = new LogStream(new MemoryStream());
            Debug.AddOutputStream(ls);

            Debug.RemoveAllOutputStreams();
            
        }


        [Test]
        public void ADL_Debug_AddPrefixForMask_Test()
        {
            //Flag is required to find tags made up of unique masks(example: 2|8)
            Debug.PrefixLookupMode =
                PrefixLookupSettings.AddPrefixIfAvailable | PrefixLookupSettings.DeconstructMaskToFind;

            ADLLogger logger = new ADLLogger(InternalADLProjectDebugConfig.Settings, "UnitTest");

            BitMask bm = new BitMask(2 | 8);
            
            logger.AddPrefixForMask(bm | 4, "Test");
            logger.AddPrefixForMask(bm | 4, "Test");
            logger.RemoveAllPrefixes();

            logger.AddPrefixForMask(bm, "HELLO");
            bool ret = logger.GetMaskPrefix(bm) == "HELLO";
            Assert.True(ret);
        }

        [Test]
        public void ADL_Debug_RemovePrefixForMask_Test()
        {
            ADLLogger logger = new ADLLogger(InternalADLProjectDebugConfig.Settings, "UnitTest");
            BitMask bm = new BitMask(2 | 8);
            logger.AddPrefixForMask(bm, "HELLO");
            logger.RemovePrefixForMask(bm);

            Assert.True(logger.GetAllPrefixes().Count == 0);
        }

        [Test]
        public void ADL_Debug_RemoveAllPrefixes_Test()
        {
            ADLLogger logger = new ADLLogger(InternalADLProjectDebugConfig.Settings, "UnitTest");
            BitMask bm = new BitMask(2 | 8);
            logger.AddPrefixForMask(bm, "HELLO");
            logger.RemoveAllPrefixes();
            Assert.True(logger.GetAllPrefixes().Count == 0);
        }

        [Test]
        public void ADL_Debug_SetAllPrefixes_Test()
        {
            //Flag is required to find tags made up of unique masks(example: 2|8)
            Debug.PrefixLookupMode = PrefixLookupSettings.AddPrefixIfAvailable;

            ADLLogger logger = new ADLLogger(InternalADLProjectDebugConfig.Settings, "UnitTest");

            logger.SetAllPrefixes("Hello", "HELLO1", "HOLA2");

            Assert.True(logger.GetMaskPrefix(1) == "Hello");
            Assert.True(logger.GetMaskPrefix(2) == "HELLO1");
            Assert.True(logger.GetMaskPrefix(4) == "HOLA2");
        }

        [Test]
        public void ADL_Debug_GetAllPrefixes_Test()
        {
            ADLLogger logger = new ADLLogger(InternalADLProjectDebugConfig.Settings, "UnitTest");
            //Flag is required to find tags made up of unique masks(example: 2|8)
            Debug.PrefixLookupMode = PrefixLookupSettings.AddPrefixIfAvailable;


            logger.SetAllPrefixes("Hello", "HELLO1", "HOLA2");

            Assert.True(logger.GetAllPrefixes().Count == 3);

            logger.RemoveAllPrefixes();
            
            logger.SetAllPrefixes("Hello", "HELLO1", "HOLA2");

            Assert.True(logger.GetAllPrefixes().Count == 3);
        }


        [Test]
        public void ADL_Debug_GetPrefixMask_Test()
        {
            ADLLogger logger = new ADLLogger(InternalADLProjectDebugConfig.Settings, "UnitTest");
            logger.SetAllPrefixes("Hello", "HELLO1", "HOLA2");
            if (logger.GetPrefixMask("Hello", out BitMask bm))
            {
                Assert.True(bm == 1);
            }

            if (logger.GetPrefixMask("HELLO1", out bm))
            {
                Assert.True(bm == 2);
            }

            if (logger.GetPrefixMask("HOLA2", out bm))
            {
                Assert.True(bm == 4);
            }
        }

        [Test]
        public void ADL_Debug_GetMaskPrefix_Test()
        {
            ADLLogger logger = new ADLLogger(InternalADLProjectDebugConfig.Settings, "UnitTest");
            //Flag is required to find tags made up of unique masks(example: 2|8)
            Debug.PrefixLookupMode = PrefixLookupSettings.AddPrefixIfAvailable;

            logger.SetAllPrefixes("Hello", "HELLO1", "HOLA2");
            BitMask bm = 2;
            Assert.True(logger.GetMaskPrefix(bm) == "HELLO1");

            Debug.PrefixLookupMode = PrefixLookupSettings.AddPrefixIfAvailable | PrefixLookupSettings.OnlyOnePrefix;

            bm = 32;
            Assert.False(logger.GetMaskPrefix(bm) == "HELLO1");

            Debug.PrefixLookupMode = PrefixLookupSettings.AddPrefixIfAvailable | PrefixLookupSettings.BakePrefixes |
                                     PrefixLookupSettings.DeconstructMaskToFind;

            bm = 32;
            Assert.False(logger.GetMaskPrefix(bm) == "HELLO1");
        }

        [Test]
        public void ADL_Debug_LogSerialization_Test()
        {
            Log l = new Log(-1, "Hello");
            byte[] buf = l.Serialize();
            MemoryStream ms = new MemoryStream(buf);
            Log ltest = Log.Deserialize(buf, 0, out _);
            LogPackage lp = LogPackage.ReadBlock(ms, 13);

            Assert.True(buf.Length == lp.GetSerialized(false).Length);

            Assert.True(lp.Logs.Count == 1 && lp.Logs[0].Mask == ltest.Mask && lp.Logs[0].Message == ltest.Message);

            Assert.True(ltest.Mask == l.Mask && ltest.Message == l.Message);
        }


        [Test]
        public void ADL_Debug_Log_Test()
        {
            LogTextStream lts = new LogTextStream(new PipeStream())
            {
                AddTimeStamp = false
            };


            Debug.PrefixLookupMode = PrefixLookupSettings.NoPrefix;
            Assert.True(Debug.PrefixLookupMode == PrefixLookupSettings.NoPrefix);


            ADLLogger logger = new ADLLogger(InternalADLProjectDebugConfig.Settings, "UnitTest");


            Debug.AddOutputStream(lts);
            logger.Log(1, "ffffffffff", 0);

            byte[] buf = new byte[lts.Length];
            lts.Read(buf, 0, buf.Length);
            string s = Debug.TextEncoding.GetString(buf);

            Assert.True(s.EndsWith("ffffffffff\n")); //ADL is appending the \n when using LogTextStreams


            logger.Log(1, "ffffffffff", 0);
            buf = new byte[lts.Length];
            lts.Read(buf, 0, buf.Length);
            s = Debug.TextEncoding.GetString(buf);

            Assert.True(s.EndsWith("ffffffffff\n"));


            Debug.PrefixLookupMode = PrefixLookupSettings.AddPrefixIfAvailable | PrefixLookupSettings.BakePrefixes;

            logger.Log(2 | 4, "CODE COVERAGE", 0);
            logger.Log(2 | 4, "CODE COVERAGE", 0);
        }

        [Test]
        public void ADL_Debug_RemoveOutputStream_Test()
        {
            LogTextStream lts = new LogTextStream(new PipeStream());
            Debug.AddOutputStream(lts);
            int newCount = Debug.LogStreamCount;

            Debug.RemoveOutputStream(lts);

            Assert.True(Debug.LogStreamCount == newCount - 1);
        }

        [Test]
        public void ADL_Debug_LoadConfig_Test()
        {
            ADLConfig config = ConfigManager.GetDefault<ADLConfig>();
            Debug.LoadConfig(config);
            Assert.True(Debug.AdlEnabled == config.AdlEnabled);
        }
    }
}