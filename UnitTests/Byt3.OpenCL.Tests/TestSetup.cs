using System;
using System.IO;
using System.Reflection;
using Byt3.OpenCL.Wrapper;


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
                    Uri codeBaseUrl = new Uri(Assembly.GetExecutingAssembly().CodeBase);
                    string codeBasePath = Uri.UnescapeDataString(codeBaseUrl.AbsolutePath);
                    string dirPath = Path.GetDirectoryName(codeBasePath);
                    string ResourceFolder = Path.Combine(dirPath, "resources", "kernel");
                    kernelDb = new KernelDatabase(CLAPI.MainThread, ResourceFolder,
                        Wrapper.TypeEnums.DataVectorTypes.Uchar1);
                }

                return kernelDb;
            }
        }
        
    }
}