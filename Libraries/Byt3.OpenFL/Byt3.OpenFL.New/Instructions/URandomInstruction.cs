using System;
using System.Collections.Generic;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenFL.New.DataObjects;
using Byt3.OpenFL.New.Exceptions;

namespace Byt3.OpenFL.New.Instructions
{
    public class URandomInstruction : Instruction
    {
        private static readonly Random Rnd = new Random();
        /// <summary>
        /// A function used as RandomFunc of type byte>
        /// </summary>
        /// <returns>a random byte</returns>
        private static byte Randombytesource()
        {
            return (byte)Rnd.Next();
        }

        public URandomInstruction(List<InstructionArgument> arguments) : base(arguments) { }


        public override void Process()
        {
            if (Arguments.Count == 0)
            {
                CLAPI.WriteRandom(Root.Instance, Root.ActiveBuffer.Buffer, Randombytesource, Root.ActiveChannels, true);
            }

            for (int i = 0; i < Arguments.Count; i++)
            {
                InstructionArgument obj = Arguments[i];

                if (obj.Type != InstructionArgumentType.Buffer)
                {
                    throw
                        new FLInvalidArgumentType("Argument: " + obj.Value, "MemoyBuffer/Image");
                }

                CLAPI.WriteRandom(Root.Instance, ((FLBufferInfo)obj.Value).Buffer, Randombytesource, Root.ActiveChannels, true);
            }
        }
    }
}