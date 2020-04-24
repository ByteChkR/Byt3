using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects.BuiltIn;
using Byt3.Serialization;
using Byt3.Serialization.Serializers;

namespace Byt3.OpenFL.Serialization.Serializers.Internal.ArgumentSerializer
{
    public class SerializableFunctionArgumentSerializer : ASerializer<SerializeFunctionArgument>
    {
        public override SerializeFunctionArgument DeserializePacket(PrimitiveValueWrapper s)
        {
            return new SerializeFunctionArgument(s.ReadString());
        }

        public override void SerializePacket(PrimitiveValueWrapper s, SerializeFunctionArgument obj)
        {
            s.Write(obj.Value);
        }
    }
}