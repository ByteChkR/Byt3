using Byt3.ObjectPipeline;
using Byt3.OpenFL.Common.Buffers.BufferCreators;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;
using Byt3.OpenFL.Common.Instructions.InstructionCreators;

namespace Byt3.OpenFL.Common.ProgramChecks
{
    public abstract class FLProgramCheck : PipelineStage<SerializableFLProgram, SerializableFLProgram>
    {
        protected FLInstructionSet InstructionSet { get; private set; }
        protected BufferCreator BufferCreator { get; private set; }

        internal void SetValues(FLInstructionSet iset, BufferCreator bc)
        {
            InstructionSet = iset;
            BufferCreator = bc;
        }
    }
}