using System.Drawing;
using System.IO;
using Byt3.Callbacks;
using Byt3.OpenFL.Common.ElementModifiers;

namespace Byt3.OpenFL.Common.Buffers.BufferCreators.BuiltIn.FromFile
{
    public class SerializableFromFileFLBuffer : SerializableFromBitmapFLBuffer
    {
        public SerializableFromFileFLBuffer(string name, string file, FLBufferModifiers modifiers, int size) : base(
            name, null, modifiers, size)
        {
            File = file;
        }

        public string File { get; }

        public override Bitmap Bitmap
        {
            get
            {
                Stream s = IOManager.GetStream(File);
                Bitmap ret = (Bitmap) Image.FromStream(s);
                s.Close();
                return ret;
            }
        }

        public override FLBuffer GetBuffer()
        {
            return new LazyFromFileFLBuffer(File, IsArray, Size, Modifiers);
        }

        public override string ToString()
        {
            return base.ToString() + File;
        }
    }
}