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
            int off = 0;
            for (int i = 0; i < Arguments.Count; i++)
            {
                if (i == 0)
                {
                    if (Arguments[i].Type == InstructionArgumentType.Buffer)
                    {
                        Root.ActiveBuffer = (FLBufferInfo)Arguments[i].Value;
                        continue;
                    }
                    else if (Arguments[i].Type == InstructionArgumentType.Script)
                    {
                        FLBufferInfo buffer = new FLBufferInfo(Root.Instance, Root.Dimensions.x, Root.Dimensions.y);
                        ParsedSource source = (ParsedSource)Arguments[i].Value;

                        source.Run(Root.Instance, Root.KernelDB, buffer);

                        Root.DefinedBuffers.Add(source.ScriptName, buffer);
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