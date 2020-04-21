using System.IO;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;
using Byt3.Serialization;
using Byt3.Serialization.Serializers;

namespace Byt3.OpenFL.Serialization.Serializers.Internal
{
    public class SerializableExternalFLFunctionSerializer : ASerializer<SerializableExternalFLFunction>
    {
        public override SerializableExternalFLFunction DeserializePacket(PrimitiveValueWrapper s)
        {
            string name = s.ReadString();

            byte[] payload = s.ReadBytes();

            MemoryStream ms = new MemoryStream(payload);
            return new SerializableExternalFLFunction(name, FLSerializer.LoadProgram(ms));
        }

        public override void SerializePacket(PrimitiveValueWrapper s, SerializableExternalFLFunction obj)
        {
            s.Write(obj.Name);
            MemoryStream ms = new MemoryStream();

            FLSerializer.SaveProgram(ms, obj.ExternalProgram, new string[0]);

            s.Write(ms.GetBuffer(), (int) ms.Position);
        }
    }
}