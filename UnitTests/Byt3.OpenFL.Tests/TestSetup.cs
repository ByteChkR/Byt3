using System.Reflection;
using Byt3.ADL;
using Byt3.ExtPP.Base;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenFL.Benchmarking;
using Byt3.OpenFL.Common;
using Byt3.OpenFL.Common.Instructions;
using Byt3.Utilities.ManifestIO;

namespace Byt3.OpenFL.Tests
{
    internal static class TestSetup
    {
        internal static void SetupTestingSession()
        {
            ExtPPDebugConfig.Settings.MinSeverity = Verbosity.Level1;
            OpenFLDebugConfig.Settings.MinSeverity = Verbosity.Level1;
            OpenCLDebugConfig.Settings.MinSeverity = Verbosity.Level1;
            LoadEmbeddedFiles();
        }

        private static void LoadEmbeddedFiles()
        {
            ManifestReader.RegisterAssembly(Assembly.GetExecutingAssembly());
            ManifestReader.RegisterAssembly(typeof(OpenFLBenchmarks).Assembly);
            ManifestReader.RegisterAssembly(typeof(KernelFLInstruction).Assembly);
            ManifestReader.PrepareManifestFiles(false);
            EmbeddedFileIOManager.Initialize();

        }
    }
}