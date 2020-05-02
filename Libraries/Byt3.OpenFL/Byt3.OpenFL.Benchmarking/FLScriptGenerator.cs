using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Byt3.OpenFL.Benchmarking
{
    public static class FLScriptGenerator
    {
        public struct FLInstructionInfo
        {
            public string Name;
            public bool[] IsDecimal;

            public static List<FLInstructionInfo> infos = new List<FLInstructionInfo>
            {
                A("setactive", "0"),
                A("setactive", "00"),
                A("setactive", "000"),
                A("setactive", "D0"),
                A("setactive", "D00"),
                A("setactive", "D000"),
                A("rnd", ""),
                A("urnd", ""),
                A("mulv", "0"),
                A("mul", "D"),
                A("modv", "0"),
                A("mod", "D"),
                A("mixt", "DD"),
                A("mixv", "D0"),
                A("invert", ""),
                A("divv", "0"),
                A("div", "D"),
                A("adjustlevel", "00"),
                A("adjustlevelrescale", "00"),
                A("add", "D"),
                A("addv", "0"),
                A("addc", "D"),
                A("addvc", "0"),
                A("sub", "D"),
                A("subv", "0"),
                A("smooth", "0"),
                A("perlin", "00"),
                A("point", "0000"),
                A("circle", "0000"),
                A("set", "D"),
                A("setv", "0"),
            };

            public static FLInstructionInfo A(string name, string isDecimal)
            {
                bool[] dec = isDecimal.Select(x => x == '0').ToArray();
                return new FLInstructionInfo { Name = name, IsDecimal = dec };
            }
        }

        public static string GenerateRandomScript(int functionCount, int bufferCount, int additionalProgramTrees,
            int functionCountPerAdditionalTree)
        {
            Random rnd = new Random();
            List<string> buffers = GenerateElementNames("Buffer", bufferCount);
            StringBuilder ret = new StringBuilder();
            buffers.ForEach(x => ret.AppendLine(GenerateBufferDefine(x, (GeneratableBufferType)rnd.Next(0, 3))));
            ret.Append(GenerateRandomFLScript("Main", "", FLInstructionInfo.infos, functionCount, buffers));

            for (int i = 0; i < additionalProgramTrees; i++)
            {
                ret.Append(GenerateRandomFLScript("_Entry_" + i, $"Add_{i}_", FLInstructionInfo.infos,
                    functionCountPerAdditionalTree, buffers));
            }

            return ret.ToString();
        }

        public static string GenerateRandomFLScript(string startFunction, string functionPrefix,
            List<FLInstructionInfo> instructions, int functionCount, List<string> bufferNames)
        {
            List<string> functionNames = GenerateElementNames(functionPrefix + "Function", functionCount);
            List<string> tempFunctionNames = new List<string>(functionNames);

            StringBuilder script = new StringBuilder();


            Queue<string> todo = new Queue<string>();
            todo.Enqueue(startFunction);

            while (todo.Count != 0)
            {
                string todoFunc = todo.Dequeue();
                StringBuilder sb = new StringBuilder(todoFunc + ":\n");
                tempFunctionNames.Remove(todoFunc);
                List<string> lines = GenerateRandomInstructions(out List<string> newFuncs, instructions,
                    tempFunctionNames, bufferNames, 5);
                foreach (string newFunc in newFuncs)
                {
                    todo.Enqueue(newFunc);
                }


                //Write Script
                foreach (string line in lines)
                {
                    sb.AppendLine(line);
                }

                sb.AppendLine();
                script.Append(sb);
            }

            return script.ToString();
        }

        private static List<string> GenerateRandomInstructions(out List<string> nextFunctionNames,
            List<FLInstructionInfo> instructions, List<string> functionNames, List<string> bufferNames, int amount)
        {
            nextFunctionNames = new List<string>();
            Random rnd = new Random();
            List<string> lines = new List<string>();
            for (int i = 0; i < amount; i++)
            {
                int idx = rnd.Next(instructions.Count);
                string line = instructions[idx].Name;
                StringBuilder args = new StringBuilder();
                for (int j = 0; j < instructions[idx].IsDecimal.Length; j++)
                {
                    if (instructions[idx].IsDecimal[j])
                    {
                        args.Append(" " + Math.Round(rnd.NextDouble(), 4).ToString().Replace(",", "."));
                    }
                    else
                    {
                        if (functionNames.Count > 0)
                        {
                            int rndElement = rnd.Next(functionNames.Count);

                            args.Append(" " + functionNames[rndElement]);
                            nextFunctionNames.Add(functionNames[rndElement]);
                            functionNames.RemoveAt(rndElement);
                        }
                        else
                        {
                            int rndElement = rnd.Next(bufferNames.Count);
                            args.Append(" " + bufferNames[rndElement]);
                        }
                    }
                }

                lines.Add(line + args);
            }

            return lines;
        }

        private static List<string> GenerateElementNames(string elementType, int amount)
        {
            List<string> ret = new List<string>();
            for (int i = 0; i < amount; i++)
            {
                ret.Add(elementType + "_" + i);
            }

            return ret;
        }

        private enum GeneratableBufferType
        {
            Empty,
            URnd,
            Rnd
        }

        private static string GenerateBufferDefine(string bufferName, GeneratableBufferType type)
        {
            string ret = $"--define texture {bufferName}: ";
            switch (type)
            {
                case GeneratableBufferType.Empty:
                    ret += "empty";
                    break;
                case GeneratableBufferType.Rnd:
                    ret += "rnd";
                    break;
                case GeneratableBufferType.URnd:
                    ret += "urnd";
                    break;
            }

            return ret;
        }
    }
}