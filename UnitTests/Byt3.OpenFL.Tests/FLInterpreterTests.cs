using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using Byt3.OpenCL.Common.Exceptions;
using Byt3.OpenCL.DataTypes;
using Byt3.OpenCL.Memory;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenCL.Wrapper.TypeEnums;
using Byt3.OpenFL.FLDataObjects;
using Byt3.OpenFL.New;
using Byt3.OpenFL.New.DataObjects;
using Byt3.OpenFL.New.Parsing;
using Xunit;

namespace Byt3.OpenFL.Tests
{
    public class FLInterpreterTests
    {
        [Fact]
        public void OpenCL_New_Parse_Test()
        {
            string path = "resources/filter/tests";
            string[] files = Directory.GetFiles(path, "*.fl");
            KernelDatabase db =
                new KernelDatabase(CLAPI.MainThread, "resources/kernel", DataVectorTypes.Uchar1);

            List<ParsedSource> src = new List<ParsedSource>();
            for (int i = 0; i < files.Length; i++)
            {
                src.Add(FLParser.ParseFile(CLAPI.MainThread, files[i])); //parsing the FL Script

                src[i].Run(CLAPI.MainThread, db, new FLBufferInfo(CLAPI.MainThread, 256, 256)); //Running it

                Bitmap bmp = new Bitmap(src[i].Dimensions.x, src[i].Dimensions.y); //Getting the Output
                CLAPI.UpdateBitmap(CLAPI.MainThread, bmp, CLAPI.ReadBuffer<byte>(CLAPI.MainThread, src[i].ActiveBuffer.Buffer, src[i].InputSize));

                string pp = Path.GetFullPath("./out/" + Path.GetFileNameWithoutExtension(files[i]) + ".png"); //Saving for debug reasons.
                bmp.Save(pp);
            }
        }

        [Fact]
        public void OpenFL_Comments_Test()
        {
            string file = Path.GetFullPath("resources/filter/comments/test.fl");
            FLInterpreter p = new FLInterpreter(CLAPI.MainThread, file,
                CLAPI.CreateEmpty<byte>(CLAPI.MainThread, 128 * 128 * 4,
                    MemoryFlag.CopyHostPointer | MemoryFlag.ReadWrite), 128, 128,
                1,
                4, new KernelDatabase(CLAPI.MainThread, "resources/kernel", DataVectorTypes.Uchar1)); //We need to Create a "fresh" database since xunit is making the cl context invalid when changing the test
            while (!p.Terminated)
            {
                p.Step();
            }
        }

        [Fact]
        public void OpenFL_DefineFile_Wrong_Test()
        {

            string file = "resources/filter/defines/test_wrong_define_invalid_file.fl";

            try
            {
                FLInterpreter p = new FLInterpreter(CLAPI.MainThread, file,
                    CLAPI.CreateEmpty<byte>(CLAPI.MainThread, 128 * 128 * 4,
                        MemoryFlag.CopyHostPointer | MemoryFlag.ReadWrite), 128,
                    128,
                    1,
                    4, TestSetup.KernelDb);
            }
            catch (Exception e)
            {
                if (!(e is FLInvalidFunctionUseException))
                {
                    Assert.True(false);
                }
                //We passed
            }
        }

        [Fact]
        public void OpenFL_Defines_Test()
        {

            string file = Path.GetFullPath("resources/filter/defines/test.fl");
            FLInterpreter p = new FLInterpreter(CLAPI.MainThread, file,
                CLAPI.CreateEmpty<byte>(CLAPI.MainThread, 128 * 128 * 4,
                    MemoryFlag.CopyHostPointer | MemoryFlag.ReadWrite), 128, 128,
                1,
                4, TestSetup.KernelDb);

            FLInterpreterStepResult ret = p.Step();


            Assert.True(ret.DefinedBuffers.Count == 5);
            Assert.True(ret.DefinedBuffers[0] == "in_unmanaged");
            Assert.True(ret.DefinedBuffers[1] == "textureD_internal");
            Assert.True(ret.DefinedBuffers[2] == "textureC_internal");
            Assert.True(ret.DefinedBuffers[3] == "textureB_internal");
            Assert.True(ret.DefinedBuffers[4] == "textureA_internal");
        }

        [Fact]
        public void OpenFL_DefineScriptFile_Wrong_Test()
        {

            string file = "resources/filter/defines/test_wrong_script_invalid_file.fl";

            for (int i = 0; i < 2; i++)
            {
                try
                {
                    FLInterpreter p = new FLInterpreter(CLAPI.MainThread, file,
                        CLAPI.CreateEmpty<byte>(CLAPI.MainThread, 128 * 128 * 4,
                            MemoryFlag.CopyHostPointer | MemoryFlag.ReadWrite), 128,
                        128,
                        1,
                        4, TestSetup.KernelDb);
                }
                catch (Exception e)
                {
                    if (!(e is FLInvalidFunctionUseException))
                    {
                        Assert.True(false);
                    }

                    //We passed
                }
            }
        }


        [Fact]
        public void OpenFL_DefineScriptNoFile_Wrong_Test()
        {

            string file = "resources/filter/defines/test_wrong_script_.fl";


            for (int i = 0; i < 2; i++)
            {
                try
                {
                    FLInterpreter p = new FLInterpreter(CLAPI.MainThread, file,
                        CLAPI.CreateEmpty<byte>(CLAPI.MainThread, 128 * 128 * 4,
                            MemoryFlag.CopyHostPointer | MemoryFlag.ReadWrite), 128,
                        128,
                        1,
                        4, TestSetup.KernelDb);
                }
                catch (Exception e)
                {
                    if (!(e is FLInvalidFunctionUseException))
                    {
                        Assert.True(false);
                    }

                    //We passed
                }
            }
        }

        [Fact]
        public void OpenFL_Kernels_Test()
        {
            string path = "resources/filter/tests";
            string[] files = Directory.GetFiles(path, "*.fl");
            KernelDatabase db =
                new KernelDatabase(CLAPI.MainThread, "resources/kernel", DataVectorTypes.Uchar1);
            foreach (string file in files)
            {
                FLInterpreter p = new FLInterpreter(CLAPI.MainThread, file,
                    CLAPI.CreateEmpty<byte>(CLAPI.MainThread, 64 * 64 * 4,
                        MemoryFlag.CopyHostPointer | MemoryFlag.ReadWrite), 64, 64,
                    1,
                    4, db); //We need to Create a "fresh" database since xunit is making the cl context invalid when changing the test
                while (!p.Terminated)
                {
                    p.Step();
                }
            }
        }

        [Fact]
        public void OpenFL_WFCDefines_Wrong_Test()
        {

            string[] files = Directory.GetFiles("resources/filter/defines/", "test_wrong_define_wfc_*.fl");


            foreach (string file in files)
            {
                try
                {
                    FLInterpreter p = new FLInterpreter(CLAPI.MainThread, file,
                        CLAPI.CreateEmpty<byte>(CLAPI.MainThread, 128 * 128 * 4,
                            MemoryFlag.CopyHostPointer | MemoryFlag.ReadWrite),
                        128,
                        128,
                        1,
                        4, TestSetup.KernelDb);
                }
                catch (Exception e)
                {
                    if (!(e is FLInvalidFunctionUseException))
                    {
                        Assert.True(false);
                    }

                    //We passed
                }
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