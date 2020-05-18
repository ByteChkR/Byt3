using System.Collections.Generic;
using Byt3.OpenFL.Common.ProgramChecks;

namespace Byt3.OpenFL.Common.Parsing.StageResults
{
    public class StaticInspectionResult : FLPipelineResult
    {
        private ImportOptions Options;

        public StaticInspectionResult(string filename, List<string> source, List<StaticFunction> functions,
            DefineStatement[] definedBuffers, DefineStatement[] definedScripts, ImportOptions options)
        {
            Options = options;
            Filename = filename;
            Source = source;
            Functions = functions;
            DefinedBuffers = definedBuffers;
            DefinedScripts = definedScripts;
        }

        public string Filename { get; }
        public List<string> Source { get; }
        public DefineStatement[] DefinedBuffers { get; }
        public List<StaticFunction> Functions { get; }
        public DefineStatement[] DefinedScripts { get; }
    }
}