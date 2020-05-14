namespace Byt3.OpenFL.Common.DataObjects.SerializableDataObjects
{
    public enum InstructionArgumentCategory
    {
        Invalid = 0,
        Value = 1,
        Function = 2,
        Script = 4,
        Buffer = 8,
        Name = 16,
        BufferArray = 32,
        DefinedElement = Function | Script | Buffer,
        DefinedFunction = Function | Script,
        InternalDefinedElement = Function | Buffer,
        NumberResolvable = Value | Name,
        AllElements = -1
    }
}