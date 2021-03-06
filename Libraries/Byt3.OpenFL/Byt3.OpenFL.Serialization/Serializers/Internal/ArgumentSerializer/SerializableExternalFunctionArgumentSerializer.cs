﻿using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects.BuiltIn;
using Byt3.Serialization;

namespace Byt3.OpenFL.Serialization.Serializers.Internal.ArgumentSerializer
{
    public class SerializableExternalFunctionArgumentSerializer : FLBaseSerializer
    {
        public override object Deserialize(PrimitiveValueWrapper s)
        {
            return new SerializeExternalFunctionArgument(ResolveId(s.ReadInt()));
        }

        public override void Serialize(PrimitiveValueWrapper s, object obj)
        {
            s.Write(ResolveName((obj as SerializeExternalFunctionArgument).Value));
        }
    }
}