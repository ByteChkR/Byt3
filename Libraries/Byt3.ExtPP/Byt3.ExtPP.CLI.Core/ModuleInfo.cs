using Byt3.CommandRunner;

namespace Byt3.ExtPP.CLI.Core
{
    public class ModuleInfo : AbstractCmdModuleInfo
    {
        public override string[] Dependencies => new[]
        {
            "Byt3.ADL.dll",
            "Byt3.ADL.Crash.dll",
            "Byt3.ExtPP.Base.dll",
            "Byt3.ExtPP.dll"
        };
        public override string ModuleName => "extpp";
        public override void RunArgs(string[] args)
        {
            CLI.Main(args);
        }
    }
}