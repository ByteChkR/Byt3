using System.IO;
using System.Windows.Forms;

namespace FLDebugger.Projects.ProjectObjects
{
    public class WorkingDirectory
    {
        internal static int FilesMax;
        private readonly FileSystemWatcher directoryWatcher;
        private readonly FSDir tree;

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

        public bool IsDirty { get; private set; }
        public string Directory { get; internal set; }

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