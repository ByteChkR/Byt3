using System.IO;

namespace FLDebugger.Projects.ProjectObjects
{
    public class FSFile : FSEntry
    {
        public FSFile(string path) : base(path)
        {
        }

        public string GetExtension()
        {
            return Path.GetExtension(EntryPath);
        }
    }
}