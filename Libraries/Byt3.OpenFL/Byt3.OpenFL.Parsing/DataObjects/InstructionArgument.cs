namespace Byt3.OpenFL.Parsing.DataObjects
{
    public class InstructionArgument : ParsedObject
    {
        public InstructionArgumentType Type
        {
            get
            {
                InstructionArgumentType ret = InstructionArgumentType.Undefined;
                switch (Value)
                {
                    case null:
                        return ret;
                    case decimal _:
                        ret = InstructionArgumentType.Number;
                        break;
                    case FLBufferInfo _:
                        ret = InstructionArgumentType.Buffer;
                        break;
                    case FunctionObject _:
                        ret = InstructionArgumentType.Function;
                        break;
                    case UnresolvedFunction _:
                        ret = InstructionArgumentType.UnresolvedFunction;
                        break;
                    case UnresolvedDefinedBuffer _:
                        ret = InstructionArgumentType.UnresolvedDefinedBuffer;
                        break;
                    default:
                        ret = InstructionArgumentType.Undefined;
                        break;
                }

                return ret;
            }
        }

        public object Value { get; internal set; }

        public InstructionArgument(object value)
        {
            Value = value;
        }

        public override void SetRoot(FLParseResult root)
        {
            base.SetRoot(root);

            //HERE
            if (Type == InstructionArgumentType.Buffer || Type == InstructionArgumentType.Function)
            {
                ((ParsedObject) Value).SetRoot(root);
            }
        }
    }
}