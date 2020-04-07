using Byt3.OpenCL.Wrapper;
using Byt3.OpenCL.Wrapper.TypeEnums;

namespace Byt3.OpenFL.Tests
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
                    kernelDb = new KernelDatabase(CLAPI.MainThread, "resources/kernel",
                        DataTypes.Uchar1);
                }

                return kernelDb;
            }
        }
    }
}