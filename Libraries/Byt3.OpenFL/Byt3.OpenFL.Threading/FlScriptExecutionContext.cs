using System;
using System.Collections.Generic;
using System.Drawing;
using Byt3.OpenCL.Memory;
using Byt3.OpenCL.Wrapper;

namespace Byt3.OpenFL.Threading
{
    /// <summary>
    /// Contains the Context in which the FL Runner is executing the enqueued items
    /// </summary>
    public struct FlScriptExecutionContext
    {
        public string Filename;
        public Action<Dictionary<Bitmap, byte[]>> OnFinishCallback;
        public byte[] Input;
        public int Width;
        public int Height;
        public Dictionary<string, Bitmap> TextureMap;

        public FlScriptExecutionContext(string filename, Bitmap tex, Dictionary<string, Bitmap> textureMap,
            Action<Dictionary<Bitmap, byte[]>> onFinishCallback)
        {
            Width = (int) tex.Width;
            Height = (int) tex.Height;
            Filename = filename;
            MemoryBuffer buf = CLAPI.CreateFromImage(CLAPI.MainThread, tex, MemoryFlag.AllocateHostPointer);
            Input = CLAPI.ReadBuffer<byte>(CLAPI.MainThread, buf, (int) buf.Size);
            buf.Dispose();
            TextureMap = textureMap;
            OnFinishCallback = onFinishCallback;
        }

        public FlScriptExecutionContext(string filename, byte[] input, int width, int height,
            Dictionary<string, Bitmap> textureMap,
            Action<Dictionary<Bitmap, byte[]>> onFinishCallback)
        {
            Width = width;
            Height = height;
            Filename = filename;
            Input = input;
            TextureMap = textureMap;
            OnFinishCallback = onFinishCallback;
        }
    }
}