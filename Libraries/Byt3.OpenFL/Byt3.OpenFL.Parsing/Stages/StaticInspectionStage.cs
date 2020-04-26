using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Byt3.ADL;
using Byt3.ExtPP.Base;
using Byt3.ObjectPipeline;
using Byt3.OpenFL.Common;
using Byt3.OpenFL.Parsing.StageResults;

namespace Byt3.OpenFL.Parsing.Stages
{

    public class StaticInspectionStage : PipelineStage<LoadSourceStageResult, StaticInspectionResult>
    {
        
        private static readonly ADLLogger<LogType> Logger =
            new ADLLogger<LogType>(OpenFLDebugConfig.Settings, "StaticInspectionStage");

        private readonly FLParser parser;

        public StaticInspectionStage(FLParser parserInstance)
        {
            parser = parserInstance;
        }

        public override StaticInspectionResult Process(LoadSourceStageResult input)
        {
            string[] definedScripts = null;
            string[] definedBuffers = null;
            List<StaticFunction> functions = null;


            Logger.Log(LogType.Log, "Statically Inspecting: " + input.Filename, 2);

            Task<string[]> scriptTask = new Task<string[]>(() => FLParser.FindDefineScriptsStatements(input.Source));
            Task<string[]> bufferTask = new Task<string[]>(() => FLParser.FindDefineStatements(input.Source));

            if (parser.WorkItemRunnerSettings.UseMultithread)
            {
                scriptTask.Start();
                bufferTask.Start();
            }
            else
            {
                scriptTask.RunSynchronously();
                bufferTask.RunSynchronously();
            }

            string[] functionsHeaders = FLParser.FindFunctionHeaders(input.Source);

            functions = WorkItemRunner.RunInWorkItems(functionsHeaders.ToList(),
                 (list, start, count) => ParseFunctionTask(list, start, count, input.Source),
                 parser.WorkItemRunnerSettings);


            Task.WaitAll(scriptTask, bufferTask);
            Logger.Log(LogType.Log, "Buffer And Script Task Finished.", 2);
            definedScripts = scriptTask.Result;
            definedBuffers = bufferTask.Result;
            

            Logger.Log(LogType.Log, "Tasks Completed.", 2);
            

            Logger.Log(LogType.Log, "Parsed Scripts: " + functions.Unpack(", "), 4);
            return new StaticInspectionResult(input.Filename, input.Source, functions, definedBuffers,
                definedScripts);
        }


        private List<StaticFunction> ParseFunctionTask(List<string> headers, int start, int count, List<string> source)
        {
            List<StaticFunction> ret = new List<StaticFunction>();

            for (int i = start; i < start + count; i++)
            {
                ret.Add(new StaticFunction(headers[i], FLParser.GetFunctionBody(headers[i], source)));
            }

            return ret;
        }
    }
}