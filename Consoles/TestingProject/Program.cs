using System;
using Byt3.ADL;
using Byt3.ADL.Configs;
using Byt3.AssemblyGenerator;
using Byt3.Engine.Tutorials;
using Byt3.Utilities.DotNet;
using TestingProjectConsole;

namespace TestingProject
{
    internal static class Program
    {
        private static readonly ADLLogger<LogType> Logger = new ADLLogger<LogType>(new ProjectDebugConfig("Test Project", (int)LogType.All, 10, PrefixLookupSettings.AddPrefixIfAvailable));
        internal static bool Exit = false;
        private static void GenerateAsmGen()
        {
            const string AsmGenProjectPath =
                @"D:\Users\Tim\Documents\Byt3\Libraries\Byt3.AssemblyGenerator\Byt3.AssemblyGenerator.Console\Byt3.AssemblyGenerator.Console.csproj";
            ConsoleEntry c = new ConsoleEntry();
            c.Run($"AsmGen.config -c -sname AsmGen.Console -sruntime none -a {AsmGenProjectPath}".Split(' '));
            c.Run($"AsmGen.config -b".Split(' '));
        }

        private static void Main(string[] args)
        {

            Debug.DefaultInitialization();

            new ConsoleEntry().Run(args);
            Console.ReadLine();
            return;


            //ILTestClass tc = new ILTestClass();


            //GenerateAsmGen();


            AssemblyGeneratorGenerateModules();

            AssemblyDefinition defs = AssemblyDefinition.Load(".\\GeneratedModules\\Byt3.assemblyconfig");
            AssemblyGeneratorBuildTest(defs);
            System.Console.ReadLine();
        }

        private const string MSBUILD_PATH =
            "dotnet";

        private static void AssemblyGeneratorGenerateModules()
        {
            string[] blacklist = new[] { "Test" };
            ModuleDefinition[] defs = AssemblyGenerator.GenerateModuleDefinitions(
                @"D:\Users\Tim\Documents\Byt3", ".\\GeneratedModules\\", false, blacklist);
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