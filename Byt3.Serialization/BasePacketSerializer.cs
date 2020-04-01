using System;
using System.IO;

namespace Byt3.Serialization
{
    public class InternalPacketSerializer : APacketSerializer<InternalNetworkPacket>
    {
        public override InternalNetworkPacket DeserializePacket(Stream s)
        {
            PrimitiveValueWrapper pvw = new PrimitiveValueWrapper(s);
            object packetType = pvw.ReadString();
            byte[] payload = pvw.ReadBytes();
            return new InternalNetworkPacket(packetType, payload);
        }

        public override void SerializePacket(Stream s, InternalNetworkPacket obj)
        {
            PrimitiveValueWrapper pvw = new PrimitiveValueWrapper(s);
            pvw.Write((string) obj.PacketType);
            pvw.Write(obj.Payload);
            pvw.CompleteWrite();
        }

        public virtual object GetKey(Type t)
        {
            return t.AssemblyQualifiedName;
        }
    }
}