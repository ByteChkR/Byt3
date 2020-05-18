using Byt3.OpenFL.Common.Buffers;
using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;

namespace Byt3.OpenFL.Common.Arguments
{
    

    public class SerializeBufferArgument : SerializableFLInstructionArgument
    {
        public SerializeBufferArgument(string index)
        {
            Value = index;
        }

        public string Value { get; }

        public override InstructionArgumentCategory ArgumentCategory => InstructionArgumentCategory.Buffer;
        public override string Identifier => Value;


        public override ImplicitCastBox GetValue(FLProgram script, FLFunction func)
        {
            return new ImplicitCastBox<FLBuffer>(() => script.DefinedBuffers[Value]);
        }

        public override string ToString()
        {
            return Value;
        }
    }
}