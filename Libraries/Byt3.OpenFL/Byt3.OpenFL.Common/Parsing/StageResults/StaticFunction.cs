using System;
using System.Linq;

namespace Byt3.OpenFL.Parsing.StageResults
{
    public class StaticFunction
    {
        public readonly string Name;
        public readonly StaticInstruction[] Body;

        public StaticFunction(string name, string[] body)
        {
            Name = name;
            Body = body.Select(x => new StaticInstruction(x.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))).ToArray();
        }
    }
}