using Byt3.OpenCL.Wrapper;

namespace Byt3.OpenFL.Parsing.Stages
{
    public class LoadSourceStageResult
    {
        public CLAPI Instance { get; }
        public string Filename { get; }
        public string[] Source { get; }

        public LoadSourceStageResult(CLAPI instance, string filename, string[] source)
        {
            Instance = instance;
            Filename = filename;
            Source = source;
        }
    }
}