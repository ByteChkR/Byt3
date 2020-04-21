namespace Byt3.OpenFL.Parsing.StageResults
{
    public class StaticInspectionResult
    {
        public string Filename { get; }
        public string[] Source { get; }
        public string[] DefinedBuffers { get; }
        public string[] Functions { get; }
        public string[] DefinedScripts { get; }

        public StaticInspectionResult(string filename, string[] source, string[] functions,
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