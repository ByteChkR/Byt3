using System;
using System.Text;
using System.Xml.Serialization;

namespace Byt3.ADL.Configs
{
    /// <summary>
    ///     Contains the Configurations of the main ADL.Debug class.
    /// </summary>
    [Serializable]
    public class ADLConfig : AbstractADLConfig
    {
        /// <summary>
        ///     Is ADL enabled when this config is loaded?
        /// </summary>
        public bool AdlEnabled;

        ///// <summary>
        /////     The prefixes that are used when a log in a specific mask gets sent.
        ///// </summary>
        //public SerializableDictionary<int, string> Prefixes;

        /// <summary>
        ///     Determines the Options on how much effort is put into finding the right tags
        /// </summary>
        public PrefixLookupSettings PrefixLookupMode;

        ///// <summary>
        /////     Should ADL Search for updates when the first log message gets writen to the streams.
        /////     It will take ~300ms to communicate with the github server.
        ///// </summary>
        //public bool CheckForUpdates;


        [XmlIgnore] public Encoding TextEncoding;


        /// <summary>
        /// The format ADL uses to convert a Time to a string representation
        /// </summary>
        public string TimeFormatString;


        /// <summary>
        ///     Standard Confuguration
        /// </summary>
        /// <returns>The standard configuration of ADL</returns>
        public override AbstractADLConfig GetStandard()
        {
            return new ADLConfig
            {
                AdlEnabled = true,
                PrefixLookupMode = PrefixLookupSettings.Addprefixifavailable |
                                   PrefixLookupSettings.Deconstructmasktofind,
                TextEncoding = Encoding.ASCII,
                TimeFormatString = "MM-dd-yyyy-H-mm-ss"
            };
        }

        #region Lookup Presets

        public static PrefixLookupSettings LowestPerformance =
            PrefixLookupSettings.Addprefixifavailable |
            PrefixLookupSettings.Deconstructmasktofind;

        public static PrefixLookupSettings LowPerformance =
            PrefixLookupSettings.Addprefixifavailable |
            PrefixLookupSettings.Deconstructmasktofind |
            PrefixLookupSettings.Onlyoneprefix;

        public static PrefixLookupSettings MediumPerformance =
            PrefixLookupSettings.Addprefixifavailable |
            PrefixLookupSettings.Deconstructmasktofind |
            PrefixLookupSettings.Bakeprefixes;

        public static PrefixLookupSettings HighPerformance =
            PrefixLookupSettings.Addprefixifavailable;

        public static PrefixLookupSettings HighestPerformance =
            PrefixLookupSettings.Noprefix;

        #endregion
    }
}