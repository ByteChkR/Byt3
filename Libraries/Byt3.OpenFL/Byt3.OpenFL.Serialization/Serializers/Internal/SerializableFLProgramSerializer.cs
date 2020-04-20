using System;
using System.Collections.Generic;
using System.IO;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;
using Byt3.OpenFL.Serialization.Exceptions;
using Byt3.Serialization;
using Byt3.Serialization.Serializers;

namespace Byt3.OpenFL.Serialization.Serializers.Internal
{
    public class SerializableFLProgramSerializer : ASerializer<SerializableFLProgram>
    {
        private readonly Byt3Serializer bufferSerializer;

        public SerializableFLProgramSerializer(Dictionary<Type, ASerializer> serializers)
        {
            bufferSerializer = Byt3Serializer.GetDefaultSerializer();
            foreach (KeyValuePair<Type, ASerializer> keyValuePair in serializers)
            {
                bufferSerializer.AddSerializer(keyValuePair.Key, keyValuePair.Value);
            }
        }

        public override SerializableFLProgram DeserializePacket(PrimitiveValueWrapper s)
        {

            int funcCount = s.ReadInt();
            int defCount = s.ReadInt();
            int extCount = s.ReadInt();

            List<SerializableFLBuffer> defs = new List<SerializableFLBuffer>();
            List<SerializableFLFunction> funcs = new List<SerializableFLFunction>();
            List<SerializableExternalFLFunction> exts = new List<SerializableExternalFLFunction>();

            for (int i = 0; i < defCount; i++)
            {
                MemoryStream temp = new MemoryStream(s.ReadBytes());
                if (!bufferSerializer.TryReadPacket(temp, out SerializableFLBuffer def))
                {
                    throw new FLDeserializationException(
                        $"Can not Deserialize Serializable Defined buffer: ID: {i}");
                }
                defs.Add(def);
            }

            for (int i = 0; i < funcCount; i++)
            {
                MemoryStream temp = new MemoryStream(s.ReadBytes());
                if (!bufferSerializer.TryReadPacket(temp, out SerializableFLFunction def))
                {
                    throw new FLDeserializationException(
                        $"Can not Deserialize Serializable Defined buffer: ID: {i}");
                }
                funcs.Add(def);
            }

            for (int i = 0; i < extCount; i++)
            {
                MemoryStream temp = new MemoryStream(s.ReadBytes());
                if (!bufferSerializer.TryReadPacket(temp, out SerializableExternalFLFunction def))
                {
                    throw new FLDeserializationException(
                        $"Can not Deserialize Serializable Defined buffer: ID: {i}");
                }
                exts.Add(def);
            }
            return new SerializableFLProgram(exts, funcs, defs);
        }

        public override void SerializePacket(PrimitiveValueWrapper s, SerializableFLProgram obj)
        {
            int funcCount = obj.Functions.Count;
            int defCount = obj.DefinedBuffers.Count;
            int extCount = obj.ExternalFunctions.Count;

            s.Write(funcCount);
            s.Write(defCount);
            s.Write(extCount);

            for (int i = 0; i < obj.DefinedBuffers.Count; i++)
            {
                MemoryStream temp = new MemoryStream();
                if (!bufferSerializer.TryWritePacket(temp, obj.DefinedBuffers[i]))
                {
                    throw new FLDeserializationException(
                        $"Can not Deserialize Serializable Defined buffer: {obj.DefinedBuffers[i].Name} ID: {i}");
                }

                s.Write(temp.GetBuffer(), (int)temp.Position);
            }

            for (int i = 0; i < obj.Functions.Count; i++)
            {
                MemoryStream temp = new MemoryStream();
                if (!bufferSerializer.TryWritePacket(temp, obj.Functions[i]))
                {
                    throw new FLDeserializationException(
                        $"Can not Deserialize Serializable Function: {obj.Functions[i].Name} ID: {i}");
                }

                s.Write(temp.GetBuffer(), (int)temp.Position);
            }

            for (int i = 0; i < obj.ExternalFunctions.Count; i++)
            {
                MemoryStream temp = new MemoryStream();
                if (!bufferSerializer.TryWritePacket(temp, obj.ExternalFunctions[i]))
                {
                    throw new FLDeserializationException(
                        $"Can not Deserialize Serializable External Function: {obj.ExternalFunctions[i].Name} ID: {i}");
                }

                s.Write(temp.GetBuffer(), (int)temp.Position);
            }

        }
    }
}