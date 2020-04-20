using Byt3.ObjectPipeline;
using Byt3.OpenFL.Common.Buffers.BufferCreators;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;
using Byt3.OpenFL.Common.Instructions.InstructionCreators;

namespace Byt3.OpenFL.Common.ProgramChecks
{
    public class FLProgramCheckPipeline : Pipeline<FLProgramCheck, SerializableFLProgram, SerializableFLProgram>
    {
        public FLInstructionSet InstructionSet { get; }
        public BufferCreator BufferCreator { get; }

        public FLProgramCheckPipeline(FLInstructionSet iset, BufferCreator bc)
        {
            InstructionSet = iset;
            BufferCreator = bc;
        }

        
        public static FLProgramCheckPipeline CreateDefaultCheckPipeline(FLInstructionSet iset, BufferCreator bc)
        {
            FLProgramCheckPipeline pipeline = new FLProgramCheckPipeline(iset, bc);
            pipeline.AddSubStage(new InstructionValidator());
            pipeline.AddSubStage(new FilePathValidator());
            return pipeline;
        }

        public override object Process(object input)
        {
            for (int i = 0; i < Stages.Count; i++)
            {
                Stages[i].SetValues(InstructionSet, BufferCreator);
            }
            return base.Process(input);
        }
    }
}