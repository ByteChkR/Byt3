﻿using System;
using System.Linq;
using System.Reflection;
using Byt3.Callbacks;
using Byt3.OpenCL.DataTypes;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenFL.Common.Buffers.BufferCreators;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;
using Byt3.OpenFL.Common.Instructions.InstructionCreators;
using Byt3.OpenFL.Common.ProgramChecks;
using Byt3.OpenFL.Parsing;
using Byt3.OpenFL.Parsing.Stages;
using Byt3.Utilities.Exceptions;
using NUnit.Framework;

namespace Byt3.OpenFL.Tests
{
    public class FL_Syntax_Tests
    {
        [Test]
        public void OpenFL_Comments_Test()
        {
            TestSetup.SetupTestingSession();
            string file =
                "resources/filter/comments/test.fl";

            FLParser parser = new FLParser();

            Assembly asm = Assembly.GetAssembly(typeof(ASerializableBufferCreator));
            parser.BufferCreator.AddBufferCreatorsInAssembly(asm);

            SerializableFLProgram pr = parser.Process(new FLParserInput(file));
            //FLFunction entryPoint = pr.EntryPoint; //Provoking an exception if main function is not found
        }

        [Test]
        public void OpenFL_DefineFile_Wrong_Test()
        {
            TestSetup.SetupTestingSession();
            string file = "resources/filter/defines/test_wrong_define_invalid_file.fl";

            Assert.Catch<Byt3Exception>(() =>
            {
                BufferCreator bc = new BufferCreator();
                FLInstructionSet iset = new FLInstructionSet();
                FLProgramCheckBuilder checkBuilder = new FLProgramCheckBuilder(iset, bc);
                checkBuilder.AddProgramCheck(new FilePathValidator());

                FLParser parser = new FLParser(iset, bc);
                checkBuilder.Attach(parser, true);

                Assembly asm = Assembly.GetAssembly(typeof(ASerializableBufferCreator));
                parser.BufferCreator.AddBufferCreatorsInAssembly(asm);

                SerializableFLProgram pr = parser.Process(new FLParserInput(file));
            });
        }

        [Test]
        public void OpenFL_Defines_Test()
        {
            TestSetup.SetupTestingSession();
            string file = "resources/filter/defines/test.fl";

            FLParser parser = new FLParser();

            Assembly asm = Assembly.GetAssembly(typeof(ASerializableBufferCreator));
            parser.BufferCreator.AddBufferCreatorsInAssembly(asm);

            SerializableFLProgram result = parser.Process(new FLParserInput(file));


            Assert.True(result.DefinedBuffers.Count == 5);
            Assert.True(result.DefinedBuffers.Count(x => x.Name == "in") == 1);
            Assert.True(result.DefinedBuffers.Count(x => x.Name == "textureD") == 1);
            Assert.True(result.DefinedBuffers.Count(x => x.Name == "textureC") == 1);
            Assert.True(result.DefinedBuffers.Count(x => x.Name == "textureB") == 1);
            Assert.True(result.DefinedBuffers.Count(x => x.Name == "textureA") == 1);
        }

        [Test]
        public void OpenFL_DefineScriptFile_Wrong_Test()
        {
            TestSetup.SetupTestingSession();
            string file = "resources/filter/defines/test_wrong_script_invalid_file.fl";
            Assert.Catch<Byt3Exception>(() =>
            {
                FLParser parser = new FLParser();

                Assembly asm = Assembly.GetAssembly(typeof(ASerializableBufferCreator));
                parser.BufferCreator.AddBufferCreatorsInAssembly(asm);

                SerializableFLProgram pr = parser.Process(new FLParserInput(file));
            });
        }


        [Test]
        public void OpenFL_DefineScriptNoFile_Wrong_Test()
        {
            TestSetup.SetupTestingSession();
            string file = "resources/filter/defines/test_wrong_script_.fl";
            Assert.Catch<Byt3Exception>(() =>
            {
                FLParser parser = new FLParser();

                Assembly asm = Assembly.GetAssembly(typeof(ASerializableBufferCreator));
                parser.BufferCreator.AddBufferCreatorsInAssembly(asm);

                SerializableFLProgram pr = parser.Process(new FLParserInput(file));
            });
        }

        [Test]
        public void OpenFL_WFCDefines_Wrong_Test()
        {
            TestSetup.SetupTestingSession();

            string[] files = IOManager.GetFiles("resources/filter/defines/", "test_wrong_define_wfc_*.fl");

            Assert.IsNotEmpty(files, "No Test FL Scripts found for "+ nameof(OpenFL_WFCDefines_Wrong_Test));

            foreach (string file in files)
            {
                Assert.Catch<AggregateException>(() =>
                {
                    FLParser parser = new FLParser();

                    Assembly asm = Assembly.GetAssembly(typeof(ASerializableBufferCreator));
                    parser.BufferCreator.AddBufferCreatorsInAssembly(asm);

                    SerializableFLProgram pr = parser.Process(new FLParserInput(file));
                });
            }
        }

        [Test]
        public void OpenFL_TypeConversion_Test()
        {
            float f = float.MaxValue / 2;
            byte b = (byte) CLTypeConverter.Convert(typeof(byte), f);
            float4 f4 = new float4(f);
            uchar4 i4 = (uchar4) CLTypeConverter.Convert(typeof(uchar4), f4);
            Assert.True(b == 128);

            for (int i = 0; i < 4; i++)
            {
                byte s = i4[i];
                Assert.True(s == 128);
            }
        }
    }
}