using System.Globalization;
using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;

namespace Byt3.OpenFL.Common.DataObjects.SerializableDataObjects.BuiltIn
{
    public class SerializeDecimalArgument : SerializableFLInstructionArgument
    {
        public decimal Value { get; }

        public override InstructionArgumentCategory ArgumentCategory => InstructionArgumentCategory.Value;
        public override string Identifier => Value.ToString(); //Not used anyway

        public SerializeDecimalArgument(decimal value)
        {
            Value = value;
        }


        public override object GetValue(FLProgram script)
        {
            return Value;
        }

        public override string ToString()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }
    }
}