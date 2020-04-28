using System;
using System.Collections.Generic;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;
using Byt3.OpenFL.Common.Instructions.InstructionCreators;
using Byt3.Serialization;
using Byt3.Serialization.Serializers;

namespace Byt3.OpenFL.Serialization.Serializers.Internal
{
    public class SerializableFLFunctionSerializer : FLSerializer
    {
        private readonly SerializableInstructionSerializer instructionSerializer;

        public SerializableFLFunctionSerializer(Dictionary<Type, ASerializer> argumentSerializers)
        {
            instructionSerializer = new SerializableInstructionSerializer(argumentSerializers);
        }

        public override object Deserialize(PrimitiveValueWrapper s)
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

        public override void Serialize(PrimitiveValueWrapper s, object obj)
        {
            SerializableFLFunction input = (SerializableFLFunction) obj;
            s.Write(input.Name);
            s.Write(input.Instructions.Count);

            for (int i = 0; i < input.Instructions.Count; i++)
            {
                instructionSerializer.SerializePacket(s, input.Instructions[i]);
            }
        }
    }
}