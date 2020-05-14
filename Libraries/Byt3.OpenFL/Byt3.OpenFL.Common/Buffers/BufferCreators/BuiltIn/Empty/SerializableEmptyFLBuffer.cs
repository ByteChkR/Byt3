using Byt3.OpenCL.Memory;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;

namespace Byt3.OpenFL.Common.Buffers.BufferCreators.BuiltIn.Empty
{
    public class SerializableEmptyFLBuffer : SerializableFLBuffer
    {
        public readonly int Size;

        public SerializableEmptyFLBuffer(string name) : base(name, false)
        {
        }
        public SerializableEmptyFLBuffer(string name, int size) : base(name, true)
        {
            Size = size;
        }



        public override FLBuffer GetBuffer()
        {

            if (!IsArray)
            {
                return new LazyLoadingFLBuffer(root =>
                    new FLBuffer(
                        CLAPI.CreateEmpty<byte>(root.Instance, root.InputSize, MemoryFlag.ReadWrite,
                            "EmptySerializableBuffer." + Name),
                        root.Dimensions.x, root.Dimensions.y));
            }
            else
            {
                return new LazyLoadingFLBuffer(root =>
                    new FLBuffer(
                        CLAPI.CreateEmpty<byte>(root.Instance, Size, MemoryFlag.ReadWrite,
                            "EmptySerializableBuffer." + Name),
                        Size, 1));
            }
        }

        public override string ToString()
        {
            return $"--define {(IsArray ? "array" : "texture")} {Name}: empty";
        }
    }
}