using System.Collections.Generic;
using Byt3.ExtPP;
using Byt3.ExtPP.Base;
using Byt3.ExtPP.Base.Interfaces;
using Byt3.ExtPP.Base.settings;
using Byt3.OpenCL.Common.Exceptions;

namespace Byt3.OpenCL.Common.ExtPP.API
{
    /// <summary>
    /// Abstract PreProcessor Configuration
    /// </summary>
    public abstract class APreProcessorConfig
    {
        protected abstract Verbosity VerbosityLevel { get; }
        protected abstract List<AbstractPlugin> Plugins { get; }
        public abstract string GetGenericInclude(string filename, string[] genType);

        public string[] Preprocess(IFileContent filename, Dictionary<string, bool> defs)
        {
            PreProcessor pp = new PreProcessor();

            PPLogger.Instance.VerbosityLevel = VerbosityLevel;


            pp.SetFileProcessingChain(Plugins);

            Definitions definitions;
            if (defs == null)
            {
                definitions = new Definitions();
            }
            else
            {
                definitions = new Definitions(defs);
            }

            string[] ret = { "FILE NOT FOUND" };
            try
            {
                ret = pp.Run(new[] { filename }, new Settings(), definitions);
            }
            catch (ProcessorException ex)
            {
                DebugHelper.Crash(
                    new TextProcessingException("Could not preprocess file: " + filename.GetFilePath(), ex), true);
            }

            return ret;
        }
    }
}