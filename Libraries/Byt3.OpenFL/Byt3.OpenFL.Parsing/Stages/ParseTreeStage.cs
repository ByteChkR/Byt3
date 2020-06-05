using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Byt3.ADL;
using Byt3.Callbacks;
using Byt3.ObjectPipeline;
using Byt3.OpenFL.Common;
using Byt3.OpenFL.Common.Arguments;
using Byt3.OpenFL.Common.Buffers.BufferCreators.BuiltIn.Empty;
using Byt3.OpenFL.Common.Buffers.BufferCreators.BuiltIn.FromFile;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;
using Byt3.OpenFL.Common.ElementModifiers;
using Byt3.OpenFL.Common.Exceptions;
using Byt3.OpenFL.Common.Parsing.StageResults;
using Byt3.Utilities.Exceptions;
using Byt3.Utilities.FastString;

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
            Logger.Log(LogType.Log, "Parsing Tree: " + input.Filename, 1);
            Logger.Log(LogType.Log, "Creating Defined Script Nodes..", 2);
            List<SerializableExternalFLFunction> scripts = ParseScriptDefines(input.DefinedScripts);
            Logger.Log(LogType.Log, "Script Nodes: " + scripts.Select(x => x.Name).Unpack(", "), 4);


            Logger.Log(LogType.Log, "Creating Defined Buffer Nodes..", 2);
            List<SerializableFLBuffer> definedBuffers = ParseDefinedBuffers(input.DefinedBuffers);
            Logger.Log(LogType.Log, "Buffer Nodes: " + definedBuffers.Select(x => x.Name).Unpack(", "), 4);

            Logger.Log(LogType.Log, "Creating Defined Function Nodes..", 2);
            List<SerializableFLFunction> flFunctions =
                ParseFunctions(input.Functions, input.DefinedBuffers, input.DefinedScripts);
            Logger.Log(LogType.Log, "Buffer Nodes: " + flFunctions.Select(x => x.Name).Unpack(", "), 4);
            return new SerializableFLProgram(input.Filename, scripts, flFunctions, definedBuffers);
        }

        private List<SerializableExternalFLFunction> ParseScriptDefines(DefineStatement[] statements)
        {
            List<SerializableExternalFLFunction> ret = new List<SerializableExternalFLFunction>();

            for (int i = 0; i < statements.Length; i++)
            {
                string name = statements[i].Name;
                string relPath = FLParser.GetScriptPath(statements[i].SourceLine);
                string p = relPath;

                if (!IOManager.FileExists(p))
                {
                    throw new FLInvalidDefineStatementException("Can not Find Script with path: " + p);
                }


                SerializableFLProgram ps = parser.Process(new FLParserInput(p, false));
                ret.Add(new SerializableExternalFLFunction(name, ps,
                    statements[i].Modifiers as FLExecutableElementModifiers));
            }

            return ret;
        }

        private List<SerializableFLFunction> ParseFunctionsTask(List<StaticFunction> functionHeaders, int start,
            int count, DefineStatement[] definedBuffers,
            DefineStatement[] definedScripts)
        {
            List<SerializableFLFunction> ret = new List<SerializableFLFunction>();
            for (int i = start; i < start + count; i++)
            {
                ret.Add(ParseFunctionObject(functionHeaders, definedBuffers, definedScripts,
                    functionHeaders[i]));
            }

            return ret;
        }

        private List<SerializableFLFunction> ParseFunctions(List<StaticFunction> functionHeaders,
            DefineStatement[] definedBuffers,
            DefineStatement[] definedScripts)
        {
            return WorkItemRunner.RunInWorkItems(functionHeaders.ToList(),
                (input, start, count) => ParseFunctionsTask(input, start, count, definedBuffers, definedScripts),
                parser.WorkItemRunnerSettings);
        }

        private SerializableFLFunction ParseFunctionObject(List<StaticFunction> functionHeaders,
            DefineStatement[] definedBuffers,
            DefineStatement[] definedScripts,
            StaticFunction currentFunction)
        {
            List<SerializableFLInstruction> instructions =
                ParseInstructions(functionHeaders, definedBuffers, definedScripts, currentFunction);
            return new SerializableFLFunction(currentFunction.Name, currentFunction.Body, instructions,
                currentFunction.Modifiers);
        }

        private List<SerializableFLInstruction> ParseInstructions(List<StaticFunction> functionHeaders,
            DefineStatement[] definedBuffers,
            DefineStatement[] definedScripts,
            StaticFunction currentFunction)
        {
            List<SerializableFLInstruction> instructions = new List<SerializableFLInstruction>();
            for (int i = 0; i < currentFunction.Body.Length; i++)
            {
                SerializableFLInstruction inst =
                    ParseInstruction(functionHeaders, definedBuffers, definedScripts, currentFunction, i);
                if (inst != null)
                {
                    instructions.Add(inst);
                }
            }

            return instructions;
        }

        private SerializableFLInstruction ParseInstruction(List<StaticFunction> functionHeaders,
            DefineStatement[] definedBuffers,
            DefineStatement[] definedScripts,
            StaticFunction currentFunction, int instructionIndex)
        {
            StaticInstruction instruction = currentFunction.Body[instructionIndex];
            if (instruction.Key == "")
            {
                return null;
            }

            //Create Argument List
            List<SerializableFLInstructionArgument> args = new List<SerializableFLInstructionArgument>();
            for (int i = 0; i < instruction.Arguments.Length; i++)
            {
                args.Add(ParseInstructionArgument(functionHeaders, definedBuffers, definedScripts,
                    instruction.Arguments[i], currentFunction));
            }


            return new SerializableFLInstruction(instruction.Key, args);
        }


        private SerializableFLInstructionArgument ParseInstructionArgument(List<StaticFunction> functionHeaders,
            DefineStatement[] definedBuffers,
            DefineStatement[] definedScripts,
            string argument,
            StaticFunction currentFunction)
        {
            if (decimal.TryParse(argument, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture,
                out decimal value))
            {
                return new SerializeDecimalArgument(value);
            }

            IEnumerable<string> bufferNames = definedBuffers.Where(x => (x.Modifiers as FLBufferModifiers).IsTexture)
                .Select(x => x.Name);
            IEnumerable<string> arrayBufferNames = definedBuffers.Where(x => (x.Modifiers as FLBufferModifiers).IsArray)
                .Select(x => x.Name);
            IEnumerable<string> scriptNames = definedScripts.Select(x => x.Name);

            StaticFunction func = functionHeaders.FirstOrDefault(x => x.Name == argument);
            if (func != null)
            {
                //if (currentFunction.Modifiers.IsStatic && !func.Modifiers.IsStatic)
                //{
                //    throw new FLInvalidFunctionUseException($"{currentFunction}", "Can not call a non static function from a static function");
                //}
                return new SerializeFunctionArgument(argument);
            }

            if (argument.StartsWith("~"))
            {
                //if (currentFunction.Modifiers.IsStatic)
                //{
                //    throw new FLInvalidFunctionUseException($"{currentFunction}", "Can not Access Buffers from a static function");
                //}

                string n = argument.Remove(0, 1);
                if (arrayBufferNames.Contains(n))
                {
                    return new SerializeArrayLengthArgument(n);
                }

                if (bufferNames.Contains(n))
                {
                    throw new InvalidOperationException("Only array buffers can be queried for length: " + n);
                }


                throw new InvalidOperationException("Invalid Length operator. Can not find buffer with name: " + n);
            }

            if (bufferNames.Contains(argument))
            {
                //if (currentFunction.Modifiers.IsStatic)
                //{
                //    throw new FLInvalidFunctionUseException($"{currentFunction}", "Can not Access Buffers from a static function");
                //}

                return new SerializeBufferArgument(argument);
            }

            if (arrayBufferNames.Contains(argument))
            {
                //if (currentFunction.Modifiers.IsStatic)
                //{
                //    throw new FLInvalidFunctionUseException($"{currentFunction}", "Can not Access Buffers from a static function");
                //}

                return new SerializeArrayBufferArgument(argument);
            }

            if (SerializeArrayElementArgument.TryParse(arrayBufferNames, argument,
                out SerializeArrayElementArgument arg))
            {
                return arg;
            }

            if (scriptNames.Contains(argument))
            {
                return new SerializeExternalFunctionArgument(argument);
            }

            //if (currentFunction.Modifiers.IsStatic)
            //{
            //    throw new FLInvalidFunctionUseException($"{currentFunction}", "Can not Access Variables from a static function");
            //}
            //else
            //{
            return new SerializeNameArgument(argument);
            //}


            //throw new InvalidOperationException("Can not parse argument: " + argument);
        }


        private List<SerializableFLBuffer> ParseDefinedBuffersTask(List<DefineStatement> defineStatements, int start,
            int count)
        {
            List<SerializableFLBuffer> definedBuffers = new List<SerializableFLBuffer>();

            for (int i = start; i < start + count; i++)
            {
                FLBufferModifiers bmod = defineStatements[i].Modifiers as FLBufferModifiers;
                string name = defineStatements[i].Name;
                List<string> parameter = defineStatements[i].Parameter.ToList();
                string bufferName = bmod.ElementName;

                if (bufferName == FLKeywords.InputBufferKey)
                {
                    SerializableFLBuffer bi = new SerializableEmptyFLBuffer(FLKeywords.InputBufferKey, bmod);
                    definedBuffers.Add(bi);
                    continue;
                }

                string creatorKey = parameter.First();
                parameter.RemoveAt(0);


                if (creatorKey.StartsWith("\"") && creatorKey.EndsWith("\"")
                ) //IOManager.FileExists(paramPart.Trim().Replace("\"", ""))
                {
                    string path = creatorKey.Trim().Replace("\"", "");
                    SerializableFromFileFLBuffer bi =
                        new SerializableFromFileFLBuffer(bufferName, path, bmod, 0);
                    definedBuffers.Add(bi);
                }
                else
                {
                    int size = -1;
                    if (bmod.IsArray && parameter.Count != 0 && !int.TryParse(parameter[0],
                            NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out size))
                    {
                    }

                    SerializableFLBuffer buf =
                        parser.BufferCreator.Create(creatorKey, bufferName, parameter.ToArray(), bmod, size);
                    if (buf != null)
                    {
                        definedBuffers.Add(buf);
                    }
                    else
                    {
                        throw new Byt3Exception($"Can not Find BufferLoader for \"{defineStatements[i]}\"");
                    }
                }
            }

            return definedBuffers;
        }

        private List<SerializableFLBuffer> ParseDefinedBuffers(DefineStatement[] defineStatements)
        {
            return WorkItemRunner.RunInWorkItems(defineStatements.ToList(), ParseDefinedBuffersTask,
                parser.WorkItemRunnerSettings);
        }
    }
}