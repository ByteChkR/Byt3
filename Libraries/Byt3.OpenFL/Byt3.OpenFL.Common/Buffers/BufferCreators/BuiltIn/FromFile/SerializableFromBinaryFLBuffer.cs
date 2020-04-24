using System.Drawing;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;

namespace Byt3.OpenFL.Common.Buffers.BufferCreators.BuiltIn.FromFile
{
    public class SerializableFromBitmapFLBuffer : SerializableFLBuffer, IBitmapBasedBuffer
    {
        public virtual Bitmap Bitmap { get; }

        public SerializableFromBitmapFLBuffer(string name, Bitmap bmp) : base(name)
        {
            Bitmap = bmp;
        }

        public override FLBuffer GetBuffer()
        {
            return new LazyLoadingFLBuffer(root =>
            {
                Bitmap bmp = new Bitmap(Bitmap, root.Dimensions.x, root.Dimensions.y);
                FLBuffer buf = new FLBuffer(root.Instance, bmp, "BitmapBuffer."+ Name);
                bmp.Dispose();
                return buf;
            });
        }


        public virtual Bitmap GetBitmap()
        {
            return Bitmap;
        }
    }

    public class SerializableFromBinaryFLBuffer : SerializableFLBuffer
    {
        public byte[] Data { get; }
        public int Width { get; }
        public int Height { get; }

        public SerializableFromBinaryFLBuffer(string name, byte[] data, int width, int height) : base(name)
        {
            Data = data;
            Width = width;
            Height = height;
        }

        public override FLBuffer GetBuffer()
        {
            return new FLBuffer(CLAPI.MainThread, Data, Width, Height, "BinaryBuffer." + Name);
        }
    }
}