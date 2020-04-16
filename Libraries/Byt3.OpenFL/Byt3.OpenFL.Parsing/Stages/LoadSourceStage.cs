﻿using System.Collections.Generic;
using Byt3.ADL;
using Byt3.ExtPP.API;
using Byt3.ExtPP.Base;
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
        private static readonly ADLLogger<LogType> Logger = new ADLLogger<LogType>(OpenFLDebugConfig.Settings, "LoadSourceStage");
        public override LoadSourceStageResult Process(FLParserInput input)
        {
            Logger.Log(LogType.Log, "Loading Source: "+ input.Filename, 2);
            return new LoadSourceStageResult(input.Instance,input.Filename,
                TextProcessorAPI.PreprocessLines(input.Filename, new Dictionary<string, bool>()));
        }
    }
}