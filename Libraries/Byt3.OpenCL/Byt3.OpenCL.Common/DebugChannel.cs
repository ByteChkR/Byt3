using System;

namespace Byt3.OpenCL.Common
{
    /// <summary>
    /// An enum of Channels that can be used to send logs
    /// </summary>
    [Flags]
    public enum DebugChannel
    {
        /// <summary>
        /// Used to Write Logs to the Debug System
        /// </summary>
        Log = 1,

        /// <summary>
        /// Used to Write Warnings to the Debug System
        /// </summary>
        Warning = 1 << 1,

        /// <summary>
        /// Used to Write Errors to the Debug System
        /// </summary>
        Error = 1 << 2,

        IO = 1 << 3,

        OpenFL = 1 << 4,
        WFC = 1 << 5,
        OpenFL_WFC = OpenFL | WFC
    }
}