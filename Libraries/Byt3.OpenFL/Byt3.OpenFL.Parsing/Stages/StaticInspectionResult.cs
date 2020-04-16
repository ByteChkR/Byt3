using Byt3.OpenCL.Wrapper;

namespace Byt3.OpenFL.Parsing.Stages
{
    public class StaticInspectionResult
    {
        public CLAPI Instance { get; }
        public string Filename { get; }
        public string[] Source { get; }
        public string[] DefinedBuffers { get; }
        public string[] Functions { get; }
        public string[] DefinedScripts { get; }

        public StaticInspectionResult(CLAPI instance, string filename, string[] source, string[] functions,
            string[] definedBuffers, string[] definedScripts)
        {
            Instance = instance;
            Filename = filename;
            Source = source;
            Functions = functions;
            DefinedBuffers = definedBuffers;
            DefinedScripts = definedScripts;
        }
    }
}