using System;
using Byt3.OpenCL.Memory;
using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;

namespace Byt3.OpenFL.Common.Buffers
{
    public class LazyLoadingFLBuffer : FLBuffer, IDisposable
    {
        public delegate FLBuffer BufferLoader(FLProgram root);

        protected BufferLoader Loader;
        private MemoryBuffer _buffer;

        public override MemoryBuffer Buffer
        {
            get
            {
                if (_buffer == null)
                {
                    FLBuffer i = Loader(Root);

                    _buffer = i.Buffer;
                    Width = i.Width;
                    Height = i.Height;
                }

                return _buffer;
            }
        }

        public LazyLoadingFLBuffer(BufferLoader loader) : base(default(MemoryBuffer), -1, -1)
        {
            Loader = loader;
        }

        public override void Dispose()
        {
            _buffer?.Dispose();
            _buffer = null;
        }
    }
}