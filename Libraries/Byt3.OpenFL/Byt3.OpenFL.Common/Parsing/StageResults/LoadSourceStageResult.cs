using System.Collections.Generic;

namespace Byt3.OpenFL.Parsing.StageResults
{
    public class LoadSourceStageResult
    {
        public string Filename { get; }
        public List<string> Source { get; }

        public LoadSourceStageResult(string filename, List<string> source)
        {
            Filename = filename;
            Source = source;
        }
    }
}