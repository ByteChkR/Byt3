using System;
using System.IO;
using System.Reflection;
using Byt3.OpenCL.Wrapper;
using Xunit;


[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace Byt3.OpenCL.Tests
{
    public static class TestSetup
    {
        private static KernelDatabase kernelDb;

        public static KernelDatabase KernelDb
        {
            get
            {
                if (kernelDb == null)
                {
                    var codeBaseUrl = new Uri(Assembly.GetExecutingAssembly().CodeBase);
                    var codeBasePath = Uri.UnescapeDataString(codeBaseUrl.AbsolutePath);
                    var dirPath = Path.GetDirectoryName(codeBasePath);
                    string ResourceFolder = Path.Combine(dirPath, "resources", "kernel");
                    kernelDb = new KernelDatabase(CLAPI.MainThread, ResourceFolder,
                        Wrapper.TypeEnums.DataTypes.Uchar1);
                }

                return kernelDb;
            }
        }

        //public static void ApplyDebugSettings()
        //{
        //    EngineConfig.LoadConfig("resources/engine.settings.xml", typeof(GameEngine).Assembly, "Engine.Core");
        //}
    }
}