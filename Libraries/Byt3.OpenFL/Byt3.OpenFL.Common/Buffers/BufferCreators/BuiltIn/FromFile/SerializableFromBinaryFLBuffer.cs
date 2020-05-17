using Byt3.OpenCL.Memory;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;
using Byt3.OpenFL.Common.ElementModifiers;

namespace Byt3.OpenFL.Common.Buffers.BufferCreators.BuiltIn.FromFile
{
    public class SerializableFromBinaryFLBuffer : SerializableFLBuffer
    {
        public SerializableFromBinaryFLBuffer(string name, byte[] data, int width, int height,
            FLBufferModifiers modifiers) : base(name, modifiers)
        {
            Data = data;
            Width = width;
            Height = height;
        }

        public byte[] Data { get; }
        public int Width { get; }
        public int Height { get; }

        public override FLBuffer GetBuffer()
        {
            MemoryFlag flag = Modifiers.IsReadOnly ? MemoryFlag.ReadOnly : MemoryFlag.ReadWrite;
            return new FLBuffer(CLAPI.MainThread, Data, Width, Height, "BinaryBuffer." + Name, flag);
        }

        public override string ToString()
        {
            return base.ToString() + $"binary({Width}/{Height})";
        }
    }
}