using Byt3.OpenCL.Wrapper;

namespace Byt3.OpenFL.Parsing.Stages
{
    public class FLParserInput
    {
        public string Filename { get; }
        public CLAPI Instance { get; }

        public FLParserInput(string filename, CLAPI instance)
        {
            Filename = filename;
            Instance = instance;
        }
        public FLParserInput(string filename):this(filename, CLAPI.MainThread) { }
    }
}