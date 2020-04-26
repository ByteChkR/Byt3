using System.Collections.Generic;
using Byt3.ADL;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;

namespace Byt3.OpenFL.Common.ProgramChecks
{
    public class RemoveUnusedBuffersOptimization : FLProgramCheck<SerializableFLProgram>
    {
        public override object Process(object o)
        {
            SerializableFLProgram input = (SerializableFLProgram) o;
            Dictionary<string, bool> buffers = new Dictionary<string, bool>();
            input.DefinedBuffers.ForEach(x => buffers.Add(x.Name, x.Name == "in"));

            foreach (SerializableFLFunction serializableFlFunction in input.Functions)
            {
                foreach (SerializableFLInstruction serializableFlInstruction in serializableFlFunction.Instructions)
                {
                    foreach (SerializableFLInstructionArgument serializableFlInstructionArgument in
                        serializableFlInstruction.Arguments)
                    {
                        switch (serializableFlInstructionArgument.ArgumentCategory)
                        {
                            case InstructionArgumentCategory.Buffer:
                                buffers[serializableFlInstructionArgument.Identifier] = true;
                                break;
                        }
                    }
                }
            }


            Logger.Log(LogType.Log, "Removing Buffers", 1);
            foreach (KeyValuePair<string, bool> keyValuePair in buffers)
            {
                if (keyValuePair.Value)
                {
                    continue; //Function used. Dont Remove
                }

                for (int i = input.DefinedBuffers.Count - 1; i >= 0; i--)
                {
                    if (input.DefinedBuffers[i].Name == keyValuePair.Key)
                    {
                        input.DefinedBuffers.RemoveAt(i);
                    }
                }
            }

            foreach (SerializableExternalFLFunction serializableFlFunction in input.ExternalFunctions
            ) //Process all Subsequent scripts
            {
                Process(serializableFlFunction.ExternalProgram);
            }

            return input;
        }
    }
}