namespace Byt3Console.OpenFL.ScriptGenerator.Settings
{
    public class FLScriptGeneratorSettings
    {
        public Range Additional;
        public Range AdditionalFunctions;
        public int Amount;
        public Range Buffers;
        public Range Functions;
        public string OutputFolder;

        public static FLScriptGeneratorSettings Default =>
            new FLScriptGeneratorSettings
            {
                Amount = 10,
                Functions = new Range {Min = 3, Max = 10},
                Buffers = new Range {Min = 1, Max = 6},
                Additional = new Range {Min = 0, Max = 3},
                AdditionalFunctions = new Range {Min = 3, Max = 10},
                OutputFolder = "output"
            };
    }
}