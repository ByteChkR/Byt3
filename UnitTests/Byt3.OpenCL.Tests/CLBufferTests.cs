﻿using System;
using Byt3.OpenCL.Memory;
using Byt3.OpenCL.Wrapper;
using NUnit.Framework;

namespace Byt3.OpenCL.Tests
{//TODO: cl kernel execution test
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

        [Test]
        public void OpenCL_CreateBuffer_Test()
        {
            byte[] b = new byte[255];
            for (int i = 0; i < b.Length; i++)
            {
                b[i] = (byte) i;
            }

            MemoryBuffer buffer =
                CLAPI.CreateBuffer(CLAPI.MainThread, b, MemoryFlag.WriteOnly | MemoryFlag.AllocateHostPointer,
                    "TestBuffer");

            Assert.True(buffer != null);
            Assert.True(buffer.Size == 255);
        }

        [Test]
        public void OpenCL_ReadBuffer_Test()
        {
            float[] b = new float[255];
            for (int i = 0; i < b.Length; i++)
            {
                b[i] = i;
            }

            MemoryBuffer buffer =
                CLAPI.CreateBuffer(CLAPI.MainThread, b, MemoryFlag.ReadWrite, "TestBuffer");
            float[] c = CLAPI.ReadBuffer<float>(CLAPI.MainThread, buffer, b.Length);


            Assert.True(CheckValues(c, b));
        }

        [Test]
        public void OpenCL_WriteBuffer_Test()
        {
            float[] b = new float[255];
            for (int i = 0; i < b.Length; i++)
            {
                b[i] = i;
            }

            MemoryBuffer buffer = CLAPI.CreateEmpty<float>(CLAPI.MainThread, b.Length,
                MemoryFlag.ReadWrite, "TestBuffer");


            CLAPI.WriteToBuffer(CLAPI.MainThread, buffer, b);

            float[] c = CLAPI.ReadBuffer<float>(CLAPI.MainThread, buffer, b.Length);


            Assert.True(CheckValues(c, b));
        }
    }
}