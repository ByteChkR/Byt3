using System;

namespace Byt3.ObjectPipeline
{
    public abstract class PipelineStage<I, O> : PipelineStage
    {
        public override Type InType => typeof(I);
        public override Type OutType => typeof(O);

        public override object Process(object input)
        {
            return Process((I)input);
        }


        public abstract O Process(I input);
    }
}
