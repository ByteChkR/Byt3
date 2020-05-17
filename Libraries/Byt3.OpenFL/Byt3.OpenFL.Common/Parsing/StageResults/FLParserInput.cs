using System.Collections.Generic;

namespace Byt3.OpenFL.Common.Parsing.StageResults
{
    public class FLParserInput
    {
        public Dictionary<string, bool> Defines;

        public FLParserInput(string filename, bool mainFile, params string[] defines)
        {
            MainFile = mainFile;
            Filename = filename;
            Defines = new Dictionary<string, bool>();
            Defines["IMPORTED"] = !MainFile;
            Defines["ENTRY"] = MainFile;

            foreach (string define in defines)
            {
                Defines[define] = true;
            }
        }

        public FLParserInput(string filename, params string[] defines) : this(filename, true, defines)
        {
        }

        public FLParserInput(string filename, string[] source, bool mainFile, params string[] defines) : this(filename,
            mainFile, defines)
        {
            Source = source;
        }

        public string Filename { get; }
        public bool MainFile { get; }
        public string[] Source { get; }
    }
}