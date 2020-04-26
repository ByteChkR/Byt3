using System.Collections.Generic;
using Byt3.ObjectPipeline;
using Byt3.OpenFL.Common.Buffers.BufferCreators;
using Byt3.OpenFL.Common.Instructions.InstructionCreators;

namespace Byt3.OpenFL.Common.ProgramChecks
{
    public class FLProgramCheckBuilder
    {
        public FLInstructionSet InstructionSet { get; }
        public BufferCreator BufferCreator { get; }

        private List<FLProgramCheck> ProgramChecks { get; }
        public bool IsAttached { get; private set; }
        public Pipeline AttachedPipeline { get; private set; }

        public static FLProgramCheckBuilder CreateDefaultCheckBuilder(FLInstructionSet iset, BufferCreator bc)
        {
            FLProgramCheckBuilder pipeline = new FLProgramCheckBuilder(iset, bc);
            pipeline.AddProgramCheck(new RemoveUnusedScriptsOptimization()); //Gets added in reverse because of insert at first valid index
            pipeline.AddProgramCheck(new RemoveUnusedBuffersOptimization()); //Gets added in reverse because of insert at first valid index
            pipeline.AddProgramCheck(new RemoveUnusedFunctionsEarlyOptimization()); //Gets added in reverse because of insert at first valid index
            pipeline.AddProgramCheck(new InstructionArgumentValidator());
            //pipeline.AddSubStage(new InstructionValidator()); ////The Parser now checks this for himself.
            pipeline.AddProgramCheck(new FilePathValidator());
            return pipeline;
        }

        public FLProgramCheckBuilder(FLInstructionSet iset, BufferCreator bc)
        {
            ProgramChecks = new List<FLProgramCheck>();
            InstructionSet = iset;
            BufferCreator = bc;
        }

        public void AddProgramCheck(FLProgramCheck check)
        {
            if (IsAttached) return;
            if (!ProgramChecks.Contains(check))
            {
                ProgramChecks.Add(check);
            }
        }

        public void RemoveProgramCheck(FLProgramCheck check)
        {
            if (IsAttached) return;
            ProgramChecks.Remove(check);
        }

        public bool Attach(Pipeline target, bool verify)
        {
            if (IsAttached) return false;
            for (int i = 0; i < ProgramChecks.Count; i++)
            {
                ProgramChecks[i].SetValues(InstructionSet, BufferCreator);
            }
            foreach (FLProgramCheck flProgramCheck in ProgramChecks)
            {
                target.InsertAtFirstValidIndex(flProgramCheck);
            }

            AttachedPipeline = target;
            IsAttached = true;
            return !verify || target.Verify();
        }

        public bool Detach(bool verify)
        {
            if (AttachedPipeline == null) return false;
            foreach (FLProgramCheck flProgramCheck in ProgramChecks)
            {
                AttachedPipeline.RemoveSubStage(flProgramCheck);
            }

            Pipeline p = AttachedPipeline;

            IsAttached = false;
            AttachedPipeline = null;
            return !verify || p.Verify();
        }
    }

    //public class FLProgramCheckPipeline : Pipeline<FLProgramCheck, FLPipelineResult, FLPipelineResult>
    //{
    //    public FLInstructionSet InstructionSet { get; }
    //    public BufferCreator BufferCreator { get; }

    //    public FLProgramCheckPipeline(FLInstructionSet iset, BufferCreator bc)
    //    {
    //        InstructionSet = iset;
    //        BufferCreator = bc;
    //    }


    //    //public static FLProgramCheckPipeline CreateDefaultCheckPipeline(FLInstructionSet iset, BufferCreator bc)
    //    //{
    //    //    FLProgramCheckPipeline pipeline = new FLProgramCheckPipeline(iset, bc);
    //    //    pipeline.AddSubStage(new RemoveUnusedFunctionsOptimization());
    //    //    pipeline.AddSubStage(new RemoveUnusedBuffersOptimization());
    //    //    pipeline.AddSubStage(new RemoveUnusedScriptsOptimization());
    //    //    pipeline.AddSubStage(new InstructionArgumentValidator());
    //    //    //pipeline.AddSubStage(new InstructionValidator()); ////The Parser now checks this for himself.
    //    //    pipeline.AddSubStage(new FilePathValidator());
    //    //    return pipeline;
    //    //}

    //    public override object Process(object input)
    //    {
    //        if (Stages.Count == 0) return input;
    //        for (int i = 0; i < Stages.Count; i++)
    //        {
    //            Stages[i].SetValues(InstructionSet, BufferCreator);
    //        }

    //        return base.Process(input);
    //    }
    //}
}