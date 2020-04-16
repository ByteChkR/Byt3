using System;
using Byt3.ADL.Configs;

namespace Byt3.ADL.Crash
{
    [Serializable]
    public class CrashConfig : AbstractADLConfig
    {
        public bool ShortenCrashInfo { get; set; }

        public override AbstractADLConfig GetStandard()
        {
            return new CrashConfig() { };
        }
    }
}