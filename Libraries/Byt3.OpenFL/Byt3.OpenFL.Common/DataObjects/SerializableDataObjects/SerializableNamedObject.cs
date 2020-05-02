namespace Byt3.OpenFL.Common.DataObjects.SerializableDataObjects
{
    public abstract class SerializableNamedObject
    {
        public string Name { get; }

        protected SerializableNamedObject(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return "Not Implemented for type: " + GetType().Name;
        }
    }
}