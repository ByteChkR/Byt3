using System;
using System.Collections.Generic;
using Byt3.ADL;
using Byt3.OpenFL.Parsing.DataObjects;

namespace Byt3.OpenFL.Parsing.Instructions
{
    public class SetActiveInstruction : Instruction
    {
        public SetActiveInstruction(List<InstructionArgument> arguments) : base(arguments) { }

        public override void Process()
        {
            byte[] newFlags = new byte[4];
            for (int i = 0; i < Arguments.Count; i++)
            {
                if (i == 0)
                {
                    if (Arguments[i].Type == InstructionArgumentType.Buffer)
                    {
                        FLBufferInfo obj = (FLBufferInfo)Arguments[i].Value;
                        Logger.Log(LogType.Log, "Setting Active Buffer: " + obj.DefinedBufferName, MIN_INSTRUCTION_SEVERITY);

                        Root.ActiveBuffer = obj;
                        continue;
                    }



                    if (Arguments[i].Type == InstructionArgumentType.Function)
                    {
                        FLBufferInfo buffer = Root.RegisterUnmanagedBuffer(new FLBufferInfo(Root.Instance, Root.Dimensions.x, Root.Dimensions.y));
                        FunctionObject source = (FunctionObject)Arguments[i].Value;
                        

                        Logger.Log(LogType.Log, $"Storing Current Execution Context", MIN_INSTRUCTION_SEVERITY + 3);
                        Root.PushContext();

                        Logger.Log(LogType.Log, $"Executing Function: {source.Name}", MIN_INSTRUCTION_SEVERITY + 2);

                        Root.ActiveBuffer = buffer;
                        source.Process();

                        FLBufferInfo output = Root.ActiveBuffer;

                        Logger.Log(LogType.Log, $"Returning from Function Context", MIN_INSTRUCTION_SEVERITY + 3);
                        Root.ReturnFromContext();

                        Root.ActiveBuffer = output;

                        continue;
                    }

                }

                if (Arguments[i].Type == InstructionArgumentType.Number)
                {
                    int channel = (int) Convert.ChangeType(Arguments[i].Value, typeof(int));
                    Logger.Log(LogType.Log, "Setting Active Channel: " + channel, MIN_INSTRUCTION_SEVERITY);
                    newFlags[channel] = 1;
                }
                else
                {
                    throw new InvalidOperationException("Invalid Channel ID");
                }

            }

            Root.ActiveChannels = newFlags;
        }
    }
}