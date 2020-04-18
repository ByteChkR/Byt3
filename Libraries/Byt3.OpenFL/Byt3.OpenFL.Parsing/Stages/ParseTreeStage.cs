using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Byt3.ADL;
using Byt3.ExtPP.Base;
using Byt3.ObjectPipeline;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenFL.Common;
using Byt3.OpenFL.Common.Buffers;
using Byt3.OpenFL.Common.DataObjects;
using Byt3.OpenFL.Common.Exceptions;
using Byt3.OpenFL.Common.Instructions;

namespace Byt3.OpenFL.Parsing.Stages
{
    public class ParseTreeStage : PipelineStage<StaticInspectionResult, ParseTreeStageResult>
    {
        private static readonly ADLLogger<LogType> Logger =
            new ADLLogger<LogType>(OpenFLDebugConfig.Settings, "ParseTreeStage");

        public override ParseTreeStageResult Process(StaticInspectionResult input)
        {
            Logger.Log(LogType.Log, "Parsing Tree: " + input.Filename, 2);
            Logger.Log(LogType.Log, "Creating Defined Script Nodes..", 3);
            Dictionary<string, FLFunction> scripts = ParseScriptDefines(input.Instance, input.DefinedScripts);
            Logger.Log(LogType.Log, "Script Nodes: " + scripts.Select(x => x.Key).Unpack(", "), 4);

            Logger.Log(LogType.Log, "Creating Defined Buffer Nodes..", 3);
            Dictionary<string, FLBuffer> definedBuffers = ParseDefinedBuffers(input.Instance, input.DefinedBuffers);
            Logger.Log(LogType.Log, "Buffer Nodes: " + definedBuffers.Select(x => x.Key).Unpack(", "), 4);

            Logger.Log(LogType.Log, "Creating Defined Function Nodes..", 3);
            FLFunction[] flFunctions =
                ParseFunctions(input.Functions, input.DefinedBuffers, input.DefinedScripts, input.Source);
            Logger.Log(LogType.Log, "Buffer Nodes: " + flFunctions.Select(x => x.Name).Unpack(", "), 4);
            return new ParseTreeStageResult(input.Instance, input.Filename, input.Source, scripts, definedBuffers,
                flFunctions);
        }

        public class ExternalFlFunction : FLFunction
        {
            private readonly FLProgram ExternalFunction;

            public ExternalFlFunction(string name, FLProgram external) : base(name, new List<FLInstruction>())
            {
                ExternalFunction = external;
            }

            public override void Process()
            {
                ExternalFunction.ActiveChannels = Root.ActiveChannels;
                ExternalFunction.SetCLVariables(Root.Instance, Root.KernelDB, Root.ActiveBuffer);
                ExternalFunction.EntryPoint.Process();
                Root.ActiveChannels = ExternalFunction.ActiveChannels;
                Root.ActiveBuffer = ExternalFunction.ActiveBuffer;
            }
        }

        private static Dictionary<string, FLFunction> ParseScriptDefines(CLAPI instance, string[] statements)
        {
            Dictionary<string, FLFunction> ret = new Dictionary<string, FLFunction>();
            for (int i = 0; i < statements.Length; i++)
            {
                string name = FLParser.GetScriptName(statements[i]);
                string relPath = FLParser.GetScriptPath(statements[i]);
                string p = relPath;

                if (!File.Exists(p))
                {
                    throw new FLInvalidDefineStatementException("Can not Find Script with path: " + p);
                }

                FLProgram ps = FLParser.Parse(new FLParserInput(p, instance));
                ret.Add(name, new ExternalFlFunction(name, ps));
            }

            return ret;
        }

        private static FLFunction[] ParseFunctions(string[] functionHeaders, string[] definedBuffers,
            string[] definedScripts,
            string[] source)
        {
            FLFunction[] flFunctions = new FLFunction[functionHeaders.Length];
            for (int i = 0; i < functionHeaders.Length; i++)
            {
                flFunctions[i] = ParseFunctionObject(functionHeaders, definedBuffers, definedScripts, functionHeaders[i],
                    FLParser.GetFunctionBody(functionHeaders[i], source));
            }

            return flFunctions;
        }

        private static FLFunction ParseFunctionObject(string[] functionHeaders, string[] definedBuffers,
            string[] definedScripts,
            string name, string[] functionPart)
        {
            List<FLInstruction> instructions =
                ParseInstructions(functionHeaders, definedBuffers, definedScripts, functionPart);
            return new FLFunction(name, instructions);
        }

        private static List<FLInstruction> ParseInstructions(string[] functionHeaders, string[] definedBuffers,
            string[] definedScripts,
            string[] functionBody)
        {
            List<FLInstruction> instructions = new List<FLInstruction>();
            for (int i = 0; i < functionBody.Length; i++)
            {
                if (!FLParser.IsComment(functionBody[i]) && !FLParser.IsDefineScriptStatement(functionBody[i]) &&
                    !FLParser.IsDefineStatement(functionBody[i]))
                {
                    FLInstruction inst =
                        ParseInstruction(functionHeaders, definedBuffers, definedScripts, functionBody[i]);
                    if (inst != null)
                    {
                        instructions.Add(inst);
                    }
                }
            }

            return instructions;
        }

        private static FLInstruction ParseInstruction(string[] functionHeaders, string[] definedBuffers,
            string[] definedScripts,
            string instruction)
        {
            if (instruction == "")
            {
                return null;
            }

            string[] parts = instruction.Split(new[] {' '}, StringSplitOptions.None);
            string inst = parts[0];

            List<FLInstructionArgument> args = new List<FLInstructionArgument>();
            for (int i = 1; i < parts.Length; i++)
            {
                if (FLParser.IsComment(parts[i]))
                {
                    break;
                }

                args.Add(ParseInstructionArgument(functionHeaders, definedBuffers, definedScripts, parts[i]));
            }

            FLInstruction ret = null;

            if (FLParser.FLInstructions.ContainsKey(inst))
            {
                ret = (FLInstruction) Activator.CreateInstance(FLParser.FLInstructions[inst], new object[] {args});
            }
            else
            {
                ret = new KernelFLInstruction(inst, args.ToList());
            }

            return ret;
        }


        private static FLInstructionArgument ParseInstructionArgument(string[] functionHeaders, string[] definedBuffers,
            string[] definedScripts,
            string argument)
        {
            if (decimal.TryParse(argument, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture,
                out decimal value))
            {
                return new FLInstructionArgument(value);
            }

            if (functionHeaders.Contains(argument))
            {
                return new FLInstructionArgument(new FLUnresolvedFunction(argument, false));
            }

            if (definedBuffers.Select(FLParser.GetBufferName).Contains(argument))
            {
                return new FLInstructionArgument(new FLUnresolvedDefinedBuffer(argument));
            }

            if (definedScripts.Select(FLParser.GetScriptName).Contains(argument))
            {
                return new FLInstructionArgument(new FLUnresolvedFunction(argument, true));
            }

            throw new InvalidOperationException("Can not parse argument: " + argument);
        }


        private static Dictionary<string, FLBuffer> ParseDefinedBuffers(CLAPI instance, string[] defineStatements)
        {
            Dictionary<string, FLBuffer> definedBuffers = new Dictionary<string, FLBuffer>();
            for (int i = 0; i < defineStatements.Length; i++)
            {
                string[] data = defineStatements[i].Replace("--define texture", "")
                    .Split(new[] {':'}, StringSplitOptions.RemoveEmptyEntries);
                if (data[0].Trim() == "in")
                {
                    FLBuffer bi = new LazyFromFileFLBuffer("INPUT");
                    bi.SetKey("in");
                    definedBuffers.Add(data[0].Trim(), bi);
                    continue;
                }

                string paramPart = data[1].Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries)[0];
                if (paramPart == "wfc" || paramPart == "wfcf")
                {
                    FLBuffer ii = WFCDefineTexture.ComputeWFC(instance,
                        data[1].Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries));
                    ii.SetKey(data[0].Trim());
                    definedBuffers.Add(ii.DefinedBufferName, ii);
                }
                else if (paramPart == "rnd")
                {
                    FLBuffer ii =
                        RandomFLInstruction.ComputeRnd(data[1].Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries));
                    ii.SetKey(data[0].Trim());
                    definedBuffers.Add(ii.DefinedBufferName, ii);
                }
                else if (paramPart == "urnd")
                {
                    FLBuffer ii =
                        URandomFLInstruction.ComputeUrnd(
                            data[1].Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries));
                    ii.SetKey(data[0].Trim());
                    definedBuffers.Add(ii.DefinedBufferName, ii);
                }
                else if (paramPart == "empty")
                {
                    FLBuffer ii = new LazyLoadingFLBuffer(root =>
                        new FLBuffer(root.Instance, root.Dimensions.x, root.Dimensions.y));
                    ii.SetKey(data[0].Trim());
                    definedBuffers.Add(ii.DefinedBufferName, ii);
                }
                else if (File.Exists(data[1].Trim().Replace("\"", "")))
                {
                    FLBuffer bi = new LazyFromFileFLBuffer(data[1].Trim().Replace("\"", ""));
                    bi.SetKey(data[0].Trim());
                    definedBuffers.Add(data[0].Trim(), bi);
                }
                else
                {
                    throw new FLInvalidDefineStatementException("Can not Find Key or File: " + data[1].Trim());
                }
            }

            return definedBuffers;
        }
    }
}