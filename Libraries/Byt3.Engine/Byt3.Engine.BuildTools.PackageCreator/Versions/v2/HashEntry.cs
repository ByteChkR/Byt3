using System;

namespace Byt3.Engine.BuildTools.PackageCreator.Versions.v2
{
    /// <summary>
    /// Containing the Hash for a file
    /// </summary>
    [Serializable]
    public struct HashEntry
    {
        public string File { get; set; }
        public string Hash { get; set; }
    }
}