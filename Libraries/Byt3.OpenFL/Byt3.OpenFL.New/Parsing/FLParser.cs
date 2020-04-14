using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Byt3.OpenFL.New.DataObjects;

namespace Byt3.OpenFL.New.Parsing
{
    public static class FLParser
    {

        private static Dictionary<string, Type> FLInstructions = new Dictionary<string, Type>();

        public static ParsedSource ParseFile(string file)
        {
            throw new NotImplementedException();
        }

        private static ParsedSource ParseSource(string[] source)
        {
            string[] defines = FindDefineStatements(source);
            string[] functionHeader = FindFunctionHeaders(source);
            Dictionary<string, FLBufferInfo> definedBuffers = ParseDefinedBuffers(FindDefineStatements(source));
            FunctionObject[] functions = ParseFunctions(functionHeader, defines, source);

            ParsedSource ret = new ParsedSource(functions, definedBuffers);

            ResolveReferences(functions, definedBuffers, ret);

            return ret;
        }

        private static void ResolveReferences(FunctionObject[] functions, Dictionary<string, FLBufferInfo> definedBuffers, ParsedSource source)
        {
            for (int i = 0; i < functions.Length; i++)
            {
                functions[i].Root = source;
                for (int j = 0; j < functions[i].Instructions.Count; j++)
                {
                    functions[i].Instructions[j].Root = source;
                    for (int k = 0; k < functions[i].Instructions[j].Arguments.Count; k++)
                    {
                        if (functions[i].Instructions[j].Arguments[k].Type ==
                            InstructionArgumentType.UnresolvedFunction)
                        {
                            UnresolvedFunction uf =
                                (UnresolvedFunction)functions[i].Instructions[j].Arguments[k].Value;
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
        }

        private static FunctionObject[] ParseFunctions(string[] functionHeaders, string[] definedBuffers, string[] source)
        {
            string[] header = FindFunctionHeaders(source);
            FunctionObject[] functions = new FunctionObject[header.Length];
            for (int i = 0; i < header.Length; i++)
            {
                functions[i] = ParseFunctionObject(functionHeaders, definedBuffers, header[i], GetFunctionBody(header[i], source));
            }

            return functions;
        }

        private static FunctionObject ParseFunctionObject(string[] functionHeaders, string[] definedBuffers, string name, string[] functionPart)
        {
            List<Instruction> instructions = ParseInstructions(functionHeaders, definedBuffers, functionPart);
            return new FunctionObject(name, instructions);
        }

        private static List<Instruction> ParseInstructions(string[] functionHeaders, string[] definedBuffers, string[] functionBody)
        {
            List<Instruction> instructions = new List<Instruction>();
            for (int i = 0; i < functionBody.Length; i++)
            {
                instructions.Add(ParseInstruction(functionHeaders, definedBuffers, functionBody[i]));
            }

            return instructions;
        }

        private static Instruction ParseInstruction(string[] functionHeaders, string[] definedBuffers, string instruction)
        {
            string[] parts = instruction.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string inst = parts[0];

            InstructionArgument[] args = new InstructionArgument[parts.Length - 1];
            for (int i = 1; i < parts.Length; i++)
            {
                args[i - 1] = ParseInstructionArgument(functionHeaders, definedBuffers, parts[i]);
            }

            if (FLInstructions.ContainsKey(inst))
            {
                return (Instruction)Activator.CreateInstance(FLInstructions[inst], new object[] { args });
            }
            return new KernelInstruction(inst, args.ToList());
        }



        private static InstructionArgument ParseInstructionArgument(string[] functionHeaders, string[] definedBuffers, string argument)
        {
            if (decimal.TryParse(argument, out decimal value))
            {
                return new InstructionArgument(value);
            }

            if (functionHeaders.Contains(argument))
            {
                return new InstructionArgument(new UnresolvedFunction(argument));
            }

            if (definedBuffers.Contains(argument))
            {
                return new InstructionArgument(new UnresolvedDefinedBuffer(argument));
            }
            throw new InvalidOperationException("Can not parse argument: " + argument);
        }

        private static Dictionary<string, FLBufferInfo> ParseDefinedBuffers(string[] defineStatements)
        {
            throw new NotImplementedException();
        }

        private static string[] FindDefineStatements(string[] source)
        {
            return source.Where(IsDefineStatement).ToArray();
        }

        private static string[] FindFunctionHeaders(string[] source)
        {
            return source.Where(IsFunctionHeader).Select(x => x.Remove(x.Length - 1, 1)).ToArray();
        }

        private static bool IsComment(string line)
        {
            return line.StartsWith("#");
        }

        private static bool IsDefineStatement(string line)
        {
            return !IsComment(line) && line.StartsWith("--define texture");
        }

        private static bool IsFunctionHeader(string line)
        {
            return !IsComment(line) && line.EndsWith(":");
        }

        private static string[] GetFunctionBody(string functionHeader, string[] source)
        {
            int index = source.ToList().IndexOf(functionHeader);
            List<string> ret = new List<string>();
            for (int i = index + 1; i < source.Length; i++)
            {
                if (IsFunctionHeader(source[i])) break;
                ret.Add(source[i]);
            }

            return ret.ToArray();
        }
    }
}