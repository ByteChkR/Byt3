using System.Collections.Generic;
using System.IO;
using System.Linq;
using Byt3.ExtPP.Base;
using Byt3.ExtPP.Base.Interfaces;
using Byt3.ExtPP.Base.Plugins;
using Byt3.ExtPP.Base.settings;

namespace Byt3.ExtPP.Tests
{
    public static class TestHelper
    {


        private static string ResourceFolderBackingProperty { get; } = Path.GetFullPath("../../../res/");
        public static string ResourceFolder { get; private set; } = "";
        
        public static void SetupPath()
        {
            if (ResourceFolder == "")
            {
                ResourceFolder = ResourceFolderBackingProperty;
            }
        }

        private static PreProcessor SetUp(List<AbstractPlugin> chain)
        {
            PreProcessor pp = new PreProcessor();
            pp.SetFileProcessingChain(chain);
            return pp;
        }

        public static ISourceScript[] SetUpAndProcess(List<AbstractPlugin> chain, Settings settings, IDefinitions definitions, params string[] fileNames)
        {

            PreProcessor pp = SetUp(chain);
            return pp.ProcessFiles(fileNames.Select(x=>new FilePathContent(Path.GetFullPath(x))).OfType<IFileContent>().ToArray(), settings, definitions);
        }


        public static ISourceScript[] SetUpAndProcess(List<AbstractPlugin> chain, params string[] fileNames)
        {
            return SetUpAndProcess(chain, new Settings(), new Definitions(), fileNames);
        }

        public static ISourceScript[] SetUpAndProcess(List<AbstractPlugin> chain, IDefinitions definitions,
            params string[] fileNames)
        {

            return SetUpAndProcess(chain, new Settings(), definitions, fileNames);
        }

        public static ISourceScript[] SetUpAndProcess(List<AbstractPlugin> chain, Settings settings, params string[] fileNames)
        {
            return SetUpAndProcess(chain, settings, new Definitions(), fileNames);
        }

        public static string[] SetUpAndCompile(List<AbstractPlugin> chain, Settings settings, IDefinitions definitions, params string[] fileNames)
        {
            PreProcessor pp = SetUp(chain);
            
            return pp.Run(fileNames.Select(x=>new FilePathContent(x)).OfType<IFileContent>().ToArray(), settings, definitions);
        }

        public static string[] SetUpAndCompile(List<AbstractPlugin> chain, params string[] fileNames)
        {
            return SetUpAndCompile(chain, new Settings(), new Definitions(), fileNames);
        }

        public static string[] SetUpAndCompile(List<AbstractPlugin> chain, IDefinitions definitions,
            params string[] fileNames)
        {

            return SetUpAndCompile(chain, new Settings(), definitions, fileNames);
        }

        public static string[] SetUpAndCompile(List<AbstractPlugin> chain, Settings settings, params string[] fileNames)
        {
            return SetUpAndCompile(chain, settings, new Definitions(), fileNames);
        }


    }
}