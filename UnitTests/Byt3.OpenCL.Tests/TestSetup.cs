using System.IO;
using System.Reflection;
using Byt3.OpenCL.Wrapper;
using Byt3.Utilities.ManifestIO;


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
                    string resourceFolder = Path.Combine("resources", "kernel");

                    EmbeddedFileIOManager.Initialize();
                    ManifestReader.RegisterAssembly(Assembly.GetExecutingAssembly());

                    kernelDb = new KernelDatabase(CLAPI.MainThread, resourceFolder,
                        Wrapper.TypeEnums.DataVectorTypes.Uchar1);
                }

                return kernelDb;
            }
        }
    }
}