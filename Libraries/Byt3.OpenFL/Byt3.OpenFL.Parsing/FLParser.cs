using System;
using System.Collections.Generic;
using System.Linq;
using Byt3.ADL;
using Byt3.ExtPP.API;
using Byt3.ObjectPipeline;
using Byt3.OpenFL.Common;
using Byt3.OpenFL.Common.Buffers.BufferCreators;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;
using Byt3.OpenFL.Common.Instructions.InstructionCreators;
using Byt3.OpenFL.Common.ProgramChecks;
using Byt3.OpenFL.Parsing.ExtPP.API.Configurations;
using Byt3.OpenFL.Parsing.Stages;

namespace Byt3.OpenFL.Parsing
{
    public class FLParser : Pipeline<FLParserInput, SerializableFLProgram>
    {
        public BufferCreator BufferCreator { get; }
        public FLInstructionSet InstructionSet { get; }
        public FLProgramCheckPipeline CheckPipeline { get; }

        static FLParser()
        {
            TextProcessorAPI.Configs[".fl"] = new FLPreProcessorConfig();
        }

        private static readonly ADLLogger<LogType> Logger =
            new ADLLogger<LogType>(OpenFLDebugConfig.Settings, "FLParser");


        public FLParser(FLInstructionSet instructionSet, BufferCreator bufferCreator,
            FLProgramCheckPipeline checkPipeline)
        {
            CheckPipeline = checkPipeline;
            InstructionSet = instructionSet;
            BufferCreator = bufferCreator;
            AddSubStage(new LoadSourceStage());
            AddSubStage(new StaticInspectionStage());
            AddSubStage(new ParseTreeStage(this));
            AddSubStage(CheckPipeline);


            Verify();
        }


        public FLParser(FLInstructionSet instructionSet, BufferCreator bufferCreator) : this(instructionSet,
            bufferCreator, new FLProgramCheckPipeline(instructionSet, bufferCreator))
        {
        }

        public FLParser() : this(new FLInstructionSet(), new BufferCreator())
        {
        }


        internal static string[] FindDefineStatements(string[] source)
        {
            List<string> ret = source.Where(IsDefineStatement).ToList();
            ret.Add("--define texture in:");
            return ret.ToArray();
        }

        internal static string[] FindDefineScriptsStatements(string[] source)
        {
            return source.Where(IsDefineScriptStatement).ToArray();
        }

        internal static string[] FindFunctionHeaders(string[] source)
        {
            return source.Where(IsFunctionHeader).Select(x =>
            {
                string r = RemoveComment(x);
                r = r.Remove(r.Length - 1, 1);
                return r;
            }).ToArray();
        }

        internal static bool IsDefineStatement(string line)
        {
            return !IsComment(line) && line.StartsWith("--define texture");
        }


        internal static string GetScriptName(string definedScriptLine)
        {
            return RemoveComment(definedScriptLine).Split(new[] {':'}, StringSplitOptions.RemoveEmptyEntries)[0]
                .Replace("--define script", "").Trim();
        }

        internal static string GetScriptPath(string definedScriptLine)
        {
            return RemoveComment(definedScriptLine).Split(new[] {':'}, StringSplitOptions.RemoveEmptyEntries)[1].Trim()
                .Replace("\"", "");
        }

        internal static string GetBufferName(string definedBufferLine)
        {
            return RemoveComment(definedBufferLine).Split(new[] {':'}, StringSplitOptions.RemoveEmptyEntries)[0]
                .Replace("--define texture", "").Trim();
        }

        internal static bool IsDefineScriptStatement(string line)
        {
            return !IsComment(line) && line.StartsWith("--define script");
        }

        internal static bool IsComment(string line)
        {
            return line.StartsWith("#");
        }


        internal static string RemoveComment(string line)
        {
            return line.Split(new[] {'#'}, StringSplitOptions.None).First().Trim();
        }

        internal static bool IsFunctionHeader(string line)
        {
            return !IsComment(line) && RemoveComment(line).EndsWith(":");
        }

        internal static string[] GetFunctionBody(string functionHeader, string[] source)
        {
            int index = source.ToList().IndexOf(functionHeader + ":");
            List<string> ret = new List<string>();
            for (int i = index + 1; i < source.Length; i++)
            {
                if (IsFunctionHeader(source[i]))
                {
                    break;
                }

                ret.Add(source[i].Trim());
            }

            return ret.ToArray();
        }
    }
}