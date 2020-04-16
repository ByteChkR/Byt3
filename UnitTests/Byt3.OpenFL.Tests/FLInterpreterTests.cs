using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Byt3.ExtPP.API;
using Byt3.ExtPP.API.Configuration;
using Byt3.OpenCL.DataTypes;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenCL.Wrapper.ExtPP.API;
using Byt3.OpenCL.Wrapper.TypeEnums;
using Byt3.OpenFL.Parsing;
using Byt3.OpenFL.Parsing.DataObjects;
using Byt3.OpenFL.Parsing.ExtPP.API.Configurations;
using Byt3.OpenFL.Parsing.Stages;
using Byt3.Utilities.Exceptions;
using Xunit;

namespace Byt3.OpenFL.Tests
{
    [CollectionDefinition("PPCollection", DisableParallelization = true)]
    public class ExtPPApiFixtureCollection : ICollectionFixture<ExtPPApiFixture> { }
    public class ExtPPApiFixture
    {
        public ExtPPApiFixture()
        {
            TextProcessorAPI.Configs = new Dictionary<string, APreProcessorConfig>()
            {
                {".fl", new FLPreProcessorConfig() },
                {".cl", new CLPreProcessorConfig() },
                {"***", new DefaultPreProcessorConfig() }
            };
        }
    }

    [Collection("PPCollection")]
    public class FLInterpreterTests
    {

        public FLInterpreterTests(ExtPPApiFixture fixture)
        {

        }

        [Fact]
        public void OpenFL_Comments_Test()
        {
            string file = Path.GetFullPath("resources/filter/comments/test.fl");

            FLParseResult pr = FLParser.Parse(new FLParserInput(file));
            FunctionObject entryPoint = pr.EntryPoint; //Provoking an exception if main function is not found
        }

        [Fact]
        public void OpenFL_DefineFile_Wrong_Test()
        {

            string file = "resources/filter/defines/test_wrong_define_invalid_file.fl";

            Assert.ThrowsAny<Byt3Exception>(() => FLParser.Parse(new FLParserInput(file)));

        }

        [Fact]
        public void OpenFL_Defines_Test()
        {

            string file = Path.GetFullPath("resources/filter/defines/test.fl");


            FLParseResult result = FLParser.Parse(new FLParserInput(file));


            Assert.True(result.DefinedBuffers.Count == 5);
            Assert.True(result.DefinedBuffers.ContainsKey("in"));
            Assert.True(result.DefinedBuffers.ContainsKey("textureD"));
            Assert.True(result.DefinedBuffers.ContainsKey("textureC"));
            Assert.True(result.DefinedBuffers.ContainsKey("textureB"));
            Assert.True(result.DefinedBuffers.ContainsKey("textureA"));
        }

        [Fact]
        public void OpenFL_DefineScriptFile_Wrong_Test()
        {

            string file = "resources/filter/defines/test_wrong_script_invalid_file.fl";
            Assert.ThrowsAny<Byt3Exception>(() => FLParser.Parse(new FLParserInput(file)));

        }


        [Fact]
        public void OpenFL_DefineScriptNoFile_Wrong_Test()
        {

            string file = "resources/filter/defines/test_wrong_script_.fl";
            Assert.ThrowsAny<Byt3Exception>(() => FLParser.Parse(new FLParserInput(file)));


        }

        [Fact]
        public void OpenFL_Kernels_Test()
        {
            string path = "resources/filter/tests";
            string[] files = Directory.GetFiles(path, "*.fl", SearchOption.TopDirectoryOnly);
            KernelDatabase db =
                new KernelDatabase(CLAPI.MainThread, "resources/kernel", DataVectorTypes.Uchar1);

            for (int i = 0; i < files.Length; i++)
            {
                FLParseResult res = FLParser.Parse(new FLParserInput(files[i]));
                FLBufferInfo buffer = new FLBufferInfo(CLAPI.MainThread, 32, 32);
                res.Run(CLAPI.MainThread, db, buffer); //Running it

                Bitmap bmp = new Bitmap(res.Dimensions.x, res.Dimensions.y); //Getting the Output
                CLAPI.UpdateBitmap(CLAPI.MainThread, bmp, CLAPI.ReadBuffer<byte>(CLAPI.MainThread, res.ActiveBuffer.Buffer, res.InputSize));

                buffer.Dispose();
                res.FreeResources();

                string pp = Path.GetFullPath("./out/" + Path.GetFileNameWithoutExtension(files[i]) + ".png"); //Saving for debug reasons.
                //bmp.Save(pp);

            }
        }

        [Fact]
        public void OpenFL_WFCDefines_Wrong_Test()
        {

            string[] files = Directory.GetFiles("resources/filter/defines/", "test_wrong_define_wfc_*.fl");


            foreach (string file in files)
            {
                Assert.ThrowsAny<Byt3Exception>(() => FLParser.Parse(new FLParserInput(file)));
            }
        }

        [Fact]
        public void OpenFL_TypeConversion_Test()
        {
            float f = float.MaxValue / 2;
            byte b = (byte)CLTypeConverter.Convert(typeof(byte), f);
            float4 f4 = new float4(f);
            uchar4 i4 = (uchar4)CLTypeConverter.Convert(typeof(uchar4), f4);
            Assert.True(b == 128);

            for (int i = 0; i < 4; i++)
            {
                byte s = i4[i];
                Assert.True(s == 128);
            }
        }
    }
}