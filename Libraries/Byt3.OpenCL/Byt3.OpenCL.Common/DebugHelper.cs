using System;
using System.IO;
using Byt3.ADL;
using Byt3.ADL.Configs;
using Byt3.ADL.Crash;
using Byt3.ADL.Streams;
using Byt3.ExtPP.Base;

namespace Byt3.OpenCL.Common
{
    /// <summary>
    /// Helper Class that Communicates with the ADL Library
    /// </summary>
    public static class DebugHelper
    {
        public static int SeverityFilter;
        public static bool ThrowOnAllExceptions = true;
        public static bool Init { get; private set; }
        private static ALogger<DebugChannel> _logger = new ALogger<DebugChannel>("OpenCL/FL");

        public static void ApplySettings(IDebugSettings settings)
        {
            Init = true;

            if (Debug.LogStreamCount != 0)
                Debug.RemoveAllOutputStreams();
            _logger.RemoveAllPrefixes();

            IOCallbacks.Callback = new PpCallbacks();

            _logger.LookupSettings = (PrefixLookupSettings)settings.PrefixLookupFlags;

            SeverityFilter = settings.SeverityFilter;

            Debug.AdlEnabled = settings.Enabled;

            for (int i = 0; i < settings.StageNames.Length; i++)
            {
                if (settings.StageNames[i] != "[]")
                {
                    _logger.AddPrefixForMask( 1 << i, settings.StageNames[i]);
                }
            }
            //Debug.SetAllPrefixes(settings.StageNames);

            _logger.AddPrefixForMask( 0, "[Silent]");

            foreach (ILogStreamSettings logStreamSettings in settings.Streams)
            {
                Debug.AddOutputStream(OpenStream(logStreamSettings));
            }
            CrashHandler.Initialize();
        }


        private static LogStream OpenFileStream(ILogStreamSettings settings)
        {
            if (File.Exists(settings.Destination))
            {
                File.Delete(settings.Destination);
            }

            return new LogTextStream(File.OpenWrite(settings.Destination), settings.Mask,
                (MaskMatchType)settings.MatchMode, settings.Timestamp);
        }

        private static LogStream OpenConsoleStream(ILogStreamSettings settings)
        {
            return new LogTextStream(Console.OpenStandardOutput(), settings.Mask, (MaskMatchType)settings.MatchMode,
                settings.Timestamp);
        }


        private static LogStream OpenStream(ILogStreamSettings settings)
        {
            if (settings.StreamType == 1) //File
            {
                return OpenFileStream(settings);
            }

            return OpenConsoleStream(settings);
        }


        public static void Crash(Exception ex, bool recoverable)
        {
            CrashHandler.Log(ex, true);

            if (!recoverable || ThrowOnAllExceptions)
            {
                throw ex;
            }
        }

        public static void Log(string message, int channel)
        {
            Log(message, channel, 0);
        }

        public static void Log(string message, int channel, int severity)
        {
            if (severity < SeverityFilter)
            {
                return;
            }

            _logger.Log(channel, $"[S:{severity}]\t" + message);
        }
    }
}