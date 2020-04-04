using System;

namespace Byt3.ObjectPipeline
{
    public sealed class Pipeline<InType, OutType> : PipelineStage<InType, OutType>
    {

        public Pipeline(params PipelineStage[] stages)
        {
            for (int i = 0; i < stages.Length; i++)
            {
                AddSubStage(stages[i]);
            }
        }

        public override OutType Process(InType input)
        {
            if (input == null) throw new ArgumentNullException("input", "Argument is not allowed to be null.");
            if (!Verified && !Verify()) throw new PipelineNotValidException(this, "Can not use a Pipline that is incomplete.");
            object currentIn = input;
            foreach (PipelineStage internalPipelineStage in Stages)
            {
                currentIn = internalPipelineStage.Process(currentIn);
            }

            return (OutType)currentIn;


        }

    }
}