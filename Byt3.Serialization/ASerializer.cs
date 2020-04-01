using System;
using System.IO;

namespace Byt3.Serialization
{


    /// <summary>
    /// Base Class for all Serializers
    /// </summary>
    public abstract class ASerializer
    {
        /// <summary>
        /// Returns the Deserialized Object
        /// </summary>
        /// <param name="s">Stream to read from</param>
        /// <returns>The Deserialized Object</returns>
        internal virtual object Deserialize(Stream s)
        {
            throw new NotImplementedException("Packet Serializer not Implemented");
        }

        /// <summary>
        /// Serializes an object into a stream
        /// </summary>
        /// <param name="s">Target Stream</param>
        /// <param name="o">Object to Serialize</param>
        internal virtual void Serialize(Stream s, object o)
        {
            throw new NotImplementedException("Packet Serializer not Implemented");
        }
    }
}