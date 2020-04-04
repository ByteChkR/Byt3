using System;
using Byt3.BuildSystem.Settings;
using Byt3.ObjectPipeline;

namespace Byt3.BuildSystem.Stages
{
    public abstract class BuilderStage : PipelineStage
    {
        public BuildStageSettings SettingsObj { get; protected set; }
    }

    public abstract class BuilderStage<TIn, TOut> : BuilderStage
    {
        public override Type InType => typeof(TIn);
        public override Type OutType => typeof(TOut);

        public override object Process(object input)
        {
            return Process((TIn)input);
        }

        public abstract TOut Process(TIn process);
    }
}