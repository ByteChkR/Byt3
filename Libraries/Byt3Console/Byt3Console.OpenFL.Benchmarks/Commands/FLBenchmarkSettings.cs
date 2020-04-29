namespace Byt3Console.OpenFL.Benchmarks.Commands
{
    public class FLBenchmarkSettings
    {
        public int ExecutionIterations;
        public int ParsingIterations;
        public int IOIterations;
        public int InitIterations;
        public string PerformanceFolder;
        public string ExtraSteps;
        public string ScriptDirectories;
        public int WorkSizeMultiplier;
        public int TotalRepetitions;
        public bool WarmProgram;

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
                TotalRepetitions = 5
            };
    }
}