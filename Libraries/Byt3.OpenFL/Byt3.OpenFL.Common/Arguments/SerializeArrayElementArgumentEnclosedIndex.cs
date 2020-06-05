using System;
using Byt3.OpenFL.Common.Buffers;
using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;

namespace Byt3.OpenFL.Common.Arguments
{
    public class SerializeArrayElementArgumentEnclosedIndex : SerializeArrayElementArgument
    {
        private readonly SerializeArrayElementArgument Index;

        public SerializeArrayElementArgumentEnclosedIndex(string value, SerializeArrayElementArgument index)
        {
            Index = index;
            Value = value;
        }

        public string Value { get; }

        public override string Identifier => Value + $"[{Index}]";

        public override ImplicitCastBox GetValue(FLProgram script, FLFunction func)
        {
            if (script.DefinedBuffers[Value] is IEditableBuffer buffer)
            {
                return new ImplicitCastBox<decimal>(() =>
                {
                    int idx = (int) (decimal) Index.GetValue(script, func).GetValue();
                    return buffer.GetData()[idx];
                }); //really slow
            }

            throw new InvalidOperationException($"{script.DefinedBuffers[Value]} does not implement IEditableBuffer");
        }

        public override string ToString()
        {
            return Identifier;
        }
    }
}