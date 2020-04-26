using System.Collections.Generic;
using Byt3.OpenFL.Common.ProgramChecks;

namespace Byt3.OpenFL.Common.DataObjects.SerializableDataObjects
{
    public class SerializableFLProgram : FLPipelineResult
    {
        public List<SerializableExternalFLFunction> ExternalFunctions { get; }
        public List<SerializableFLFunction> Functions { get; }
        public List<SerializableFLBuffer> DefinedBuffers { get; }

        public SerializableFLProgram(List<SerializableExternalFLFunction> externalFunctions,
            List<SerializableFLFunction> functions, List<SerializableFLBuffer> definedBuffers)
        {
            ExternalFunctions = externalFunctions;
            Functions = functions;
            DefinedBuffers = definedBuffers;
        }
    }
}