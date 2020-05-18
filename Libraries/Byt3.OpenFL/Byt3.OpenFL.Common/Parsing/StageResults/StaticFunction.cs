using System;
using System.Linq;
using Byt3.OpenFL.Common.ElementModifiers;

namespace Byt3.OpenFL.Common.Parsing.StageResults
{
    public class StaticFunction
    {
        public readonly StaticInstruction[] Body;
        public readonly FLFunctionElementModifiers Modifiers;
        public readonly string Name;

        public StaticFunction(string name, string[] body, string[] modifiers)
        {
            Modifiers = new FLFunctionElementModifiers(name, modifiers);


            Name = name;
            Body = body.Select(x => new StaticInstruction(x.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries)))
                .ToArray();
        }

        public override string ToString()
        {
            return $"{Name}: {Modifiers}";
        }
    }
}