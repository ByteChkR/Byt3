using System;
using System.Collections.Generic;
using Byt3.OpenFL.New.DataObjects;

namespace Byt3.OpenFL.New.Instructions
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
                        Root.ActiveBuffer = (FLBufferInfo)Arguments[i].Value;
                        continue;
                    }



                    if (Arguments[i].Type == InstructionArgumentType.Function)
                    {
                        FLBufferInfo buffer = Root.RegisterUnmanagedBuffer(new FLBufferInfo(Root.Instance, Root.Dimensions.x, Root.Dimensions.y));
                        FunctionObject source = (FunctionObject)Arguments[i].Value;
                        
                        Root.PushContext();
                        Root.Run(Root.Instance, Root.KernelDB, buffer, source);
                        FLBufferInfo output = Root.ActiveBuffer;
                        Root.ReturnFromContext();
                        Root.ActiveBuffer = output;
                        continue;
                    }

                }

                if (Arguments[i].Type == InstructionArgumentType.Number)
                    newFlags[(int)Convert.ChangeType(Arguments[i].Value, typeof(int))] = 1;
                else
                {
                    throw new InvalidOperationException("Invalid Channel ID");
                }

            }

            Root.ActiveChannels = newFlags;
        }
    }
}