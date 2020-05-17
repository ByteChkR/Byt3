using System;
using System.IO;
using Byt3.Serialization.Serializers;
using NUnit.Framework;

namespace Byt3.Serialization.Tests
{
    public class Byt3SerializerTests
    {
        [Test]
        public void Serializer_ReadWrite_Test()
        {
            Packet p = new Packet(true);

            Byt3Serializer s = Byt3Serializer.GetDefaultSerializer();

            s.AddSerializer<Packet>(new PacketSerializer());

            MemoryStream stream = new MemoryStream();
            bool ret = s.TryWritePacket(stream, p);
            Assert.True(ret);
            stream.Position = 0;

            s = Byt3Serializer.GetDefaultSerializer();

            s.AddSerializer<Packet>(new PacketSerializer());


            bool p2ret = s.TryReadPacket(stream, out Packet p2);


            Assert.True(p2ret);

            Assert.True(Math.Abs(p.valfloat - p2.valfloat) < 0.001f);
            Assert.True(Math.Abs(p.valdouble - p2.valdouble) < 0.001f);

            Assert.True(p.valint == p2.valint);
            Assert.True(p.valuint == p2.valuint);

            Assert.True(p.valshort == p2.valshort);
            Assert.True(p.valushort == p2.valushort);

            Assert.True(p.vallong == p2.vallong);
            Assert.True(p.valulong == p2.valulong);

            Assert.True(p.valstring == p2.valstring);

            for (int i = 0; i < p.valbytes.Length; i++)
            {
                Assert.True(p.valbytes[i] == p2.valbytes[i]);
            }

            for (int i = 0; i < p.arrFloats.Length; i++)
            {
                Assert.AreEqual(p.arrFloats[i], p2.arrFloats[i], 0.01f);
            }

            for (int i = 0; i < p.arrStrings.Length; i++)
            {
                Assert.AreEqual(p.arrStrings[i], p2.arrStrings[i]);
            }
        }

        #region Example Packet / Serializer

        private class Packet
        {
            public float[] arrFloats;

            public string[] arrStrings;

            public byte valbyte;
            public byte[] valbytes;
            public double valdouble;
            public float valfloat;

            public int valint;

            public long vallong;

            public short valshort;

            public string valstring;
            public uint valuint;
            public ulong valulong;
            public ushort valushort;

            public Packet(bool setValues = false)
            {
                if (setValues)
                {
                    valfloat = 1f;
                    valdouble = 2d;
                    valint = 3;
                    valuint = 4;
                    valshort = 5;
                    valushort = 6;
                    vallong = 7;
                    valulong = 8;
                    valbytes = new byte[] {1, 1, 1, 1, 0, 0, 0, 1, 1, 1};
                    valbyte = 9;
                    valstring = "TESTTEST";
                    arrStrings = new[] {"A", "B", "Farts"};
                    arrFloats = new[] {420f, 69f};
                }
            }
        }

        private class PacketSerializer : ASerializer<Packet>
        {
            public override Packet DeserializePacket(PrimitiveValueWrapper wrapper)
            {
                Packet p = new Packet
                {
                    valfloat = wrapper.ReadFloat(),
                    valdouble = wrapper.ReadDouble(),
                    valint = wrapper.ReadInt(),
                    valuint = wrapper.ReadUInt(),
                    valshort = wrapper.ReadShort(),
                    valushort = wrapper.ReadUShort(),
                    vallong = wrapper.ReadLong(),
                    valulong = wrapper.ReadULong(),
                    valbytes = wrapper.ReadBytes(),
                    valbyte = wrapper.ReadByte(),
                    valstring = wrapper.ReadString(),
                    arrStrings = wrapper.ReadArray<string>(),
                    arrFloats = wrapper.ReadArray<float>()
                };

                return p;
            }

            public override void SerializePacket(PrimitiveValueWrapper wrapper, Packet obj)
            {
                wrapper.Write(obj.valfloat);
                wrapper.Write(obj.valdouble);

                wrapper.Write(obj.valint);
                wrapper.Write(obj.valuint);

                wrapper.Write(obj.valshort);
                wrapper.Write(obj.valushort);

                wrapper.Write(obj.vallong);
                wrapper.Write(obj.valulong);

                wrapper.Write(obj.valbytes);
                wrapper.Write(obj.valbyte);

                wrapper.Write(obj.valstring);
                wrapper.WriteArray(obj.arrStrings);
                wrapper.WriteArray(obj.arrFloats);
            }
        }

        #endregion
    }
}