using Byt3.OpenCL.Wrapper;
using Byt3.OpenCL.Wrapper.TypeEnums;

namespace Byt3.OpenFL.Tests
{
    public static class TestSetup
    {
        private static KernelDatabase _kernelDb;

        public static KernelDatabase KernelDb
        {
            get
            {
                if (_kernelDb == null)
                {
                    _kernelDb = new KernelDatabase(Clapi.MainThread, "resources/kernel",
                        DataTypes.Uchar1);
                }

                return _kernelDb;
            }
        }
    }
}