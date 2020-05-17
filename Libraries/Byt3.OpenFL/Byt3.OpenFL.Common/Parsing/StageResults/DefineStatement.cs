using System;
using System.Collections.Generic;
using System.Linq;
using Byt3.OpenFL.Common.ElementModifiers;

namespace Byt3.OpenFL.Common.Parsing.StageResults
{
    public class DefineStatement : Statement
    {
        public readonly FLElementModifiers Modifiers;
        public readonly string Name;
        public readonly string[] Parameter;

        public DefineStatement(string sourceLine) : base(sourceLine)
        {
            string line = sourceLine.Trim();
            string[] parts = line.Split(':');
            Parameter = parts[1].Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            List<string> mods = parts[0].Trim().Remove(0, FLKeywords.DefineKey.Length)
                .Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries).ToList();
            if (mods.Count < 2)
            {
                throw new InvalidOperationException("Not Enough Keywords on line: " + SourceLine);
            }

            string type = mods.First();
            Name = mods.Last();
            mods.Remove(Name);

            if (type == FLKeywords.ScriptKey)
            {
                Modifiers = new FLExecutableElementModifiers(Name, mods.ToArray());
            }
            else if (type == FLKeywords.ArrayKey || type == FLKeywords.TextureKey)
            {
                Modifiers = new FLBufferModifiers(Name, mods.ToArray());
            }
            else
            {
                throw new InvalidOperationException("Invalid Define Type: " + type);
            }
        }
    }
}