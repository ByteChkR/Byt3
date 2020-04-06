using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Byt3.ExtPP.Base;
using Byt3.ExtPP.Base.Interfaces;

namespace Byt3.OpenCL.Common.ExtPP.API
{
    /// <summary>
    /// A static Wrapper class around the ext_pp project.
    /// </summary>
    public static class TextProcessorAPI
    {
        public static IIOCallback PpCallback = null;

        private static Dictionary<string, APreProcessorConfig> _configs = new Dictionary<string, APreProcessorConfig>
        {
            {".fl", new FLPreProcessorConfig()},
            {".vs", new GLCLPreProcessorConfig()},
            {".fs", new GLCLPreProcessorConfig()},
            {".cl", new GLCLPreProcessorConfig()},
            {"***", new DefaultPreProcessorConfig()}
        };

        public static string[] GenericIncludeToSource(string ext, string file, params string[] genType)
        {
            return new[] { _configs[ext].GetGenericInclude(file, genType) };
        }

        public static string[] PreprocessLines(string filename, Dictionary<string, bool> defs)
        {
            return PreprocessLines(new FilePathContent(filename), defs);
        }

        public static string[] PreprocessLines(string[] lines, string incDir, Dictionary<string, bool> defs)
        {
            return PreprocessLines(new FileContent(lines, incDir), defs);
        }

        internal static string[] PreprocessLines(IFileContent file, Dictionary<string, bool> defs)
        {
            //TODO: Check if this works
            string ext = new string(file.GetFilePath().Reverse().Take(3).Reverse().ToArray());
            string key = "";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                key = "WIN";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                key = "OSX";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                key = "LINUX";
            }

            if (defs == null) defs = new Dictionary<string, bool>();
            if (!defs.ContainsKey(key))
            {
                defs.Add(key, true);
            }

            if (_configs.ContainsKey(ext))
            {
                DebugHelper.Log("Found Matching PreProcessor Config for: " + ext, 1 | (1 << 21));
                return _configs[ext].Preprocess(file, defs);
            }

            DebugHelper.Log("Loading File with Default PreProcessing", 1 | (1 << 21));
            return _configs["***"].Preprocess(file, defs);
        }


        public static string PreprocessSource(string filename, Dictionary<string, bool> defs)
        {
            return PreprocessSource(new FilePathContent(filename), defs);
        }

        public static string PreprocessSource(string[] lines, string incDir, Dictionary<string, bool> defs)
        {
            return PreprocessSource(new FileContent(lines, incDir), defs);
        }


        /// <summary>
        /// Loads and preprocesses the file specified
        /// </summary>
        /// <param name="filename">the filepath</param>
        /// <param name="defs">definitions</param>
        /// <returns>the source as string</returns>
        internal static string PreprocessSource(IFileContent filename, Dictionary<string, bool> defs)
        {
            StringBuilder sb = new StringBuilder();
            string[] src = PreprocessLines(filename, defs);
            for (int i = 0; i < src.Length; i++)
            {
                sb.Append(src[i] + "\n");
            }

            return sb.ToString();
        }


    }
}