using System;
using Byt3.OpenCL.Common;
using Byt3.OpenCL.Memory;
using Byt3.OpenCL.Wrapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Byt3.OpenCL.Tests
{
    [TestClass]
    public class ClBuffers
    {
     
        [TestMethod]
        private static bool CheckValues(float[] values, float[] reference)
        {
            DebugHelper.ThrowOnAllExceptions = true;
            bool working = true;
            for (int i = 0; i < values.Length; i++)
            {
                if (Math.Abs(values[i] - reference[i]) > 0.01f)
                {
                    working = false;
                }
            }

            return working;
        }

        [TestMethod]
        public void OpenCL_CreateBuffer_Test()
        {
            Clapi.Reinitialize();
            DebugHelper.ThrowOnAllExceptions = true;
            byte[] b = new byte[255];
            for (int i = 0; i < b.Length; i++)
            {
                b[i] = (byte) i;
            }

            MemoryBuffer buffer =
                Clapi.CreateBuffer(Clapi.MainThread, b, MemoryFlag.CopyHostPointer | MemoryFlag.ReadWrite);

            Assert.IsTrue(buffer != null);
            Assert.IsTrue(buffer.Size == 255);
        }

        [TestMethod]
        public void OpenCL_ReadBuffer_Test()
        {
            Clapi.Reinitialize();
            DebugHelper.ThrowOnAllExceptions = true;
            float[] b = new float[255];
            for (int i = 0; i < b.Length; i++)
            {
                b[i] = i;
            }

            MemoryBuffer buffer =
                Clapi.CreateBuffer(Clapi.MainThread, b, MemoryFlag.CopyHostPointer | MemoryFlag.ReadWrite);

            float[] c = Clapi.ReadBuffer<float>(Clapi.MainThread, buffer, b.Length);


            Assert.IsTrue(CheckValues(c, b));
        }

        [TestMethod]
        public void OpenCL_WriteBuffer_Test()
        {
            Clapi.Reinitialize();
            DebugHelper.ThrowOnAllExceptions = true;
            float[] b = new float[255];
            for (int i = 0; i < b.Length; i++)
            {
                b[i] = i;
            }

            MemoryBuffer buffer = Clapi.CreateEmpty<float>(Clapi.MainThread, b.Length,
                MemoryFlag.CopyHostPointer | MemoryFlag.ReadWrite);


            Clapi.WriteToBuffer(Clapi.MainThread, buffer, b);

            float[] c = Clapi.ReadBuffer<float>(Clapi.MainThread, buffer, b.Length);


            Assert.IsTrue(CheckValues(c, b));
        }
    }
}