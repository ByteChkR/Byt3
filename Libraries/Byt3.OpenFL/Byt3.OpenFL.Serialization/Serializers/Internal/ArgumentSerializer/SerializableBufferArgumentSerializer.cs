using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects.BuiltIn;
using Byt3.Serialization;
using Byt3.Serialization.Serializers;

namespace Byt3.OpenFL.Serialization.Serializers.Internal.ArgumentSerializer
{
    public class SerializableBufferArgumentSerializer : ASerializer<SerializeBufferArgument>
    {
        public override SerializeBufferArgument DeserializePacket(PrimitiveValueWrapper s)
        {
            return new SerializeBufferArgument(s.ReadString());
        }

        public override void SerializePacket(PrimitiveValueWrapper s, SerializeBufferArgument obj)
        {
            s.Write(obj.Value);
        }
    }
}