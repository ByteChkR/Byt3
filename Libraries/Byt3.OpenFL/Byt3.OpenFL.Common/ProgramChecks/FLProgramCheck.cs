using System;
using System.Collections.Generic;
using Byt3.ADL;
using Byt3.ADL.Configs;
using Byt3.ObjectPipeline;
using Byt3.OpenFL.Common.Buffers.BufferCreators;
using Byt3.OpenFL.Common.Instructions.InstructionCreators;

namespace Byt3.OpenFL.Common.ProgramChecks
{
    public abstract class FLProgramCheck : PipelineStage
    {
        private static readonly ProjectDebugConfig Settings = new ProjectDebugConfig("OpenFL.Common.ProgramChecks", -1, 4, PrefixLookupSettings.AddPrefixIfAvailable);

        protected ADLLogger<LogType> Logger => CreatedLoggers[GetType()];

        private static readonly Dictionary<Type, ADLLogger<LogType>> CreatedLoggers = new Dictionary<Type, ADLLogger<LogType>>();

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

            if (!CreatedLoggers.ContainsKey(GetType()))
            {
                ADLLogger<LogType> l = new ADLLogger<LogType>(Settings, GetType().Name);
                CreatedLoggers[GetType()] = l;
            }
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