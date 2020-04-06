using System.IO;
using Byt3.OpenCL.Common;

namespace Byt3.OpenCL.Wrapper
{
    /// <summary>
    /// Callback CLass that is used to make the text preprocessor compatible with the IO Wrapper system.
    /// </summary>
    public class PPIOCallbacks : IIoCallback
    {
        public bool FileExists(string file)
        {
            return Clapi.FileExists(file);
        }

        public string[] ReadAllLines(string file)
        {
            TextReader tr = new StreamReader(Clapi.GetStream(file));
            string[] ret = tr.ReadToEnd().Replace("\r", "").Split('\n');
            tr.Close();
            return ret;
        }

        public string[] GetFiles(string path, string searchPattern = "*")
        {
            return Clapi.GetFiles(path, searchPattern);
        }
    }
}