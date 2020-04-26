using System;
using System.Collections.Generic;
using Byt3.ADL;
using Byt3.OpenFL.Common.Buffers;
using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;

namespace Byt3.OpenFL.Common.Instructions
{
    public class SetActiveFLInstruction : FLInstruction
    {
        public SetActiveFLInstruction(List<FLInstructionArgument> arguments) : base(arguments)
        {
        }

        public override void Process()
        {
            byte[] newFlags = new byte[4];
            for (int i = 0; i < Arguments.Count; i++)
            {
                if (i == 0)
                {
                    if (Arguments[i].Type == FLInstructionArgumentType.Buffer)
                    {
                        FLBuffer obj = (FLBuffer) Arguments[i].Value;
                        Logger.Log(LogType.Log, "Setting Active Buffer: " + obj.DefinedBufferName,
                            MIN_INSTRUCTION_SEVERITY);

                        Root.ActiveBuffer = obj;
                        continue;
                    }


                    if (Arguments[i].Type == FLInstructionArgumentType.Function)
                    {

                        //FLBuffer buffer =
                        //    Root.RegisterUnmanagedBuffer(new FLBuffer(Root.Instance, Root.Dimensions.x,
                        //        Root.Dimensions.y, "FunctionInputBuffer_Registered"));

                        //Logger.Log(LogType.Log, $"Storing Current Execution Context", MIN_INSTRUCTION_SEVERITY + 3);
                        //Root.PushContext(); //Store Dynamic Variables

                        //FLFunction flFunction = (FLFunction)Arguments[i].Value; //Process the Function Object

                        //Logger.Log(LogType.Log, $"Executing Function: {flFunction.Name}", MIN_INSTRUCTION_SEVERITY + 2);

                        //Root.Run(Root.Instance, buffer, flFunction);

                        //Logger.Log(LogType.Log, $"Returning from Function Context", MIN_INSTRUCTION_SEVERITY + 3);
                        //Root.ReturnFromContext(); //Restore active channels and buffer

                        FLBuffer buffer =
                            Root.RegisterUnmanagedBuffer(new FLBuffer(Root.Instance, Root.Dimensions.x,
                                Root.Dimensions.y, "FunctionInputBuffer_Registered"));
                        FLFunction source = (FLFunction)Arguments[i].Value;


                        Logger.Log(LogType.Log, $"Storing Current Execution Context", MIN_INSTRUCTION_SEVERITY + 3);
                        Root.PushContext();

                        Logger.Log(LogType.Log, $"Executing Function: {source.Name}", MIN_INSTRUCTION_SEVERITY + 2);

                        Root.Run(Root.Instance, buffer, true, source);

                        FLBuffer output = Root.ActiveBuffer;

                        Logger.Log(LogType.Log, $"Returning from Function Context", MIN_INSTRUCTION_SEVERITY + 3);
                        Root.ReturnFromContext();

                        Root.ActiveBuffer = output;

                        continue;
                    }
                }

                if (Arguments[i].Type == FLInstructionArgumentType.Number)
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