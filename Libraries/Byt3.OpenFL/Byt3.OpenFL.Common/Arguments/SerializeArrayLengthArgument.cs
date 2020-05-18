using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;

namespace Byt3.OpenFL.Common.Arguments
{
    public class SerializeArrayLengthArgument : SerializableFLInstructionArgument
    {
        public SerializeArrayLengthArgument(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public override InstructionArgumentCategory ArgumentCategory => InstructionArgumentCategory.Value;
        public override string Identifier => "~" + Value;

        public override ImplicitCastBox GetValue(FLProgram script, FLFunction func)
        {
            return new ImplicitCastBox<decimal>(() => script.DefinedBuffers[Value].Size);
        }

        public override string ToString()
        {
            return Identifier;
        }
    }
}