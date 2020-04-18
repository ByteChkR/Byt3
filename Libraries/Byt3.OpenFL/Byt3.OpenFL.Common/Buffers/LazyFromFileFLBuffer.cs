using System.Drawing;

namespace Byt3.OpenFL.Common.Buffers
{
    public class LazyFromFileFLBuffer : LazyLoadingFLBuffer
    {
        private readonly string File;


        public LazyFromFileFLBuffer(string file) : base(null)
        {
            Loader = root =>
            {
                if (File == "INPUT")
                {
                    return root.Input;
                }

                Bitmap bmp = new Bitmap((Bitmap) Image.FromFile(File), root.Dimensions.x, root.Dimensions.y);
                return new FLBuffer(root.Instance, bmp);
            };
            File = file;
        }
    }
}