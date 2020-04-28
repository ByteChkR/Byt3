namespace Byt3Console.OpenFL.ScriptGenerator.Commands
{
    public struct FLScriptGeneratorSettings
    {
        public int Amount;
        public string OutputFolder;
        public Range Functions;
        public Range Buffers;
        public Range Additional;
        public Range AdditionalFunctions;

        public static FLScriptGeneratorSettings Default =>
            new FLScriptGeneratorSettings
            {
                Amount = 10,
                Functions = new Range { Min = 3, Max = 10 },
                Buffers = new Range { Min = 1, Max = 6 },
                Additional = new Range { Min = 0, Max = 3 },
                AdditionalFunctions = new Range { Min = 3, Max = 10 },
                OutputFolder = "output"
            };
    }
}