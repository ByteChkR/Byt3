using System;

namespace Byt3.OpenFL.Common.DataObjects.SerializableDataObjects
{
    [Flags]
    public enum InstructionArgumentCategory
    {
        Invalid = 0,
        Value = 1,
        Function = 2,
        Script = 4,
        Buffer = 8,
        Name = 16,
        BufferArray = 32,

        /// <summary>
        /// Buffer | BufferArray
        /// </summary>
        AnyBuffer = Buffer | BufferArray,

        /// <summary>
        /// Function | Script | AnyBuffer
        /// </summary>
        DefinedElement = Function | Script | AnyBuffer,

        /// <summary>
        /// Function | Script
        /// </summary>
        DefinedFunction = Function | Script,

        /// <summary>
        /// Function | AnyBuffer
        /// </summary>
        InternalDefinedElement = Function | AnyBuffer,

        /// <summary>
        /// Value | Name
        /// </summary>
        NumberResolvable = Value | Name,

        /// <summary>
        /// All Elements.
        /// </summary>
        AllElements = -1
    }
}