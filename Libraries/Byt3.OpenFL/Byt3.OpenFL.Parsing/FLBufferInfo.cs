using System;
using System.Drawing;
using Byt3.OpenCL.Memory;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenFL.Parsing.DataObjects;
using Image = System.Drawing.Image;

namespace Byt3.OpenFL.Parsing
{
    public class UnloadedDefinedFLBufferInfo : FLBufferInfo, IDisposable
    {
        public delegate FLBufferInfo BufferLoader(FLParseResult root);
        protected BufferLoader Loader;
        private MemoryBuffer _buffer;

        public override MemoryBuffer Buffer
        {
            get
            {
                if (_buffer == null)
                {
                    FLBufferInfo i = Loader(Root);

                    _buffer = i.Buffer;
                    Width = i.Width;
                    Height = i.Height;
                }

                return _buffer;
            }
        }

        public UnloadedDefinedFLBufferInfo(BufferLoader loader) : base(default(MemoryBuffer), -1, -1)
        {
            Loader = loader;
        }

        public override void Dispose()
        {
            _buffer?.Dispose();
            _buffer = null;
        }
    }


    public class UnloadedFLBufferInfo : UnloadedDefinedFLBufferInfo
    {
        private readonly string File;


        public UnloadedFLBufferInfo(string file) : base(null)
        {
            Loader = root =>
            {
                if (File == "INPUT")
                {
                    return root.Input;
                }

                Bitmap bmp = new Bitmap((Bitmap)Image.FromFile(File), root.Dimensions.x, root.Dimensions.y);
                return new FLBufferInfo(root.Instance, bmp);
            };
            File = file;

        }

    }

    /// <summary>
    /// Wrapper for the Memory Buffer holding some useful additional data
    /// </summary>
    public class FLBufferInfo : ParsedObject, IDisposable
    {
        /// <summary>
        /// The buffer
        /// </summary>
        public virtual MemoryBuffer Buffer { get; }

        public int Width;
        public int Height;
        public long Size => Buffer.Size;

        public FLBufferInfo(CLAPI instance, int width, int height) : this(
            CLAPI.CreateEmpty<byte>(instance, width * height * 4, MemoryFlag.ReadWrite), width, height)
        {

        }

        public FLBufferInfo(CLAPI instance, byte[] data, int width, int height) : this(
            CLAPI.CreateBuffer(instance, data, MemoryFlag.ReadWrite), width, height)
        {

        }

        public FLBufferInfo(CLAPI instance, Bitmap bitmap) : this(CLAPI.CreateFromImage(instance, bitmap, MemoryFlag.ReadWrite), bitmap.Width, bitmap.Height)
        {

        }

        /// <summary>
        /// The Internal Constructor
        /// </summary>
        /// <param name="buffer">The inner buffer</param>
        public FLBufferInfo(MemoryBuffer buffer, int width, int height)
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
        internal void SetKey(string key)
        {
            DefinedBufferName = key;
        }

        /// <summary>
        /// To string ovverride
        /// </summary>
        /// <returns>Console friendly string</returns>
        public override string ToString()
        {
            return DefinedBufferName + "_" + (IsInternal ? "internal" : "unmanaged");
        }


        public virtual void Dispose()
        {
            Buffer.Dispose();
        }
    }
}