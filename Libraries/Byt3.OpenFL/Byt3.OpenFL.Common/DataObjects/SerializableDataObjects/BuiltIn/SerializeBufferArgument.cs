using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;

namespace Byt3.OpenFL.Common.DataObjects.SerializableDataObjects.BuiltIn
{
    public class SerializeBufferArgument : SerializableFLInstructionArgument
    {
        public string Value { get; }

        public SerializeBufferArgument(string index)
        {
            Value = index;
        }


        public override object GetValue(FLProgram script)
        {
            return script.DefinedBuffers[Value];
        }
    }
}