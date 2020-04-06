using System.IO;
using Byt3.OpenCL.Wrapper;

namespace Byt3.OpenCL.Tests
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
                    string s = Directory.GetCurrentDirectory();
                    _kernelDb = new KernelDatabase(CLAPI.MainThread, "resources/kernel",
                        Wrapper.TypeEnums.DataTypes.Uchar1);
                }

                return _kernelDb;
            }
        }

        //public static void ApplyDebugSettings()
        //{
        //    EngineConfig.LoadConfig("resources/engine.settings.xml", typeof(GameEngine).Assembly, "Engine.Core");
        //}
    }
}