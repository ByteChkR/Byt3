using System.Drawing;
using System.IO;
using Byt3.Callbacks;

namespace Byt3.OpenFL.Common.Buffers.BufferCreators.BuiltIn.FromFile
{
    public class SerializableFromFileFLBuffer : SerializableFromBitmapFLBuffer
    {
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

        public SerializableFromFileFLBuffer(string name, string file, bool isArray, int size) : base(name, null, isArray, size)
        {
            File = file;
        }

        public override FLBuffer GetBuffer()
        {
            return new LazyFromFileFLBuffer(File, IsArray, Size);
        }

        public override string ToString()
        {
            return $"--define {(IsArray ? "array" : "texture")} {Name}: {File}";
        }
    }
}