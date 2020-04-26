using System.Collections.Generic;
using Byt3.OpenFL.Common.ProgramChecks;

namespace Byt3.OpenFL.Parsing.StageResults
{
    public class StaticInspectionResult : FLPipelineResult
    {
        public string Filename { get; }
        public List<string> Source { get; }
        public string[] DefinedBuffers { get; }
        public List<StaticFunction> Functions { get; }
        public string[] DefinedScripts { get; }

        public StaticInspectionResult(string filename, List<string> source, List<StaticFunction> functions,
            string[] definedBuffers, string[] definedScripts)
        {
            Filename = filename;
            Source = source;
            Functions = functions;
            DefinedBuffers = definedBuffers;
            DefinedScripts = definedScripts;
        }
    }
}