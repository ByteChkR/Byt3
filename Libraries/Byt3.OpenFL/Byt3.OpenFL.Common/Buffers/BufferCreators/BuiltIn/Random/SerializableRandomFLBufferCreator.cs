using System;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;
using Byt3.OpenFL.Common.ElementModifiers;

namespace Byt3.OpenFL.Common.Buffers.BufferCreators.BuiltIn.Random
{
    public class SerializableRandomFLBufferCreator : ASerializableBufferCreator
    {
        public override SerializableFLBuffer CreateBuffer(string name, string[] args, FLBufferModifiers modifiers,
            int arraySize)
        {
            if (modifiers.IsArray && arraySize <= 0)
            {
                throw new InvalidOperationException(
                    $"Random Array buffer \"{name}\" has to be initialized with a size as the first argument");
            }

            return new SerializableRandomFLBuffer(name, modifiers, arraySize);
        }

        public override bool IsCorrectBuffer(string bufferKey)
        {
            return bufferKey == "rnd";
        }
    }
}