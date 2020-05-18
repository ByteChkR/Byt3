using System;
using Byt3.OpenFL.Common.Exceptions;

namespace Byt3.OpenFL.Parsing.Stages
{
    internal class StaticFunctionHeader
    {
        public readonly string FunctionName;
        public readonly string[] Modifiers;

        public StaticFunctionHeader(string functionHeader)
        {
            string[] f = functionHeader.Split(new[] {':'}, StringSplitOptions.None);
            if (f.Length == 1)
            {
                throw new FLInvalidFunctionUseException(functionHeader, "Invalid line.");
            }

            FunctionName = f[0];
            Modifiers = f[1].Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}