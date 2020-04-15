using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Byt3.ExtPP.API;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenFL.New.DataObjects;
using Byt3.OpenFL.New.Instructions;

namespace Byt3.OpenFL.New.Parsing
{
    public static class FLParser
    {
        private static readonly Dictionary<string, Type> FLInstructions = new Dictionary<string, Type>
        {
            {"setactive", typeof(SetActiveInstruction)},
            {"jmp", typeof(JumpInstruction) },
            {"urnd", typeof(URandomInstruction)},
            {"rnd", typeof(RandomInstruction) },
        };

        public static ParsedSource ParseFile(CLAPI instance, string file)
        {
            string[] source = TextProcessorAPI.PreprocessLines(file, new Dictionary<string, bool>());
            return ParseSource(instance, file, source);
        }

        private static ParsedSource ParseSource(CLAPI instance, string path, string[] source)
        {
            string[] scriptStrings = FindDefineScriptsStatements(source);

            Dictionary<string, FunctionObject> scripts =
                ParseScriptDefines(instance, path, scriptStrings);


            string[] defines = FindDefineStatements(source);

            Dictionary<string, FLBufferInfo> definedBuffers = ParseDefinedBuffers(instance, path, defines);
            string[] functionHeader = FindFunctionHeaders(source);
            FunctionObject[] functions = ParseFunctions(functionHeader, defines, scriptStrings, source);

            ParsedSource ret = new ParsedSource(path, functions, definedBuffers, scripts);

            ResolveReferences(functions, definedBuffers, scripts, ret);

            return ret;
        }

        private static Dictionary<string, FunctionObject> ParseScriptDefines(CLAPI instance, string path, string[] statements)
        {
            Dictionary<string, FunctionObject> ret = new Dictionary<string, FunctionObject>();
            string dir = Path.GetDirectoryName(path);
            for (int i = 0; i < statements.Length; i++)
            {
                string name = GetScriptName(statements[i]);
                string relPath = GetScriptPath(statements[i]);
                string p = relPath;
                ParsedSource ps = ParseFile(instance, p);
                ps.ScriptName = name;

                ret.Add(name, ps.EntryPoint);
            }

            return ret;
        }

        private static void ResolveReferences(FunctionObject[] functions,
            Dictionary<string, FLBufferInfo> definedBuffers, Dictionary<string, FunctionObject> definedScripts, ParsedSource source)
        {
            

            for (int i = 0; i < functions.Length; i++)
            {
                for (int j = 0; j < functions[i].Instructions.Count; j++)
                {
                    for (int k = 0; k < functions[i].Instructions[j].Arguments.Count; k++)
                    {
                        if (functions[i].Instructions[j].Arguments[k].Type ==
                            InstructionArgumentType.UnresolvedFunction)
                        {
                            UnresolvedFunction uf =
                                (UnresolvedFunction)functions[i].Instructions[j].Arguments[k].Value;
                            if (uf.External) functions[i].Instructions[j].Arguments[k].Value =
                                 definedScripts[uf.FunctionName];
                            else
                                functions[i].Instructions[j].Arguments[k].Value =
                                    functions.First(x => x.Name == uf.FunctionName);
                        }
                        else if (functions[i].Instructions[j].Arguments[k].Type ==
                                 InstructionArgumentType.UnresolvedDefinedBuffer)
                        {
                            UnresolvedDefinedBuffer uf =
                                (UnresolvedDefinedBuffer)functions[i].Instructions[j].Arguments[k].Value;
                            functions[i].Instructions[j].Arguments[k].Value = definedBuffers[uf.BufferName];
                        }
                    }
                }
            }

            foreach (KeyValuePair<string, FLBufferInfo> definedBuffer in definedBuffers)
            {
                definedBuffer.Value.SetRoot(source);
            }

            foreach (KeyValuePair<string, FunctionObject> functionObject in definedScripts)
            {
                functionObject.Value.SetRoot(source);
            }

            source.EntryPoint.SetRoot(source);
        }

        private static FunctionObject[] ParseFunctions(string[] functionHeaders, string[] definedBuffers, string[] definedScripts,
            string[] source)
        {
            FunctionObject[] functions = new FunctionObject[functionHeaders.Length];
            for (int i = 0; i < functionHeaders.Length; i++)
            {
                functions[i] = ParseFunctionObject(functionHeaders, definedBuffers, definedScripts, functionHeaders[i],
                    GetFunctionBody(functionHeaders[i], source));
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
                if (!IsComment(functionBody[i]) && !IsDefineScriptStatement(functionBody[i]) &&
                    !IsDefineStatement(functionBody[i]))
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
                if (IsComment(parts[i])) break;
                args.Add(ParseInstructionArgument(functionHeaders, definedBuffers, definedScripts, parts[i]));
            }

            Instruction ret = null;

            if (FLInstructions.ContainsKey(inst))
            {
                ret = (Instruction)Activator.CreateInstance(FLInstructions[inst], new object[] { args });
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

            if (definedBuffers.Select(GetBufferName).Contains(argument))
            {
                return new InstructionArgument(new UnresolvedDefinedBuffer(argument));
            }

            if (definedScripts.Select(GetScriptName).Contains(argument))
            {
                return new InstructionArgument(new UnresolvedFunction(argument, true));
            }

            throw new InvalidOperationException("Can not parse argument: " + argument);
        }


        private static string GetScriptName(string definedScriptLine)
        {
            return definedScriptLine.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries)[0]
                .Replace("--define script", "").Trim();
        }
        private static string GetScriptPath(string definedScriptLine)
        {
            return definedScriptLine.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries)[1].Trim().Replace("\"", "");
        }

        private static string GetBufferName(string definedBufferLine)
        {
            return definedBufferLine.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries)[0]
                .Replace("--define texture", "").Trim();
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

        private static string[] FindDefineStatements(string[] source)
        {
            List<string> ret = source.Where(IsDefineStatement).ToList();
            ret.Add("--define texture in:");
            return ret.ToArray();
        }

        private static bool IsDefineStatement(string line)
        {
            return !IsComment(line) && line.StartsWith("--define texture");
        }

        private static string[] FindDefineScriptsStatements(string[] source)
        {
            return source.Where(IsDefineScriptStatement).ToArray();
        }

        private static bool IsDefineScriptStatement(string line)
        {
            return !IsComment(line) && line.StartsWith("--define script");
        }

        private static string[] FindFunctionHeaders(string[] source)
        {
            return source.Where(IsFunctionHeader).Select(x => x.Remove(x.Length - 1, 1)).ToArray();
        }

        private static bool IsComment(string line)
        {
            return line.StartsWith("#");
        }


        private static bool IsFunctionHeader(string line)
        {
            return !IsComment(line) && line.EndsWith(":");
        }

        private static string[] GetFunctionBody(string functionHeader, string[] source)
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