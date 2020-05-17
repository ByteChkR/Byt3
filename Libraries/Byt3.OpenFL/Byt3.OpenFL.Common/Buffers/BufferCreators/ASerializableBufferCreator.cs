﻿using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;
using Byt3.OpenFL.Common.ElementModifiers;

namespace Byt3.OpenFL.Common.Buffers.BufferCreators
{
    /// <summary>
    /// Used when parsing
    /// Creates a ParsableBuffer object based on the string arguments.
    /// </summary>
    public abstract class ASerializableBufferCreator
    {
        public abstract bool IsCorrectBuffer(string bufferKey);

        public abstract SerializableFLBuffer CreateBuffer(string name, string[] args, FLBufferModifiers modifiers,
            int arraySize);
    }
}