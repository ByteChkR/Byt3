﻿using System;
using System.Collections.Generic;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;
using Byt3.Serialization;

namespace Byt3.OpenFL.Serialization.Serializers.Internal
{
    public class SerializableFLFunctionSerializer : FLBaseSerializer
    {
        private readonly SerializableInstructionSerializer instructionSerializer;
        private Dictionary<Type, FLBaseSerializer> argSerializers;

        public SerializableFLFunctionSerializer(Dictionary<Type, FLBaseSerializer> argumentSerializers)
        {
            argSerializers = argumentSerializers;
            instructionSerializer = new SerializableInstructionSerializer(argumentSerializers);
        }
        public override void SetIdMap(string[] idMap)
        {
            base.SetIdMap(idMap);

            instructionSerializer.SetIdMap(idMap);
            foreach (KeyValuePair<Type, FLBaseSerializer> flBaseSerializer in argSerializers)
            {
                flBaseSerializer.Value.SetIdMap(idMap);
            }
        }

        public override object Deserialize(PrimitiveValueWrapper s)
        {
            string name = ResolveId(s.ReadInt());
            int instC = s.ReadInt();
            List<SerializableFLInstruction> inst = new List<SerializableFLInstruction>();
            for (int i = 0; i < instC; i++)
            {
                inst.Add((SerializableFLInstruction)instructionSerializer.Deserialize(s));
            }

            return new SerializableFLFunction(name, inst);
        }

        public override void Serialize(PrimitiveValueWrapper s, object obj)
        {
            SerializableFLFunction input = (SerializableFLFunction)obj;
            s.Write(ResolveName(input.Name));
            s.Write(input.Instructions.Count);

            for (int i = 0; i < input.Instructions.Count; i++)
            {
                instructionSerializer.Serialize(s, input.Instructions[i]);
            }
        }
    }
}