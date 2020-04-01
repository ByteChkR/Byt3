using System.IO;

namespace Byt3.Serialization
{
    /// <summary>
    /// Generic Implementation for the ASerializer
    /// Overrides the not generic Functions and replaces them with generic ones.
    /// </summary>
    /// <typeparam name="T">Type that this serializer is able to deserialize</typeparam>
    public abstract class ATSerializer<T> : ASerializer
    {
        /// <summary>
        /// Deserializes a Packet from the stream.
        /// </summary>
        /// <param name="s">Input Stream</param>
        /// <returns>Deserialized Packet</returns>
        public abstract T DeserializePacket(Stream s);
        /// <summary>
        /// Serializes a Packet to the Stream
        /// </summary>
        /// <param name="s">Target Stream</param>
        /// <param name="obj">Object to Serialize</param>
        public abstract void SerializePacket(Stream s, T obj);

        /// <summary>
        /// Non Generic Override for the ASerializer.
        /// </summary>
        /// <param name="s">Input Stream</param>
        /// <returns>Non Generic Version of the Deserialized Object</returns>
        internal override object Deserialize(Stream s)
        {
            return Deserialize(s);
        }

        /// <summary>
        /// Non Generic Override for the ASerializer.
        /// </summary>
        /// <param name="s">Input Stream</param>
        /// <param name="o">Non Generic version of the Object to Serialize</param>
        /// <returns>Non Generic Version of the Serialized Object</returns>
        internal override void Serialize(Stream s, object o)
        {
            SerializePacket(s, (T)o);
        }
    }
}