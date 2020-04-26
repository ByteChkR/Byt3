using System.Collections.Generic;
using Byt3.ADL;
using Byt3.ObjectPipeline;
using Byt3.OpenFL.Common;
using Byt3.OpenFL.Parsing.StageResults;
using Byt3.Utilities.FastString;

namespace Byt3.OpenFL.Parsing.Stages
{
    public class RemoveCommentStage : PipelineStage<LoadSourceStageResult, LoadSourceStageResult>
    {
        private static readonly ADLLogger<LogType> Logger =
            new ADLLogger<LogType>(OpenFLDebugConfig.Settings, "LoadSourceStage");

        private readonly FLParser parser;

        public RemoveCommentStage(FLParser parserInstance)
        {
            parser = parserInstance;
        }

        private static readonly string COMMENT_PREFIX = "#";

        public override LoadSourceStageResult Process(LoadSourceStageResult input)
        {
            Logger.Log(LogType.Log, "Removing Comments.. ", 2);


            WorkItemRunner.RunInWorkItems(input.Source, RemoveCommentTask, parser.WorkItemRunnerSettings);


            Logger.Log(LogType.Log, $"Optimizing Script Length..", 2);
            for (int i = input.Source.Count - 1; i >= 0; i--)
            {
                if (string.IsNullOrWhiteSpace(input.Source[i]))
                {
                    input.Source.RemoveAt(i);
                    continue;
                }

                input.Source[i] = input.Source[i].Trim();
            }

            return input;
        }


        private void RemoveCommentTask(List<string> input, int start, int count)
        {
            for (int i = start; i < start + count; i++)
            {
                input[i] = input[i].Trim();

                int idx = FString.FastIndexOf(input[i], COMMENT_PREFIX);

                if (idx == 0)
                {
                    input[i] = string.Empty;
                }
                else if (idx > 0)
                {
                    input[i] = input[i].Substring(0, idx).Trim();
                }
            }
        }
    }
}