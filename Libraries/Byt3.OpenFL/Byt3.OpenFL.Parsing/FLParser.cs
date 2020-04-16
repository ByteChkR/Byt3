using System;
using System.Collections.Generic;
using System.Linq;
using Byt3.ADL;
using Byt3.ExtPP.Base;
using Byt3.ObjectPipeline;
using Byt3.OpenFL.Parsing.Instructions;
using Byt3.OpenFL.Parsing.Stages;

namespace Byt3.OpenFL.Parsing
{
    public class FLParser : Pipeline<FLParserInput, FLParseResult>
    {

        private static readonly ADLLogger<LogType> Logger = new ADLLogger<LogType>(OpenFLDebugConfig.Settings, "FLParser");


        internal static Dictionary<string, Type> FLInstructions => new Dictionary<string, Type>
        {
            {"setactive", typeof(SetActiveInstruction)},
            {"jmp", typeof(JumpInstruction) },
            {"urnd", typeof(URandomInstruction)},
            {"rnd", typeof(RandomInstruction) },
        };

        public static FLParseResult Parse(FLParserInput input)
        {
            Logger.Log(LogType.Log, "Parsing File: " + input.Filename, 1);
            FLParser parser = new FLParser();
            return parser.Process(input);
        }


        private FLParser()
        {
            AddSubStage(new LoadSourceStage());
            AddSubStage(new StaticInspectionStage());
            AddSubStage(new ParseTreeStage());
            AddSubStage(new ResolveReferencesStage());
            Verify();
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
                r=r.Remove(r.Length - 1, 1);
                return r;
            }).ToArray();
        }

        internal static bool IsDefineStatement(string line)
        {
            return !IsComment(line) && line.StartsWith("--define texture");
        }


        internal static string GetScriptName(string definedScriptLine)
        {
            return RemoveComment(definedScriptLine).Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries)[0]
                .Replace("--define script", "").Trim();
        }
        internal static string GetScriptPath(string definedScriptLine)
        {
            return RemoveComment(definedScriptLine).Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries)[1].Trim().Replace("\"", "");
        }

        internal static string GetBufferName(string definedBufferLine)
        {
            return RemoveComment(definedBufferLine).Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries)[0]
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
            return line.Split(new[] { '#' }, StringSplitOptions.None).First().Trim();
        }

        internal static bool IsFunctionHeader(string line)
        {
            string l = RemoveComment(line);
            return !IsComment(line) && RemoveComment(line).EndsWith(":");
        }

        internal static string[] GetFunctionBody(string functionHeader, string[] source)
        {
            int index = source.ToList().IndexOf(functionHeader + ":");
            List<string> ret = new List<string>();
            for (int i = index + 1; i < source.Length; i++)
            {
                if (IsFunctionHeader(source[i])) break;
                ret.Add(source[i].Trim());
            }

            return ret.ToArray();
        }
    }
}