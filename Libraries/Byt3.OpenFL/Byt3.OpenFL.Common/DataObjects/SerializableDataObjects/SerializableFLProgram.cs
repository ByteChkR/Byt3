using System.Collections.Generic;

namespace Byt3.OpenFL.Common.DataObjects.SerializableDataObjects
{
    public class SerializableFLProgram
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