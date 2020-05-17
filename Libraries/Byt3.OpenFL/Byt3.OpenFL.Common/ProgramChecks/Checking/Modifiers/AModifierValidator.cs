using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;

namespace Byt3.OpenFL.Common.ProgramChecks.Checking.Modifiers
{
    public abstract class AModifierValidator : FLProgramCheck<SerializableFLProgram>
    {
        public override FLProgramCheckType CheckType => FLProgramCheckType.Validation;
        public override bool Recommended => true;
        protected abstract InstructionArgumentCategory InvalidArguments { get; }

        protected abstract void Validate(SerializableFLProgram prog, SerializableFLFunction func,
            SerializableFLInstruction inst, SerializableFLInstructionArgument arg);

        protected abstract bool AppliesOnInstruction(SerializableFLInstruction instr);

        public override object Process(object o)
        {
            SerializableFLProgram input = (SerializableFLProgram) o;
            foreach (SerializableFLFunction function in input.Functions)
            {
                foreach (SerializableFLInstruction instruction in function.Instructions)
                {
                    if (AppliesOnInstruction(instruction))
                    {
                        foreach (SerializableFLInstructionArgument arg in instruction.Arguments)
                        {
                            if ((arg.ArgumentCategory & InvalidArguments) != 0)
                            {
                                Validate(input, function, instruction, arg);
                            }
                        }
                    }
                }
            }

            foreach (SerializableExternalFLFunction serializableFlFunction in input.ExternalFunctions)
            {
                Process(serializableFlFunction.ExternalProgram);
            }

            return input;
        }
    }
}