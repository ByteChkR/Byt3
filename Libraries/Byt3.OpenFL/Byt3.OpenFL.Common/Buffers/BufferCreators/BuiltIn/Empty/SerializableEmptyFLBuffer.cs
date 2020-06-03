using Byt3.OpenCL.Memory;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;
using Byt3.OpenFL.Common.ElementModifiers;

namespace Byt3.OpenFL.Common.Buffers.BufferCreators.BuiltIn.Empty
{
    public class SerializableEmptyFLBuffer : SerializableFLBuffer
    {
        public readonly int Size;

        public SerializableEmptyFLBuffer(string name, FLBufferModifiers modifiers) : base(name, modifiers)
        {
        }

        public SerializableEmptyFLBuffer(string name, int size, FLBufferModifiers modifiers) : base(name, modifiers)
        {
            Size = size;
        }


        public override FLBuffer GetBuffer()
        {
            MemoryFlag flag = Modifiers.IsReadOnly ? MemoryFlag.ReadOnly : MemoryFlag.ReadWrite;
            if (!IsArray)
            {
                return new LazyLoadingFLBuffer(root =>
                    new FLBuffer(
                        CLAPI.CreateEmpty<byte>(root.Instance, root.InputSize, flag,
                            "EmptySerializableBuffer." + Name),
                        root.Dimensions.x, root.Dimensions.y, root.Dimensions.z), Modifiers.InitializeOnStart);
            }

            return new LazyLoadingFLBuffer(root =>
                new FLBuffer(
                    CLAPI.CreateEmpty<byte>(root.Instance, Size, flag,
                        "EmptySerializableBuffer." + Name),
                    Size, 1, 1), Modifiers.InitializeOnStart);
        }

        public override string ToString()
        {
            return base.ToString() + "empty";
        }
    }
}