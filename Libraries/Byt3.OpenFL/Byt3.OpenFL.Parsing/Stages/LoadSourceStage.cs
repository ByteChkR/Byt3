using System.Collections.Generic;
using System.Linq;
using Byt3.ADL;
using Byt3.ExtPP.API;
using Byt3.ObjectPipeline;
using Byt3.OpenFL.Common;
using Byt3.OpenFL.Common.Parsing.StageResults;

namespace Byt3.OpenFL.Parsing.Stages
{
    public class LoadSourceStage : PipelineStage<FLParserInput, LoadSourceStageResult>
    {
        private static readonly ADLLogger<LogType> Logger =
            new ADLLogger<LogType>(OpenFLDebugConfig.Settings, "LoadSourceStage");

        public override LoadSourceStageResult Process(FLParserInput input)
        {
            if (input.Source != null)
            {
                return new LoadSourceStageResult(input.Filename, input.Source.ToList(), input.MainFile);
            }

            Logger.Log(LogType.Log, "Loading Source: " + input.Filename, 1);

            Dictionary<string, bool> defines = input.Defines;


            return new LoadSourceStageResult(input.Filename,
                TextProcessorAPI.PreprocessLines(input.Filename, defines).ToList(), input.MainFile);
        }
    }
}