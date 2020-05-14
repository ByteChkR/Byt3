using Byt3.OpenFL.Common.Buffers;

namespace Byt3.OpenFL.Common.DataObjects.SerializableDataObjects
{


    public abstract class SerializableFLBuffer : SerializableNamedObject
    {
        public bool IsArray { get; private set; }
        protected SerializableFLBuffer(string name, bool isArray) : base(name)
        {
            IsArray = isArray;
        }

        public abstract FLBuffer GetBuffer();
    }
}