using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace Byt3.Serialization
{
    public static class NetworkSerializer
    {
        private static readonly Dictionary<object, PacketSerializer> Serializers = new Dictionary<object, PacketSerializer>();
        private static readonly Dictionary<object, Type> _StringTypeCache = new Dictionary<object, Type>();
        private static InternalPacketSerializer _InternalSerializer;

        static NetworkSerializer()
        {
            _InternalSerializer = new InternalPacketSerializer();
        }

        public static void AddSerializer<T>(PacketSerializer packetSerializer)
        {
            AddSerializer(typeof(T), packetSerializer);
        }

        public static void SetInternalSerializer(InternalPacketSerializer internalSerializer)
        {
            _InternalSerializer = internalSerializer ?? throw new ArgumentNullException("internalSerializer", "Base serializer is not allowed to be null");
        }

        public static void WritePacket(Stream stream, object obj)
        {
            if (obj == null) throw new ArgumentNullException("obj", "Can not be null.");
            if (stream == null) throw new ArgumentNullException("stream", "Can not be null.");

            if (!CanSerialize(obj.GetType())) throw new SerializationException("Can not Serialize Type: " + obj.GetType());
            object key = GetTypeKey(obj.GetType());
            Serializers[key].Serialize(stream, obj);
        }

        public static object ReadPacket(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException("stream", "Can not be null.");
            InternalNetworkPacket packet = _InternalSerializer.DeserializePacket(stream);

            if (!Serializers.ContainsKey(packet.PacketType)) throw new SerializationException("Could not find a deserializer for type key: " + packet.PacketType);

            MemoryStream ms = new MemoryStream(packet.Payload);
            ms.Position = 0;
            object ret = Serializers[packet.PacketType].Deserialize(ms);
            ms.Close();
            return ret;
        }

        public static T ReadPacket<T>(Stream stream)
        {
            return (T)ReadPacket(stream);
        }

        public static void WritePacket<T>(Stream stream, T obj)
        {
            WritePacket(stream, (object)obj);
        }

        public static void AddSerializer(Type type, PacketSerializer packetSerializer)
        {
            if (CanSerialize(type)) return;
            Serializers.Add(GetTypeKey(type), packetSerializer);
        }

        public static bool CanSerialize(Type t)
        {
            object key = GetTypeKey(t);
            return Serializers.ContainsKey(key);
        }

        public static bool CanSerialize<T>()
        {
            return CanSerialize(typeof(T));
        }

        private static Type GetType(object key)
        {
            if (_StringTypeCache.ContainsKey(key)) return _StringTypeCache[key];
            throw new Exception("Could not Find the Type: " + key);
        }

        private static object GetTypeKey(Type t)
        {
            object key = _InternalSerializer.GetKey(t);
            if (!_StringTypeCache.ContainsKey(key))
                _StringTypeCache.Add(key, t);
            return key;
        }
    }
}