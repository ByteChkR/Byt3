using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Byt3Console.Runner
{
    public class AppDomainController : MarshalByRefObject, IDisposable
    {
        //private readonly AppDomain Domain;
        private readonly string[] ExtraAssemblyFolders;
        private readonly Dictionary<string, Assembly> LoadedAssemblies = new Dictionary<string, Assembly>();

        private AppDomainController(AppDomain domain, string[] extraAssemblyFolders)
        {
            ExtraAssemblyFolders = extraAssemblyFolders;
            //AppDomain.CurrentDomain.AssemblyResolve += Domain_AssemblyResolve;
        }

        public void Dispose()
        {
            // AppDomain.Unload(Domain);
        }

        public static AppDomainController Create(string domainName, string[] extraAssemblyFolders)
        {
            //AppDomain domain = AppDomain.CreateDomain(domainName);
            return new AppDomainController(null, extraAssemblyFolders);
        }

        private Assembly Domain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            for (int i = 0; i < ExtraAssemblyFolders.Length; i++)
            {
                string[] files = Directory.GetFiles(ExtraAssemblyFolders[i], "*.dll",
                    SearchOption.TopDirectoryOnly);
                for (int j = 0; j < files.Length; j++)
                {
                    if (files[j].Contains(args.Name))
                    {
                        try
                        {
                            return Assembly.LoadFile(files[i]);
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }

            throw new AssemblyLoadException("Can not Resolve Assembly with name: " + args.Name +
                                            ".\n Requesting Assembly: " + args.RequestingAssembly.GetName().Name);
        }

        public Type GetType(string assemblyPath, string fullType)
        {
            Assembly asm = LoadAssembly(assemblyPath);
            Type t = asm.GetType(fullType);
            return t;
        }

        public Type[] GetTypes(string assemblyPath, string name)
        {
            Assembly asm = LoadAssembly(assemblyPath);
            List<Type> types = asm.GetExportedTypes().ToList();
            for (int i = types.Count - 1; i >= 0; i--)
            {
                if (types[i].Name != name)
                {
                    types.RemoveAt(i);
                }
            }

            return types.ToArray();
        }

        public Type[] GetTypes(string assemblyPath, Type baseType)
        {
            Assembly asm = LoadAssembly(assemblyPath);
            List<Type> types = asm.GetExportedTypes().ToList();

            for (int i = types.Count - 1; i >= 0; i--)
            {
                if (!baseType.IsAssignableFrom(types[i]))
                {
                    types.RemoveAt(i);
                }
            }

            return types.ToArray();
        }

        public object GetPropertyValue(string assemblyPath, string fullType, string propertyName)
        {
            Type t = GetType(assemblyPath, fullType);
            PropertyInfo pi = t.GetProperty(propertyName);

            object instance = Activator.CreateInstance(t);
            return pi.GetValue(instance, null);
        }

        public Assembly LoadAssembly(string assemblyPath)
        {
            if (LoadedAssemblies.ContainsKey(assemblyPath))
            {
                return LoadedAssemblies[assemblyPath];
            }

            Assembly asm = Assembly.LoadFrom(assemblyPath);
            LoadedAssemblies.Add(assemblyPath, asm);
            return asm;
        }

        public object LoadTypeAndRunFunction(string assemblyPath, string fullType, string functionName, object[] args)
        {
            Type t = GetType(assemblyPath, fullType);
            MethodInfo info = t.GetMethod(functionName, args.Select(x => x.GetType()).ToArray());

            object instance = Activator.CreateInstance(t);

            object ret = info.Invoke(instance, args);
            return ret;
        }

        private class AssemblyLoadException : Exception
        {
            public AssemblyLoadException(string message) : base(message)
            {
            }
        }
    }
}