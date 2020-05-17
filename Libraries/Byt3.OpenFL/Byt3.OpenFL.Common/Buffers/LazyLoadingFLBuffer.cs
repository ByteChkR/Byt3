using System;
using Byt3.OpenCL.Memory;
using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;

namespace Byt3.OpenFL.Common.Buffers
{
    public class LazyLoadingFLBuffer : FLBuffer, IDisposable, IWarmable
    {
        public delegate FLBuffer BufferLoader(FLProgram root);

        private readonly bool WarmOnStart;
        private MemoryBuffer _buffer;

        protected BufferLoader Loader;


        public LazyLoadingFLBuffer(BufferLoader loader, bool warmOnStart) : base(default(MemoryBuffer), -1, -1)
        {
            WarmOnStart = warmOnStart;
            Loader = loader;
        }

        public override MemoryBuffer Buffer
        {
            get
            {
                InitializeBuffer();
                return _buffer;
            }
            protected set
            {
                _buffer?.Dispose();
                _buffer = value;
            }
        }

        public override void Dispose()
        {
            _buffer?.Dispose();
            _buffer = null;
            Loader = null;
        }

        public void Warm(bool force)
        {
            if (!WarmOnStart && !force)
            {
                return;
            }

            InitializeBuffer();
        }

        private void InitializeBuffer()
        {
            if (_buffer == null)
            {
                FLBuffer i = Loader(Root);
                _buffer = i.Buffer;
                Width = i.Width;
                Height = i.Height;
            }
        }

        internal override void ReplaceUnderlyingBuffer(MemoryBuffer buf, int width, int height)
        {
            Width = width;
            Height = height;
            Buffer = buf;
        }
    }
}