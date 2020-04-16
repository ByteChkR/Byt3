using System;
using Byt3.ADL;
using Byt3.AssemblyGenerator;
using Byt3.Utilities.DotNet;

namespace TestingProject
{
    internal static class Program
    {
        private static void Main(string[] args)
        {

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
            string[] blacklist = new[] {"Test", "CLI"};
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