using Byt3.OpenFL.Common.Buffers;

namespace Byt3.OpenFL.Common.DataObjects.SerializableDataObjects
{
    public abstract class SerializableFLBuffer : SerializableNamedObject
    {
        protected SerializableFLBuffer(string name) : base(name)
        {
        }

        public abstract FLBuffer GetBuffer();
    }
}