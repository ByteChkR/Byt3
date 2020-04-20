using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using Byt3.OpenCL.DataTypes;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenCL.Wrapper.TypeEnums;
using Byt3.OpenFL.Common;
using Byt3.OpenFL.Common.Buffers;
using Byt3.OpenFL.Common.Buffers.BufferCreators;
using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;
using Byt3.OpenFL.Common.Instructions;
using Byt3.OpenFL.Common.Instructions.InstructionCreators;
using Byt3.OpenFL.Common.ProgramChecks;
using Byt3.OpenFL.Parsing;
using Byt3.OpenFL.Parsing.Stages;
using Byt3.OpenFL.Serialization;
using Byt3.Utilities.Exceptions;
using NUnit.Framework;

namespace Byt3.OpenFL.Tests
{
    public class FLInterpreterTests
    {
        [Test]
        public void OpenFL_Comments_Test()
        {
            string file = Path.GetFullPath("resources/filter/comments/test.fl");

            FLParser parser = new FLParser();

            Assembly asm = Assembly.GetAssembly(typeof(ASerializableBufferCreator));
            parser.BufferCreator.AddBufferCreatorsInAssembly(asm);

            SerializableFLProgram pr = parser.Process(new FLParserInput(file));
            //FLFunction entryPoint = pr.EntryPoint; //Provoking an exception if main function is not found
        }

        [Test]
        public void OpenFL_DefineFile_Wrong_Test()
        {
            string file = "resources/filter/defines/test_wrong_define_invalid_file.fl";


            Assert.Catch<Byt3Exception>(() =>
            {
                BufferCreator bc = new BufferCreator();
                FLInstructionSet iset = new FLInstructionSet();
                FLProgramCheckPipeline checkPipeline = new FLProgramCheckPipeline(iset, bc);
                checkPipeline.AddSubStage(new FilePathValidator());

                FLParser parser = new FLParser(iset, bc, checkPipeline);

                Assembly asm = Assembly.GetAssembly(typeof(ASerializableBufferCreator));
                parser.BufferCreator.AddBufferCreatorsInAssembly(asm);

                SerializableFLProgram pr = parser.Process(new FLParserInput(file));
            });
        }

        [Test]
        public void OpenFL_Defines_Test()
        {
            string file = Path.GetFullPath("resources/filter/defines/test.fl");

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
        public void OpenFL_Parser_Test()
        {



            ADL.Debug.DefaultInitialization();
            string path = "resources/filter/tests";
            string[] files = Directory.GetFiles(path, "*.fl", SearchOption.TopDirectoryOnly);
            KernelDatabase db =
                new KernelDatabase(CLAPI.MainThread, "resources/kernel", DataVectorTypes.Uchar1);

            FLInstructionSet iset = new FLInstructionSet();
            iset.AddInstructionWithDefaultCreator<JumpFLInstruction>("jmp");
            iset.AddInstructionWithDefaultCreator<SetActiveFLInstruction>("setactive");
            iset.AddInstructionWithDefaultCreator<RandomFLInstruction>("rnd");
            iset.AddInstructionWithDefaultCreator<URandomFLInstruction>("urnd");
            iset.AddInstruction(new KernelFLInstructionCreator(db));


            BufferCreator bc = new BufferCreator();
            Assembly asm = Assembly.GetAssembly(typeof(ASerializableBufferCreator));
            bc.AddBufferCreatorsInAssembly(asm);

            FLParser parser = new FLParser(iset, bc);
            FLRunner runner = new FLRunner(iset);

#if DEBUG
            Directory.CreateDirectory("./out/image");
#endif

            for (int i = 0; i < files.Length; i++)
            {
                string file = files[i];
#if DEBUG
                Bitmap bmp = new Bitmap(128, 128);
#else
                Bitmap bmp = new Bitmap(32, 32);
#endif
                FLBuffer buf = new FLBuffer(CLAPI.MainThread, bmp);

                SerializableFLProgram parsedProgram = parser.Process(new FLParserInput(files[i]));

                FLProgram program = runner.Initialize(parsedProgram);

                program.Run(CLAPI.MainThread, buf);


#if DEBUG
                CLAPI.UpdateBitmap(CLAPI.MainThread, bmp, buf.Buffer);

                

                string p = Path.Combine("./out/image", Path.GetFileNameWithoutExtension(files[i]) + ".png");
                bmp.Save(p + ".png");
#endif
                program.FreeResources();
                buf.Dispose();
                bmp.Dispose();
            }
        }

        [Test]
        public void OpenCL_Serializer_Serialize_Tests()
        {

            ADL.Debug.DefaultInitialization();
            string path = "resources/filter/tests";
            string[] files = Directory.GetFiles(path, "*.fl", SearchOption.TopDirectoryOnly);
            KernelDatabase db =
                new KernelDatabase(CLAPI.MainThread, "resources/kernel", DataVectorTypes.Uchar1);

            FLInstructionSet iset = new FLInstructionSet();
            iset.AddInstructionWithDefaultCreator<JumpFLInstruction>("jmp");
            iset.AddInstructionWithDefaultCreator<SetActiveFLInstruction>("setactive");
            iset.AddInstructionWithDefaultCreator<RandomFLInstruction>("rnd");
            iset.AddInstructionWithDefaultCreator<URandomFLInstruction>("urnd");
            iset.AddInstruction(new KernelFLInstructionCreator(db));


            BufferCreator bc = new BufferCreator();
            Assembly asm = Assembly.GetAssembly(typeof(ASerializableBufferCreator));
            bc.AddBufferCreatorsInAssembly(asm);

            FLParser parser = new FLParser(iset, bc);
            FLRunner runner = new FLRunner(iset);

            Directory.CreateDirectory("./out/serialized");
#if DEBUG
            Directory.CreateDirectory("./out/image-serialized");
#endif

            for (int i = 0; i < files.Length; i++)
            {
#if DEBUG
                Bitmap bmp = new Bitmap(128, 128);
#else
                Bitmap bmp = new Bitmap(32, 32);
#endif

                FLBuffer buf = new FLBuffer(CLAPI.MainThread, bmp);
                SerializableFLProgram pr = parser.Process(new FLParserInput(files[i]));
                string pCompiledOut = Path.Combine("./out/serialized", Path.GetFileNameWithoutExtension(files[i])+".flc");


                Stream cs = File.OpenWrite(pCompiledOut);
                FLSerializer.SaveProgram(cs, pr, new string[]{ });
                cs.Close();


                cs = File.OpenRead(pCompiledOut);
                SerializableFLProgram loaded = FLSerializer.LoadProgram(cs);
                cs.Close();


                FLProgram program = runner.Initialize(loaded);
                program.Run(CLAPI.MainThread, buf);


#if DEBUG
                CLAPI.UpdateBitmap(CLAPI.MainThread, bmp, buf.Buffer);
                string p = Path.Combine("./out/image-serialized", Path.GetFileNameWithoutExtension(files[i]) + ".png");
                bmp.Save(p);
#endif

                program.FreeResources();
                buf.Dispose();
                bmp.Dispose();
            }
        }

        [Test]
        public void OpenFL_WFCDefines_Wrong_Test()
        {
            string[] files = Directory.GetFiles("resources/filter/defines/", "test_wrong_define_wfc_*.fl");


            foreach (string file in files)
            {
                Assert.Catch<Byt3Exception>(() =>
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
            byte b = (byte)CLTypeConverter.Convert(typeof(byte), f);
            float4 f4 = new float4(f);
            uchar4 i4 = (uchar4)CLTypeConverter.Convert(typeof(uchar4), f4);
            Assert.True(b == 128);

            for (int i = 0; i < 4; i++)
            {
                byte s = i4[i];
                Assert.True(s == 128);
            }
        }
    }
}