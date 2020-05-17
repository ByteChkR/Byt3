namespace Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects
{
    public interface IParsedObject
    {
        FLProgram Root { get; }
        void SetRoot(FLProgram root);
    }
}