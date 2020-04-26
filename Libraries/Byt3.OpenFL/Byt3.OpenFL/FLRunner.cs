using Byt3.OpenCL.Memory;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenFL.Common;
using Byt3.OpenFL.Common.Buffers;
using Byt3.OpenFL.Common.Buffers.BufferCreators;
using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;
using Byt3.OpenFL.Common.Instructions.InstructionCreators;
using Byt3.OpenFL.Common.ProgramChecks;
using Byt3.OpenFL.Parsing;
using Byt3.OpenFL.Parsing.Stages;

namespace Byt3.OpenFL
{
    public class FLRunner
    {
        public FLInstructionSet InstructionSet { get; }
        public BufferCreator BufferCreator { get; }
        public FLParser Parser { get; }
        public CLAPI Instance { get; }

        public FLRunner(CLAPI instance, FLInstructionSet instructionSet, BufferCreator bufferCreator,
            FLProgramCheckBuilder checkPipeline)
        {
            InstructionSet = instructionSet;
            BufferCreator = bufferCreator;
            Parser = new FLParser(InstructionSet, BufferCreator);
            checkPipeline.Attach(Parser, true);
            Instance = instance;
        }

        public FLRunner(FLInstructionSet instructionSet, BufferCreator bufferCreator,
            FLProgramCheckBuilder checkPipeline) : this(CLAPI.MainThread, instructionSet, bufferCreator, checkPipeline) { }

        public FLRunner(CLAPI instance, FLInstructionSet instructionSet, BufferCreator bufferCreator) : this(instance, instructionSet, bufferCreator, FLProgramCheckBuilder.CreateDefaultCheckBuilder(instructionSet, bufferCreator)) { }

        public FLRunner(FLInstructionSet instructionSet, BufferCreator bufferCreator) : this(CLAPI.MainThread, instructionSet, bufferCreator) { }

        public FLRunner(CLAPI instance, KernelDatabase database) : this(instance, FLInstructionSet.CreateWithBuiltInTypes(database), BufferCreator.CreateWithBuiltInTypes()) { }

        public FLRunner(KernelDatabase database) : this(CLAPI.MainThread, database) { }

        public FLRunner(CLAPI instance, string kernelPath) : this(FLInstructionSet.CreateWithBuiltInTypes(instance, kernelPath), BufferCreator.CreateWithBuiltInTypes()) { }

        public FLRunner(string kernelPath) : this(CLAPI.MainThread, kernelPath) { }


        public FLProgram Run(string file, int width, int height)
        {
            return Run(Parser.Process(new FLParserInput(file)), width, height);
        }

        public FLProgram Run(SerializableFLProgram file, int width, int height)
        {
            return Run(file.Initialize(InstructionSet), width, height);
        }

        public FLProgram Run(FLProgram file, int width, int height)
        {
            FLBuffer buffer = new FLBuffer(CLAPI.CreateEmpty<byte>(Instance, height * width * 4, MemoryFlag.ReadWrite, "FLRunnerExecutionCreatedBuffer"), width, height);
            return Run(file, buffer, true);
        }

        public FLProgram Run(FLProgram file, FLBuffer input, bool makeInternal)
        {
            file.Run(Instance, input, makeInternal);
            return file;
        }

        public FLProgram Initialize(SerializableFLProgram file)
        {
            return file.Initialize(InstructionSet);
        }
    }
}