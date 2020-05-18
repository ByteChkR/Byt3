using System.IO;
using System.Windows.Forms;

namespace FLDebugger.Projects.ProjectObjects
{
    public abstract class FSEntry : TreeNode
    {
        internal static int Entries;
        public readonly string EntryPath;

        protected FSEntry(string path)
        {
            Entries++;
            Text = Path.GetFileName(path);
            EntryPath = path;
        }

        public virtual string GetName()
        {
            return Path.GetFileName(EntryPath);
        }
    }
}