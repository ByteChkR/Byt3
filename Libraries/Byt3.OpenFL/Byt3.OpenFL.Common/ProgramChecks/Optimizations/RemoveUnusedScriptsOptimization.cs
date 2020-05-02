using System.Collections.Generic;
using Byt3.ADL;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;

namespace Byt3.OpenFL.Common.ProgramChecks.Optimizations
{
    public class RemoveUnusedScriptsOptimization : FLProgramCheck<SerializableFLProgram>
    {
        public override bool ChangesOutput => true;
        public override object Process(object o)
        {
            SerializableFLProgram input = (SerializableFLProgram) o;
            Dictionary<string, bool> scripts = new Dictionary<string, bool>();
            input.ExternalFunctions.ForEach(x => scripts.Add(x.Name, false));

            foreach (SerializableFLFunction serializableFlFunction in input.Functions)
            {
                foreach (SerializableFLInstruction serializableFlInstruction in serializableFlFunction.Instructions)
                {
                    foreach (SerializableFLInstructionArgument serializableFlInstructionArgument in
                        serializableFlInstruction.Arguments)
                    {
                        switch (serializableFlInstructionArgument.ArgumentCategory)
                        {
                            case InstructionArgumentCategory.Script:
                                scripts[serializableFlInstructionArgument.Identifier] = true;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }


            foreach (KeyValuePair<string, bool> keyValuePair in scripts)
            {
                if (keyValuePair.Value)
                {
                    continue; //Function used. Dont Remove
                }

                for (int i = input.ExternalFunctions.Count - 1; i >= 0; i--)
                {
                    if (input.ExternalFunctions[i].Name == keyValuePair.Key)
                    {
                        Logger.Log(LogType.Log, "Removing Script: " + keyValuePair.Key, 1);
                        input.ExternalFunctions.RemoveAt(i);
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