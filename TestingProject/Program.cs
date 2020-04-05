using System;
using System.Threading;
using Byt3.BuildSystem;

namespace TestingProject
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            AssemblyGeneratorGenerateModules();

            AssemblyDefinitions defs = AssemblyDefinitions.Load(".\\GeneratedModules\\Byt3.assemblyconfig");
            AssemblyGeneratorBuildTest(defs);
            Console.ReadLine();
            return;
        }



        private static void AssemblyGeneratorGenerateModules()
        {
            string[] blacklist = new []{"Test"};
            AssemblyGenerator.GenerateModuleDefinitions(@"D:\Users\Tim\Documents\MasterServer\Byt3", ".\\GeneratedModules\\", false, blacklist);
            AssemblyGenerator.GenerateAssemblyDefinitions("Byt3", ".\\GeneratedModules\\");
        }

        private static void AssemblyGeneratorBuildTest(AssemblyDefinitions defs)
        {
            AssemblyGenerator.GenerateAssembly(defs, $".\\{defs.AssemblyName}_Build\\");
        }

    }
}
