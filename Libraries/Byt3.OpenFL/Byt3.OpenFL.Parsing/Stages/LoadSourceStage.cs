using System.Collections.Generic;
using Byt3.ExtPP.API;
using Byt3.ObjectPipeline;
using Byt3.OpenCL.Wrapper;

namespace Byt3.OpenFL.Parsing.Stages
{
    public class FLParserInput
    {
        public string Filename;
        public CLAPI Instance;

        public FLParserInput(string filename, CLAPI instance)
        {
            Filename = filename;
            Instance = instance;
        }
        public FLParserInput(string filename):this(filename, CLAPI.MainThread) { }
    }

    public class LoadSourceStage : PipelineStage<FLParserInput, LoadSourceStageResult>
    {
        public override LoadSourceStageResult Process(FLParserInput input)
        {
            return new LoadSourceStageResult(input.Instance,input.Filename,
                TextProcessorAPI.PreprocessLines(input.Filename, new Dictionary<string, bool>()));
        }
    }
}