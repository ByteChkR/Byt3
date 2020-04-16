using System.Collections.Generic;
using Byt3.ADL;
using Byt3.ExtPP.API.Exceptions;
using Byt3.ExtPP.Base;
using Byt3.ExtPP.Base.Interfaces;
using Byt3.ExtPP.Base.Plugins;
using Byt3.ExtPP.Base.settings;

namespace Byt3.ExtPP.API.Configuration
{
    /// <summary>
    /// Abstract PreProcessor Configuration
    /// </summary>
    public abstract class APreProcessorConfig : ALoggable<LogType>
    {
        protected APreProcessorConfig() : base(ExtPPDebugConfig.Settings)
        {
        }

        public abstract string FileExtension { get; }
        protected abstract List<AbstractPlugin> Plugins { get; }
        public abstract string GetGenericInclude(string filename, string[] genType);

        public string[] Preprocess(IFileContent filename, Dictionary<string, bool> defs)
        {
            PreProcessor pp = new PreProcessor();


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

            string[] ret;
            try
            {
                ret = pp.Run(new[] {filename}, new Settings(), definitions);
            }
            catch (ProcessorException ex)
            {
                throw
                    new TextProcessingException("Could not preprocess file: " + filename.GetFilePath(), ex);
            }

            return ret;
        }
    }
}