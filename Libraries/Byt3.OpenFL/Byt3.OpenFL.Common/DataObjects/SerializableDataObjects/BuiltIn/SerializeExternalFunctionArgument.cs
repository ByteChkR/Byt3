using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;

namespace Byt3.OpenFL.Common.DataObjects.SerializableDataObjects.BuiltIn
{
    public class SerializeExternalFunctionArgument : SerializableFLInstructionArgument
    {
        public string Value { get; }

        public SerializeExternalFunctionArgument(string index)
        {
            Value = index;
        }


        public override object GetValue(FLProgram script)
        {
            return script.DefinedScripts[Value];
        }
    }
}