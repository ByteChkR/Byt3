using System.Collections.Generic;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenFL.Parsing.DataObjects;

namespace Byt3.OpenFL.Parsing.Stages
{
    public class ParseTreeStageResult
    {
        public CLAPI Instance { get; }
        public string Filename { get; }
        public string[] Source { get; }
        public Dictionary<string, FLBufferInfo> DefinedBuffers { get; }
        public FunctionObject[] Functions { get; }
        public Dictionary<string, FunctionObject> DefinedScripts { get; }

        public ParseTreeStageResult(CLAPI instance, string filename, string[] source, Dictionary<string, FunctionObject> definedScripts, Dictionary<string, FLBufferInfo> definedBuffers, FunctionObject[] functions)
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