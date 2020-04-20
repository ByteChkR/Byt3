using System;
using System.Collections.Generic;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;
using Byt3.Serialization;
using Byt3.Serialization.Serializers;

namespace Byt3.OpenFL.Serialization.Serializers.Internal
{
    public class SerializableFLFunctionSerializer : ASerializer<SerializableFLFunction>
    {
        private readonly SerializableInstructionSerializer instructionSerializer;

        public SerializableFLFunctionSerializer(Dictionary<Type, ASerializer> argumentSerializers)
        {
            instructionSerializer = new SerializableInstructionSerializer(argumentSerializers);
        }
        public override SerializableFLFunction DeserializePacket(PrimitiveValueWrapper s)
        {
            string name = s.ReadString();
            int instC = s.ReadInt();
            List<SerializableFLInstruction> inst = new List<SerializableFLInstruction>();

            for (int i = 0; i < instC; i++)
            {
                inst.Add(instructionSerializer.DeserializePacket(s));
            }

            return new SerializableFLFunction(name, inst);
        }

        public override void SerializePacket(PrimitiveValueWrapper s, SerializableFLFunction obj)
        {
            s.Write(obj.Name);
            s.Write(obj.Instructions.Count);

            for (int i = 0; i < obj.Instructions.Count; i++)
            {
                instructionSerializer.SerializePacket(s, obj.Instructions[i]);
            }

        }
    }
}