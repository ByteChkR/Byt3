using System.IO;
using Byt3.ExtPP.Base;

namespace Byt3.OpenCL.Common.ExtPP.API
{
    /// <summary>
    /// IO Callbacks for the IO Operations of the Text Processor
    /// </summary>
    public class PPCallbacks : IOCallbacks
    {
        public override bool FileExists(string file)
        {
            if (TextProcessorAPI.PpCallback == null)
            {
                return base.FileExists(file);
            }

            string p = file.Remove(0, Directory.GetCurrentDirectory().Length);// Path.GetRelativePath(Directory.GetCurrentDirectory(), file);
            return TextProcessorAPI.PpCallback.FileExists(p);
        }

        public override string[] ReadAllLines(string file)
        {
            if (TextProcessorAPI.PpCallback == null)
            {
                return base.ReadAllLines(file);
            }

            
            string p = file.Remove(0, Directory.GetCurrentDirectory().Length);
            return TextProcessorAPI.PpCallback.ReadAllLines(p);
        }

        public override string[] GetFiles(string path, string searchPattern = "*")
        {
            if (TextProcessorAPI.PpCallback == null)
            {
                return base.GetFiles(path, searchPattern);
            }

            string p = path.Remove(0, Directory.GetCurrentDirectory().Length);
            return TextProcessorAPI.PpCallback.GetFiles(p, searchPattern);
        }
    }
}