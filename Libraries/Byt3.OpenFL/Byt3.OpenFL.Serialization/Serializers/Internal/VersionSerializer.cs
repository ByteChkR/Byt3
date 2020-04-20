using System;
using Byt3.Serialization;
using Byt3.Serialization.Serializers;

namespace Byt3.OpenFL.Serialization.Serializers.Internal
{
    public class VersionSerializer : ASerializer<Version>
    {
        public override Version DeserializePacket(PrimitiveValueWrapper s)
        {
            return Version.Parse(s.ReadString());
        }

        public override void SerializePacket(PrimitiveValueWrapper s, Version obj)
        {
            s.Write(obj.ToString());
        }
    }
}