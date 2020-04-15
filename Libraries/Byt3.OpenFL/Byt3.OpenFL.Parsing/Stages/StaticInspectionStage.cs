using System.Collections.Generic;
using System.Linq;
using Byt3.ObjectPipeline;

namespace Byt3.OpenFL.Parsing.Stages
{
    public class StaticInspectionStage : PipelineStage<LoadSourceStageResult, StaticInspectionResult>
    {
        public override StaticInspectionResult Process(LoadSourceStageResult input)
        {
            string[] definedScripts = FLParser.FindDefineScriptsStatements(input.Source);
            string[] definedBuffers = FLParser.FindDefineStatements(input.Source);
            string[] functions = FLParser.FindFunctionHeaders(input.Source);
            return new StaticInspectionResult(input.Instance, input.Filename, input.Source, functions, definedBuffers, definedScripts);
        }

        
    }
}