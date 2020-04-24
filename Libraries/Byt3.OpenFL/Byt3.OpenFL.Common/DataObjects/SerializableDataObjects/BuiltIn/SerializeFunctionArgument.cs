using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;

namespace Byt3.OpenFL.Common.DataObjects.SerializableDataObjects.BuiltIn
{
    public class SerializeFunctionArgument : SerializableFLInstructionArgument
    {
        public string Value { get; }

        public override InstructionArgumentCategory ArgumentCategory => InstructionArgumentCategory.Function;
        public override string Identifier => Value;

        public SerializeFunctionArgument(string name)
        {
            Value = name;
        }


        public override object GetValue(FLProgram script)
        {
            return script.FlFunctions[Value];
        }
    }
}