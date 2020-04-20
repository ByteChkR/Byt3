using System;
using Byt3.ADL;
using Byt3.AssemblyGenerator;
using Byt3.AssemblyGenerator.Console;
using Byt3.Utilities.DotNet;
using Byt3.Utilities.IL;
using Byt3.Utilities.Versioning;

namespace TestingProject
{
    internal static class Program
    {
        private static void GenerateAsmGen()
        {
            const string AsmGenProjectPath =
                @"D:\Users\Tim\Documents\MasterServer\Byt3\Libraries\Byt3.AssemblyGenerator\Byt3.AssemblyGenerator.Console\Byt3.AssemblyGenerator.Console.csproj";
            ConsoleEntry c = new ConsoleEntry();
            c.Run($"AsmGen.config -c -sname AsmGen.Console -sruntime none -a {AsmGenProjectPath}".Split(' '));
            c.Run($"AsmGen.config -b".Split(' '));

        }

        private static void Main(string[] args)
        {

            ILTestClass tc = new ILTestClass();

            while (true)
            {
                Version v = Version.Parse("0.1.0.0");
                Console.Write(">");
                string format = Console.ReadLine();

                Console.WriteLine(Byt3.VersionHelper.Console.ConsoleEntry.ChangeVersion(v, format));
            }
            return;

            VersionAccumulatorManager.SearchForAssemblies();

            GenerateAsmGen();

            Debug.DefaultInitialization();

            AssemblyGeneratorGenerateModules();

            AssemblyDefinition defs = AssemblyDefinition.Load(".\\GeneratedModules\\Byt3.assemblyconfig");
            AssemblyGeneratorBuildTest(defs);
            Console.ReadLine();
        }

        private const string MSBUILD_PATH =
            "dotnet";

        private static void AssemblyGeneratorGenerateModules()
        {
            string[] blacklist = new[] { "Test", "CLI" };
            ModuleDefinition[] defs = AssemblyGenerator.GenerateModuleDefinitions(
                @"D:\Users\Tim\Documents\MasterServer\Byt3", ".\\GeneratedModules\\", false, blacklist);
            AssemblyDefinition.Save(".\\GeneratedModules\\Byt3.assemblyconfig",
                AssemblyGenerator.GenerateAssemblyDefinition("Byt3", defs));
        }

        private static void AssemblyGeneratorBuildTest(AssemblyDefinition defs)
        {
            AssemblyGenerator.GenerateAssembly(MSBUILD_PATH, defs, $".\\{defs.AssemblyName}_Build\\",
                AssemblyGeneratorBuildType.Publish, true);
        }
    }
}