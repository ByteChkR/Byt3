using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Byt3.ADL;
using Byt3.CommandRunner;
using Byt3.CommandRunner.SetSettings;
using Byt3.ObjectPipeline;
using Byt3.OpenFL.Common;
using Byt3.OpenFL.Common.Exceptions;
using Byt3.OpenFL.Common.Parsing;
using Byt3.OpenFL.Common.Parsing.StageResults;
using Byt3.OpenFL.Parsing.StageResults;
using Byt3.Utilities.FastString;

namespace Byt3.OpenFL.Parsing.Stages
{
    internal class StaticFunctionHeader
    {
        public readonly string FunctionName;
        public readonly string[] Modifiers;

        public StaticFunctionHeader(string functionHeader)
        {
            string[] f = functionHeader.Split(new[] {':'}, StringSplitOptions.None);
            if (f.Length == 1)
            {
                throw new FLInvalidFunctionUseException(functionHeader, "Invalid line.");
            }

            FunctionName = f[0];
            Modifiers = f[1].Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
        }
    }

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
            ImportOptions options = new ImportOptions();
            Runner runner = new Runner();
            runner._AddCommand(
                SetSettingsCommand.CreateSettingsCommand("Import", new[] {FLKeywords.SetParserOptionKey}, options));
            List<string> opts = FLParser.FindParserOptions(input.Source).ToList();
            opts.Insert(0, FLKeywords.SetParserOptionKey);
            runner._RunCommands(opts.ToArray());

            if (options.OnlyAllowImport && input.MainFile)
            {
                throw new InvalidOperationException(
                    $"The Script {input.Filename} can not be loaded as Entry point. As the Option: Import.OnlyAllowImport is set to true in the script");
            }

            if (options.NoImport && !input.MainFile)
            {
                throw new InvalidOperationException(
                    $"The Script {input.Filename} can not be imported. As the Option: Import.NoImport is set to true in the script");
            }


            DefineStatement[] definedScripts = null;
            DefineStatement[] definedBuffers = null;
            List<StaticFunction> functions = null;


            Logger.Log(LogType.Log, "Statically Inspecting: " + input.Filename, 1);

            Task<DefineStatement[]> scriptTask =
                new Task<DefineStatement[]>(() => FLParser.FindDefineScriptsStatements(input.Source));
            Task<DefineStatement[]> bufferTask =
                new Task<DefineStatement[]>(() => FLParser.FindDefineStatements(input.Source));

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

            StaticFunctionHeader[] functionsHeaders = FLParser.FindFunctionHeaders(input.Source)
                .Select(x => new StaticFunctionHeader(x)).ToArray();


            functions = WorkItemRunner.RunInWorkItems(functionsHeaders.ToList(),
                (list, start, count) => ParseFunctionTask(list, start, count, input.Source),
                parser.WorkItemRunnerSettings);


            Task.WaitAll(scriptTask, bufferTask);
            Logger.Log(LogType.Log, "Buffer And Script Tasks Finished.", 1);
            definedScripts = scriptTask.Result;
            definedBuffers = bufferTask.Result;


            Logger.Log(LogType.Log, "Tasks Completed.", 1);


            Logger.Log(LogType.Log, "Parsed Scripts: " + functions.Unpack(", "), 4);
            return new StaticInspectionResult(input.Filename, input.Source, functions, definedBuffers,
                definedScripts, options);
        }


        private List<StaticFunction> ParseFunctionTask(List<StaticFunctionHeader> headers, int start, int count,
            List<string> source)
        {
            List<StaticFunction> ret = new List<StaticFunction>();

            for (int i = start; i < start + count; i++)
            {
                ret.Add(new StaticFunction(headers[i].FunctionName,
                    FLParser.GetFunctionBody(headers[i].FunctionName, source), headers[i].Modifiers));
            }

            return ret;
        }
    }
}