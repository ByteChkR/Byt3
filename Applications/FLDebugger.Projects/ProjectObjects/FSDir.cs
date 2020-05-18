using System.IO;
using System.Windows.Forms;

namespace FLDebugger.Projects.ProjectObjects
{
    public class FSDir : FSEntry
    {
        public FSDir(string path) : base(path)
        {
            DirectoryInfo di = new DirectoryInfo(path);

            foreach (DirectoryInfo entry in di.GetDirectories("*", SearchOption.TopDirectoryOnly))
            {
                Nodes.Add(new FSDir(entry.FullName));
            }

            FileInfo[] files = di.GetFiles("*", SearchOption.TopDirectoryOnly);
            for (int i = 0; i < files.Length; i++)
            {
                FileInfo fileInfo = files[i];
                Nodes.Add(new FSFile(fileInfo.FullName));
            }
        }


        internal FSEntry Find(string fullPath)
        {
            foreach (TreeNode treeNode in Nodes)
            {
                FSEntry e = (FSEntry) treeNode;
                if (fullPath == e.EntryPath)
                {
                    return e;
                }

                if (fullPath.Contains(e.EntryPath))
                {
                    FSDir dir = (FSDir) e;
                    FSEntry ret = dir.Find(fullPath);
                    return ret;
                }
            }

            return null;
        }
    }
}