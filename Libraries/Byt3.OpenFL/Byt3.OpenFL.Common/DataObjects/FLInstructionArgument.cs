using Byt3.OpenFL.Common.Buffers;

namespace Byt3.OpenFL.Common.DataObjects
{
    public class FLInstructionArgument : FLParsedObject
    {
        public FLInstructionArgumentType Type
        {
            get
            {
                FLInstructionArgumentType ret = FLInstructionArgumentType.Undefined;
                switch (Value)
                {
                    case null:
                        return ret;
                    case decimal _:
                        ret = FLInstructionArgumentType.Number;
                        break;
                    case FLBuffer _:
                        ret = FLInstructionArgumentType.Buffer;
                        break;
                    case FLFunction _:
                        ret = FLInstructionArgumentType.Function;
                        break;
                    case FLUnresolvedFunction _:
                        ret = FLInstructionArgumentType.UnresolvedFunction;
                        break;
                    case FLUnresolvedDefinedBuffer _:
                        ret = FLInstructionArgumentType.UnresolvedDefinedBuffer;
                        break;
                    default:
                        ret = FLInstructionArgumentType.Undefined;
                        break;
                }

                return ret;
            }
        }

        public object Value { get; set; }

        public FLInstructionArgument(object value)
        {
            Value = value;
        }

        public override void SetRoot(FLProgram root)
        {
            base.SetRoot(root);

            //HERE
            if (Type == FLInstructionArgumentType.Buffer || Type == FLInstructionArgumentType.Function)
            {
                ((FLParsedObject) Value).SetRoot(root);
            }
        }
    }
}