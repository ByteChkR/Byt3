using System;
using Byt3.ADL;
using Byt3.ObjectPipeline;
using Byt3.OpenFL.Common.Buffers.BufferCreators;
using Byt3.OpenFL.Common.Instructions.InstructionCreators;

namespace Byt3.OpenFL.Common.ProgramChecks
{
    public abstract class FLProgramCheck : PipelineStage
    {
        protected readonly ADLLogger<LogType> Logger;
        protected FLInstructionSet InstructionSet { get; private set; }
        protected BufferCreator BufferCreator { get; private set; }
        public abstract bool ChangesOutput { get; }

        internal void SetValues(FLInstructionSet iset, BufferCreator bc)
        {
            InstructionSet = iset;
            BufferCreator = bc;
        }

        protected FLProgramCheck(Type inType, Type outType) : base(inType, outType)
        {
            Logger = new ADLLogger<LogType>(OpenFLDebugConfig.Settings, GetType().Name);
        }
    }

    public abstract class FLProgramCheck<T> : FLProgramCheck
        where T : FLPipelineResult
    {
        protected FLProgramCheck() : base(typeof(T), typeof(T))
        {
        }
    }
}