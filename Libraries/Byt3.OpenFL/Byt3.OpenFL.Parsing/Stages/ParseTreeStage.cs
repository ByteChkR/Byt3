using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Byt3.ADL;
using Byt3.ExtPP.Base;
using Byt3.ObjectPipeline;
using Byt3.OpenFL.Common;
using Byt3.OpenFL.Common.Buffers.BufferCreators.BuiltIn.Empty;
using Byt3.OpenFL.Common.Buffers.BufferCreators.BuiltIn.FromFile;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects.BuiltIn;
using Byt3.OpenFL.Common.Exceptions;
using Byt3.OpenFL.Parsing.StageResults;
using Byt3.Utilities.Exceptions;

namespace Byt3.OpenFL.Parsing.Stages
{
    public class ParseTreeStage : PipelineStage<StaticInspectionResult, SerializableFLProgram>
    {
        private static readonly ADLLogger<LogType> Logger =
            new ADLLogger<LogType>(OpenFLDebugConfig.Settings, "ParseTreeStage");

        private readonly FLParser parser;

        public ParseTreeStage(FLParser parserInstance)
        {
            parser = parserInstance;
        }

        public override SerializableFLProgram Process(StaticInspectionResult input)
        {
            Logger.Log(LogType.Log, "Parsing Tree: " + input.Filename, 2);
            Logger.Log(LogType.Log, "Creating Defined Script Nodes..", 3);
            List<SerializableExternalFLFunction> scripts = ParseScriptDefines(input.DefinedScripts);
            Logger.Log(LogType.Log, "Script Nodes: " + scripts.Select(x => x.Name).Unpack(", "), 4);

            Logger.Log(LogType.Log, "Creating Defined Buffer Nodes..", 3);
            List<SerializableFLBuffer> definedBuffers = ParseDefinedBuffers(input.DefinedBuffers);
            Logger.Log(LogType.Log, "Buffer Nodes: " + definedBuffers.Select(x => x.Name).Unpack(", "), 4);

            Logger.Log(LogType.Log, "Creating Defined Function Nodes..", 3);
            List<SerializableFLFunction> flFunctions = ParseFunctions(input.Functions, input.DefinedBuffers,
                input.DefinedScripts, input.Source);
            Logger.Log(LogType.Log, "Buffer Nodes: " + flFunctions.Select(x => x.Name).Unpack(", "), 4);
            return new SerializableFLProgram(scripts, flFunctions, definedBuffers);
        }


        private List<SerializableExternalFLFunction> ParseScriptDefines(string[] statements)
        {
            List<SerializableExternalFLFunction> ret = new List<SerializableExternalFLFunction>();
            for (int i = 0; i < statements.Length; i++)
            {
                string name = FLParser.GetScriptName(statements[i]);
                string relPath = FLParser.GetScriptPath(statements[i]);
                string p = relPath;

                if (!File.Exists(p))
                {
                    throw new FLInvalidDefineStatementException("Can not Find Script with path: " + p);
                }


                SerializableFLProgram ps = parser.Process(new FLParserInput(p));
                ret.Add(new SerializableExternalFLFunction(name, ps));
            }

            return ret;
        }

        private List<SerializableFLFunction> ParseFunctions(string[] functionHeaders, string[] definedBuffers,
            string[] definedScripts,
            string[] source)
        {
            SerializableFLFunction[] flFunctions = new SerializableFLFunction[functionHeaders.Length];
            for (int i = 0; i < functionHeaders.Length; i++)
            {
                flFunctions[i] = ParseFunctionObject(functionHeaders, definedBuffers, definedScripts,
                    functionHeaders[i],
                    FLParser.GetFunctionBody(functionHeaders[i], source));
            }

            return flFunctions.ToList();
        }

        private SerializableFLFunction ParseFunctionObject(string[] functionHeaders, string[] definedBuffers,
            string[] definedScripts,
            string name, string[] functionPart)
        {
            List<SerializableFLInstruction> instructions =
                ParseInstructions(functionHeaders, definedBuffers, definedScripts, functionPart);
            return new SerializableFLFunction(name, instructions);
        }

        private List<SerializableFLInstruction> ParseInstructions(string[] functionHeaders, string[] definedBuffers,
            string[] definedScripts,
            string[] functionBody)
        {
            List<SerializableFLInstruction> instructions = new List<SerializableFLInstruction>();
            for (int i = 0; i < functionBody.Length; i++)
            {
                if (!FLParser.IsComment(functionBody[i]) && !FLParser.IsDefineScriptStatement(functionBody[i]) &&
                    !FLParser.IsDefineStatement(functionBody[i]))
                {
                    SerializableFLInstruction inst =
                        ParseInstruction(functionHeaders, definedBuffers, definedScripts, functionBody[i]);
                    if (inst != null)
                    {
                        instructions.Add(inst);
                    }
                }
            }

            return instructions;
        }

        private SerializableFLInstruction ParseInstruction(string[] functionHeaders, string[] definedBuffers,
            string[] definedScripts,
            string instruction)
        {
            if (instruction == "")
            {
                return null;
            }

            string[] parts = instruction.Split(new[] {' '}, StringSplitOptions.None);
            string inst = parts[0];

            //Create Argument List
            List<SerializableFLInstructionArgument> args = new List<SerializableFLInstructionArgument>();
            for (int i = 1; i < parts.Length; i++)
            {
                if (FLParser.IsComment(parts[i]))
                {
                    break;
                }

                args.Add(ParseInstructionArgument(functionHeaders, definedBuffers, definedScripts, parts[i]));
            }


            return new SerializableFLInstruction(inst, args);

            //SerializableFLInstruction ret = null;
            //if (FLParser.FLInstructions.ContainsKey(inst))
            //{
            //    ret = (FLInstruction) Activator.CreateInstance(FLParser.FLInstructions[inst], new object[] {args});
            //}
            //else
            //{
            //    ret = new KernelFLInstruction(inst, args.ToList());
            //}

            //return ret;
        }


        private SerializableFLInstructionArgument ParseInstructionArgument(string[] functionHeaders,
            string[] definedBuffers,
            string[] definedScripts,
            string argument)
        {
            if (decimal.TryParse(argument, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture,
                out decimal value))
            {
                return new SerializeDecimalArgument(value);
            }

            if (functionHeaders.Contains(argument))
            {
                return new SerializeFunctionArgument(functionHeaders.ToList().IndexOf(argument));
            }

            if (definedBuffers.Select(FLParser.GetBufferName).Contains(argument))
            {
                return new SerializeBufferArgument(argument);
            }

            if (definedScripts.Select(FLParser.GetScriptName).Contains(argument))
            {
                return new SerializeExternalFunctionArgument(argument);
            }

            throw new InvalidOperationException("Can not parse argument: " + argument);
        }


        private List<SerializableFLBuffer> ParseDefinedBuffers(string[] defineStatements)
        {
            List<SerializableFLBuffer> definedBuffers = new List<SerializableFLBuffer>();
            for (int i = 0; i < defineStatements.Length; i++)
            {
                string[] data = defineStatements[i].Replace("--define texture", "")
                    .Split(new[] {':'}, StringSplitOptions.RemoveEmptyEntries);
                string bufferName = data[0].Trim();
                if (bufferName == "in")
                {
                    SerializableFLBuffer bi = new SerializableEmptyFLBuffer("in");
                    definedBuffers.Add(bi);
                    continue;
                }


                string paramPart = data[1].Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries)[0];

                SerializableFLBuffer buf = parser.BufferCreator.Create(paramPart, bufferName,
                    data[1].Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries));
                if (buf != null)
                {
                    definedBuffers.Add(buf);
                }
                else if (File.Exists(data[1].Trim().Replace("\"", "")))
                {
                    SerializableFromFileFLBuffer bi =
                        new SerializableFromFileFLBuffer(bufferName, data[1].Trim().Replace("\"", ""));
                    definedBuffers.Add(bi);
                }
                else
                {
                    throw new Byt3Exception($"Can not Find BufferLoader for \"{defineStatements[i]}\"");
                }
            }

            return definedBuffers;
        }
    }
}