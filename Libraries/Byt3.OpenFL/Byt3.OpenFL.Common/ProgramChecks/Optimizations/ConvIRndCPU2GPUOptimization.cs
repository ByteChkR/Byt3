using System.Collections.Generic;
using Byt3.ADL;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;

namespace Byt3.OpenFL.Common.ProgramChecks.Optimizations
{
    public class ConvIRndCPU2GPUOptimization : FLProgramCheck<SerializableFLProgram>
    {
        public override bool ChangesOutput => true;
        public override object Process(object o)
        {
            SerializableFLProgram input = (SerializableFLProgram)o;

            foreach (SerializableFLFunction serializableFlFunction in input.Functions)
            {
                for (int i = 0; i < serializableFlFunction.Instructions.Count; i++)
                {
                    SerializableFLInstruction serializableFlInstruction = serializableFlFunction.Instructions[i];
                    if (serializableFlInstruction.InstructionKey == "urnd" ||
                        serializableFlInstruction.InstructionKey == "rnd")
                    {
                        if (serializableFlInstruction.Arguments.Count == 0)
                        {
                            serializableFlFunction.Instructions[i] = new SerializableFLInstruction(
                                serializableFlInstruction.InstructionKey + "_gpu",
                                new List<SerializableFLInstructionArgument>());
                            Logger.Log(LogType.Log, "Weaved: " + serializableFlFunction.Instructions[i], 2);
                        }
                        else
                        {
                            Logger.Log(LogType.Log, "Can not Weave random instruction with arguments.", 1);
                        }
                    }
                }
            }

            return input;
        }
    }
}