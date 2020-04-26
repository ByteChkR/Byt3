using System.Collections.Generic;
using Byt3.ADL;
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

        private const string DEFINE_SCRIPT_KEY = "--define script";
        private const string DEFINE_TEXTURE_KEY = "--define texture";

        static FLParser()
        {
            TextProcessorAPI.Configs[".fl"] = new FLPreProcessorConfig();
        }

        private static readonly ADLLogger<LogType> Logger =
            new ADLLogger<LogType>(OpenFLDebugConfig.Settings, "FLParser");


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
            return (SerializableFLProgram) base.Process(input);
        }


        internal static string[] FindDefineStatements(List<string> source)
        {
            List<string> ret = new List<string>();
            for (int i = 0; i < source.Count; i++)
            {
                if (IsDefineStatement(source[i]))
                {
                    ret.Add(source[i]);
                }
            }

            //List<string> ret = source.Where(IsDefineStatement).ToList();
            ret.Add("--define texture in:");
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
            //return source.Where(IsDefineScriptStatement).ToArray();
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
            //return source.Where(IsFunctionHeader).Select(x =>
            //{
            //    string r = RemoveComment(x);
            //    r = r.Remove(r.Length - 1, 1);
            //    return r;
            //}).ToArray();
        }


        internal static string GetScriptPath(string definedScriptLine)
        {
            return GetPath(ref definedScriptLine).Replace("\"", string.Empty);
            //return RemoveComment(definedScriptLine).Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries)[1].Trim()
            //    .Replace("\"", "");
        }

        private static string GetPath(ref string line)
        {
            int idx = FString.FastIndexOf(ref line, ":") + 1;
            return line.Substring(idx, line.Length - idx).Trim();
        }

        internal static string GetScriptName(string definedScriptLine)
        {
            return GetName(ref definedScriptLine, DEFINE_SCRIPT_KEY);
            //return RemoveComment(definedScriptLine).Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries)[0]
            //    .Replace("--define script", "").Trim();
        }

        internal static string GetBufferName(string definedBufferLine)
        {
            return GetName(ref definedBufferLine, DEFINE_TEXTURE_KEY);
            //return definedBufferLine.Substring(DEFINE_TEXTURE_KEY.Length,
            //    FString.FastIndexOf(ref definedBufferLine, ":")- DEFINE_TEXTURE_KEY.Length);
            //return RemoveComment(definedBufferLine).Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries)[0]
            //    .Replace("--define texture", "").Trim();
        }

        private static string GetName(ref string line, string key)
        {
            int len = FString.FastIndexOf(ref line, ":") - key.Length;
            return line.Substring(key.Length, len).TrimStart();
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
    }
}