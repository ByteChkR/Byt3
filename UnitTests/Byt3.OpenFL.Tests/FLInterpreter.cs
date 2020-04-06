using System;
using System.IO;
using Byt3.OpenCL.Common;
using Byt3.OpenCL.Common.Exceptions;
using Byt3.OpenCL.DataTypes;
using Byt3.OpenCL.Memory;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenCL.Wrapper.TypeEnums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Byt3.OpenFL.Tests
{
    [TestClass]
    public class FlInterpreter
    {
   

        [TestMethod]
        public void OpenFL_Comments_Test()
        {
            DebugHelper.ThrowOnAllExceptions = true;
            string file = Path.GetFullPath("resources/filter/comments/test.fl");
            Interpreter p = new Interpreter(Clapi.MainThread, file,
                Clapi.CreateEmpty<byte>(Clapi.MainThread, 128 * 128 * 4,
                    MemoryFlag.CopyHostPointer | MemoryFlag.ReadWrite), 128, 128,
                1,
                4, new KernelDatabase(Clapi.MainThread, "resources/kernel", DataTypes.Uchar1)); //We need to Create a "fresh" database since xunit is making the cl context invalid when changing the test
            while (!p.Terminated)
            {
                p.Step();
            }
        }

        [TestMethod]
        public void OpenFL_DefineFile_Wrong_Test()
        {
            DebugHelper.ThrowOnAllExceptions = true;

            string file = "resources/filter/defines/test_wrong_define_invalid_file.fl";


            for (int i = 0; i < 2; i++)
            {
                DebugHelper.ThrowOnAllExceptions = i == 0;
                try
                {
                    Interpreter p = new Interpreter(Clapi.MainThread, file,
                        Clapi.CreateEmpty<byte>(Clapi.MainThread, 128 * 128 * 4,
                            MemoryFlag.CopyHostPointer | MemoryFlag.ReadWrite), 128,
                        128,
                        1,
                        4, TestSetup.KernelDb);
                    Assert.IsTrue(!DebugHelper.ThrowOnAllExceptions);
                }
                catch (Exception e)
                {
                    Assert.IsTrue(DebugHelper.ThrowOnAllExceptions);
                    if (!(e is FLInvalidFunctionUseException))
                    {
                        Assert.IsTrue(false);
                    }

                    //We passed
                }
            }
        }

        [TestMethod]
        public void OpenFL_Defines_Test()
        {
            DebugHelper.ThrowOnAllExceptions = true;

            string file = Path.GetFullPath("resources/filter/defines/test.fl");
            Interpreter p = new Interpreter(Clapi.MainThread, file,
                Clapi.CreateEmpty<byte>(Clapi.MainThread, 128 * 128 * 4,
                    MemoryFlag.CopyHostPointer | MemoryFlag.ReadWrite), 128, 128,
                1,
                4, TestSetup.KernelDb);

            InterpreterStepResult ret = p.Step();


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
            DebugHelper.ThrowOnAllExceptions = true;

            string file = "resources/filter/defines/test_wrong_script_invalid_file.fl";

            for (int i = 0; i < 2; i++)
            {
                try
                {
                    Interpreter p = new Interpreter(Clapi.MainThread, file,
                        Clapi.CreateEmpty<byte>(Clapi.MainThread, 128 * 128 * 4,
                            MemoryFlag.CopyHostPointer | MemoryFlag.ReadWrite), 128,
                        128,
                        1,
                        4, TestSetup.KernelDb);
                    Assert.IsTrue(!DebugHelper.ThrowOnAllExceptions);
                }
                catch (Exception e)
                {
                    Assert.IsTrue(DebugHelper.ThrowOnAllExceptions);
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
            DebugHelper.ThrowOnAllExceptions = true;

            string file = "resources/filter/defines/test_wrong_script_.fl";


            for (int i = 0; i < 2; i++)
            {
                try
                {
                    Interpreter p = new Interpreter(Clapi.MainThread, file,
                        Clapi.CreateEmpty<byte>(Clapi.MainThread, 128 * 128 * 4,
                            MemoryFlag.CopyHostPointer | MemoryFlag.ReadWrite), 128,
                        128,
                        1,
                        4, TestSetup.KernelDb);
                    Assert.IsTrue(!DebugHelper.ThrowOnAllExceptions);
                }
                catch (Exception e)
                {
                    Assert.IsTrue(DebugHelper.ThrowOnAllExceptions);
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
            DebugHelper.ThrowOnAllExceptions = true;
            DebugHelper.SeverityFilter = 10;
            string path = "resources/filter/tests";
            string[] files = Directory.GetFiles(path, "*.fl");
            KernelDatabase db =
                new KernelDatabase(Clapi.MainThread, "resources/kernel", DataTypes.Uchar1);
            foreach (string file in files)
            {
                Interpreter p = new Interpreter(Clapi.MainThread, file,
                    Clapi.CreateEmpty<byte>(Clapi.MainThread, 128 * 128 * 4,
                        MemoryFlag.CopyHostPointer | MemoryFlag.ReadWrite), 128, 128,
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
            DebugHelper.ThrowOnAllExceptions = true;

            string[] files = Directory.GetFiles("resources/filter/defines/", "test_wrong_define_wfc_*.fl");


            for (int i = 0; i < 2; i++)
            {
                DebugHelper.ThrowOnAllExceptions = i == 0;
                foreach (string file in files)
                {
                    try
                    {
                        Interpreter p = new Interpreter(Clapi.MainThread, file,
                            Clapi.CreateEmpty<byte>(Clapi.MainThread, 128 * 128 * 4,
                                MemoryFlag.CopyHostPointer | MemoryFlag.ReadWrite),
                            128,
                            128,
                            1,
                            4, TestSetup.KernelDb);
                        Assert.IsTrue(!DebugHelper.ThrowOnAllExceptions);
                    }
                    catch (Exception e)
                    {
                        Assert.IsTrue(DebugHelper.ThrowOnAllExceptions);
                        if (!(e is FLInvalidFunctionUseException))
                        {
                            Assert.IsTrue(false);
                        }

                        //We passed
                    }
                }
            }
        }

        [TestMethod]
        public void OpenFL_TypeConversion_Test()
        {
            float f = float.MaxValue / 2;
            byte b = (byte) ClTypeConverter.Convert(typeof(byte), f);
            float4 f4 = new float4(f);
            uchar4 i4 = (uchar4) ClTypeConverter.Convert(typeof(uchar4), f4);
            Assert.IsTrue(b == 128);

            for (int i = 0; i < 4; i++)
            {
                byte s = i4[i];
                Assert.IsTrue(s == 128);
            }
        }
    }
}