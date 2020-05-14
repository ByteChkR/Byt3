﻿using System;
using System.Collections.Generic;
using Byt3.ADL.Configs;

namespace Byt3.ADL
{
    /// <summary>
    /// An empty interface that is used to Log where the log is coming from.
    /// Just add to any script and you can write logs by using this.Log(..)
    /// </summary>
    public abstract class ALoggable<T> where T : struct
    {
        protected ADLLogger<T> Logger => CreatedLoggers[GetType()];

        private static readonly Dictionary<Type, ADLLogger<T>> CreatedLoggers = new Dictionary<Type, ADLLogger<T>>();
        protected ALoggable(IProjectDebugConfig settings)
        {
            if(!CreatedLoggers.ContainsKey(GetType()))
            {
                ADLLogger<T> l = new ADLLogger<T>(settings, GetType().Name);
                CreatedLoggers[GetType()] = l;
            }
        }
    }
}