namespace Byt3.OpenFL.Common.DataObjects.SerializableDataObjects
{
    public class SerializableExternalFLFunction : SerializableNamedObject
    {
        public SerializableFLProgram ExternalProgram { get; }

        public SerializableExternalFLFunction(string name, SerializableFLProgram externalProgram) : base(name)
        {
            ExternalProgram = externalProgram;
        }
    }
}