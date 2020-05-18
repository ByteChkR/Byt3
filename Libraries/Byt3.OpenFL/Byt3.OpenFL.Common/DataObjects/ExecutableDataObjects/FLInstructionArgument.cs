using System;
using Byt3.OpenFL.Common.Buffers;

namespace Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects
{
    public class FLInstructionArgument : FLParsedObject
    {
        private static readonly Type[] PossibleValueTypes =
            {typeof(decimal), typeof(FLBuffer), typeof(IFunction), typeof(string)};

        private FLFunction Parent;

        public FLInstructionArgument(ImplicitCastBox value)
        {
            Value = value;
        }

        public FLInstructionArgumentType Type
        {
            get
            {
                Type t = Value.BoxedType;


                for (int i = 0; i < PossibleValueTypes.Length; i++)
                {
                    if (PossibleValueTypes[i].IsAssignableFrom(t))
                    {
                        return (FLInstructionArgumentType) i + 1;
                    }
                }

                return FLInstructionArgumentType.Undefined;
            }
        }

        private ImplicitCastBox Value { get; }

        public object GetValue()
        {
            return Value.GetValue();
        }

        public void SetParent(FLFunction func)
        {
            Parent = func;
        }

        public override void SetRoot(FLProgram root)
        {
            base.SetRoot(root);

            //HERE
            if (Type == FLInstructionArgumentType.Buffer || Type == FLInstructionArgumentType.Function)
            {
                ((FLParsedObject) Value.GetValue()).SetRoot(root);
            }
        }

        public override string ToString()
        {
            if (Type == FLInstructionArgumentType.Number)
            {
                return Value.ToString();
            }

            if (Type == FLInstructionArgumentType.Function)
            {
                object obj = GetValue();
                return (obj as IFunction).Name;
            }

            if (Type == FLInstructionArgumentType.Buffer)
            {
                object obj = GetValue();
                return (obj as FLBuffer).DefinedBufferName;
            }

            if (Type == FLInstructionArgumentType.Name && Parent != null && Parent.Variables.IsDefined(Value.ToString()))
            {
                return Parent.Variables.GetVariable(Value.ToString()).ToString();
            }

            if (Type == FLInstructionArgumentType.Name && (Parent == null || !Parent.Variables.IsDefined(Value.ToString())))
            {
                return Value.ToString();
            }

            return "[ERR]";
        }
    }
}