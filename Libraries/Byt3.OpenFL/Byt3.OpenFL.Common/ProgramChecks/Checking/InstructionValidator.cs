using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;
using Byt3.OpenFL.Common.Exceptions;

namespace Byt3.OpenFL.Common.ProgramChecks
{
    public class InstructionValidator : FLProgramCheck<SerializableFLProgram>
    {
        public override FLProgramCheckType CheckType => FLProgramCheckType.Validation;
        public override int Priority => 5;
        public override bool Recommended => true;

        public override object Process(object o)
        {
            SerializableFLProgram input = (SerializableFLProgram) o;
            foreach (SerializableFLFunction serializableFlFunction in input.Functions)
            {
                foreach (SerializableFLInstruction serializableFlInstruction in serializableFlFunction.Instructions)
                {
                    if (!InstructionSet.HasInstruction(serializableFlInstruction.InstructionKey))
                    {
                        throw new FLProgramCheckException(
                            "The Script is referencing the instruction with key: " +
                            serializableFlInstruction.InstructionKey +
                            " but the Instruction is not in the Instruction Set", this);
                    }

                    //if (serializableFlFunction.Modifiers.IsStatic)
                    //{
                    //    FLInstructionCreator c = InstructionSet.GetCreator(serializableFlInstruction);
                    //    if (!c.AllowStaticUse)
                    //    {
                    //        throw new FLProgramCheckException(
                    //            $"The Script is referencing the instruction with key: " +
                    //            serializableFlInstruction.InstructionKey +
                    //            " but the Instruction is not Available in static functions", this);
                    //    }
                    //}
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