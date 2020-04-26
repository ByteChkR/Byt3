using System;

namespace Byt3.OpenFL.Parsing.StageResults
{
    public class StaticInstruction
    {
        public string Key;
        public string[] Arguments;

        public StaticInstruction(string[] lineParts)
        {
            Key = lineParts[0];
            Arguments = new string[lineParts.Length - 1];
            Array.Copy(lineParts, 1, Arguments, 0, lineParts.Length - 1);
        }
    }
}