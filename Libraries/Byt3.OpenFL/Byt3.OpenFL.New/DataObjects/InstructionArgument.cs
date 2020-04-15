using Byt3.OpenFL.New.Parsing;

namespace Byt3.OpenFL.New.DataObjects
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
                }

                return ret;
            }
        }

        public object Value;

        public InstructionArgument(object value)
        {
            Value = value;
        }

        public override void SetRoot(ParsedSource root)
        {
            base.SetRoot(root);

            //HERE
            if (Type == InstructionArgumentType.Buffer || Type == InstructionArgumentType.Function)
            {
                ((ParsedObject)Value).SetRoot(root);
            }
        }
    }
}