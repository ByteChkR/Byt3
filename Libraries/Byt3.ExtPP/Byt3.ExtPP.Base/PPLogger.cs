using System;
using Byt3.ADL;
using Byt3.ADL.Crash;
using Byt3.ExtPP.Base.settings;

namespace Byt3.ExtPP.Base
{
    public class PPLogger : ALogger<DebugLevel>
    {
        public static readonly PPLogger Instance = new PPLogger();

        public bool ThrowOnError { get; set; } = true;
        public bool ThrowOnWarning { get; set; }

        public int ErrorCount { get; private set; }
        public int WarningCount { get; private set; }

        public void ResetWarnErrorCounter()
        {
            ErrorCount = 0;
            WarningCount = 0;
        }

        public PPLogger() : base("ExtPP") { }

        /// <summary>
        /// The Verbosity level
        /// Everything lower than this will be sent to the log output
        /// </summary>
        public Verbosity VerbosityLevel { get; set; } = Verbosity.LEVEL2;

        /// <summary>
        /// Logs a message in the specified mask and verbosity level
        /// </summary>
        /// <param name="mask">the mask that is used to log</param>
        /// <param name="level">the debug level of the log</param>
        /// <param name="format">the format string(the message)</param>
        /// <param name="objs">the format params</param>
        private void Log(int mask, Verbosity level, string format, params object[] objs)
        {
            if (level <= VerbosityLevel)
            {
                base.Log(mask, string.Format(format, objs));
            }

        }

        /// <summary>
        /// Logs a message in the specified Debug and VerbosityLevel
        /// </summary>
        /// <param name="mask">the mask that is used to log</param>
        /// <param name="level">the debug level of the log</param>
        /// <param name="format">the format string(the message)</param>
        /// <param name="objs">the format params</param>
        public void Log(DebugLevel mask, Verbosity level, string format, params object[] objs)
        {
            Log((int)mask, level, format, objs);
        }


        public void Warning(string format, params object[] objs)
        {
            WarningCount++;
            if (ThrowOnWarning)
            {
                Crash(format, true, objs);
            }
            else
            {
                Log(DebugLevel.WARNINGS, Verbosity.LEVEL1, format, objs);
            }
        }

        public void Error(string format, params object[] objs)
        {
            ErrorCount++;
            if (ThrowOnError)
            {
                Crash(format, true, objs);
            }
            else
            {
                Log(DebugLevel.ERRORS, Verbosity.SILENT, format, objs);
            }
        }

        public void Crash(string format, bool throwEx, params object[] objs)
        {
            Crash(new ProcessorException(string.Format(format, objs)), throwEx);
        }




        /// <summary>
        /// Implements ADL.Crash to log a Exeption to the output stream as detailed as possible.
        /// </summary>
        /// <param name="ex">The exception</param>
        /// <param name="throwEx">if it should only log and not throw the exception specify this to be false</param>
        public static void Crash(Exception ex, bool throwEx)
        {
            CrashHandler.Log(ex);
            if (throwEx)
            {
                throw ex;
            }
        }

        public static void Crash(Exception ex)
        {
            Crash(ex, false);
        }
    }
}