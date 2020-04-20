namespace Byt3.OpenFL.Parsing.StageResults
{
    public class LoadSourceStageResult
    {
        public string Filename { get; }
        public string[] Source { get; }

        public LoadSourceStageResult(string filename, string[] source)
        {
            Filename = filename;
            Source = source;
        }
    }
}