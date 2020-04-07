using System;

namespace Byt3.OpenCL.Common
{
    /// <summary>
    /// Helper Interface for the generic ThreadManager implementation.
    /// 
    /// </summary>
    internal interface IThreadManager
    {
        /// <summary>
        /// Type of the Thread Manager 
        /// </summary>
        Type Type { get; }

        /// <summary>
        /// Checks the States of the currently running tasks
        /// </summary>
        /// <returns>True if all tasks finished</returns>
        bool CheckStates();
    }
}