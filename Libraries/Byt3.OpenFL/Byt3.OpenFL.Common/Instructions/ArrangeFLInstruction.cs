using System;
using System.Collections.Generic;
using Byt3.ADL;
using Byt3.OpenFL.Common.Buffers;
using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;
using Byt3.Utilities.FastString;

namespace Byt3.OpenFL.Common.Instructions
{
    public abstract class ArrangeFLInstruction : FLInstruction
    {


        protected ArrangeFLInstruction(List<FLInstructionArgument> arguments) : base(arguments)
        {

        }

        public override void Process()
        {
            byte[] newOrder = new byte[Root.ActiveChannels.Length];
            for (int i = 0; i < newOrder.Length; i++)
            {
                if (i >= Arguments.Count)
                {
                    newOrder[i] = (byte)i;
                }
                else
                {
                    if (Arguments[i].Type == FLInstructionArgumentType.Number)
                    {
                        byte channel = (byte)Convert.ChangeType(Arguments[i].Value, typeof(byte));
                        newOrder[i] = channel;
                    }
                    else
                    {
                        throw new InvalidOperationException("Invalid Channel ID");
                    }
                }
            }

            Arrange(newOrder);

        }

        protected abstract void Arrange(byte[] newOrder);

        public override string ToString()
        {
            return "arrange " + Arguments.Unpack(" ");
        }


    }
}