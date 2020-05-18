using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;

namespace Byt3.OpenFL.Common.Arguments
{
    public class SerializeNameArgument : SerializableFLInstructionArgument
    {
        public SerializeNameArgument(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public override InstructionArgumentCategory ArgumentCategory => InstructionArgumentCategory.Name;
        public override string Identifier => Value; //Not used anyway


        public override ImplicitCastBox GetValue(FLProgram script, FLFunction func)
        {
            return new ImplicitCastBox<string>(() => Value);
        }

        public override string ToString()
        {
            return Value;
        }
    }
}