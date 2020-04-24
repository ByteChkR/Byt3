using System.IO;

namespace Byt3.Callbacks
{
    public static class IOManager
    {
        public static IOCallback Callback { get; private set; } = new DefaultIOCallback();

        public static void SetIOCallback(IOCallback callback)
        {
            Callback = callback;
        }

        public static bool FileExists(string file)
        {
            return Callback.FileExists(file);
        }
        public static bool DirectoryExists(string file)
        {
            return Callback.DirectoryExists(file);
        }

        public static string[] ReadAllLines(string file)
        {
            return Callback.ReadLines(file);
        }
        public static string ReadText(string file)
        {
            return Callback.ReadText(file);
        }

        public static Stream GetStream(string file)
        {
            return Callback.GetStream(file);
        }

        public static string[] GetFiles(string path, string searchPattern = "*.*")
        {
            return Callback.GetFiles(path, searchPattern);
        }
    }
}