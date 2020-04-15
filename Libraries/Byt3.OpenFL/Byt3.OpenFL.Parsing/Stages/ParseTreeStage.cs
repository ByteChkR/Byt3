using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Byt3.ObjectPipeline;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenFL.Parsing.DataObjects;
using Byt3.OpenFL.Parsing.Instructions;

namespace Byt3.OpenFL.Parsing.Stages
{
    public class ParseTreeStage : PipelineStage<StaticInspectionResult, ParseTreeStageResult>
    {


        public override ParseTreeStageResult Process(StaticInspectionResult input)
        {
            Dictionary<string, FunctionObject> scripts = ParseScriptDefines(input.Instance, input.Filename, input.DefinedScripts);
            Dictionary<string, FLBufferInfo> definedBuffers = ParseDefinedBuffers(input.Instance, input.Filename, input.DefinedBuffers);
            FunctionObject[] functions = ParseFunctions(input.Functions, input.DefinedBuffers, input.DefinedScripts, input.Source);
            return new ParseTreeStageResult(input.Instance, input.Filename, input.Source, scripts, definedBuffers,
                functions);
        }

        private static Dictionary<string, FunctionObject> ParseScriptDefines(CLAPI instance, string path, string[] statements)
        {
            Dictionary<string, FunctionObject> ret = new Dictionary<string, FunctionObject>();
            string dir = Path.GetDirectoryName(path);
            for (int i = 0; i < statements.Length; i++)
            {
                string name = FLParser.GetScriptName(statements[i]);
                string relPath = FLParser.GetScriptPath(statements[i]);
                string p = relPath;


                FLParseResult ps = FLParser.Parse(new FLParserInput(p, instance));
                ret.Add(name, ps.EntryPoint);
            }

            return ret;
        }

        private static FunctionObject[] ParseFunctions(string[] functionHeaders, string[] definedBuffers, string[] definedScripts,
            string[] source)
        {
            FunctionObject[] functions = new FunctionObject[functionHeaders.Length];
            for (int i = 0; i < functionHeaders.Length; i++)
            {
                functions[i] = ParseFunctionObject(functionHeaders, definedBuffers, definedScripts, functionHeaders[i],
                   FLParser.GetFunctionBody(functionHeaders[i], source));
            }

            return functions;
        }

        private static FunctionObject ParseFunctionObject(string[] functionHeaders, string[] definedBuffers, string[] definedScripts,
            string name, string[] functionPart)
        {
            List<Instruction> instructions = ParseInstructions(functionHeaders, definedBuffers, definedScripts, functionPart);
            return new FunctionObject(name, instructions);
        }

        private static List<Instruction> ParseInstructions(string[] functionHeaders, string[] definedBuffers, string[] definedScripts,
            string[] functionBody)
        {
            List<Instruction> instructions = new List<Instruction>();
            for (int i = 0; i < functionBody.Length; i++)
            {
                if (!FLParser.IsComment(functionBody[i]) && !FLParser.IsDefineScriptStatement(functionBody[i]) &&
                    !FLParser.IsDefineStatement(functionBody[i]))
                {
                    Instruction inst =
                        ParseInstruction(functionHeaders, definedBuffers, definedScripts, functionBody[i]);
                    if (inst != null)
                        instructions.Add(inst);
                }
            }

            return instructions;
        }

        private static Instruction ParseInstruction(string[] functionHeaders, string[] definedBuffers, string[] definedScripts,
            string instruction)
        {
            if (instruction == "") return null;
            string[] parts = instruction.Split(new[] { ' ' }, StringSplitOptions.None);
            string inst = parts[0];

            List<InstructionArgument> args = new List<InstructionArgument>();
            for (int i = 1; i < parts.Length; i++)
            {
                if (FLParser.IsComment(parts[i])) break;
                args.Add(ParseInstructionArgument(functionHeaders, definedBuffers, definedScripts, parts[i]));
            }

            Instruction ret = null;

            if (FLParser.FLInstructions.ContainsKey(inst))
            {
                ret = (Instruction)Activator.CreateInstance(FLParser.FLInstructions[inst], new object[] { args });
            }
            else
            {
                ret = new KernelInstruction(inst, args.ToList());
            }
            return ret;
        }


        private static InstructionArgument ParseInstructionArgument(string[] functionHeaders, string[] definedBuffers, string[] definedScripts,
            string argument)
        {
            if (decimal.TryParse(argument, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal value))
            {
                return new InstructionArgument(value);
            }

            if (functionHeaders.Contains(argument))
            {
                return new InstructionArgument(new UnresolvedFunction(argument, false));
            }

            if (definedBuffers.Select(FLParser.GetBufferName).Contains(argument))
            {
                return new InstructionArgument(new UnresolvedDefinedBuffer(argument));
            }

            if (definedScripts.Select(FLParser.GetScriptName).Contains(argument))
            {
                return new InstructionArgument(new UnresolvedFunction(argument, true));
            }

            throw new InvalidOperationException("Can not parse argument: " + argument);
        }







        private static Dictionary<string, FLBufferInfo> ParseDefinedBuffers(CLAPI instance, string path,
            string[] defineStatements)
        {
            Dictionary<string, FLBufferInfo> definedBuffers = new Dictionary<string, FLBufferInfo>();
            string dir = Path.GetDirectoryName(path);
            for (int i = 0; i < defineStatements.Length; i++)
            {
                string[] data = defineStatements[i].Replace("--define texture", "")
                    .Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                if (data[0].Trim() == "in")
                {
                    FLBufferInfo bi = new UnloadedFLBufferInfo("INPUT");
                    bi.SetKey("in");
                    definedBuffers.Add(data[0].Trim(), bi);
                    continue;
                }

                string paramPart = data[1].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0];
                if (paramPart == "wfc" || paramPart == "wfcf")
                {
                    FLBufferInfo ii = WFCDefineTexture.ComputeWFC(instance, data[1].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
                    ii.SetKey(data[0].Trim());
                    definedBuffers.Add(ii.DefinedBufferName, ii);
                }
                else if (paramPart == "rnd")
                {
                    FLBufferInfo ii = WFCDefineTexture.ComputeRnd(data[1].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
                    ii.SetKey(data[0].Trim());
                    definedBuffers.Add(ii.DefinedBufferName, ii);
                }
                else if (paramPart == "urnd")
                {
                    FLBufferInfo ii = WFCDefineTexture.ComputeUrnd(data[1].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
                    ii.SetKey(data[0].Trim());
                    definedBuffers.Add(ii.DefinedBufferName, ii);
                }
                else
                {
                    FLBufferInfo bi = new UnloadedFLBufferInfo(data[1].Trim().Replace("\"", ""));
                    bi.SetKey(data[0].Trim());
                    definedBuffers.Add(data[0].Trim(), bi);
                }
            }

            return definedBuffers;
        }
    }
}