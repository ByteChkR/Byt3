using Byt3.ADL;
using Byt3.ExtPP.Base;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenFL.Common;

namespace Byt3.OpenFL.Tests
{
    internal static class TestSetup
    {
        internal static void SetupLogOutput()
        {
            ExtPPDebugConfig.Settings.MinSeverity = Verbosity.Level1;
            OpenFLDebugConfig.Settings.MinSeverity = Verbosity.Level1;
            OpenCLDebugConfig.Settings.MinSeverity = Verbosity.Level1;
        }
    }
}