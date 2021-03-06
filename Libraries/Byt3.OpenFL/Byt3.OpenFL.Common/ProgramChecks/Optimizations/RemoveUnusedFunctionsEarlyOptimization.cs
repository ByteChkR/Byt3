﻿using System.Collections.Generic;
using Byt3.ADL;
using Byt3.OpenFL.Parsing.StageResults;

namespace Byt3.OpenFL.Common.ProgramChecks.Optimizations
{
    public class RemoveUnusedFunctionsEarlyOptimization : FLProgramCheck<StaticInspectionResult>
    {
        public override bool ChangesOutput => true;
        private Dictionary<string, bool> ParseFunctions(StaticInspectionResult input)
        {
            Dictionary<string, bool> funcs = new Dictionary<string, bool>();
            input.Functions.ForEach(x => funcs.Add(x.Name, x.Name == "Main"));
            Logger.Log(LogType.Log, $"Finding Unused Functions.", 2);

            foreach (StaticFunction serializableFlFunction in input.Functions)
            {
                foreach (StaticInstruction instructionLine in serializableFlFunction.Body)
                {
                    foreach (string instructionArgument in instructionLine.Arguments)
                    {
                        if (funcs.ContainsKey(instructionArgument))
                        {
                            funcs[instructionArgument] = true;
                        }
                    }
                }
            }

            return funcs;
        }

        public override object Process(object o)
        {
            StaticInspectionResult input = (StaticInspectionResult) o;
            bool stop = false;
            int pass = 1;


            while (!stop)
            {
                Logger.Log(LogType.Log, $"Pass {pass}: Generating Usage Info", 2);
                Dictionary<string, bool> funcs = ParseFunctions(input);
                bool removedOne = false;

                Logger.Log(LogType.Log, $"Pass {pass}: Applying Usage Info", 2);

                for (int i = input.Functions.Count - 1; i >= 0; i--)
                {
                    if (!funcs[input.Functions[i].Name])
                    {
                        input.Functions.RemoveAt(i);
                        removedOne = true;
                    }
                }

                stop = !removedOne;
                pass++;
            }
            
            return input;
        }
    }
}