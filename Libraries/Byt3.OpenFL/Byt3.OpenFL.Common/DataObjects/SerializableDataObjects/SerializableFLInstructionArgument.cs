using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;

namespace Byt3.OpenFL.Common.DataObjects.SerializableDataObjects
{
    public abstract class SerializableFLInstructionArgument
    {
        internal static InstructionArgumentCategory Parse(char input)
        {
            switch (input)
            {
                case 'V': //Value
                    return InstructionArgumentCategory.Value;
                case 'F': //Function
                    return InstructionArgumentCategory.Function;
                case 'S': //Script
                    return InstructionArgumentCategory.Script;
                case 'B': //Buffer
                    return InstructionArgumentCategory.Buffer;
                case 'E': //Defined Element
                    return InstructionArgumentCategory.DefinedElement;
                case 'X': //Defined Executable(Script|Function)
                    return InstructionArgumentCategory.DefinedFunction;
                case 'I': //Internal Defined Element(E but without Scripts)
                    return InstructionArgumentCategory.InternalDefinedElement;
                default:
                    return InstructionArgumentCategory.Invalid;
            }
            
        }


        public abstract InstructionArgumentCategory ArgumentCategory { get; }
        public abstract string Identifier { get; }
        public abstract object GetValue(FLProgram script);

        public override string ToString()
        {
            return "Not Implemented for Argument Type: " + GetType().Name;
        }
    }
}