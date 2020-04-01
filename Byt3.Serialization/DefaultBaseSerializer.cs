using System;
using System.IO;

namespace Byt3.Serialization
{
    /// <summary>
    /// Default Implementation for the BaseSerializer.
    /// Uses Type.AssemblyQualifiedName as Unique Type Key
    /// </summary>
    public class DefaultBaseSerializer : ABaseSerializer
    {
        /// <summary>
        /// Deserializes a BasePacket from the stream.
        /// </summary>
        /// <param name="s">Input Stream</param>
        /// <returns>Deserialized BasePacket</returns>
        public override BasePacket DeserializePacket(Stream s)
        {
            PrimitiveValueWrapper pvw = new PrimitiveValueWrapper(s);
            object packetType = pvw.ReadString();
            byte[] payload = pvw.ReadBytes();
            return new BasePacket(packetType, payload);
        }

        /// <summary>
        /// Serializes a BasePacket to the Stream
        /// </summary>
        /// <param name="s">Target Stream</param>
        /// <param name="obj">BasePacket to Serialize</param>
        public override void SerializePacket(Stream s, BasePacket obj)
        {
            PrimitiveValueWrapper pvw = new PrimitiveValueWrapper(s);
            pvw.Write((string)obj.PacketType);
            pvw.Write(obj.Payload);
            pvw.CompleteWrite();
        }

        /// <summary>
        /// Returns the Unique Key for each Type
        /// using t.AssemblyQualifiedName
        /// </summary>
        /// <param name="t">Type to generate key for</param>
        /// <returns>The Unique Key per type</returns>
        public override object GetKey(Type t)
        {
            return t.AssemblyQualifiedName;
        }
    }
}