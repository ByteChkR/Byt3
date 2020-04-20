using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects.BuiltIn;
using Byt3.Serialization;
using Byt3.Serialization.Serializers;

namespace Byt3.OpenFL.Serialization.Serializers.Internal.ArgumentSerializer
{
    public class SerializableDecimalArgumentSerializer : ASerializer<SerializeDecimalArgument>
    {
        public override SerializeDecimalArgument DeserializePacket(PrimitiveValueWrapper s)
        {
            return new SerializeDecimalArgument((decimal)s.ReadFloat());
        }

        public override void SerializePacket(PrimitiveValueWrapper s, SerializeDecimalArgument obj)
        {
            s.Write((float)obj.Value);
        }
    }
}