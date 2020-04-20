using System;
using System.IO;
using System.Reflection;
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
            VersionAccumulatorManager.SearchForAssemblies();
            Debug.DefaultInitialization();

            ILTestClass tc = new ILTestClass();

            Version v = Version.Parse("1.0.0.0");
            while (true)
            {
                System.Console.WriteLine(v);
                Console.Write(">");
                string format = Console.ReadLine();
                if (format == "r")
                {
                    v = Version.Parse("1.0.0.0");
                    continue;
                }

                v = Byt3.VersionHelper.Console.ConsoleEntry.ChangeVersion(v, format);
            }
            return;



            GenerateAsmGen();


            AssemblyGeneratorGenerateModules();

            AssemblyDefinition defs = AssemblyDefinition.Load(".\\GeneratedModules\\Byt3.assemblyconfig");
            AssemblyGeneratorBuildTest(defs);
            Console.ReadLine();
        }

        private const string MSBUILD_PATH =
            "dotnet";

        private static void AssemblyGeneratorGenerateModules()
        {
            string[] blacklist = new[] { "Test" };
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