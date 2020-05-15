using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;

namespace Byt3.OpenFL.Common.DataObjects.SerializableDataObjects.BuiltIn
{
    public class SerializeNameArgument : SerializableFLInstructionArgument
    {
        public string Value { get; }

        public override InstructionArgumentCategory ArgumentCategory => InstructionArgumentCategory.Name;
        public override string Identifier => Value; //Not used anyway

        public SerializeNameArgument(string value)
        {
            Value = value;
        }


        public override object GetValue(FLProgram script)
        {
            return Value;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}