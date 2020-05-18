using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;

namespace Byt3.OpenFL.Common.Arguments
{
    public class SerializeArrayBufferArgument : SerializeBufferArgument
    {
        public SerializeArrayBufferArgument(string index) : base(index)
        {
        }

        public override InstructionArgumentCategory ArgumentCategory => InstructionArgumentCategory.BufferArray;
    }
}