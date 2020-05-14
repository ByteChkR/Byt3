using System;
using System.Collections.Generic;
using System.Diagnostics;
using Byt3.ADL;
using Byt3.ADL.Configs;
using Byt3.ExtPP.API;
using Byt3.ObjectPipeline;
using Byt3.OpenFL.Common;
using Byt3.OpenFL.Common.Buffers.BufferCreators;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;
using Byt3.OpenFL.Common.Instructions.InstructionCreators;
using Byt3.OpenFL.Parsing.ExtPP.API.Configurations;
using Byt3.OpenFL.Parsing.Stages;
using Byt3.Utilities.FastString;

namespace Byt3.OpenFL.Parsing
{
    public class FLParser : Pipeline
    {
        public WorkItemRunnerSettings WorkItemRunnerSettings { get; }
        public BufferCreator BufferCreator { get; }
        public FLInstructionSet InstructionSet { get; }

        private static readonly ADLLogger<LogType> Logger =
            new ADLLogger<LogType>(OpenFLDebugConfig.Settings, "FLParserPipeline");

        public const string DEFINE_SCRIPT_KEY = "--define script";
        public const string DEFINE_TEXTURE_KEY = "--define texture";
        public const string DEFINE_ARRAY_KEY = "--define array";

        static FLParser()
        {
            TextProcessorAPI.Configs[".fl"] = new FLPreProcessorConfig();
        }



        public FLParser(FLInstructionSet instructionSet, BufferCreator bufferCreator,
            WorkItemRunnerSettings settings = null) : base(typeof(FLParserInput), typeof(SerializableFLProgram))
        {
            InstructionSet = instructionSet;
            BufferCreator = bufferCreator;
            WorkItemRunnerSettings = settings ?? WorkItemRunnerSettings.Default;
            AddSubStage(new LoadSourceStage());
            AddSubStage(new RemoveCommentStage(this));
            AddSubStage(new StaticInspectionStage(this));
            AddSubStage(new ParseTreeStage(this));


            Verify();
        }

        public FLParser() : this(new FLInstructionSet(), new BufferCreator())
        {
        }

        public SerializableFLProgram Process(FLParserInput input)
        {
            return (SerializableFLProgram)Process((object)input);
        }


        internal static string[] FindDefineStatements(List<string> source)
        {
            List<string> ret = new List<string>();
            ret.AddRange(FindDefineArrayBuffers(source));
            for (int i = 0; i < source.Count; i++)
            {
                if (IsDefineStatement(source[i]))
                {
                    ret.Add(source[i]);
                }
            }

            ret.Add("--define texture in:");
            return ret.ToArray();
        }

        internal static string[] FindDefineArrayBuffers(List<string> source)
        {
            List<string> ret = new List<string>();
            for (int i = 0; i < source.Count; i++)
            {
                if (IsDefineArrayBuffer(source[i]))
                {
                    ret.Add(source[i]);
                }
            }

            return ret.ToArray();
        }

        internal static string[] FindDefineScriptsStatements(List<string> source)
        {
            List<string> ret = new List<string>();
            for (int i = 0; i < source.Count; i++)
            {
                if (IsDefineScriptStatement(source[i]))
                {
                    ret.Add(source[i]);
                }
            }

            return ret.ToArray();
        }

        internal static string[] FindFunctionHeaders(List<string> source)
        {
            List<string> ret = new List<string>();
            for (int i = 0; i < source.Count; i++)
            {
                if (IsFunctionHeader(source[i]))
                {
                    ret.Add(source[i].Remove(source[i].Length - 1, 1));
                }
            }

            return ret.ToArray();
        }


        internal static string GetScriptPath(string definedScriptLine)
        {
            return GetPath(ref definedScriptLine).Replace("\"", string.Empty);
        }

        private static string GetPath(ref string line)
        {
            int idx = FString.FastIndexOf(ref line, ":") + 1;
            return line.Substring(idx, line.Length - idx).Trim();
        }

        internal static string GetScriptName(string definedScriptLine)
        {
            return GetName(ref definedScriptLine, DEFINE_SCRIPT_KEY);
        }

        internal static string GetBufferName(string definedBufferLine)
        {
            return GetName(ref definedBufferLine, DEFINE_TEXTURE_KEY);
        }

        internal static string GetBufferArrayName(string definedBufferLine)
        {
            return GetName(ref definedBufferLine, DEFINE_ARRAY_KEY);
        }

        private static string GetName(ref string line, string key)
        {
            int len = FString.FastIndexOf(ref line, ":") - key.Length;
            return line.Substring(key.Length, len).TrimStart();
        }

        internal static bool IsDefineArrayBuffer(string line)
        {
            return FString.FastIndexOf(ref line, DEFINE_ARRAY_KEY) == 0;
        }

        internal static bool IsDefineStatement(string line)
        {
            return FString.FastIndexOf(ref line, DEFINE_TEXTURE_KEY) == 0;
        }

        internal static bool IsDefineScriptStatement(string line)
        {
            return FString.FastIndexOf(ref line, DEFINE_SCRIPT_KEY) == 0;
        }


        internal static bool IsFunctionHeader(string line)
        {
            if (line.Length == 0)
            {
                return false;
            }

            return line[line.Length - 1] == ':';
        }

        internal static string[] GetFunctionBody(string functionHeader, List<string> source)
        {
            int index = source.IndexOf(functionHeader + ":");
            List<string> ret = new List<string>();
            for (int i = index + 1; i < source.Count; i++)
            {
                if (IsFunctionHeader(source[i]) ||
                    IsDefineScriptStatement(source[i]) ||
                    IsDefineStatement(source[i]))
                {
                    break;
                }

                ret.Add(source[i].Trim());
            }

            return ret.ToArray();
        }

        public override object Process(object input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input", "Argument is not allowed to be null.");
            }

            if (!Verified && !Verify())
            {
                throw new PipelineNotValidException(this, "Can not use a Pipline that is incomplete.");
            }

            object currentIn = input;
            Stopwatch sw = new Stopwatch();
            Tuple<string, long>[] timing = new Tuple<string, long>[Stages.Count];
            long totalTime = 0;
            for (int i = 0; i < Stages.Count; i++)
            {
                PipelineStage internalPipelineStage = Stages[i];
                sw.Start();
                currentIn = internalPipelineStage.Process(currentIn);
                timing[i] = new Tuple<string, long>(internalPipelineStage.GetType().Name, sw.ElapsedMilliseconds);
                totalTime += sw.ElapsedMilliseconds;
                sw.Reset();
                //Logger.Log(LogType.Log, $"Stage {timing[i].Item1} finished in {timing[i].Item2.ToString()} ms", 2);
            }

            //Logger.Log(LogType.Log, $"_______________________________________________", 1);
            //Logger.Log(LogType.Log, $"Total: {totalTime} ms", 1);
            return currentIn;
        }
    }
}