﻿using System;
using System.Reflection;

namespace Byt3.Utilities.ConsoleInternals
{
    public class ResolverWrapper
    {
        private readonly object instance;

        public string ActualResolverName => instance.GetType().Name;

        public string FileExtension =>
            (string) instance.GetType().GetProperty("FileExtension").GetValue(instance, null);

        public string ResolveLibrary(string libraryFile)
        {
            Type t = instance.GetType();
            MethodInfo meth = t.GetMethod("ResolveLibrary");
            return (string) meth.Invoke(instance, new object[] {libraryFile});
        }

        public ResolverWrapper(object resolver)
        {
            instance = resolver;
        }
    }
}