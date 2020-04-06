using System.Collections.Generic;

namespace Byt3.OpenFL.FLDataObjects
{
    /// <summary>
    /// Contains information on the FL Script
    /// </summary>
    public struct FLScriptData
    {
        public List<string> Source;
        public Dictionary<string, CLBufferInfo> Defines;
        public Dictionary<string, int> JumpLocations;
        public List<FLInstructionData> ParsedSource;

        public FLScriptData(List<string> source)
        {
            Source = source;
            Defines = new Dictionary<string, CLBufferInfo>();
            JumpLocations = new Dictionary<string, int>();
            ParsedSource = new List<FLInstructionData>();
        }
    }
}