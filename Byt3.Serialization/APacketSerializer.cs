using System.IO;

namespace Byt3.Serialization
{
    public abstract class APacketSerializer<T> : PacketSerializer
    {
        public abstract T DeserializePacket(Stream s);
        public abstract void SerializePacket(Stream s, T obj);

        internal override object Deserialize(Stream s)
        {
            return Deserialize(s);
        }

        internal override void Serialize(Stream s, object o)
        {
            SerializePacket(s, (T)o);
        }
    }
}