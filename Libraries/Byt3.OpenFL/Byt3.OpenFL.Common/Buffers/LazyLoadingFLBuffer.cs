using System;
using Byt3.OpenCL.Memory;
using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;

namespace Byt3.OpenFL.Common.Buffers
{
    public interface IWarmable
    {
        void Warm();
    }

    public class LazyLoadingFLBuffer : FLBuffer, IDisposable, IWarmable
    {
        public delegate FLBuffer BufferLoader(FLProgram root);

        protected BufferLoader Loader;
        private MemoryBuffer _buffer;

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

        public void Warm()
        {
            InitializeBuffer();
        }


        public LazyLoadingFLBuffer(BufferLoader loader) : base(default(MemoryBuffer), -1, -1)
        {
            Loader = loader;
        }

        internal override void ReplaceUnderlyingBuffer(MemoryBuffer buf, int width, int height)
        {
            Width = width;
            Height = height;
            Buffer = buf;
        }

        public override void Dispose()
        {
            _buffer?.Dispose();
            _buffer = null;
            Loader = null;
        }
    }
}