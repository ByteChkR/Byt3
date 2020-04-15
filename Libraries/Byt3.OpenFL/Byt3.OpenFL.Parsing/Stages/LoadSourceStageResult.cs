using Byt3.OpenCL.Wrapper;

namespace Byt3.OpenFL.Parsing.Stages
{
    public class LoadSourceStageResult
    {
        public readonly CLAPI Instance;
        public readonly string Filename;
        public readonly string[] Source;

        public LoadSourceStageResult(CLAPI instance, string filename, string[] source)
        {
            Instance = instance;
            Filename = filename;
            Source = source;
        }
    }
}