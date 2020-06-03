using System;
using System.Drawing;
using Byt3.OpenCL.Memory;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;

namespace Byt3.OpenFL.Threading
{
    /// <summary>
    /// Contains the Context in which the FL Runner is executing the enqueued items
    /// </summary>
    public struct FlScriptExecutionContext
    {
        public bool IsCompiled => Filename.EndsWith(".flc");
        public string Filename;
        public Action<FLProgram> OnFinishCallback;
        public byte[] Input;
        public int Width;
        public int Height;
        public int Depth;

        public FlScriptExecutionContext(string filename, Bitmap tex, Action<FLProgram> onFinishCallback)
        {
            Width = tex.Width;
            Height = tex.Height;
            Depth = 1;
            Filename = filename;
            MemoryBuffer buf = CLAPI.CreateFromImage(CLAPI.MainThread, tex, MemoryFlag.AllocateHostPointer, filename);
            Input = CLAPI.ReadBuffer<byte>(CLAPI.MainThread, buf, (int) buf.Size);
            buf.Dispose();
            OnFinishCallback = onFinishCallback;
        }

        public FlScriptExecutionContext(string filename, byte[] input, int width, int height, int depth,
            Action<FLProgram> onFinishCallback)
        {
            Width = width;
            Height = height;
            Depth = depth;
            Filename = filename;
            Input = input;
            OnFinishCallback = onFinishCallback;
        }
    }
}