using System.IO;
using System.Windows.Forms;

namespace FLDebugger.Projects
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

        public virtual string GetName() => Path.GetFileName(EntryPath);
    }

    public class FSFile : FSEntry
    {
        public FSFile(string path) : base(path)
        {
        }

        public string GetExtension() => Path.GetExtension(EntryPath);
    }

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
                FSEntry e = (FSEntry)treeNode;
                if (fullPath == e.EntryPath) return e;
                if (fullPath.Contains(e.EntryPath))
                {
                    FSDir dir = (FSDir)e;
                    FSEntry ret = dir.Find(fullPath);
                    return ret;
                }
            }

            return null;
        }
    }

    public class WorkingDirectory
    {
        public bool IsDirty { get; private set; }
        public string Directory { get; internal set; }
        private FileSystemWatcher directoryWatcher;
        private FSDir tree;
        internal static int FilesMax;

        public WorkingDirectory(string directory)
        {
            Directory = directory;
            FilesMax = new DirectoryInfo(Directory).GetFileSystemInfos("*", SearchOption.AllDirectories).Length;
            tree = new FSDir(Directory);
            directoryWatcher = new FileSystemWatcher(directory);
            directoryWatcher.EnableRaisingEvents = true;
            directoryWatcher.Renamed += OnEntryRenamed;
            directoryWatcher.Changed += OnEntryChanged;
            directoryWatcher.Created += OnEntryCreated;
            directoryWatcher.Deleted += OnEntryDeleted;
            directoryWatcher.Error += OnWatcherError;
        }

        public void UpdateTreeView(TreeView tv)
        {
            if (IsDirty)
            {
                //FilesMax = new DirectoryInfo(Directory).GetFileSystemInfos("*", SearchOption.AllDirectories).Length;
                IsDirty = false;
                //tree = new FSDir(Directory);
            }
            tv.Nodes.Clear();
            tv.Nodes.Add(tree);
        }

        private void OnWatcherError(object sender, ErrorEventArgs e)
        {
            throw e.GetException();
        }

        private void OnEntryDeleted(object sender, FileSystemEventArgs e)
        {
            IsDirty = true;
        }

        private void OnEntryCreated(object sender, FileSystemEventArgs e)
        {
            IsDirty = true;
        }

        private void OnEntryChanged(object sender, FileSystemEventArgs e)
        {
            IsDirty = true;
        }

        private void OnEntryRenamed(object sender, RenamedEventArgs e)
        {
            IsDirty = true;
        }
    }
}