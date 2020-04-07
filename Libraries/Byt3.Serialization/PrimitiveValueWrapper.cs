using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Byt3.Utilities.Serialization;

namespace Byt3.Serialization
{
    /// <summary>
    /// Simple Wrapper that Sequentially Read and Write Primitive Values to a Specified Stream
    /// </summary>
    public class PrimitiveValueWrapper
    {
        /// <summary>
        /// Underlaying Stream
        /// </summary>
        private readonly Stream stream;

        public bool IsValid { get; private set; }

        /// <summary>
        /// Packet Cache used to cache the written values
        /// </summary>
        private readonly List<byte> packetCache = new List<byte>();

        /// <summary>
        /// Public Constructor
        /// </summary>
        /// <param name="s">Underlaying Stream</param>
        public PrimitiveValueWrapper(Stream s)
        {
            IsValid = true;
            stream = s;
        }

        /// <summary>
        /// Reads an int from the Underlaying Stream
        /// </summary>
        /// <returns>Deserialized int</returns>
        public int ReadInt()
        {
            if (!IsValid) throw new Exception("Trying to access an invalid PrimitiveValueWrapper.");
            byte[] buf = new byte[sizeof(int)];
            stream.Read(buf, 0, buf.Length);
            return BitConverter.ToInt32(buf, 0);
        }
        /// <summary>
        /// Reads an uint from the Underlaying Stream
        /// </summary>
        /// <returns>Deserialized uint</returns>
        public uint ReadUInt()
        {
            if (!IsValid) throw new Exception("Trying to access an invalid PrimitiveValueWrapper.");
            byte[] buf = new byte[sizeof(uint)];
            stream.Read(buf, 0, buf.Length);
            return BitConverter.ToUInt32(buf, 0);
        }
        /// <summary>
        /// Reads a long from the Underlaying Stream
        /// </summary>
        /// <returns>Deserialized long</returns>
        public long ReadLong()
        {
            if (!IsValid) throw new Exception("Trying to access an invalid PrimitiveValueWrapper.");
            byte[] buf = new byte[sizeof(long)];
            stream.Read(buf, 0, buf.Length);
            return BitConverter.ToInt64(buf, 0);
        }
        /// <summary>
        /// Reads an ulong from the Underlaying Stream
        /// </summary>
        /// <returns>Deserialized ulong</returns>
        public ulong ReadULong()
        {
            if (!IsValid) throw new Exception("Trying to access an invalid PrimitiveValueWrapper.");
            byte[] buf = new byte[sizeof(ulong)];
            stream.Read(buf, 0, buf.Length);
            return BitConverter.ToUInt64(buf, 0);
        }
        /// <summary>
        /// Reads a short from the Underlaying Stream
        /// </summary>
        /// <returns>Deserialized short</returns>
        public short ReadShort()
        {
            if (!IsValid) throw new Exception("Trying to access an invalid PrimitiveValueWrapper.");
            byte[] buf = new byte[sizeof(short)];
            stream.Read(buf, 0, buf.Length);
            return BitConverter.ToInt16(buf, 0);
        }
        /// <summary>
        /// Reads an ushort from the Underlaying Stream
        /// </summary>
        /// <returns>Deserialized ushort</returns>
        public ushort ReadUShort()
        {
            if (!IsValid) throw new Exception("Trying to access an invalid PrimitiveValueWrapper.");
            byte[] buf = new byte[sizeof(ushort)];
            stream.Read(buf, 0, buf.Length);
            return BitConverter.ToUInt16(buf, 0);
        }
        /// <summary>
        /// Reads a bool from the Underlaying Stream
        /// </summary>
        /// <returns>Deserialized bool</returns>
        public bool ReadBool()
        {
            if (!IsValid) throw new Exception("Trying to access an invalid PrimitiveValueWrapper.");
            byte[] buf = new byte[sizeof(bool)];
            stream.Read(buf, 0, buf.Length);
            return BitConverter.ToBoolean(buf, 0);
        }
        /// <summary>
        /// Reads a float from the Underlaying Stream
        /// </summary>
        /// <returns>Deserialized float</returns>
        public float ReadFloat()
        {
            if (!IsValid) throw new Exception("Trying to access an invalid PrimitiveValueWrapper.");
            byte[] buf = new byte[sizeof(float)];
            stream.Read(buf, 0, buf.Length);
            return BitConverter.ToSingle(buf, 0);
        }
        /// <summary>
        /// Reads a double from the Underlaying Stream
        /// </summary>
        /// <returns>Deserialized double</returns>
        public double ReadDouble()
        {
            byte[] buf = new byte[sizeof(double)];
            stream.Read(buf, 0, buf.Length);
            return BitConverter.ToDouble(buf, 0);
        }
        /// <summary>
        /// Reads a string from the Underlaying Stream
        /// </summary>
        /// <returns>Deserialized string</returns>
        public string ReadString()
        {
            if (!IsValid) throw new Exception("Trying to access an invalid PrimitiveValueWrapper.");
            int len = ReadInt();
            byte[] buf = new byte[len];
            stream.Read(buf, 0, buf.Length);
            return Encoding.ASCII.GetString(buf);
        }

        public byte ReadByte()
        {
            return (byte)stream.ReadByte();
        }

        /// <summary>
        /// Reads a Byte Array from the Underlaying Stream
        /// </summary>
        /// <returns>Deserialized Byte Array</returns>
        public byte[] ReadBytes()
        {
            if (!IsValid) throw new Exception("Trying to access an invalid PrimitiveValueWrapper.");
            int len = ReadInt();
            byte[] buf = new byte[len];
            stream.Read(buf, 0, buf.Length);
            return buf;
        }

        /// <summary>
        /// Writes a Byte Array to the Stream
        /// </summary>
        /// <param name="value">Value to Write</param>
        /// <returns>Bytes Written</returns>
        public int Write(byte[] value)
        {
            if (!IsValid) throw new Exception("Trying to access an invalid PrimitiveValueWrapper.");
            int w = Write(value.Length);
            packetCache.AddRange(value);
            return value.Length + w;
        }

        /// <summary>
        /// Writes an int to the Stream
        /// </summary>
        /// <param name="value">Value to Write</param>
        /// <returns>Bytes Written</returns>
        public int Write(int value)
        {
            if (!IsValid) throw new Exception("Trying to access an invalid PrimitiveValueWrapper.");
            byte[] buf = BitConverter.GetBytes(value);
            packetCache.AddRange(buf);
            return buf.Length;
        }

        /// <summary>
        /// Writes an uint to the Stream
        /// </summary>
        /// <param name="value">Value to Write</param>
        /// <returns>Bytes Written</returns>
        public int Write(uint value)
        {
            if (!IsValid) throw new Exception("Trying to access an invalid PrimitiveValueWrapper.");
            byte[] buf = BitConverter.GetBytes(value);
            packetCache.AddRange(buf);
            return buf.Length;
        }

        /// <summary>
        /// Writes a short to the Stream
        /// </summary>
        /// <param name="value">Value to Write</param>
        /// <returns>Bytes Written</returns>
        public int Write(short value)
        {
            if (!IsValid) throw new Exception("Trying to access an invalid PrimitiveValueWrapper.");
            byte[] buf = BitConverter.GetBytes(value);
            packetCache.AddRange(buf);
            return buf.Length;
        }

        /// <summary>
        /// Writes an ushort to the Stream
        /// </summary>
        /// <param name="value">Value to Write</param>
        /// <returns>Bytes Written</returns>
        public int Write(ushort value)
        {
            if (!IsValid) throw new Exception("Trying to access an invalid PrimitiveValueWrapper.");
            byte[] buf = BitConverter.GetBytes(value);
            packetCache.AddRange(buf);
            return buf.Length;
        }

        /// <summary>
        /// Writes an long to the Stream
        /// </summary>
        /// <param name="value">Value to Write</param>
        /// <returns>Bytes Written</returns>
        public int Write(long value)
        {
            if (!IsValid) throw new Exception("Trying to access an invalid PrimitiveValueWrapper.");
            byte[] buf = BitConverter.GetBytes(value);
            packetCache.AddRange(buf);
            return buf.Length;
        }

        /// <summary>
        /// Writes an ulong to the Stream
        /// </summary>
        /// <param name="value">Value to Write</param>
        /// <returns>Bytes Written</returns>
        public int Write(ulong value)
        {
            if (!IsValid) throw new Exception("Trying to access an invalid PrimitiveValueWrapper.");
            byte[] buf = BitConverter.GetBytes(value);
            packetCache.AddRange(buf);
            return buf.Length;
        }

        /// <summary>
        /// Writes a sbyte to the Stream
        /// </summary>
        /// <param name="value">Value to Write</param>
        /// <returns>Bytes Written</returns>
        public int Write(sbyte value)
        {
            if (!IsValid) throw new Exception("Trying to access an invalid PrimitiveValueWrapper.");
            byte[] buf = BitConverter.GetBytes(value);
            packetCache.AddRange(buf);
            return buf.Length;
        }

        /// <summary>
        /// Writes a byte to the Stream
        /// </summary>
        /// <param name="value">Value to Write</param>
        /// <returns>Bytes Written</returns>
        public int Write(byte value)
        {
            if (!IsValid) throw new Exception("Trying to access an invalid PrimitiveValueWrapper.");
            packetCache.Add(value);
            return 1;
        }

        /// <summary>
        /// Writes a bool to the Stream
        /// </summary>
        /// <param name="value">Value to Write</param>
        /// <returns>Bytes Written</returns>
        public int Write(bool value)
        {
            if (!IsValid) throw new Exception("Trying to access an invalid PrimitiveValueWrapper.");
            byte[] buf = BitConverter.GetBytes(value);
            packetCache.AddRange(buf);
            return buf.Length;
        }

        /// <summary>
        /// Writes a float to the Stream
        /// </summary>
        /// <param name="value">Value to Write</param>
        /// <returns>Bytes Written</returns>
        public int Write(float value)
        {
            if (!IsValid) throw new Exception("Trying to access an invalid PrimitiveValueWrapper.");
            byte[] buf = BitConverter.GetBytes(value);
            packetCache.AddRange(buf);
            return buf.Length;
        }

        /// <summary>
        /// Writes a double to the Stream
        /// </summary>
        /// <param name="value">Value to Write</param>
        /// <returns>Bytes Written</returns>
        public int Write(double value)
        {
            if (!IsValid) throw new Exception("Trying to access an invalid PrimitiveValueWrapper.");
            byte[] buf = BitConverter.GetBytes(value);
            packetCache.AddRange(buf);
            return buf.Length;
        }

        /// <summary>
        /// Writes a string to the Stream
        /// </summary>
        /// <param name="value">Value to Write</param>
        /// <returns>Bytes Written</returns>
        public int Write(string value)
        {
            if (!IsValid) throw new Exception("Trying to access an invalid PrimitiveValueWrapper.");
            byte[] buf = Encoding.ASCII.GetBytes(value);
            int w = Write(buf.Length);
            packetCache.AddRange(buf);
            return buf.Length + w;
        }

        public void WriteSimpleStruct<T>(T obj) where T : struct
        {
            byte[] bytes = StructConverter.StructToBytes(obj);

            Write(bytes);
        }

        public T ReadSimpleStruct<T>() where T : struct
        {
            byte[] bytes = ReadBytes();

            T ret = default(T);
            StructConverter.BytesToStruct(bytes, ref ret);
            return ret;
        }

        internal void SetInvalid()
        {
            IsValid = false;
        }

        /// <summary>
        /// Writes the PacketCache to the Underlaying Stream.
        /// </summary>
        internal void CompleteWrite()
        {
            if (!IsValid) throw new Exception("Trying to access an invalid PrimitiveValueWrapper.");
            stream.Write(packetCache.ToArray(), 0, packetCache.Count);
            packetCache.Clear();
        }
    }
}