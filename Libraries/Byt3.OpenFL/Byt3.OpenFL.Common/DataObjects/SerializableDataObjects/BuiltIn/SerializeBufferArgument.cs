using System;
using Byt3.OpenFL.Common.Buffers;
using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;

namespace Byt3.OpenFL.Common.DataObjects.SerializableDataObjects.BuiltIn
{
    public class ImplicitCastBox<T> : ImplicitCastBox
    {
        private readonly Func<T> ValueProvider;

        public ImplicitCastBox(Func<T> valueProvider)
        {
            ValueProvider = valueProvider;
        }

        public override Type BoxedType => typeof(T);

        public override object GetValue()
        {
            return ValueProvider();
        }

        public static implicit operator T(ImplicitCastBox<T> box)
        {
            return box.ValueProvider();
        }
    }

    public class SerializeBufferArgument : SerializableFLInstructionArgument
    {
        public SerializeBufferArgument(string index)
        {
            Value = index;
        }

        public string Value { get; }

        public override InstructionArgumentCategory ArgumentCategory => InstructionArgumentCategory.Buffer;
        public override string Identifier => Value;


        public override ImplicitCastBox GetValue(FLProgram script)
        {
            return new ImplicitCastBox<FLBuffer>(() => script.DefinedBuffers[Value]);
        }

        public override string ToString()
        {
            return Value;
        }
    }
}