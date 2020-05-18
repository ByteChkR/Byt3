using System.Globalization;
using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;

namespace Byt3.OpenFL.Common.Arguments
{
    public class SerializeDecimalArgument : SerializableFLInstructionArgument
    {
        public SerializeDecimalArgument(decimal value)
        {
            Value = value;
        }

        public decimal Value { get; }

        public override InstructionArgumentCategory ArgumentCategory => InstructionArgumentCategory.Value;
        public override string Identifier => Value.ToString(); //Not used anyway


        public override ImplicitCastBox GetValue(FLProgram script, FLFunction func)
        {
            return new ImplicitCastBox<decimal>(() => Value);
        }

        public override string ToString()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }
    }
}