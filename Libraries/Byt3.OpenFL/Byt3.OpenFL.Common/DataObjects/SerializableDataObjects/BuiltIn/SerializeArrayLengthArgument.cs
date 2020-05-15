using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;

namespace Byt3.OpenFL.Common.DataObjects.SerializableDataObjects.BuiltIn
{
    public class SerializeArrayLengthArgument : SerializableFLInstructionArgument
    {
        public string Value { get; }

        public override InstructionArgumentCategory ArgumentCategory => InstructionArgumentCategory.Value;
        public override string Identifier => "~" + Value;
        public SerializeArrayLengthArgument(string value)
        {
            Value = value;
        }

        public override object GetValue(FLProgram script)
        {
            return (decimal)script.DefinedBuffers[Value].Size;
        }

        public override string ToString()
        {
            return Identifier;
        }
    }
}