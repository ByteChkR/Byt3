using Byt3.ExtPP.Base.Interfaces;

namespace Byt3.ExtPP.API
{
    /// <summary>
    /// File Content that is used as an abstraction to files
    /// </summary>
    public class FileContent : IFileContent
    {
        private readonly string incDir;
        private readonly string[] lines;

        public FileContent(string[] lines, string incDir)
        {
            this.lines = lines;
            this.incDir = incDir;
        }

        private string Key => incDir + "/memoryFile";
        private string Path => incDir + "/memoryFile";
        public bool HasValidFilepath => false;

        public bool TryGetLines(out string[] lines)
        {
            lines = this.lines;
            return true;
        }

        public string GetKey()
        {
            return Key;
        }

        public void SetKey(string key)
        {
            //Nothing
        }

        public string GetFilePath()
        {
            return Path;
        }

        public string GetDefinedName()
        {
            return Key;
        }

        public override string ToString()
        {
            return Key;
        }
    }
}