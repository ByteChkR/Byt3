using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;

namespace Byt3.OpenFL.Common.DataObjects.SerializableDataObjects
{
    public abstract class SerializableFLInstructionArgument
    {
        public abstract object GetValue(FLProgram script);
    }
}