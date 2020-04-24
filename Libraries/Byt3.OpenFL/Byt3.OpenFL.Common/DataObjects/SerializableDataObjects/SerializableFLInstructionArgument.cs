using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;

namespace Byt3.OpenFL.Common.DataObjects.SerializableDataObjects
{
    public enum InstructionArgumentCategory
    {
        Value,
        Function,
        Script,
        Buffer
    }
    public abstract class SerializableFLInstructionArgument
    {
        public abstract InstructionArgumentCategory ArgumentCategory { get; }
        public abstract string Identifier { get; }
        public abstract object GetValue(FLProgram script);
    }
}