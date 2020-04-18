using Byt3.ADL;
using Byt3.ExtPP.Base;
using Byt3.ObjectPipeline;
using Byt3.OpenFL.Common;

namespace Byt3.OpenFL.Parsing.Stages
{
    public class StaticInspectionStage : PipelineStage<LoadSourceStageResult, StaticInspectionResult>
    {
        private static readonly ADLLogger<LogType> Logger =
            new ADLLogger<LogType>(OpenFLDebugConfig.Settings, "LoadSourceStage");

        public override StaticInspectionResult Process(LoadSourceStageResult input)
        {
            Logger.Log(LogType.Log, "Statically Inspecting: " + input.Filename, 2);
            Logger.Log(LogType.Log, "Parsing Defined Scripts..", 3);
            string[] definedScripts = FLParser.FindDefineScriptsStatements(input.Source);
            Logger.Log(LogType.Log, "Parsed Scripts: " + definedScripts.Unpack(", "), 4);
            Logger.Log(LogType.Log, "Parsing Defined Buffers..", 3);
            string[] definedBuffers = FLParser.FindDefineStatements(input.Source);
            Logger.Log(LogType.Log, "Parsed Scripts: " + definedBuffers.Unpack(", "), 4);
            Logger.Log(LogType.Log, "Parsing Defined Functions..", 3);
            string[] functions = FLParser.FindFunctionHeaders(input.Source);
            Logger.Log(LogType.Log, "Parsed Scripts: " + functions.Unpack(", "), 4);
            return new StaticInspectionResult(input.Instance, input.Filename, input.Source, functions, definedBuffers,
                definedScripts);
        }
    }
}