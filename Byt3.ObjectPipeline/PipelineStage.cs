using System;

namespace Byt3.ObjectPipeline
{
    public abstract class PipelineStage<I, O> : InternalPipelineStage
    {
        internal override Type InType => typeof(I);
        internal override Type OutType => typeof(O);

        internal override object Process(object input)
        {
            return Process((I)input);
        }


        public abstract O Process(I input);
    }
}
