using System.IO;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;
using Byt3.Serialization;
using Byt3.Serialization.Serializers;

namespace Byt3.OpenFL.Serialization.Serializers.Internal
{
    public class SerializableExternalFLFunctionSerializer : FLSerializer
    {
        public override object Deserialize(PrimitiveValueWrapper s)
        {
            string name = s.ReadString();

            byte[] payload = s.ReadBytes();

            MemoryStream ms = new MemoryStream(payload);
            return new SerializableExternalFLFunction(name, Serialization.FLSerializer.LoadProgram(ms));
        }

        public override void Serialize(PrimitiveValueWrapper s, object obj)
        {
            SerializableExternalFLFunction input = (SerializableExternalFLFunction) obj;
            s.Write(input.Name);
            MemoryStream ms = new MemoryStream();

            Serialization.FLSerializer.SaveProgram(ms, input.ExternalProgram, new string[0]);

            s.Write(ms.GetBuffer(), (int) ms.Position);
        }
    }
}