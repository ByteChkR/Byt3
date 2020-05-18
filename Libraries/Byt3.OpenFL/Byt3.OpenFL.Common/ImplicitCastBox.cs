using System;

namespace Byt3.OpenFL.Common
{
    public abstract class ImplicitCastBox
    {
        public abstract Type BoxedType { get; }
        public abstract object GetValue();

        public override string ToString()
        {
            return GetValue().ToString();
        }
    }


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
}