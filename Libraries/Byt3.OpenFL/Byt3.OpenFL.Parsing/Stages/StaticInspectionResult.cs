using Byt3.OpenCL.Wrapper;

namespace Byt3.OpenFL.Parsing.Stages
{
    public class StaticInspectionResult
    {
        public readonly CLAPI Instance;
        public readonly string Filename;
        public readonly string[] Source;
        public readonly string[] DefinedBuffers;
        public readonly string[] Functions;
        public readonly string[] DefinedScripts;

        public StaticInspectionResult(CLAPI instance,string filename,string[] source, string[] functions, string[] definedBuffers, string[] definedScripts)
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