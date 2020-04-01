using System;
using System.IO;

namespace Byt3.Serialization
{

    public abstract class PacketSerializer
    {
        internal virtual object Deserialize(Stream s)
        {
            throw new NotImplementedException("Packet Serializer not Implemented");
        }

        internal virtual void Serialize(Stream s, object o)
        {
            throw new NotImplementedException("Packet Serializer not Implemented");
        }
    }
}