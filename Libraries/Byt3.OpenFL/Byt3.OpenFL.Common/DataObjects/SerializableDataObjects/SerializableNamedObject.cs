namespace Byt3.OpenFL.Common.DataObjects.SerializableDataObjects
{
    public abstract class SerializableNamedObject
    {
        protected SerializableNamedObject(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public override string ToString()
        {
            return "Not Implemented for type: " + GetType().Name;
        }
    }
}