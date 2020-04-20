using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;

namespace Byt3.OpenFL.Common.DataObjects.SerializableDataObjects.BuiltIn
{
    public class SerializeFunctionArgument : SerializableFLInstructionArgument
    {
        public int Value { get; }

        public SerializeFunctionArgument(int index)
        {
            Value = index;
        }


        public override object GetValue(FLProgram script)
        {
            return script.FlFunctions[Value];
        }
    }
}