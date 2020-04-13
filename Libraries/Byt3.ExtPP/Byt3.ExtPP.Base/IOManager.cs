using Byt3.Callbacks;

namespace Byt3.ExtPP.Base
{
    public class IOManager
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

        public static string[] ReadAllLines(string file)
        {
            return Callback.ReadLines(file);
        }

        public static string[] GetFiles(string path, string searchPattern = "*.*")
        {
            return Callback.GetFiles(path, searchPattern);
        }
    }
}