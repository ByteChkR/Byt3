using System;
using System.Collections.Generic;
using System.IO;
using Byt3.OpenFL.Common.Arguments;
using Byt3.OpenFL.Common.Buffers.BufferCreators.BuiltIn.Empty;
using Byt3.OpenFL.Common.Buffers.BufferCreators.BuiltIn.FromFile;
using Byt3.OpenFL.Common.Buffers.BufferCreators.BuiltIn.Random;
using Byt3.OpenFL.Common.Buffers.BufferCreators.BuiltIn.WFC;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;
using Byt3.OpenFL.Common.Instructions.InstructionCreators;
using Byt3.OpenFL.Serialization.Exceptions;
using Byt3.OpenFL.Serialization.FileFormat;
using Byt3.OpenFL.Serialization.Serializers.Internal;
using Byt3.OpenFL.Serialization.Serializers.Internal.ArgumentSerializer;
using Byt3.OpenFL.Serialization.Serializers.Internal.BufferSerializer;
using Byt3.OpenFL.Serialization.Serializers.Internal.FileFormatSerializer;
using Byt3.Serialization;

namespace Byt3.OpenFL.Serialization
{
    public static class FLSerializer
    {
        private static Byt3Serializer CreateLoader(FLInstructionSet iset)
        {
            SerializableBufferArgumentSerializer bbuf = new SerializableBufferArgumentSerializer();
            SerializableDecimalArgumentSerializer debuf = new SerializableDecimalArgumentSerializer();
            SerializableFunctionArgumentSerializer fabuf = new SerializableFunctionArgumentSerializer();
            SerializableExternalFunctionArgumentSerializer exbuf = new SerializableExternalFunctionArgumentSerializer();
            Dictionary<Type, FLBaseSerializer> argumentParser = new Dictionary<Type, FLBaseSerializer>
            {
                {typeof(SerializeBufferArgument), bbuf},
                {typeof(SerializeDecimalArgument), debuf},
                {typeof(SerializeFunctionArgument), fabuf},
                {typeof(SerializeExternalFunctionArgument), exbuf}
            };

            SerializableFLFunctionSerializer efunc = new SerializableFLFunctionSerializer(argumentParser);
            SerializableExternalFLFunctionSerializer exfunc = new SerializableExternalFLFunctionSerializer(iset);
            EmptyFLBufferSerializer ebuf = new EmptyFLBufferSerializer();
            RandomFLBufferSerializer rbuf = new RandomFLBufferSerializer();
            UnifiedRandomFLBufferSerializer urbuf = new UnifiedRandomFLBufferSerializer();
            FromImageFLBufferSerializer fibuf = new FromImageFLBufferSerializer(true);
            WFCFLBufferSerializer wfcbuf = new WFCFLBufferSerializer();
            Dictionary<Type, FLBaseSerializer> bufferParser =
                new Dictionary<Type, FLBaseSerializer>
                {
                    {typeof(SerializableExternalFLFunction), exfunc},
                    {typeof(SerializableFLFunction), efunc},
                    {typeof(SerializableEmptyFLBuffer), ebuf},
                    {typeof(SerializableRandomFLBuffer), rbuf},
                    {typeof(SerializableUnifiedRandomFLBuffer), urbuf},
                    {typeof(SerializableFromFileFLBuffer), fibuf},
                    {typeof(SerializableWaveFunctionCollapseFLBuffer), wfcbuf}
                };

            SerializableFLProgramSerializer prog = new SerializableFLProgramSerializer(bufferParser, iset);
            Byt3Serializer main = Byt3Serializer.GetDefaultSerializer();
            main.AddSerializer(typeof(SerializableFLProgram), prog);
            main.AddSerializer(typeof(FLFileFormat), new FLFileFormatSerializer());
            return main;
        }


        public static SerializableFLProgram LoadProgram(Stream s, FLInstructionSet iset)
        {
            Byt3Serializer main = CreateLoader(iset);

            if (!main.TryReadPacket(s, out FLFileFormat file))
            {
                throw new FLDeserializationException("Can not parse FL File Format");
            }

            MemoryStream programStream = new MemoryStream(file.Program);

            if (!main.TryReadPacket(programStream, out SerializableFLProgram program))
            {
                throw new FLDeserializationException("Program Data is Corrupt");
            }

            return program;
        }

        public static void SaveProgram(Stream s, SerializableFLProgram program, FLInstructionSet iset,
            string[] extraSteps,
            FLProgramHeader programHeader = null)
        {
            Byt3Serializer main = CreateLoader(iset);

            MemoryStream ms = new MemoryStream();

            if (!main.TryWritePacket(ms, program))
            {
                throw new FLDeserializationException("Can not parse stream");
            }

            if (programHeader == null)
            {
                programHeader = new FLProgramHeader("Program", "NONE", Version.Parse("0.0.0.1"));
            }

            FLHeader header = new FLHeader(FLVersions.HeaderVersion, FLVersions.SerializationVersion,
                FLVersions.CommonVersion, extraSteps);

            byte[] p = ms.ToArray();

            ms.Close();

            FLFileFormat file = new FLFileFormat(header, programHeader, p);

            if (!main.TryWritePacket(s, file))
            {
                throw new FLDeserializationException("Can not parse stream");
            }
        }
    }
}