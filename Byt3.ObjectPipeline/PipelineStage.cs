using System;
using System.Collections.Generic;
using System.Linq;

namespace Byt3.ObjectPipeline
{
    public abstract class PipelineStage
    {

        public Type InType { get; protected set; }
        public Type OutType { get; protected set; }
        protected PipelineStage(Type inType, Type outType)
        {
            InType = inType;
            OutType = outType;
        }
        public abstract object Process(object input);
    }

    public abstract class PipelineStage<StageBase> : PipelineStage
        where StageBase : PipelineStage
    {

        protected PipelineStage(Type inType, Type outType) : base(inType, outType) { }


        protected bool Verified { get; private set; }
        protected List<StageBase> Stages = new List<StageBase>();

        public void AddSubStage(StageBase stage)
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
                PipelineStage last = Stages.Last();
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
                if (Stages[i] is Pipeline)
                    ret &= (Stages[i] as Pipeline).Verify();
            }

            Verified = ret && Stages.Last().OutType == OutType;
            return Verified;
        }
    }


    public abstract class PipelineStage<I, O> : PipelineStage
    {
        protected PipelineStage() : base(typeof(I), typeof(O)) { }

        public override object Process(object input)
        {
            return Process((I)input);
        }


        public abstract O Process(I input);
    }
}
