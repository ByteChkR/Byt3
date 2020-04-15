using System;
using Byt3.OpenCL.Memory;
using Byt3.OpenCL.Wrapper;
using Xunit;

namespace Byt3.OpenCL.Tests
{
    public class CLBufferTests
    {
        private static bool CheckValues(float[] values, float[] reference)
        {
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

        [Fact]
        public void OpenCL_CreateBuffer_Test()
        {
            //CLAPI.Reinitialize();
            byte[] b = new byte[255];
            for (int i = 0; i < b.Length; i++)
            {
                b[i] = (byte) i;
            }

            MemoryBuffer buffer =
                CLAPI.CreateBuffer(CLAPI.MainThread, b,MemoryFlag.ReadWrite);

            Assert.True(buffer != null);
            Assert.True(buffer.Size == 255);
        }

        [Fact]
        public void OpenCL_ReadBuffer_Test()
        {
            //CLAPI.Reinitialize();
            float[] b = new float[255];
            for (int i = 0; i < b.Length; i++)
            {
                b[i] = i;
            }

            MemoryBuffer buffer =
                CLAPI.CreateBuffer(CLAPI.MainThread, b,  MemoryFlag.ReadWrite);

            float[] c = CLAPI.ReadBuffer<float>(CLAPI.MainThread, buffer, b.Length);


            Assert.True(CheckValues(c, b));
        }

        [Fact]
        public void OpenCL_WriteBuffer_Test()
        {

            //CLAPI.Reinitialize();
            float[] b = new float[255];
            for (int i = 0; i < b.Length; i++)
            {
                b[i] = i;
            }

            MemoryBuffer buffer = CLAPI.CreateEmpty<float>(CLAPI.MainThread, b.Length,
                 MemoryFlag.ReadWrite);


            CLAPI.WriteToBuffer(CLAPI.MainThread, buffer, b);

            float[] c = CLAPI.ReadBuffer<float>(CLAPI.MainThread, buffer, b.Length);


            Assert.True(CheckValues(c, b));
        }
    }
}