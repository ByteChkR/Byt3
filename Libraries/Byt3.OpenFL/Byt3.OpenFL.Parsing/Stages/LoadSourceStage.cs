using System.Collections.Generic;
using System.Linq;
using Byt3.ADL;
using Byt3.ExtPP.API;
using Byt3.ObjectPipeline;
using Byt3.OpenFL.Common;
using Byt3.OpenFL.Parsing.StageResults;

namespace Byt3.OpenFL.Parsing.Stages
{
    public class LoadSourceStage : PipelineStage<FLParserInput, LoadSourceStageResult>
    {
        private static readonly ADLLogger<LogType> Logger =
            new ADLLogger<LogType>(OpenFLDebugConfig.Settings, "LoadSourceStage");

        public override LoadSourceStageResult Process(FLParserInput input)
        {
            Logger.Log(LogType.Log, "Loading Source: " + input.Filename, 1);
            return new LoadSourceStageResult(input.Filename,
                TextProcessorAPI.PreprocessLines(input.Filename, new Dictionary<string, bool>()).ToList());
        }
    }
}