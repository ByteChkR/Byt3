using System.Collections.Generic;

namespace Byt3.OpenFL.Common.DataObjects.SerializableDataObjects
{
    public class SerializableFLFunction : SerializableNamedObject
    {
        public List<SerializableFLInstruction> Instructions { get; }

        public SerializableFLFunction(string name, List<SerializableFLInstruction> instructions) : base(name)
        {
            Instructions = instructions;
        }
    }
}