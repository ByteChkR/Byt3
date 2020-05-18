using System.Collections.Generic;
using System.Linq;
using Byt3.ADL;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;
using Byt3.OpenFL.Common.Exceptions;
using Byt3.OpenFL.Common.Instructions.InstructionCreators;
using Byt3.OpenFL.Common.Instructions.SignatureParsing;

namespace Byt3.OpenFL.Common.ProgramChecks.Checking.Signatures
{
    public class InstructionArgumentValidator : FLProgramCheck<SerializableFLProgram>
    {
        public override FLProgramCheckType CheckType => FLProgramCheckType.Validation;
        public override int Priority => 6;
        public override bool Recommended => true;

        private void VerifySignature(FLInstructionCreator creator, SerializableFLInstruction inst)
        {
            bool hasDefinedSig = SignatureParser.ParseCreatorSig(
                creator.GetArgumentSignatureForInstruction(inst.InstructionKey),
                out List<InstructionArgumentSignature> creatorSigs);
            if (!hasDefinedSig)
            {
                Logger.Log(LogType.Warning,
                    "FL Creator: " + creator.GetType().Name + " has no Argument Signatures defined. Skipping.",
                    1);
                return;
            }

            InstructionArgumentSignature sig = new InstructionArgumentSignature
            {
                Signature = inst.Arguments
                    .Select(x => x.ArgumentCategory).ToList()
            };
            bool matches = false;
            foreach (InstructionArgumentSignature instructionArgumentSignature in creatorSigs)
            {
                if (instructionArgumentSignature.Compare(sig.Signature))
                {
                    matches = true;
                    break;
                }
            }

            if (!matches)
            {
                throw new FLProgramCheckException(
                    "The Script is using the instruction with key: " +
                    inst.InstructionKey +
                    " with the Arguments: " + sig + " but no such argument overload is found.", this);
            }
        }

        public override object Process(object o)
        {
            SerializableFLProgram input = (SerializableFLProgram) o;
            foreach (SerializableFLFunction function in input.Functions)
            {
                foreach (SerializableFLInstruction instruction in function.Instructions)
                {
                    FLInstructionCreator creator = InstructionSet.GetCreator(instruction);

                    VerifySignature(creator, instruction);

                    if (function.Modifiers.IsStatic)
                    {
                        foreach (SerializableFLInstructionArgument instructionArgument in instruction.Arguments)
                        {
                            if ((instructionArgument.ArgumentCategory &
                                 InstructionArgumentCategory.DefinedElement) != 0 &&
                                instructionArgument.Identifier != "current")
                            {
                                throw new FLProgramCheckException(
                                    $"The Function {function.Name} is using the instruction with key: " +
                                    instruction.InstructionKey +
                                    " which contains parameter to Defined Elements. This is not allowed when the function is marked as static",
                                    this);
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