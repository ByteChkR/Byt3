using System.Drawing;

namespace Byt3.OpenFL.Common.Buffers
{
    public class LazyFromFileFLBuffer : LazyLoadingFLBuffer
    {
        public readonly string File;


        public LazyFromFileFLBuffer(string file) : base(null)
        {
            File = file;
            Loader = root =>
            {
                if (File == "INPUT")
                {
                    return root.Input;
                }

                Bitmap bmp = new Bitmap(Image.FromFile(File), root.Dimensions.x, root.Dimensions.y);
                FLBuffer buf = new FLBuffer(root.Instance, bmp, DefinedBufferName + ":" + File);
                bmp.Dispose();
                return buf;
            };
        }
    }
}