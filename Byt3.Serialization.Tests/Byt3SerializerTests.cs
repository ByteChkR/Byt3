using System;
using System.IO;
using Byt3.Serialization.Serializers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Byt3.Serialization.Tests
{
    [TestClass]
    public class Byt3SerializerTests
    {

        #region Example Packet / Serializer

        private class Packet
        {
            public float valfloat;
            public double valdouble;

            public int valint;
            public uint valuint;

            public short valshort;
            public ushort valushort;

            public long vallong;
            public ulong valulong;

            public string valstring;

            public byte valbyte;
            public byte[] valbytes;

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
                    valbytes = new byte[] { 1, 1, 1, 1, 0, 0, 0, 1, 1, 1 };
                    valbyte = 9;
                    valstring = "TESTTEST";
                }
            }
        }

        private class PacketSerializer : ASerializer<Packet>
        {

            public override Packet DeserializePacket(PrimitiveValueWrapper wrapper)
            {
                Packet p = new Packet();

                p.valfloat = wrapper.ReadFloat();
                p.valdouble = wrapper.ReadDouble();

                p.valint = wrapper.ReadInt();
                p.valuint = wrapper.ReadUInt();

                p.valshort = wrapper.ReadShort();
                p.valushort = wrapper.ReadUShort();

                p.vallong = wrapper.ReadLong();
                p.valulong = wrapper.ReadULong();

                p.valbytes = wrapper.ReadBytes();
                p.valbyte = wrapper.ReadByte();

                p.valstring = wrapper.ReadString();

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


            }
        }

        #endregion

        [TestMethod]
        public void Serializer_ReadWriteTest()
        {

            Packet p = new Packet(true);

            Byt3Serializer.AddSerializer<Packet>(new PacketSerializer());

            MemoryStream stream = new MemoryStream();
            Byt3Serializer.WritePacket(stream, p);
            stream.Position = 0;

            Packet p2 = Byt3Serializer.ReadPacket<Packet>(stream);

            Assert.IsTrue(Math.Abs(p.valfloat - p2.valfloat) < 0.001f);
            Assert.IsTrue(Math.Abs(p.valdouble - p2.valdouble) < 0.001f);

            Assert.IsTrue(p.valint == p2.valint);
            Assert.IsTrue(p.valuint == p2.valuint);

            Assert.IsTrue(p.valshort == p2.valshort);
            Assert.IsTrue(p.valushort == p2.valushort);

            Assert.IsTrue(p.vallong == p2.vallong);
            Assert.IsTrue(p.valulong == p2.valulong);

            Assert.IsTrue(p.valstring == p2.valstring);

            for (int i = 0; i < p.valbytes.Length; i++)
            {
                Assert.IsTrue(p.valbytes[i] == p2.valbytes[i]);
            }

        }
    }
}
