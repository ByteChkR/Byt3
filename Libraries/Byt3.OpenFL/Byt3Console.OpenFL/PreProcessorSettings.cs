namespace Byt3Console.OpenFL
{
    public struct Resolution
    {
        public int X;
        public int Y;

        public Resolution(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    public struct PreProcessorSettings
    {
        public string KernelFolder;
        public Resolution InternalResolution;


        public static PreProcessorSettings GetDefault()
        {
            return new PreProcessorSettings {InternalResolution = new Resolution(512, 512), KernelFolder = "./kernels"};
        }
    }
}