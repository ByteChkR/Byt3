using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;

namespace Byt3.OpenFL.Common.DataObjects.SerializableDataObjects.BuiltIn
{
    public class SerializeDecimalArgument : SerializableFLInstructionArgument
    {
        public decimal Value { get; }

        public SerializeDecimalArgument(decimal value)
        {
            Value = value;
        }


        public override object GetValue(FLProgram script)
        {
            return Value;
        }
    }
}