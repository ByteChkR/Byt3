using System;
using System.Drawing;
using Byt3.OpenCL.Memory;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenFL.New.DataObjects;
using Image = System.Drawing.Image;

namespace Byt3.OpenFL.New
{
    public class UnloadedDefinedFLBufferInfo : FLBufferInfo
    {
        public delegate Bitmap BufferLoader();
        private readonly BufferLoader _loader;
        private MemoryBuffer _buffer;

        public override MemoryBuffer Buffer
        {
            get
            {
                if (_buffer == null)
                {
                    Bitmap bmp = _loader();

                    FLBufferInfo i = new FLBufferInfo(Root.Instance, new Bitmap(bmp, Root.Dimensions.x, Root.Dimensions.y));

                    _buffer = i.Buffer;
                    Width = i.Width;
                    Height = i.Height;
                }

                return _buffer;
            }
        }

        public UnloadedDefinedFLBufferInfo(BufferLoader Loader) : base(default(MemoryBuffer), -1, -1)
        {
            _loader = Loader;
        }
    }

    public class UnloadedFLBufferInfo : FLBufferInfo
    {
        private readonly string File;

        private MemoryBuffer _buffer;
        private readonly CLAPI _initInstance;

        public override MemoryBuffer Buffer
        {
            get
            {
                if (_buffer == null)
                {
                    if (File == "INPUT")
                    {
                        return Root.Input.Buffer;
                    }
                    Bitmap bmp = new Bitmap((Bitmap)Image.FromFile(File), Root.Dimensions.x, Root.Dimensions.y);
                    _buffer = CLAPI.CreateFromImage(_initInstance, bmp, MemoryFlag.CopyHostPointer);
                    Width = bmp.Width;
                    Height = bmp.Height;
                }

                return _buffer;
            }
        }

        public UnloadedFLBufferInfo(CLAPI instance, string file) : base(default(MemoryBuffer), -1, -1)
        {
            File = file;
            _initInstance = instance;
        }
    }

    /// <summary>
    /// Wrapper for the Memory Buffer holding some useful additional data
    /// </summary>
    public class FLBufferInfo : ParsedObject
    {
        /// <summary>
        /// The buffer
        /// </summary>
        public virtual MemoryBuffer Buffer { get; protected set; }

        public int Width;
        public int Height;

        public FLBufferInfo(CLAPI instance, int width, int height) : this(
            CLAPI.CreateEmpty<byte>(instance, width * height * 4, MemoryFlag.CopyHostPointer), width, height)
        {

        }

        public FLBufferInfo(CLAPI instance, byte[] data, int width, int height) : this(
            CLAPI.CreateBuffer(instance, data, MemoryFlag.CopyHostPointer), width, height)
        {

        }

        public FLBufferInfo(CLAPI instance, Bitmap bitmap) : this(CLAPI.CreateFromImage(instance, bitmap, MemoryFlag.CopyHostPointer), bitmap.Width, bitmap.Height)
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
    }
}