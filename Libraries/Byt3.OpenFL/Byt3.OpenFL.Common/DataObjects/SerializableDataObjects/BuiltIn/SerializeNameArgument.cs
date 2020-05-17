using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;

namespace Byt3.OpenFL.Common.DataObjects.SerializableDataObjects.BuiltIn
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


        public override ImplicitCastBox GetValue(FLProgram script)
        {
            return new ImplicitCastBox<string>(() => Value);
        }

        public override string ToString()
        {
            return Value;
        }
    }
}