﻿using Byt3.OpenCL.Wrapper;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;

namespace Byt3.OpenFL.Common.Buffers.BufferCreators.BuiltIn.FromFile
{
    public class SerializableFromBinaryFLBuffer : SerializableFLBuffer
    {
        public byte[] Data { get; }
        public int Width { get; }
        public int Height { get; }

        public SerializableFromBinaryFLBuffer(string name, byte[] data, int width, int height, bool isArray) : base(name, isArray)
        {
            Data = data;
            Width = width;
            Height = height;
        }

        public override FLBuffer GetBuffer()
        {
            return new FLBuffer(CLAPI.MainThread, Data, Width, Height, "BinaryBuffer." + Name);
        }

        public override string ToString()
        {
            return $"{(IsArray? FLKeywords.DefineArrayKey: FLKeywords.DefineTextureKey)} {Name} binary({Width}/{Height})";
        }
    }
}