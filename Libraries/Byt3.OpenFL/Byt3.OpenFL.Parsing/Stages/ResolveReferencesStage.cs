using System.Collections.Generic;
using System.Linq;
using Byt3.ObjectPipeline;
using Byt3.OpenFL.Parsing.DataObjects;

namespace Byt3.OpenFL.Parsing.Stages
{
    public class ResolveReferencesStage : PipelineStage<ParseTreeStageResult, FLParseResult>
    {
        public override FLParseResult Process(ParseTreeStageResult input)
        {
            

            FLParseResult parseResult = new FLParseResult(input.Filename, input.Source, input.DefinedScripts, input.DefinedBuffers, input.Functions);

            ResolveReferences(parseResult);

            return parseResult;

        }


        private static void ResolveReferences(FLParseResult source)
        {


            for (int i = 0; i < source.Functions.Length; i++)
            {
                for (int j = 0; j < source.Functions[i].Instructions.Count; j++)
                {
                    for (int k = 0; k < source.Functions[i].Instructions[j].Arguments.Count; k++)
                    {
                        if (source.Functions[i].Instructions[j].Arguments[k].Type ==
                            InstructionArgumentType.UnresolvedFunction)
                        {
                            UnresolvedFunction uf =
                                (UnresolvedFunction)source.Functions[i].Instructions[j].Arguments[k].Value;
                            if (uf.External) source.Functions[i].Instructions[j].Arguments[k].Value =
                                 source.DefinedScripts[uf.FunctionName];
                            else
                                source.Functions[i].Instructions[j].Arguments[k].Value =
                                    source.Functions.First(x => x.Name == uf.FunctionName);
                        }
                        else if (source.Functions[i].Instructions[j].Arguments[k].Type ==
                                 InstructionArgumentType.UnresolvedDefinedBuffer)
                        {
                            UnresolvedDefinedBuffer uf =
                                (UnresolvedDefinedBuffer)source.Functions[i].Instructions[j].Arguments[k].Value;
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