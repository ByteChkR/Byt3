using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;
using Byt3.OpenFL.Common.Instructions.InstructionCreators;
using Byt3.OpenFL.Serialization.Exceptions;
using Byt3.Serialization;
using Byt3.Serialization.Serializers;

namespace Byt3.OpenFL.Serialization.Serializers.Internal
{
    public abstract class FLBaseSerializer : ASerializer
    {
        protected List<string> idMap { get; private set; }

        public virtual void SetIdMap(string[] map)
        {

            idMap = map.ToList();
        }

        protected string ResolveId(int id)
        {
            return idMap[id];
        }

        protected int ResolveName(string name)
        {
            return idMap.IndexOf(name);
        }
    }

    public class SerializableFLProgramSerializer : ASerializer<SerializableFLProgram>
    {
        private readonly Byt3Serializer bufferSerializer;
        private readonly FLInstructionSet instructionSet;

        public SerializableFLProgramSerializer(Dictionary<Type, FLBaseSerializer> serializers, FLInstructionSet iset)
        {
            instructionSet = iset;
            bufferSerializer = Byt3Serializer.GetDefaultSerializer();
            int i = 0;
            foreach (KeyValuePair<Type, FLBaseSerializer> keyValuePair in serializers)
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

            string[] idMap = ReadStringArray(s);
            SetIdMap(idMap);

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

            return new SerializableFLProgram("DeserializedScript", exts, funcs, defs);
        }

        private void WriteStringArray(PrimitiveValueWrapper s, string[] arr)
        {
            s.Write(arr.Length);
            for (int i = 0; i < arr.Length; i++)
            {
                s.Write(arr[i]);
            }
        }

        private string[] ReadStringArray(PrimitiveValueWrapper s)
        {
            string[] ret = new string[s.ReadInt()];
            for (int i = 0; i < ret.Length; i++)
            {
                ret[i] = s.ReadString();
            }

            return ret;
        }

        private void SetIdMap(string[] idMap)
        {
            for (int i = 0; i < bufferSerializer.ContainedSerializers; i++)
            {
                if (bufferSerializer.GetSerializerAt(i) is FLBaseSerializer flBufferSerializer)
                {
                    flBufferSerializer.SetIdMap(idMap.ToArray());
                }
            }
        }

        public override void SerializePacket(PrimitiveValueWrapper s, SerializableFLProgram obj)
        {
            int funcCount = obj.Functions.Count;
            int defCount = obj.DefinedBuffers.Count;
            int extCount = obj.ExternalFunctions.Count;

            s.Write(funcCount);
            s.Write(defCount);
            s.Write(extCount);

            string[] funcMap = obj.Functions.Select(x => x.Name).ToArray();
            string[] exMap = obj.ExternalFunctions.Select(x => x.Name).ToArray();
            string[] bufMap = obj.DefinedBuffers.Select(x => x.Name).ToArray();

            List<string> idMap = new List<string>();
            idMap.AddRange(funcMap);
            idMap.AddRange(exMap);
            idMap.AddRange(bufMap);
            idMap.AddRange(instructionSet.GetInstructionNames());

            WriteStringArray(s, idMap.ToArray());

            SetIdMap(idMap.ToArray());


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