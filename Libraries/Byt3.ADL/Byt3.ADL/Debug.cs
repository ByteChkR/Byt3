﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Byt3.ADL.Configs;
using Byt3.ADL.Streams;

/// <summary>
/// Namespace ADL is the "Root" namespace of ADL. It contains the Code needed to use ADL. But also in sub namespaces you will find other helpful tools.
/// </summary>
namespace Byt3.ADL
{
    /// <summary>
    ///     Main Debug Class. No instanciation needed.
    /// </summary>
    public static class Debug
    {
        #region Private Variables

        /// <summary>
        ///     Flag to check wether this is the first execution.
        /// </summary>
        private static bool _firstLog = true;

        /// <summary>
        ///     Dictionary of Prefixes for the corresponding Masks
        /// </summary>
        private static Dictionary<int, string> _prefixes = new Dictionary<int, string>();

        private static readonly object PrefixLock = new object();

        /// <summary>
        ///     The mask that gets used when _sendUpdateMessage is true
        /// </summary>
        private static int _updateMask = new BitMask(true);

        /// <summary>
        ///     The mask that gets used when ADL is sending warnings
        /// </summary>
        private static int _adlWarningMask = new BitMask(true);

        /// <summary>
        ///     String Builder to assemble the log
        /// </summary>
        private static readonly StringBuilder StringBuilder = new StringBuilder();

        /// <summary>
        ///     List of LogStreams that are active
        /// </summary>
        private static readonly List<LogStream> Streams = new List<LogStream>();

        /// <summary>
        ///     Contains the flags that determine the way prefixes get looked up
        /// </summary>
        private static PrefixLookupSettings _lookupMode = PrefixLookupSettings.Addprefixifavailable;

        /// <summary>
        ///     The extracted flag if we should put tags at all
        /// </summary>
        private static bool _addPrefix;

        /// <summary>
        ///     The extracted flag if we should deconstruct the mask to find potential tags
        /// </summary>
        private static bool _deconstructtofind;

        /// <summary>
        ///     The extracted flag if we should end the lookup when one tag was found.(Does nothing if deconstruct flag is set to
        ///     false)
        /// </summary>
        private static bool _onlyone;

        /// <summary>
        ///     The extracted flag if ADL should bake prefixes on the fly to archieve better lookup performance.
        ///     This makes every new flag that is not prefixed a new prefix entry with the combined mask.
        /// </summary>
        private static bool _bakePrefixes;


        #endregion

        #region Public Properties

        /// <summary>
        ///     The Encoding that is going to be used by all text in ADL.
        /// </summary>
        public static Encoding TextEncoding = Encoding.ASCII;

        /// <summary>
        /// The format ADL uses to convert a Time to a string representation
        /// </summary>
        public static string TimeFormatString;

        /// <summary>
        ///     Public property, used to disable ADl
        /// </summary>
        public static bool AdlEnabled { get; set; } = true;

        /// <summary>
        ///     Public property, used to disable update check.(Saves ~500ms)
        /// </summary>
        public static bool CheckForUpdates { get; set; } = true;

        /// <summary>
        ///     Determines if ADL should send warnings about potential wrong use to the log streams
        /// </summary>
        public static bool SendWarnings { get; set; } = true;

        /// <summary>
        ///     Update Mask. This mask gets used when one of the ADL Components are checking for updates.
        ///     To disable the update messages, simly change the mask to new Bitmask(false)
        /// </summary>
        public static BitMask UpdateMask
        {
            get => _updateMask;
            set => _updateMask = value;
        }

        /// <summary>
        ///     Warning Mask. This mask gets used when ADL sends warnings about (possible)wrong use.
        /// </summary>
        public static BitMask AdlWarningMask
        {
            get => _adlWarningMask;
            set => _adlWarningMask = value;
        }

        /// <summary>
        ///     The number of Streams that ADL writes to
        /// </summary>
        public static int LogStreamCount
        {
            get { lock (Streams) return Streams.Count; }
        }

        public static PrefixLookupSettings PrefixLookupMode
        {
            get => _lookupMode;
            set
            {
                _addPrefix =
                    BitMask.IsContainedInMask((int)value, (int)PrefixLookupSettings.Addprefixifavailable, false);
                _deconstructtofind = BitMask.IsContainedInMask((int)value,
                    (int)PrefixLookupSettings.Deconstructmasktofind, false);
                _onlyone = BitMask.IsContainedInMask((int)value, (int)PrefixLookupSettings.Onlyoneprefix, false);
                _lookupMode = value;
                _bakePrefixes = BitMask.IsContainedInMask((int)value, (int)PrefixLookupSettings.Bakeprefixes, false);
            }
        }

        #endregion

        #region Streams

        /// <summary>
        ///     Adds another stream to the debug logs.
        /// </summary>
        /// <param name="stream">The stream you want to add</param>
        public static void AddOutputStream(LogStream stream)
        {
            if (stream == null)
            {
                Log(_adlWarningMask, "AddOutputStream(NULL): The Supplied stream is a nullpointer.");
                return;
            }

            if (!AdlEnabled)
                Log(_adlWarningMask,
                    "AddOutputStream(" + stream.Mask +
                    "): ADL is disabled, you are adding an Output Stream while ADL is disabled.");
            var contains=false;
            lock (Streams)
            {
                contains = Streams.Contains(stream);
            }

            if (contains)
            {
                Log(_adlWarningMask,
                    "AddOutputStream(" + stream.Mask + "): Supplied stream is already in the list. Aborting!");
                return;
            }

            lock (Streams)
            {
                Streams.Add(stream);
            }
        }

        /// <summary>
        ///     Removes the specified Stream.
        /// </summary>
        /// <param name="stream">The stream you want to remove</param>
        /// ///
        /// <param name="closeStream">If streams should be closed upon removal from the system</param>
        public static void RemoveOutputStream(LogStream stream, bool closeStream = true)
        {
            var contains = false;
            lock (Streams)
            {
                contains = Streams.Contains(stream);
            }

            if (!contains)
            {
                Log(_adlWarningMask,
                    "RemoveOutputStream(" + stream.Mask + "): Supplied stream is not in the list. Aborting!");
                return;
            }

            if (!AdlEnabled)
                Log(_adlWarningMask,
                    "RemoveOutputStream(" + stream.Mask +
                    "): ADL is disabled, you are removing an Output Stream while while ADL is disabled.");
            lock (Streams)
            {
                Streams.Remove(stream);
            }

            if (closeStream) stream.Close();
        }

        /// <summary>
        ///     Removes all output streams from the list. Everything gets written to file.
        /// </summary>
        /// <param name="closeStream">If streams should be closed upon removal from the system</param>
        public static void RemoveAllOutputStreams(bool closeStream = true)
        {
            Log(-1, "Debug Queue Emptied");
            lock (Streams)
            {
                if (closeStream)
                    foreach (var ls in Streams)
                        ls.Close();
                Streams.Clear();
            }
        }

        #endregion

        #region Prefixes

        /// <summary>
        ///     Adds a prefix for the specified level
        /// </summary>
        /// <param name="mask">flag combination</param>
        /// <param name="prefix">desired prefix</param>
        public static void AddPrefixForMask(BitMask mask, string prefix)
        {
            if (!AdlEnabled)
                Log(_adlWarningMask,
                    "AddPrefixForMask(" + mask +
                    "): ADL is disabled, you are adding a prefix for a mask while ADL is disabled.");
            if (!BitMask.IsUniqueMask(mask))
                Log(_adlWarningMask,
                    "AddPrefixForMask(" + mask + "): Adding Prefix: " + prefix + " for mask: " + mask +
                    ". Mask is not unique.");
            lock (PrefixLock)
            {
                if (_prefixes.ContainsKey(mask))
                    _prefixes[mask] = prefix;
                else
                    _prefixes.Add(mask, prefix);
            }
        }

        /// <summary>
        ///     Removes Prefix from prefix lookup table
        /// </summary>
        /// <param name="mask"></param>
        public static void RemovePrefixForMask(BitMask mask)
        {
            if (!AdlEnabled)
                Log(_adlWarningMask,
                    "RemovePrefixForMask(" + mask +
                    "): ADL is disabled, you are removing a prefix for a mask while ADL is disabled.");

            lock (PrefixLock)
            {
                if (!_prefixes.ContainsKey(mask)) return;
                _prefixes.Remove(mask);
            }
        }

        /// <summary>
        ///     Clears all Prefixes
        /// </summary>
        public static void RemoveAllPrefixes()
        {
            lock (PrefixLock)
            {
                _prefixes.Clear();
            }
        }

        /// <summary>
        ///     Sets all Prefixes from the list from low to high.
        ///     You can not specify the level, because it will fill the prefixes by power of 2. So prefixes[0] = level1 and
        ///     prefixes[2] = level4 and so on
        /// </summary>
        /// <param name="prefixes">List of prefixes</param>
        public static void SetAllPrefixes(params string[] prefixes)
        {
            if (!AdlEnabled)
            {
                var info = "";
                prefixes.ToList().ForEach(x => info += x + ", ");
                Log(_adlWarningMask,
                    "SetAllPrefixes(" + info +
                    "): ADL is disabled, you are removing a prefix for a mask while ADL is disabled.");
            }

            RemoveAllPrefixes();

            for (var i = 0; i < prefixes.Length; i++) AddPrefixForMask(Utils.IntPow(2, i), prefixes[i]);
        }

        /// <summary>
        ///     Gets all Tags with corresponding masks.
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, string> GetAllPrefixes()
        {
            if (!AdlEnabled)
                Log(_adlWarningMask,
                    "GetAllPrefixes(): ADL is disabled, you are getting all prefixes while ADL is disabled.");
            lock (_prefixes) return new Dictionary<int, string>(_prefixes);
        }

        #endregion

        #region Logging



        /// <summary>
        ///     Fire Log Messsage with desired level(flag) and message
        /// </summary>
        /// <param name="mask">the flag</param>
        /// <param name="message">the message</param>
        public static void Log(int mask, string message)
        {
            if (!AdlEnabled || mask == AdlWarningMask && !SendWarnings) return;

            if (_firstLog)
            {
                _firstLog = false;

                if (CheckForUpdates)
                {
                    var msg = UpdateDataObject.CheckUpdate(typeof(Debug));

                    Log(UpdateMask, msg);
                }
            }

            var messg = message + Utils.NewLine;
            var mesg = GetMaskPrefix(mask) + messg;

            lock (Streams)
            {
                for (int i = Streams.Count-1; i >= 0; i--)
                {
                    if (Streams[i].IsClosed)
                        Streams.RemoveAt(i);
                }
                foreach (var logs in Streams)
                {
                    
                    if (logs.IsContainedInMask(mask))
                    {
                        logs.Write(logs.OverrideChannelTag ? new Log(mask, messg) : new Log(mask, mesg));
                    }
                }
            }
        }

        /// <summary>
        ///     Generic Version. T is your Enum
        /// </summary>
        /// <typeparam name="T">Enum</typeparam>
        /// <param name="mask">Enum Mask</param>
        /// <param name="message">Message</param>
        public static void LogGen<T>(T mask, string message) where T : struct
        {
            var m = Convert.ToInt32(mask);
            if (!AdlEnabled && (!SendWarnings || m != AdlWarningMask)) return;
            Log(m, message);
        }

        /// <summary>
        ///     Gets the Mask of the Specified Prefix
        /// </summary>
        /// <param name="prefix">Prefix</param>
        /// <param name="mask">Mask returned by the function</param>
        /// <returns>True if mask is found in Dictionary</returns>
        public static bool GetPrefixMask(string prefix, out BitMask mask)
        {
            mask = 0;
            Dictionary<int, string> prefx;
            lock (PrefixLock)
            {
                prefx = new Dictionary<int, string>(_prefixes);
            }

            if (!prefx.ContainsValue(prefix)) return false;
            foreach (var kvp in prefx)
                if (prefix == kvp.Value)
                {
                    mask = kvp.Key;
                    return true;
                }

            return false;
        }

        /// <summary>
        ///     Returns the concatenated string of all the Prefixes that are fallin in that mask.
        /// </summary>
        /// <param name="mask"></param>
        /// <returns>All Prefixes for specified mask</returns>
        public static string GetMaskPrefix(BitMask mask)
        {
            if (!_addPrefix) return "";
            StringBuilder.Length = 0;
            lock (_prefixes) if (_prefixes.ContainsKey(mask))
                {
                    //We happen to have a custom prefix for the level
                    StringBuilder.Append(_prefixes[mask]);
                }
                else if (_deconstructtofind) //We have no Prefix specified for this particular level
                {
                    var flags = BitMask.GetUniqueMasksSet(mask); //Lets try to split all the flags into unique ones
                    foreach (var t in flags)
                        if (_prefixes.ContainsKey(t))
                        {
                            StringBuilder.Insert(0, _prefixes[t]);

                            if (_onlyone)
                                break;
                        }
                        else //If still not in prefix lookup table, better have a prefix than having just plain text.
                        {
                            StringBuilder.Insert(0, "[Log Mask:" + t + "]");

                            if (_onlyone)
                                break;
                        }

                    if (!_bakePrefixes) return StringBuilder.ToString();
                    lock (PrefixLock)
                    {
                        _prefixes.Add(mask,
                            StringBuilder.ToString()); //Create a "custom prefix" with the constructed mask.
                    }

                    //Log(new BitMask(true), "Baked Prefix: "+_stringBuilder.ToString());
                }

            return StringBuilder.ToString();
        }

        #endregion

        #region Config

        /// <summary>
        ///     Loads a supplied ADLConfig.
        /// </summary>
        /// <param name="config">Config to load</param>
        public static void LoadConfig(AdlConfig config)
        {
            AdlEnabled = config.AdlEnabled;
            CheckForUpdates = config.CheckForUpdates;
            UpdateMask = config.UpdateMask;
            AdlWarningMask = config.WarningMask;
            SendWarnings = config.SendWarnings;
            TimeFormatString = config.TimeFormatString;
            lock (_prefixes) _prefixes = config.Prefixes.ToDictionary();
            PrefixLookupMode = config.PrefixLookupMode;
        }

        /// <summary>
        ///     Loads the ADL Config from the file at the supplied path
        /// </summary>
        /// <param name="path">file path</param>
        public static void LoadConfig(string path = "adl_config.xml")
        {
            var config = ConfigManager.ReadFromFile<AdlConfig>(path);
            LoadConfig(config);
        }

        /// <summary>
        ///     Saves the configuration to the given file path
        /// </summary>
        /// <param name="config">config to save</param>
        /// <param name="path">file path</param>
        public static void SaveConfig(AdlConfig config, string path = "adl_config.xml")
        {
            ConfigManager.SaveToFile(path, config);
        }


        /// <summary>
        ///     Saves the current configuration of ADL to the given file path
        /// </summary>
        /// <param name="path">File path.</param>
        public static void SaveConfig(string path = "adl_config.xml")
        {
            AdlConfig config = ConfigManager.GetDefault<AdlConfig>();
            config.AdlEnabled = AdlEnabled;
            config.CheckForUpdates = CheckForUpdates;
            config.UpdateMask = UpdateMask;
            config.WarningMask = AdlWarningMask;
            config.SendWarnings = SendWarnings;
            config.TimeFormatString = TimeFormatString;
            lock (_prefixes) config.Prefixes = new SerializableDictionary<int, string>(_prefixes);
            config.PrefixLookupMode = PrefixLookupMode;
            SaveConfig(config, path);
        }

        #endregion
    }
}