namespace Byt3.Serialization
{
    public class InternalNetworkPacket
    {

        public const int PacketMaxSize = ushort.MaxValue;
        public object PacketType;
        public byte[] Payload;

        public InternalNetworkPacket(object packetType, byte[] payload)
        {
            PacketType = packetType;
            Payload = payload;
        }
    }
}