﻿using Byt3.OpenCL.Memory;

namespace Byt3.OpenFL
{
    /// <summary>
    /// Wrapper for the Memory Buffer holding some useful additional data
    /// </summary>
    public class ClBufferInfo
    {
        /// <summary>
        /// The buffer
        /// </summary>
        public MemoryBuffer Buffer;

        /// <summary>
        /// The Internal Constructor
        /// </summary>
        /// <param name="buffer">The inner buffer</param>
        /// <param name="isInternal">a flag indicating if the buffer is for internal use</param>
        internal ClBufferInfo(MemoryBuffer buffer, bool isInternal)
        {
            IsInternal = isInternal;
            Buffer = buffer;
            DefinedBufferName = "UnnamedBuffer";
        }

        /// <summary>
        /// Flag that is used to keep track of memory buffers that stayed inside the engine code and can not possibly be changed or used by the user.
        /// </summary>
        public bool IsInternal { get; private set; }

        /// <summary>
        /// The Buffer name
        /// </summary>
        public string DefinedBufferName { get; private set; }

        /// <summary>
        /// Sets the IsInernal Flag to the specified state
        /// </summary>
        /// <param name="internalState">The state</param>
        internal void SetInternalState(bool internalState)
        {
            IsInternal = internalState;
        }

        /// <summary>
        /// Sets the buffer name
        /// </summary>
        /// <param name="key">The Name of the buffer</param>
        internal void SetKey(string key)
        {
            DefinedBufferName = key;
        }

        /// <summary>
        /// To string ovverride
        /// </summary>
        /// <returns>Console friendly string</returns>
        public override string ToString()
        {
            return DefinedBufferName + "_" + (IsInternal ? "internal" : "unmanaged");
        }
    }
}