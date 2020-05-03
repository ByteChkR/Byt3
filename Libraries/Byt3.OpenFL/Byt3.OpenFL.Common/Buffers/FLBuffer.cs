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
        /// <summary>
        /// The buffer
        /// </summary>
        public virtual MemoryBuffer Buffer { get; protected set; }

        public int Width { get; protected set; }

        public int Height { get; protected set; }
        public long Size => Buffer.Size;

        public FLBuffer(CLAPI instance, int width, int height, object handleIdentifier) : this(
            CLAPI.CreateEmpty<byte>(instance, width * height * 4, MemoryFlag.ReadWrite, handleIdentifier), width,
            height)
        {
        }

        public FLBuffer(CLAPI instance, byte[] data, int width, int height, object handleIdentifier) : this(
            CLAPI.CreateBuffer(instance, data, MemoryFlag.ReadWrite, handleIdentifier), width, height)
        {
        }

        public FLBuffer(CLAPI instance, Bitmap bitmap, object handleIdentifier) : this(
            CLAPI.CreateFromImage(instance, bitmap, MemoryFlag.ReadWrite, handleIdentifier), bitmap.Width,
            bitmap.Height)
        {
        }

        /// <summary>
        /// The Internal Constructor
        /// </summary>
        /// <param name="buffer">The inner buffer</param>
        public FLBuffer(MemoryBuffer buffer, int width, int height)
        {
            Width = width;
            Height = height;
            Buffer = buffer;
            DefinedBufferName = "UnnamedBuffer";
        }

        /// <summary>
        /// Flag that is used to keep track of memory buffers that stayed inside the engine code and can not possibly be changed or used by the user.
        /// </summary>
        public bool IsInternal { get; private set; }

        /// <summary>
        /// The Buffer name
        /// </summary>
        public string DefinedBufferName { get; private set; }

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

        internal virtual void ReplaceUnderlyingBuffer(MemoryBuffer buf, int width, int height)
        {
            Width = width;
            Height = height;
            Buffer?.Dispose();
            Buffer = buf;
        }


        public virtual void Dispose()
        {
            Buffer.Dispose();
        }
    }
}