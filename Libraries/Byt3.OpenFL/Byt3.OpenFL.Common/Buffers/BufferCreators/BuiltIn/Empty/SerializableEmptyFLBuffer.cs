﻿using Byt3.OpenCL.Memory;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;

namespace Byt3.OpenFL.Common.Buffers.BufferCreators.BuiltIn.Empty
{
    public class SerializableEmptyFLBuffer : SerializableFLBuffer
    {
        public SerializableEmptyFLBuffer(string name) : base(name)
        {
        }

        public override FLBuffer GetBuffer()
        {
            return new LazyLoadingFLBuffer(root =>
                new FLBuffer(
                    CLAPI.CreateEmpty<byte>(root.Instance, root.InputSize, MemoryFlag.ReadWrite,
                        "EmptySerializableBuffer." + Name),
                    root.Dimensions.x, root.Dimensions.y));
        }

        public override string ToString()
        {
            return "--define texture " + Name + ": empty";
        }
    }
}