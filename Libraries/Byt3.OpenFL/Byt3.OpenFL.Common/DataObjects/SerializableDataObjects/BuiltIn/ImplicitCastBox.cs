using System;

namespace Byt3.OpenFL.Common.DataObjects.SerializableDataObjects.BuiltIn
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
}