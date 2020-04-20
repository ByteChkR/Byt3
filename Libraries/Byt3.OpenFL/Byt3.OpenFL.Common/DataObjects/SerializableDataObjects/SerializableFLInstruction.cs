using System.Collections.Generic;

namespace Byt3.OpenFL.Common.DataObjects.SerializableDataObjects
{
    public class SerializableFLInstruction
    {
        public string InstructionKey { get; }
        public List<SerializableFLInstructionArgument> Arguments { get; }

        public SerializableFLInstruction(string instructionKey, List<SerializableFLInstructionArgument> arguments)
        {
            InstructionKey = instructionKey;
            Arguments = arguments;
        }
    }
}