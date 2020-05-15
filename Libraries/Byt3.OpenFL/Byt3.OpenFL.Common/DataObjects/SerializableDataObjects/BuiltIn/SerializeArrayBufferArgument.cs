namespace Byt3.OpenFL.Common.DataObjects.SerializableDataObjects.BuiltIn
{
    public class SerializeArrayBufferArgument : SerializeBufferArgument
    {
        public override InstructionArgumentCategory ArgumentCategory => InstructionArgumentCategory.BufferArray;

        public SerializeArrayBufferArgument(string index) : base(index)
        {
        }

    }
}