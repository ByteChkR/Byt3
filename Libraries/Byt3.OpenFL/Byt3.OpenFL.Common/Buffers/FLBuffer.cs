using System;
using System.Drawing;
using Byt3.OpenCL.Memory;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;

namespace Byt3.OpenFL.Common.Buffers
{
    /// <summary>
    /// Wrapper for the Memory Buffer holding some useful additional data
    /// </summary>
    public class FLBuffer : FLParsedObject, IDisposable
    {
        public FLBuffer(CLAPI instance, int width, int height, int depth, object handleIdentifier,
            MemoryFlag flag = MemoryFlag.ReadWrite) : this(
            CLAPI.CreateEmpty<byte>(instance, width * height * depth * 4, flag, handleIdentifier), width,
            height, depth)
        {
        }

        public FLBuffer(CLAPI instance, byte[] data, int width, int height, int depth, object handleIdentifier,
            MemoryFlag flag = MemoryFlag.ReadWrite) : this(
            CLAPI.CreateBuffer(instance, data, flag, handleIdentifier), width, height, depth)
        {
        }

        public FLBuffer(CLAPI instance, Bitmap bitmap, object handleIdentifier,
            MemoryFlag flag = MemoryFlag.ReadWrite) : this(
            CLAPI.CreateFromImage(instance, bitmap, flag, handleIdentifier), bitmap.Width,
            bitmap.Height, 1)
        {
        }

        /// <summary>
        /// The Internal Constructor
        /// </summary>
        /// <param name="buffer">The inner buffer</param>
        public FLBuffer(MemoryBuffer buffer, int width, int height, int depth)
        {
            Width = width;
            Height = height;
            Depth = depth;
            Buffer = buffer;
            DefinedBufferName = "UnnamedBuffer";
        }

        /// <summary>
        /// The buffer
        /// </summary>
        public virtual MemoryBuffer Buffer { get; protected set; }

        public int Width { get; protected set; }
        public int Height { get; protected set; }
        public int Depth { get; protected set; }

        public long Size => Buffer.Size;

        /// <summary>
        /// Flag that is used to keep track of memory buffers that stayed inside the engine code and can not possibly be changed or used by the user.
        /// </summary>
        public bool IsInternal { get; private set; }

        /// <summary>
        /// The Buffer name
        /// </summary>
        public string DefinedBufferName { get; private set; }


        public virtual void Dispose()
        {
            Buffer.Dispose();
        }

        /// <summary>
        /// Sets the IsInernal Flag to the specified state
        /// </summary>
        /// <param name="internalState">The state</param>
        internal void SetInternalState(bool internalState)
        {
            IsInternal = internalState;
        }

        /// <summary>
        /// Sets the buffer name
        /// </summary>
        /// <param name="key">The Name of the buffer</param>
        public void SetKey(string key)
        {
            DefinedBufferName = key;
        }

        /// <summary>
        /// To string override
        /// </summary>
        /// <returns>Console friendly string</returns>
        public override string ToString()
        {
            return DefinedBufferName;
        }

        internal virtual void ReplaceUnderlyingBuffer(MemoryBuffer buf, int width, int height, int depth)
        {
            Width = width;
            Height = height;
            Depth = Depth;
            Buffer?.Dispose();
            Buffer = buf;
        }
    }
}