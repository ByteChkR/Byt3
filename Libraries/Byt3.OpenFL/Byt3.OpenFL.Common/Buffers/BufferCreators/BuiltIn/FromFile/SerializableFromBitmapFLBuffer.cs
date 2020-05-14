using System.Drawing;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;

namespace Byt3.OpenFL.Common.Buffers.BufferCreators.BuiltIn.FromFile
{
    public class SerializableFromBitmapFLBuffer : SerializableFLBuffer, IBitmapBasedBuffer
    {
        public readonly int Size;
        public virtual Bitmap Bitmap { get; }

        public SerializableFromBitmapFLBuffer(string name, Bitmap bmp, bool isArray, int size) : base(name, isArray)
        {
            Bitmap = bmp;
            Size = size;
        }

        public override FLBuffer GetBuffer()
        {
            if (IsArray)
            {

                return new LazyLoadingFLBuffer(root =>
                {
                    FLBuffer buf = new FLBuffer(root.Instance, Bitmap, "BitmapBuffer." + Name);
                    return buf;
                });
            }
            return new LazyLoadingFLBuffer(root =>
            {
                Bitmap bmp = new Bitmap(Bitmap, root.Dimensions.x, root.Dimensions.y);
                FLBuffer buf = new FLBuffer(root.Instance, bmp, "BitmapBuffer." + Name);
                bmp.Dispose();
                return buf;
            });
        }


        public virtual Bitmap GetBitmap(int width, int height)
        {
            return new Bitmap(Bitmap, width, height);
        }
    }
}