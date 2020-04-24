using System.Collections.Generic;
using System.ComponentModel;
using Byt3.ADL;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;
using Byt3.OpenFL.Common.Exceptions;

namespace Byt3.OpenFL.Common.ProgramChecks
{
    public class RemoveUnusedFunctionsOptimization : FLProgramCheck
    {
        public override SerializableFLProgram Process(SerializableFLProgram input)
        {
            Dictionary<string, bool> funcs = new Dictionary<string, bool>();
            input.Functions.ForEach(x => funcs.Add(x.Name, x.Name == "Main"));


            foreach (SerializableFLFunction serializableFlFunction in input.Functions)
            {
                foreach (SerializableFLInstruction serializableFlInstruction in serializableFlFunction.Instructions)
                {
                    foreach (SerializableFLInstructionArgument serializableFlInstructionArgument in serializableFlInstruction.Arguments)
                    {
                        switch (serializableFlInstructionArgument.ArgumentCategory)
                        {

                            case InstructionArgumentCategory.Function:
                                funcs[serializableFlInstructionArgument.Identifier] = true;
                                break;
                                //Find all Unused functions first so we can remove them before we check for unused scripts and buffers.
                                //case InstructionArgumentCategory.Buffer:
                                //    buffers[serializableFlInstructionArgument.Identifier] = true;
                                //    break;
                                //case InstructionArgumentCategory.Script:
                                //    scripts[serializableFlInstructionArgument.Identifier] = true;
                                //    break;
                        }
                    }
                }
            }


            foreach (KeyValuePair<string, bool> keyValuePair in funcs)
            {
                if (keyValuePair.Value) continue;  //Function used. Dont Remove
                for (int i = input.Functions.Count - 1; i >= 0; i--)
                {
                    if (input.Functions[i].Name == keyValuePair.Key)
                    {
                        Logger.Log(LogType.Log, "Removing Function: " + keyValuePair.Key, 1);
                        input.Functions.RemoveAt(i);
                    }
                }
            }

            foreach (SerializableExternalFLFunction serializableFlFunction in input.ExternalFunctions) //Process all Subsequent scripts
            {
                Process(serializableFlFunction.ExternalProgram);
            }


            return input;
        }
    }


    public class RemoveUnusedBuffersOptimization : FLProgramCheck
    {
        public override SerializableFLProgram Process(SerializableFLProgram input)
        {
            Dictionary<string, bool> buffers = new Dictionary<string, bool>();
            input.DefinedBuffers.ForEach(x => buffers.Add(x.Name, x.Name == "in"));

            foreach (SerializableFLFunction serializableFlFunction in input.Functions)
            {
                foreach (SerializableFLInstruction serializableFlInstruction in serializableFlFunction.Instructions)
                {
                    foreach (SerializableFLInstructionArgument serializableFlInstructionArgument in serializableFlInstruction.Arguments)
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




            foreach (KeyValuePair<string, bool> keyValuePair in buffers)
            {
                if (keyValuePair.Value) continue;  //Function used. Dont Remove
                for (int i = input.DefinedBuffers.Count - 1; i >= 0; i--)
                {
                    if (input.DefinedBuffers[i].Name == keyValuePair.Key)
                    {
                        Logger.Log(LogType.Log, "Removing Buffer: " + keyValuePair.Key, 1);
                        input.DefinedBuffers.RemoveAt(i);
                    }
                }
            }

            foreach (SerializableExternalFLFunction serializableFlFunction in input.ExternalFunctions) //Process all Subsequent scripts
            {
                Process(serializableFlFunction.ExternalProgram);
            }

            return input;
        }
    }
    public class RemoveUnusedScriptsOptimization : FLProgramCheck
    {
        public override SerializableFLProgram Process(SerializableFLProgram input)
        {
            Dictionary<string, bool> scripts = new Dictionary<string, bool>();
            input.ExternalFunctions.ForEach(x => scripts.Add(x.Name, false));

            foreach (SerializableFLFunction serializableFlFunction in input.Functions)
            {
                foreach (SerializableFLInstruction serializableFlInstruction in serializableFlFunction.Instructions)
                {
                    foreach (SerializableFLInstructionArgument serializableFlInstructionArgument in serializableFlInstruction.Arguments)
                    {
                        switch (serializableFlInstructionArgument.ArgumentCategory)
                        {
                            case InstructionArgumentCategory.Script:
                                scripts[serializableFlInstructionArgument.Identifier] = true;
                                break;
                        }
                    }
                }
            }


            foreach (KeyValuePair<string, bool> keyValuePair in scripts)
            {
                if (keyValuePair.Value) continue;  //Function used. Dont Remove
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