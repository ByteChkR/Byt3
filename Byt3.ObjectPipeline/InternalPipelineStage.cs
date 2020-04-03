using System;
using System.Collections.Generic;
using System.Linq;

namespace Byt3.ObjectPipeline
{
    public abstract class InternalPipelineStage
    {
        internal abstract Type InType { get; }
        internal abstract Type OutType { get; }

        internal abstract object Process(object input);

        protected bool Verified { get; private set; }
        protected List<InternalPipelineStage> Stages = new List<InternalPipelineStage>();

        public void AddSubStage(InternalPipelineStage stage)
        {
            if (stage == null) throw new ArgumentNullException("stage", "Argument is not allowed to be null.");
            Verified = false;
            if (Stages.Count == 0)
            {
                if (stage.InType != InType)
                    throw new PipelineNotValidException(this,
                        $"Can not Add stage with in type {stage.InType} as first element in the Pipeline. it has to be the same type as the pipeline in type({InType})");
                else
                {
                    Stages.Add(stage);
                }
            }
            else
            {
                InternalPipelineStage last = Stages.Last();
                if (last.OutType != stage.InType)
                {
                    throw new PipelineNotValidException(this,
                        $"Can not Add stage with in type {stage.InType} in the pipeline. it has to be the same type as the previous pipeline out type({last.OutType})");
                }
                else
                {
                    Stages.Add(stage);
                }
            }
        }

        public bool Verify()
        {
            if (Stages.Count == 0)
            {
                Verified = true;
                return true;
            }


            bool ret = true;

            for (int i = 0; i < Stages.Count; i++)
            {
                ret &= Stages[i].Verify();
            }

            Verified = ret && Stages.Last().OutType == OutType;
            return Verified;
        }

    }
}