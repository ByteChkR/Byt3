using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using Byt3.OpenCL.Memory;
using Byt3.OpenFL.Common.Arguments;
using Byt3.Serialization;

namespace Byt3.OpenFL.Serialization.Serializers.Internal.ArgumentSerializer
{
    public class SerializableArrayLengthSerializer : FLBaseSerializer
    {
        public override object Deserialize(PrimitiveValueWrapper s)
        {
            return new SerializeArrayLengthArgument(s.ReadString());
        }

        public override void Serialize(PrimitiveValueWrapper s, object obj)
        {
            s.Write((obj as SerializeArrayLengthArgument).Value);
        }
    }

    //SerializeArrayElementArgumentEnclosedIndex

    public class SerializableArrayElementArgumentEnclosedIndexSerializer : FLBaseSerializer
    {
        private readonly Byt3Serializer ElementArgumentSerializers = Byt3Serializer.GetDefaultSerializer();

        public SerializableArrayElementArgumentEnclosedIndexSerializer()
        {
            ElementArgumentSerializers.AddSerializer(typeof(SerializeArrayElementArgumentValueIndex), new SerializableArrayElementArgumentValueIndexSerializer());
            ElementArgumentSerializers.AddSerializer(typeof(SerializeArrayElementArgumentVariableIndex), new SerializableArrayElementArgumentVariableIndexSerializer());
            ElementArgumentSerializers.AddSerializer(typeof(SerializeNameArgument), new SerializableNameArgumentSerializer());
            ElementArgumentSerializers.AddSerializer(typeof(SerializeArrayElementArgumentEnclosedIndex), this);
        }


        public override void SetIdMap(string[] map)
        {
            if (idMap != null && idMap.Count == map.Length && idMap.All(map.Contains)) return;

            base.SetIdMap(map);

            for (int i = 0; i < ElementArgumentSerializers.ContainedSerializers; i++)
            {
                if (ElementArgumentSerializers.GetSerializerAt(i) is FLBaseSerializer s)
                {
                    s.SetIdMap(map);
                }
            }
        }

        public override object Deserialize(PrimitiveValueWrapper s)
        {
            byte[] bytes = s.ReadBytes();
            Stream str = new MemoryStream(bytes);
            if (!ElementArgumentSerializers.TryReadPacket(str, out object obj)) { throw new Exception("FUCK"); }

            return new SerializeArrayElementArgumentEnclosedIndex(ResolveId(s.ReadInt()), (SerializeArrayElementArgument)obj);
        }

        public override void Serialize(PrimitiveValueWrapper s, object obj)
        {
            Stream str = new MemoryStream();
            if (!ElementArgumentSerializers.TryWritePacket(str, (obj as SerializeArrayElementArgumentEnclosedIndex).Index)) { throw new Exception("FUCK"); }


            byte[] arr = new byte[str.Position];

            str.Position = 0;
            str.Read(arr, 0, arr.Length);
            s.Write(arr);
            s.Write(ResolveName((obj as SerializeArrayElementArgumentEnclosedIndex).Value));
        }
    }


    public class SerializableArrayElementArgumentValueIndexSerializer : FLBaseSerializer
    {
        public override object Deserialize(PrimitiveValueWrapper s)
        {
            return new SerializeArrayElementArgumentValueIndex(ResolveId(s.ReadInt()), s.ReadInt());
        }

        public override void Serialize(PrimitiveValueWrapper s, object obj)
        {
            s.Write(ResolveName((obj as SerializeArrayElementArgumentValueIndex).Value));
            s.Write((obj as SerializeArrayElementArgumentValueIndex).Index);
        }
    }

    public class SerializableArrayElementArgumentVariableIndexSerializer : FLBaseSerializer
    {
        public override object Deserialize(PrimitiveValueWrapper s)
        {
            return new SerializeArrayElementArgumentVariableIndex(s.ReadString(), s.ReadString());
        }

        public override void Serialize(PrimitiveValueWrapper s, object obj)
        {
            s.Write((obj as SerializeArrayElementArgumentVariableIndex).Value);
            s.Write((obj as SerializeArrayElementArgumentVariableIndex).Index);
        }
    }

    public class SerializableNameArgumentSerializer : FLBaseSerializer
    {
        public override object Deserialize(PrimitiveValueWrapper s)
        {
            return new SerializeNameArgument(s.ReadString());
        }

        public override void Serialize(PrimitiveValueWrapper s, object obj)
        {
            s.Write((obj as SerializeNameArgument).Value);
        }
    }
}