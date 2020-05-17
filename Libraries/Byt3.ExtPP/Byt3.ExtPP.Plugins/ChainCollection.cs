using System;
using System.Collections.Generic;
using Byt3.ExtPP.Base.settings;

namespace Byt3.ExtPP.Plugins
{
    public class ChainCollection : IChainCollection
    {
        public string Name { get; } = "Default";

        public List<Type> Chain { get; } = new List<Type>
        {
            typeof(MultiLinePlugin),
            typeof(KeyWordReplacer),
            typeof(ConditionalPlugin),
            typeof(FakeGenericsPlugin),
            typeof(IncludePlugin),
            typeof(ExceptionPlugin),
            typeof(BlankLineRemover),
            typeof(ChangeCharCase),
            typeof(TextEncoderPlugin)
        };
    }
}