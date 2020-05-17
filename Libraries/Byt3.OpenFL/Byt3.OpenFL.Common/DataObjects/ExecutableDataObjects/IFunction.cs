namespace Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects
{
    public interface IFunction : IParsedObject
    {
        string Name { get; }
        void Process();
    }
}