using System.Linq;
using Byt3.OpenFL.Common.ProgramChecks;
using Byt3.Utilities.FastString;

namespace Byt3Console.OpenFL.Benchmarks.Commands
{
    public class FLBenchmarkSettings
    {
        public FLProgramCheckType CheckProfile;
        public int ExecutionIterations;
        public string ExtraSteps;
        public int InitIterations;
        public int IOIterations;
        public int ParsingIterations;
        public string PerformanceFolder;
        public string ScriptDirectories;
        public int TotalRepetitions;
        public bool WarmProgram;
        public int WorkSizeMultiplier;

        public static FLBenchmarkSettings Default =>
            new FLBenchmarkSettings
            {
                ExecutionIterations = 5,
                ParsingIterations = 20,
                IOIterations = 100,
                InitIterations = 100,
                PerformanceFolder = "performance",
                WorkSizeMultiplier = 2,
                ExtraSteps = "",
                ScriptDirectories = "resources/filter/tests",
                TotalRepetitions = 5,
                CheckProfile = FLProgramCheckType.InputValidation
            };
    }
}