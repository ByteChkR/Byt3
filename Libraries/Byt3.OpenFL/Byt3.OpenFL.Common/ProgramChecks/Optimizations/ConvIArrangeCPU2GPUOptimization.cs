using System;
using Byt3.ADL;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;

namespace Byt3.OpenFL.Common.ProgramChecks.Optimizations
{
    public class ConvIArrangeCPU2GPUOptimization : FLProgramCheck<SerializableFLProgram>
    {
        public override bool ChangesOutput => true;
        public override object Process(object o)
        {
            SerializableFLProgram input = (SerializableFLProgram)o;
            if (!InstructionSet.HasInstruction("gpu_arrange"))
            {
                throw new InvalidOperationException("Can not Convert CPU Arrange instruction when there is no gpu_arrange instruction provided.");
            }

            foreach (SerializableFLFunction serializableFlFunction in input.Functions)
            {
                for (int i = 0; i < serializableFlFunction.Instructions.Count; i++)
                {
                    SerializableFLInstruction serializableFlInstruction = serializableFlFunction.Instructions[i];
                    if (serializableFlInstruction.InstructionKey == "arrange")
                    {

                        serializableFlFunction.Instructions[i] = new SerializableFLInstruction(
                            "gpu_arrange",
                            serializableFlFunction.Instructions[i].Arguments);
                        Logger.Log(LogType.Log, "Weaved: " + serializableFlFunction.Instructions[i], 2);

                    }
                }
            }

            return input;
        }
    }
}