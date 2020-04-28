using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Byt3.ADL;
using Byt3.ExtPP.API.Configuration;
using Byt3.ExtPP.Base;
using Byt3.ExtPP.Base.Interfaces;

namespace Byt3.ExtPP.API
{
    /// <summary>
    /// A static Wrapper class around the ext_pp project.
    /// </summary>
    public static class TextProcessorAPI
    {
        private static readonly ADLLogger<LogType> Logger =
            new ADLLogger<LogType>(ExtPPDebugConfig.Settings, "TextProcessorAPI");

        private static Dictionary<string, APreProcessorConfig> _configs;

        public static Dictionary<string, APreProcessorConfig> Configs
        {
            get
            {
                if (_configs == null)
                {
                    _configs = new Dictionary<string, APreProcessorConfig> {["***"] = new DefaultPreProcessorConfig()};
                }

                return _configs;
            }
            set => _configs = value;
        }

        public static string[] GenericIncludeToSource(string ext, string file, params string[] genType)
        {
            return new[] {Configs[ext].GetGenericInclude(file, genType)};
        }


        #region Preprocess Lines

        public static string[] PreprocessLines(string filename, Dictionary<string, bool> defs)
        {
            return PreprocessLines(new FilePathContent(filename, filename), defs);
        }

        public static string[] PreprocessLines(string[] lines, string incDir, string ext, Dictionary<string, bool> defs)
        {
            return PreprocessLines(new FileContent(lines, incDir, ext), defs);
        }

        internal static string[] PreprocessLines(IFileContent file, Dictionary<string, bool> defs)
        {
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

            if (defs == null)
            {
                defs = new Dictionary<string, bool>();
            }

            if (!defs.ContainsKey(key))
            {
                defs.Add(key, true);
            }


            if (Configs.ContainsKey(ext))
            {
                Logger.Log(LogType.Log, "Found Matching PreProcessor Config for: " + ext, 5);
                return Configs[ext].Preprocess(file, defs);
            }

            Logger.Log(LogType.Log, "Loading File with Default PreProcessing", 5);
            return Configs["***"].Preprocess(file, defs);
        }

        #endregion

        #region Preprocess Source

        public static string PreprocessSource(string filename, Dictionary<string, bool> defs)
        {
            return PreprocessSource(new FilePathContent(filename, filename), defs);
        }

        public static string PreprocessSource(string[] lines, string incDir, string ext, Dictionary<string, bool> defs)
        {
            return PreprocessSource(new FileContent(lines, incDir, ext), defs);
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

        #endregion
    }
}