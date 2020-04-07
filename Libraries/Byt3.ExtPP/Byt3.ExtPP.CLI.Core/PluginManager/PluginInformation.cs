using System;
using System.Linq;
using System.Xml.Serialization;
using Byt3.ExtPP.Base;

namespace Byt3.ExtPP.CLI.Core.PluginManager
{
    /// <summary>
    /// Basic information about the plugin
    /// </summary>
    [Serializable]
    public class PluginInformation
    {
        /// <summary>
        /// All the prefixes used by the plugin
        /// </summary>
        [XmlElement]
        public string[] Prefixes { get; set; }

        /// <summary>
        /// Assembly Name
        /// </summary>
        [XmlElement]
        public string Name { get; set; }

        /// <summary>
        /// Path of the library
        /// </summary>
        [XmlElement]
        public string Path { get; set; }

        /// <summary>
        /// The Cached information about each command.
        /// </summary>
        [XmlElement]
        public CommandMetaData[] Data { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="prefixes">the prefixes of the plugin</param>
        /// <param name="name">the name</param>
        /// <param name="path">the path</param>
        /// <param name="data">the meta data</param>
        public PluginInformation(string[] prefixes, string name, string path, CommandMetaData[] data)
        {
            Path = path;
            Prefixes = prefixes;
            Name = name;
            Data = data;
        }

        public PluginInformation()
        {

        }

        /// <summary>
        /// returns a description of the plugin
        /// </summary>
        /// <param name="shortDesc">flag to optionally only return a small description</param>
        /// <returns>the description of the Plugin</returns>
        public string GetDescription(bool shortDesc)
        {
            return Name + ": \n" + Path + "\nPrefixes:\n\t" + Prefixes.Unpack("\n\t") + (shortDesc ? "" : "\nCommand Info: \n\t" + Data.Select(x => x.ToString()).Unpack("\n\t"));
        }

        /// <summary>
        /// returns a description of the plugin
        /// </summary>
        /// <returns>the description of the Plugin</returns>
        public string GetDescription()
        {
            return GetDescription(true);
        }

        /// <summary>
        /// Overriden tostring method
        /// </summary>
        /// <returns>The content of GetDescription()</returns>
        public override string ToString()
        {
            return GetDescription();
        }
    }
}