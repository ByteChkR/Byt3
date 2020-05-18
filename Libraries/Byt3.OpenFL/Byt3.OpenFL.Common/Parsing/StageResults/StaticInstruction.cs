using System;
using Byt3.Utilities.FastString;

namespace Byt3.OpenFL.Common.Parsing.StageResults
{
    public class StaticInstruction
    {
        public string[] Arguments;
        public string Key;

        public StaticInstruction(string[] lineParts)
        {
            Key = lineParts[0];
            Arguments = new string[lineParts.Length - 1];
            Array.Copy(lineParts, 1, Arguments, 0, lineParts.Length - 1);
        }

        public override string ToString()
        {
            return $"{Key} " + Arguments.Unpack(" ");
        }
    }
}