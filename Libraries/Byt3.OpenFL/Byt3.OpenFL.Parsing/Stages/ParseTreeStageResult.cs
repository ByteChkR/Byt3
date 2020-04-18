using System.Collections.Generic;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenFL.Common.Buffers;
using Byt3.OpenFL.Common.DataObjects;

namespace Byt3.OpenFL.Parsing.Stages
{
    public class ParseTreeStageResult
    {
        public CLAPI Instance { get; }
        public string Filename { get; }
        public string[] Source { get; }
        public Dictionary<string, FLBuffer> DefinedBuffers { get; }
        public FLFunction[] FlFunctions { get; }
        public Dictionary<string, FLFunction> DefinedScripts { get; }

        public ParseTreeStageResult(CLAPI instance, string filename, string[] source,
            Dictionary<string, FLFunction> definedScripts, Dictionary<string, FLBuffer> definedBuffers,
            FLFunction[] flFunctions)
        {
            Instance = instance;
            Filename = filename;
            Source = source;
            FlFunctions = flFunctions;
            DefinedBuffers = definedBuffers;
            DefinedScripts = definedScripts;
        }
    }
}