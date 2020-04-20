using System;
using System.Collections.Generic;

namespace Byt3.ExtPP.Console.PluginManager
{
    /// <summary>
    /// gets cached to disk.
    /// </summary>
    [Serializable]
    public class PluginManagerDatabase
    {
        /// <summary>
        /// The directories that will be automatically added when refreshed.
        /// </summary>
        public List<string> IncludedDirectories { get; set; } = new List<string>();

        /// <summary>
        /// the included files that were included manually.
        /// </summary>
        public List<string> IncludedFiles { get; set; } = new List<string>();

        /// <summary>
        /// The cache of the plugin information.
        /// </summary>

        public List<PluginInformation> Cache { get; set; } = new List<PluginInformation>();
    }
}