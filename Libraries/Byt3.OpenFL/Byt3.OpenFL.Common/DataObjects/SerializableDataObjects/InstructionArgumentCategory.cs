namespace Byt3.OpenFL.Common.DataObjects.SerializableDataObjects
{
    public enum InstructionArgumentCategory
    {
        Invalid =0,
        Value = 1,
        Function = 2,
        Script = 4,
        Buffer = 8,
        DefinedElement = Function|Script|Buffer,
        DefinedFunction = Function|Script,
        InternalDefinedElement = Function|Buffer,
        
    }
}