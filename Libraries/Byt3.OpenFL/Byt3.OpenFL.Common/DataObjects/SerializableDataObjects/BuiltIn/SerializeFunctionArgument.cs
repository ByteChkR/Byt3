using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;

namespace Byt3.OpenFL.Common.DataObjects.SerializableDataObjects.BuiltIn
{
    public class SerializeFunctionArgument : SerializableFLInstructionArgument
    {
        public SerializeFunctionArgument(string name)
        {
            Value = name;
        }

        public string Value { get; }

        public override InstructionArgumentCategory ArgumentCategory => InstructionArgumentCategory.Function;
        public override string Identifier => Value;


        public override ImplicitCastBox GetValue(FLProgram script)
        {
            return new ImplicitCastBox<IFunction>(() => script.FlFunctions[Value]);
        }

        public override string ToString()
        {
            return Value;
        }
    }
}