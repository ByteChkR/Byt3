using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;

namespace Byt3.OpenFL.Common.Arguments
{
    public class SerializeExternalFunctionArgument : SerializableFLInstructionArgument
    {
        public SerializeExternalFunctionArgument(string index)
        {
            Value = index;
        }

        public string Value { get; }

        public override InstructionArgumentCategory ArgumentCategory => InstructionArgumentCategory.Script;
        public override string Identifier => Value;


        public override ImplicitCastBox GetValue(FLProgram script, FLFunction func)
        {
            return new ImplicitCastBox<IFunction>(() => script.DefinedScripts[Value]);
        }

        public override string ToString()
        {
            return Value;
        }
    }
}