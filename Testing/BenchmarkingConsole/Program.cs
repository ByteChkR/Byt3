using System;
using BenchmarkDotNet.Running;

namespace BenchmarkingConsole
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<ILExecutionBenchmark>();

            Console.ReadLine();
        }
    }
}
