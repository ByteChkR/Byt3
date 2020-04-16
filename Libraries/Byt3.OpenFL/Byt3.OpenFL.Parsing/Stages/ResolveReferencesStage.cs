using System.Collections.Generic;
using System.Linq;
using Byt3.ADL;
using Byt3.ObjectPipeline;
using Byt3.OpenFL.Parsing.DataObjects;

namespace Byt3.OpenFL.Parsing.Stages
{
    public class ResolveReferencesStage : PipelineStage<ParseTreeStageResult, FLParseResult>
    {

        private static readonly ADLLogger<LogType> Logger = new ADLLogger<LogType>(OpenFLDebugConfig.Settings, "LoadSourceStage");

        public override FLParseResult Process(ParseTreeStageResult input)
        {
            Logger.Log(LogType.Log, "Resolving References: " + input.Filename, 2);

            FLParseResult parseResult = new FLParseResult(input.Filename, input.Source, input.DefinedScripts, input.DefinedBuffers, input.Functions);

            ResolveReferences(parseResult);

            return parseResult;

        }


        private static void ResolveReferences(FLParseResult source)
        {


            for (int i = 0; i < source.Functions.Length; i++)
            {
                string name = source.Functions[i].Name;
                Logger.Log(LogType.Log, "Resolving References for Function: " + name, 3);
                for (int j = 0; j < source.Functions[i].Instructions.Count; j++)
                {
                    string instructionName = $"{name}.{source.Functions[i].Instructions[j].GetType().Name}";
                    Logger.Log(LogType.Log, $"Resolving References for Instruction: {instructionName}", 4);
                    for (int k = 0; k < source.Functions[i].Instructions[j].Arguments.Count; k++)
                    {

                        string instructionArgumentName = $"{instructionName}.{source.Functions[i].Instructions[j].Arguments[k].Type}";
                        Logger.Log(LogType.Log,
                            $"Resolving References for Argument: {instructionArgumentName}", 5);
                        if (source.Functions[i].Instructions[j].Arguments[k].Type ==
                            InstructionArgumentType.UnresolvedFunction)
                        {

                            UnresolvedFunction uf =
                                (UnresolvedFunction)source.Functions[i].Instructions[j].Arguments[k].Value;

                            Logger.Log(LogType.Log,
                                $"Resolving {(uf.External ? "External " : "")}Function for Argument: {instructionArgumentName}", 6);

                            if (uf.External)
                            {
                                source.Functions[i].Instructions[j].Arguments[k].Value =
                                    source.DefinedScripts[uf.FunctionName];
                            }
                            else
                            {
                                source.Functions[i].Instructions[j].Arguments[k].Value =
                                    source.Functions.First(x => x.Name == uf.FunctionName);
                            }
                        }
                        else if (source.Functions[i].Instructions[j].Arguments[k].Type ==
                                 InstructionArgumentType.UnresolvedDefinedBuffer)
                        {
                            UnresolvedDefinedBuffer uf =
                                (UnresolvedDefinedBuffer)source.Functions[i].Instructions[j].Arguments[k].Value;


                            Logger.Log(LogType.Log,
                                $"Resolving Buffer \"{uf.BufferName}\" for Argument: {instructionArgumentName}", 6);

                            source.Functions[i].Instructions[j].Arguments[k].Value = source.DefinedBuffers[uf.BufferName];
                        }
                    }
                }
            }

            foreach (KeyValuePair<string, FLBufferInfo> definedBuffer in source.DefinedBuffers)
            {
                definedBuffer.Value.SetRoot(source);
            }

            foreach (KeyValuePair<string, FunctionObject> functionObject in source.DefinedScripts)
            {
                functionObject.Value.SetRoot(source);
            }

            source.EntryPoint.SetRoot(source);
        }
    }
}