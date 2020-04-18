using System.Collections.Generic;
using System.Linq;
using Byt3.ADL;
using Byt3.ObjectPipeline;
using Byt3.OpenFL.Common;
using Byt3.OpenFL.Common.Buffers;
using Byt3.OpenFL.Common.DataObjects;

namespace Byt3.OpenFL.Parsing.Stages
{
    public class ResolveReferencesStage : PipelineStage<ParseTreeStageResult, FLProgram>
    {
        private static readonly ADLLogger<LogType> Logger =
            new ADLLogger<LogType>(OpenFLDebugConfig.Settings, "LoadSourceStage");

        public override FLProgram Process(ParseTreeStageResult input)
        {
            Logger.Log(LogType.Log, "Resolving References: " + input.Filename, 2);

            FLProgram parseResult = new FLProgram(input.DefinedScripts,
                input.DefinedBuffers, input.FlFunctions, DebugData.None);

            ResolveReferences(parseResult);

            return parseResult;
        }


        private static void ResolveReferences(FLProgram source)
        {
            for (int i = 0; i < source.FlFunctions.Length; i++)
            {
                string name = source.FlFunctions[i].Name;
                Logger.Log(LogType.Log, "Resolving References for Function: " + name, 3);
                for (int j = 0; j < source.FlFunctions[i].Instructions.Count; j++)
                {
                    string instructionName = $"{name}.{source.FlFunctions[i].Instructions[j].GetType().Name}";
                    Logger.Log(LogType.Log, $"Resolving References for Instruction: {instructionName}", 4);
                    for (int k = 0; k < source.FlFunctions[i].Instructions[j].Arguments.Count; k++)
                    {
                        string instructionArgumentName =
                            $"{instructionName}.{source.FlFunctions[i].Instructions[j].Arguments[k].Type}";
                        Logger.Log(LogType.Log,
                            $"Resolving References for Argument: {instructionArgumentName}", 5);
                        if (source.FlFunctions[i].Instructions[j].Arguments[k].Type ==
                            FLInstructionArgumentType.UnresolvedFunction)
                        {
                            FLUnresolvedFunction uf =
                                (FLUnresolvedFunction) source.FlFunctions[i].Instructions[j].Arguments[k].Value;

                            Logger.Log(LogType.Log,
                                $"Resolving {(uf.External ? "External " : "")}Function for Argument: {instructionArgumentName}",
                                6);

                            if (uf.External)
                            {
                                source.FlFunctions[i].Instructions[j].Arguments[k].Value =
                                    source.DefinedScripts[uf.FunctionName];
                            }
                            else
                            {
                                source.FlFunctions[i].Instructions[j].Arguments[k].Value =
                                    source.FlFunctions.First(x => x.Name == uf.FunctionName);
                            }
                        }
                        else if (source.FlFunctions[i].Instructions[j].Arguments[k].Type ==
                                 FLInstructionArgumentType.UnresolvedDefinedBuffer)
                        {
                            FLUnresolvedDefinedBuffer uf =
                                (FLUnresolvedDefinedBuffer) source.FlFunctions[i].Instructions[j].Arguments[k].Value;


                            Logger.Log(LogType.Log,
                                $"Resolving Buffer \"{uf.BufferName}\" for Argument: {instructionArgumentName}", 6);

                            source.FlFunctions[i].Instructions[j].Arguments[k].Value =
                                source.DefinedBuffers[uf.BufferName];
                        }
                    }
                }
            }

            foreach (KeyValuePair<string, FLBuffer> definedBuffer in source.DefinedBuffers)
            {
                definedBuffer.Value.SetRoot(source);
            }


            source.EntryPoint.SetRoot(source);
        }
    }
}