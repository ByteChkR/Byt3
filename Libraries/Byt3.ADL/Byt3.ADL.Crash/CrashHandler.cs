using System;
using System.Collections;
using System.Text;
using Byt3.ADL.Configs;

namespace Byt3.ADL.Crash
{
    public static class CrashHandler
    {
        private enum CrashLogType
        {
            CrashShort, Error, Crash
        }

        private static bool initialized = false;

        private static readonly CrashConfig Config = ConfigManager.GetDefault<CrashConfig>();

        private static readonly ADLLogger<CrashLogType> CrashLogger = new ADLLogger<CrashLogType>("ADL.Crash");

        public static void SaveCurrentConfig(string configPath = "adl_crash.xml")
        {
            ConfigManager.SaveToFile(configPath, Config);
        }
        public static void Initialize(string configPath = "adl_crash.xml")
        {
            Initialize(ConfigManager.ReadFromFile<CrashConfig>(configPath));
        }
        public static void Initialize(CrashConfig config)
        {
            Initialize(config.ShortenCrashInfo);
        }
        public static void Initialize(bool shortenCrashInfo)
        {
            Config.ShortenCrashInfo = shortenCrashInfo;


            initialized = true;
        }

        public static void Log(Exception exception, bool includeInner = true)
        {
            if (!initialized)
            {
                CrashLogger.Log(CrashLogType.Error, "Crash handler was not initialized");
                return;
            }

            if (Config.ShortenCrashInfo)
            {
                CrashLogger.Log(CrashLogType.CrashShort, ExceptionHeader(exception));
            }
            else
            {
                CrashLogger.Log(CrashLogType.Crash, ExceptionToString(exception, includeInner) );
            }

            CrashLogType lt = Config.ShortenCrashInfo ? CrashLogType.CrashShort : CrashLogType.Crash;

            

        }

        private static string ExceptionHeader(Exception exception)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\nException Logged: ");
            sb.Append(exception.GetType().FullName);
            if (exception.Message != null)
            {
                sb.Append("\nException Message: ");
                sb.Append(exception.Message);
            }
            if (exception.Source != null)
            {
                sb.Append("\nException Source: ");
                sb.Append(exception.Source);
            }
            return sb.ToString();
        }

        private static string ExceptionToString(Exception exception, bool includeInner)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\nException Type: ");
            sb.Append(exception.GetType().FullName);
            if (exception.Message != null)
            {
                sb.Append("\nException Message: ");
                sb.Append(exception.Message);
            }
            if (exception.Source != null)
            {
                sb.Append("\nException Source: ");
                sb.Append(exception.Source);
            }
            if (exception.HelpLink != null)
            {
                sb.Append("\nException Help Link: ");
                sb.Append(exception.HelpLink);
            }
            sb.Append("\nException HResult: ");
            sb.Append(exception.HResult.ToString());

            if (exception.StackTrace != null)
            {
                sb.Append("\nException Stacktrace: \n");
                sb.Append(exception.StackTrace);
            }

            if (exception.Data.Count != 0)
            {
                sb.Append("\nException Data:");
                foreach (DictionaryEntry dictionaryEntry in exception.Data)
                {
                    sb.Append("\n");
                    sb.Append(dictionaryEntry.Key);
                    sb.Append(":");
                    if (!dictionaryEntry.Value.GetType().IsArray)
                        sb.Append(dictionaryEntry.Value.ToString());
                    else
                    {
                        sb.Append(UnpackToString(dictionaryEntry.Value));
                    }
                }
            }

            if (includeInner && exception.InnerException != null)
            {
                sb.Append("\nInner Exception:");
                sb.Append(ExceptionToString(exception.InnerException, true));
            }

            return sb.ToString();

        }


        public static string UnpackToString(object obj, int depth = 0)
        {
            string ret = "";
            if (obj.GetType().IsArray)
            {
                IEnumerable o = (IEnumerable)obj;
                foreach (object entry in o)
                {
                    ret += UnpackToString(entry, depth + 1) + "\n";
                }
            }
            else
            {
                string ind = "";
                for (int i = 0; i < depth; i++)
                {
                    ind += "\t";
                }
                ret = ind + obj.ToString();
            }

            return ret;
        }

    }
}