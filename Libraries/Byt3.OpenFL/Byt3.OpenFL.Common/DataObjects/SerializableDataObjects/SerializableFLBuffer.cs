using Byt3.OpenFL.Common.Buffers;
using Byt3.OpenFL.Common.ElementModifiers;

namespace Byt3.OpenFL.Common.DataObjects.SerializableDataObjects
{
    public abstract class SerializableFLBuffer : SerializableNamedObject
    {
        protected SerializableFLBuffer(string name, FLBufferModifiers modifiers) : base(name)
        {
            Modifiers = modifiers;
        }

        public bool IsArray => Modifiers.IsArray;
        public FLBufferModifiers Modifiers { get; }

        public abstract FLBuffer GetBuffer();

        public override string ToString()
        {
            return $"{FLKeywords.DefineKey} {Modifiers} {Name}: ";
        }
    }
}