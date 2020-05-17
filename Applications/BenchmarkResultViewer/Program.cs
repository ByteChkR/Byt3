using System;

namespace BenchmarkResultViewer
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            new FLBenchmarkViewerConsole().Run(args);
        }
    }
}