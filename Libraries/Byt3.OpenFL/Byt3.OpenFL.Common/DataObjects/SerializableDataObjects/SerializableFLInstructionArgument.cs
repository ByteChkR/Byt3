using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;

namespace Byt3.OpenFL.Common.DataObjects.SerializableDataObjects
{
    public abstract class SerializableFLInstructionArgument
    {
        public abstract InstructionArgumentCategory ArgumentCategory { get; }
        public abstract string Identifier { get; }
        public abstract ImplicitCastBox GetValue(FLProgram script,FLFunction func);

        public override string ToString()
        {
            return "Not Implemented for Argument Type: " + GetType().Name;
        }
    }
}