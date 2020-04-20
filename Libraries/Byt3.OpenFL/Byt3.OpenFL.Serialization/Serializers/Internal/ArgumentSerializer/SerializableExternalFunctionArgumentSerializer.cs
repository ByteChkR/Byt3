using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects.BuiltIn;
using Byt3.Serialization;
using Byt3.Serialization.Serializers;

namespace Byt3.OpenFL.Serialization.Serializers.Internal.ArgumentSerializer
{
    public class SerializableExternalFunctionArgumentSerializer : ASerializer<SerializeExternalFunctionArgument>
    {
        public override SerializeExternalFunctionArgument DeserializePacket(PrimitiveValueWrapper s)
        {
            return new SerializeExternalFunctionArgument(s.ReadString());
        }

        public override void SerializePacket(PrimitiveValueWrapper s, SerializeExternalFunctionArgument obj)
        {
            s.Write(obj.Value);
        }
    }
}