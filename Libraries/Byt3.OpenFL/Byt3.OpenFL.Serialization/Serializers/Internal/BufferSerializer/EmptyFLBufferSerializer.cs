﻿using Byt3.OpenFL.Common.Buffers.BufferCreators.BuiltIn.Empty;
using Byt3.Serialization;

namespace Byt3.OpenFL.Serialization.Serializers.Internal.BufferSerializer
{
    public class EmptyFLBufferSerializer :FLBaseSerializer
    {
        public override object Deserialize(PrimitiveValueWrapper s)
        {
            int name = s.ReadInt();
            bool isArray = s.ReadBool();
            if (isArray)
            {
                return new SerializableEmptyFLBuffer(ResolveId(name), s.ReadInt());
            }
            return new SerializableEmptyFLBuffer(ResolveId(name));
        }

        public override void Serialize(PrimitiveValueWrapper s, object obj)
        {

            s.Write(ResolveName(((SerializableEmptyFLBuffer)obj).Name));
            s.Write(((SerializableEmptyFLBuffer)obj).IsArray);
            if (((SerializableEmptyFLBuffer) obj).IsArray)
            {
                s.Write(((SerializableEmptyFLBuffer)obj).Size);

            }
        }
    }
}