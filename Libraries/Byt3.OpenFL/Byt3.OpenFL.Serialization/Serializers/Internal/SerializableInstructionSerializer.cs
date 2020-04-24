﻿using System;
using System.Collections.Generic;
using System.IO;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;
using Byt3.OpenFL.Serialization.Exceptions;
using Byt3.Serialization;
using Byt3.Serialization.Serializers;

namespace Byt3.OpenFL.Serialization.Serializers.Internal
{
    public class SerializableInstructionSerializer : ASerializer<SerializableFLInstruction>
    {
        private readonly Byt3Serializer argSerializer;

        public SerializableInstructionSerializer(Dictionary<Type, ASerializer> serializers)
        {
            argSerializer = Byt3Serializer.GetDefaultSerializer();

            foreach (KeyValuePair<Type, ASerializer> keyValuePair in serializers)
            {
                argSerializer.AddSerializer(keyValuePair.Key, keyValuePair.Value);
            }
        }

        public override SerializableFLInstruction DeserializePacket(PrimitiveValueWrapper s)
        {
            string key = s.ReadString();

            int argCount = s.ReadInt();

            List<SerializableFLInstructionArgument> args = new List<SerializableFLInstructionArgument>();

            for (int i = 0; i < argCount; i++)
            {
                MemoryStream temp = new MemoryStream(s.ReadBytes());
                if (!argSerializer.TryReadPacket(temp, out SerializableFLInstructionArgument arg))
                {
                    throw new FLDeserializationException(
                        $"Can not Deserialize Serializable Argument of Instruction: {key} ID: {i}");
                }

                args.Add(arg);
            }

            return new SerializableFLInstruction(key, args);
        }

        public override void SerializePacket(PrimitiveValueWrapper s, SerializableFLInstruction obj)
        {
            s.Write(obj.InstructionKey);
            s.Write(obj.Arguments.Count);


            for (int i = 0; i < obj.Arguments.Count; i++)
            {
                MemoryStream temp = new MemoryStream();
                if (!argSerializer.TryWritePacket(temp, obj.Arguments[i]))
                {
                    throw new FLSerializationException("Can not serialize Serializable Argument: " +
                                                       obj.Arguments[i].GetType());
                }

                s.Write(temp.GetBuffer(), (int) temp.Position);
            }
        }
    }
}