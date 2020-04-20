using System.Drawing;

namespace Byt3.OpenFL.Common.Buffers.BufferCreators.BuiltIn.FromFile
{
    public class SerializableFromFileFLBuffer : SerializableFromBitmapFLBuffer
    {
        public string File { get; }

        public override Bitmap Bitmap => (Bitmap)Image.FromFile(File);

        public SerializableFromFileFLBuffer(string name, string file) : base(name, null)
        {
            File = file;
        }

        public override FLBuffer GetBuffer()
        {
            return new LazyFromFileFLBuffer(File);
        }


    }
}