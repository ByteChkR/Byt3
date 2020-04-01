using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace Byt3.Serialization
{
    /// <summary>
    /// Serializer API Class
    /// </summary>
    public static class Byt3Serializer
    {
        /// <summary>
        /// Serializer Dictionary of serializers with custom key
        /// </summary>
        private static readonly Dictionary<Type, ASerializer> Serializers = new Dictionary<Type, ASerializer>();

        /// <summary>
        /// Cache that gets used to store the map from object => Type
        /// that gets used during Deserialization
        /// </summary>
        private static readonly Dictionary<Type, object> TypeKeyCache = new Dictionary<Type, object>();
        private static readonly Dictionary<object, Type> KeyTypeCache = new Dictionary<object, Type>();

        /// <summary>
        /// The Base Initializer that is beeing used.
        /// </summary>
        private static ABaseSerializer BaseSerializer = new DefaultBaseSerializer();

        /// <summary>
        /// Adds a Serializer that can de/serialize objects of type T to the Serializers
        /// </summary>
        /// <typeparam name="T">Type of object that is de/serializable</typeparam>
        /// <param name="packetSerializer">The Serializer that can de/serialize objects of type T</param>
        public static void AddSerializer<T>(ASerializer packetSerializer)
        {
            AddSerializer(typeof(T), packetSerializer);
        }


        /// <summary>
        /// Sets the Base Serializer to a user defined implementation
        /// </summary>
        /// <param name="baseSerializer">The Base Initializer to use</param>
        public static void SetBaseSerializer(ABaseSerializer baseSerializer)
        {
            BaseSerializer = baseSerializer ?? throw new ArgumentNullException("baseSerializer", "Base serializer is not allowed to be null");
        }

        /// <summary>
        /// Writes the object to the Specified Stream
        /// </summary>
        /// <param name="stream">Target Stream</param>
        /// <param name="obj">Object to Serialize</param>
        public static void WritePacket(Stream stream, object obj)
        {
            if (obj == null) throw new ArgumentNullException("obj", "Can not be null.");
            if (stream == null) throw new ArgumentNullException("stream", "Can not be null.");

            if (!CanSerialize(obj.GetType())) throw new SerializationException("Can not Serialize Type: " + obj.GetType());
            MemoryStream ms = new MemoryStream();
            Serializers[obj.GetType()].Serialize(ms, obj);
            ms.Position = 0;
            byte[] buf = new byte[ms.Length];
            ms.Read(buf, 0, buf.Length);
            BaseSerializer.Serialize(stream, new BasePacket(GetKeyByType(obj.GetType()), buf));
            ms.Close();
        }

        /// <summary>
        /// Reads an Object from the Specified Stream
        /// </summary>
        /// <param name="stream">Input Stream</param>
        /// <returns>The Deserialized Object</returns>
        public static object ReadPacket(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException("stream", "Can not be null.");
            BasePacket packet = BaseSerializer.DeserializePacket(stream);


            if (!CanSerializeByKey(packet.PacketType))
                throw new SerializationException("Could not find a deserializer for type key: " + packet.PacketType);

            MemoryStream ms = new MemoryStream(packet.Payload);
            ms.Position = 0;
            Type packetType = GetTypeByKey(packet.PacketType);
            object ret = Serializers[packetType].Deserialize(ms);
            ms.Close();
            return ret;
        }

        /// <summary>
        /// Generic Implementation of ReadPacket
        /// </summary>
        /// <typeparam name="T">Type of Object to Deserialize</typeparam>
        /// <param name="stream">Input Stream</param>
        /// <returns>The Packet that was Deserialized</returns>
        public static T ReadPacket<T>(Stream stream)
        {
            return (T)ReadPacket(stream);
        }

        /// <summary>
        /// Generic Implementation of WritePacket
        /// </summary>
        /// <typeparam name="T">Type of Object to Serialize</typeparam>
        /// <param name="stream">Target Stream</param>
        /// <param name="obj">Object to Serialize</param>
        public static void WritePacket<T>(Stream stream, T obj)
        {
            WritePacket(stream, (object)obj);
        }
        /// <summary>
        /// Adds a Serializer that can de/serialize objects of the specialized type to the Serializers
        /// </summary>
        /// <param name="type">Type that can be de/serialized</param>
        /// <param name="packetSerializer">The Serializer that can de/serialize objects of type</param>
        public static void AddSerializer(Type type, ASerializer packetSerializer)
        {
            if (CanSerialize(type)) return;
            object key = BaseSerializer.GetKey(type);
            KeyTypeCache.Add(key, type);
            TypeKeyCache.Add(type, key);
            Serializers.Add(type, packetSerializer);
        }

        /// <summary>
        /// Returns True if the Specified key is in the KeyTypeCache(e.g. has a serializer added)
        /// </summary>
        /// <param name="key">Key of the Type</param>
        /// <returns>true if the Serializer for this type can be found</returns>
        public static bool CanSerializeByKey(object key)
        {
            return KeyTypeCache.ContainsKey(key);
        }

        /// <summary>
        /// Returns True if the Specified Type can be Serialized(e.g. has a serializer added)
        /// </summary>
        /// <param name="t">The Type to Check for</param>
        /// <returns>true if the Serializer for this type can be found</returns>
        public static bool CanSerialize(Type t)
        {
            return TypeKeyCache.ContainsKey(t);
        }

        /// <summary>
        /// Returns True if the Specified Type can be Serialized(e.g. has a serializer added)
        /// </summary>
        /// <typeparam name="T">The Type to Check for</typeparam>
        /// <returns>true if the Serializer for this type can be found</returns>
        public static bool CanSerialize<T>()
        {
            return CanSerialize(typeof(T));
        }

        /// <summary>
        /// Returns the Type based on the Key.
        /// </summary>
        /// <param name="key">Key of the Type</param>
        /// <returns>Type mapped to this key</returns>
        /// <exception cref="Exception">Gets thrown when The KeyTypeCache does not contain the key.</exception>
        private static object GetKeyByType(Type type)
        {
            if (TypeKeyCache.ContainsKey(type)) return TypeKeyCache[type];
            throw new Exception("Could not Find the Key for Type: " + type);
        }

        /// <summary>
        /// Returns the Type based on the Key.
        /// </summary>
        /// <param name="key">Key of the Type</param>
        /// <returns>Type mapped to this key</returns>
        /// <exception cref="Exception">Gets thrown when The KeyTypeCache does not contain the key.</exception>
        private static Type GetTypeByKey(object key)
        {
            if (KeyTypeCache.ContainsKey(key)) return KeyTypeCache[key];
            throw new Exception("Could not Find the Type with Key: " + key);
        }

    }
}