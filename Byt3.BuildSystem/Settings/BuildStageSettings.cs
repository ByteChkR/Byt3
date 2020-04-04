using System;

namespace Byt3.BuildSystem.Settings
{
    public abstract class BuildStageSettings<T> : BuildStageSettings
    {
        public override Type BuilderStageType => typeof(T);
    }

    public abstract class BuildStageSettings
    {
        public abstract Type BuilderStageType { get; }
    }
}