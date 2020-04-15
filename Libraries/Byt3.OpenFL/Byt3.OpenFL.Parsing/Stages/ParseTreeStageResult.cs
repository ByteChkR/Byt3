using System.Collections.Generic;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenFL.Parsing.DataObjects;

namespace Byt3.OpenFL.Parsing.Stages
{
    public class ParseTreeStageResult
    {
        public readonly CLAPI Instance;
        public readonly string Filename;
        public readonly string[] Source;
        public readonly Dictionary<string, FLBufferInfo> DefinedBuffers;
        public readonly FunctionObject[] Functions;
        public readonly Dictionary<string, FunctionObject> DefinedScripts;

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