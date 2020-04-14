using System;

namespace Byt3.ADL
{
    public class LevelFilteredLogger<T> : ADLLogger<T>
        where T : struct
    {
        public LevelFilteredLogger(string projectName) : base(projectName)
        {
        }

        /// <summary>
        /// The Verbosity level
        /// Everything lower than this will be sent to the log output
        /// </summary>
        public Verbosity VerbosityLevel { get; set; } = Verbosity.Level8;

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
        public void Log(T mask, Verbosity level, string format, params object[] objs)
        {
            Log(Convert.ToInt32(mask), level, format, objs);
        }
    }
}