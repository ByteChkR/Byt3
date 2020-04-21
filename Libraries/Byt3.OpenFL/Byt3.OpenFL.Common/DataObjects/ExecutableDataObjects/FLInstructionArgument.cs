using System;
using Byt3.OpenFL.Common.Buffers;

namespace Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects
{
    public class FLInstructionArgument : FLParsedObject
    {
        private static readonly Type[] PossibleValueTypes =
            new[] {typeof(decimal), typeof(FLBuffer), typeof(FLFunction)};

        public FLInstructionArgumentType Type
        {
            get
            {
                Type t = Value.GetType();

                for (int i = 0; i < PossibleValueTypes.Length; i++)
                {
                    if (PossibleValueTypes[i].IsInstanceOfType(Value))
                    {
                        return (FLInstructionArgumentType) i + 1;
                    }
                }

                return FLInstructionArgumentType.Undefined;
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