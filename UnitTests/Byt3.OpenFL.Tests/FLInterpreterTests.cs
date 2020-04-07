using System;
using System.IO;
using Byt3.OpenCL.Common.Exceptions;
using Byt3.OpenCL.Memory;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenCL.Wrapper.TypeEnums;
using Byt3.OpenCLNetStandard.DataTypes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Byt3.OpenFL.Tests
{
    [TestClass]
    public class FLInterpreterTests
    {
   

        [TestMethod]
        public void OpenFL_Comments_Test()
        {
            string file = Path.GetFullPath("resources/filter/comments/test.fl");
            OpenFL.FLInterpreter p = new OpenFL.FLInterpreter(CLAPI.MainThread, file,
                CLAPI.CreateEmpty<byte>(CLAPI.MainThread, 128 * 128 * 4,
                    MemoryFlag.CopyHostPointer | MemoryFlag.ReadWrite), 128, 128,
                1,
                4, new KernelDatabase(CLAPI.MainThread, "resources/kernel", DataTypes.Uchar1)); //We need to Create a "fresh" database since xunit is making the cl context invalid when changing the test
            while (!p.Terminated)
            {
                p.Step();
            }
        }

        [TestMethod]
        public void OpenFL_DefineFile_Wrong_Test()
        {

            string file = "resources/filter/defines/test_wrong_define_invalid_file.fl";

            try
            {
                OpenFL.FLInterpreter p = new OpenFL.FLInterpreter(CLAPI.MainThread, file,
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
                    Assert.IsTrue(false);
                }
                //We passed
            }
        }

        [TestMethod]
        public void OpenFL_Defines_Test()
        {

            string file = Path.GetFullPath("resources/filter/defines/test.fl");
            OpenFL.FLInterpreter p = new OpenFL.FLInterpreter(CLAPI.MainThread, file,
                CLAPI.CreateEmpty<byte>(CLAPI.MainThread, 128 * 128 * 4,
                    MemoryFlag.CopyHostPointer | MemoryFlag.ReadWrite), 128, 128,
                1,
                4, TestSetup.KernelDb);

            FLInterpreterStepResult ret = p.Step();


            Assert.IsTrue(ret.DefinedBuffers.Count == 5);
            Assert.IsTrue(ret.DefinedBuffers[0] == "in_unmanaged");
            Assert.IsTrue(ret.DefinedBuffers[1] == "textureD_internal");
            Assert.IsTrue(ret.DefinedBuffers[2] == "textureC_internal");
            Assert.IsTrue(ret.DefinedBuffers[3] == "textureB_internal");
            Assert.IsTrue(ret.DefinedBuffers[4] == "textureA_internal");
        }

        [TestMethod]
        public void OpenFL_DefineScriptFile_Wrong_Test()
        {

            string file = "resources/filter/defines/test_wrong_script_invalid_file.fl";

            for (int i = 0; i < 2; i++)
            {
                try
                {
                    OpenFL.FLInterpreter p = new OpenFL.FLInterpreter(CLAPI.MainThread, file,
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
                        Assert.IsTrue(false);
                    }

                    //We passed
                }
            }
        }


        [TestMethod]
        public void OpenFL_DefineScriptNoFile_Wrong_Test()
        {

            string file = "resources/filter/defines/test_wrong_script_.fl";


            for (int i = 0; i < 2; i++)
            {
                try
                {
                    OpenFL.FLInterpreter p = new OpenFL.FLInterpreter(CLAPI.MainThread, file,
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
                        Assert.IsTrue(false);
                    }

                    //We passed
                }
            }
        }

        [TestMethod]
        public void OpenFL_Kernels_Test()
        {
            string path = "resources/filter/tests";
            string[] files = Directory.GetFiles(path, "*.fl");
            KernelDatabase db =
                new KernelDatabase(CLAPI.MainThread, "resources/kernel", DataTypes.Uchar1);
            foreach (string file in files)
            {
                OpenFL.FLInterpreter p = new OpenFL.FLInterpreter(CLAPI.MainThread, file,
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

        [TestMethod]
        public void OpenFL_WFCDefines_Wrong_Test()
        {

            string[] files = Directory.GetFiles("resources/filter/defines/", "test_wrong_define_wfc_*.fl");


            foreach (string file in files)
            {
                try
                {
                    OpenFL.FLInterpreter p = new OpenFL.FLInterpreter(CLAPI.MainThread, file,
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
                        Assert.IsTrue(false);
                    }

                    //We passed
                }
            }
        }

        [TestMethod]
        public void OpenFL_TypeConversion_Test()
        {
            float f = float.MaxValue / 2;
            byte b = (byte) CLTypeConverter.Convert(typeof(byte), f);
            float4 f4 = new float4(f);
            uchar4 i4 = (uchar4) CLTypeConverter.Convert(typeof(uchar4), f4);
            Assert.IsTrue(b == 128);

            for (int i = 0; i < 4; i++)
            {
                byte s = i4[i];
                Assert.IsTrue(s == 128);
            }
        }
    }
}