using System;
using System.IO;
using System.Reflection;

namespace Byt3.Console.Runner
{
    public static class ConsolePaths
    {
        public static readonly string ConsoleFolder = Path.GetDirectoryName(Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).AbsolutePath));
        public static readonly string AssemblyPath = Path.Combine(ConsoleFolder, "consoles");
        public static readonly string RunnerConfig = Path.Combine(ConsoleFolder, "RunnerConfig.xml");

        public static void SetUpPaths()
        {
            if (!Directory.Exists(AssemblyPath)) Directory.CreateDirectory(AssemblyPath);
        }
    }
}