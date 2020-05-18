using System.Collections.Generic;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenFL.Common.Buffers;
using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;

namespace Byt3.OpenFL.Common.Parsing.StageResults
{
    public class ParseTreeStageResult
    {
        public ParseTreeStageResult(CLAPI instance, Dictionary<string, FLFunction> definedScripts,
            Dictionary<string, FLBuffer> definedBuffers, FLFunction[] flFunctions)
        {
            Instance = instance;
            FlFunctions = flFunctions;
            DefinedBuffers = definedBuffers;
            DefinedScripts = definedScripts;
        }

        public CLAPI Instance { get; }

        //Task:
        //  Replace FLBuffer Class with AParsableBuffer that implements GetBuffer() which returns the FLBuffer object
        //      Because: Serializer can then Serialize the Parsable Buffer
        public Dictionary<string, FLBuffer> DefinedBuffers { get; }
        public FLFunction[] FlFunctions { get; }
        public Dictionary<string, FLFunction> DefinedScripts { get; }
    }
}