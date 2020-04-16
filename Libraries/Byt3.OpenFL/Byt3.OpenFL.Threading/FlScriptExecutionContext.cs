using System;
using System.Collections.Generic;
using System.Drawing;
using Byt3.OpenCL.Memory;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenFL.Parsing;

namespace Byt3.OpenFL.Threading
{
    /// <summary>
    /// Contains the Context in which the FL Runner is executing the enqueued items
    /// </summary>
    public struct FlScriptExecutionContext
    {
        public string Filename;
        public Action<FLParseResult> OnFinishCallback;
        public byte[] Input;
        public int Width;
        public int Height;

        public FlScriptExecutionContext(string filename, Bitmap tex, Action<FLParseResult> onFinishCallback)
        {
            Width = (int)tex.Width;
            Height = (int)tex.Height;
            Filename = filename;
            MemoryBuffer buf = CLAPI.CreateFromImage(CLAPI.MainThread, tex, MemoryFlag.AllocateHostPointer);
            Input = CLAPI.ReadBuffer<byte>(CLAPI.MainThread, buf, (int)buf.Size);
            buf.Dispose();
            OnFinishCallback = onFinishCallback;
        }

        public FlScriptExecutionContext(string filename, byte[] input, int width, int height, Action<FLParseResult> onFinishCallback)
        {
            Width = width;
            Height = height;
            Filename = filename;
            Input = input;
            OnFinishCallback = onFinishCallback;
        }
    }
}