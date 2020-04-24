using System.IO;
using Byt3.Callbacks;
using Byt3.ExtPP.Base.Interfaces;

namespace Byt3.ExtPP.Base
{
    public class FilePathContent : IFileContent
    {
        private readonly string filePath;
        private string key;
        private string definedName;
        public bool HasValidFilepath => true;

        public FilePathContent(string filePath, string definedName)
        {

            this.definedName = definedName;
            key = this.filePath = filePath;
        }

        public bool TryGetLines(out string[] lines)
        {
            lines = null;
            if (!IOManager.FileExists(filePath))
            {
                return false;
            }

            lines = IOManager.ReadAllLines(filePath);

            return true;
        }

        public string GetKey()
        {
            return key;
        }

        public void SetKey(string key)
        {
            this.key = key;
        }

        public string GetDefinedName()
        {
            return definedName;
        }

        public string GetFilePath()
        {
            return filePath;
        }

        public override string ToString()
        {
            return key;
        }
    }
}