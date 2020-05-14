using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Byt3.Utilities.ConsoleInternals;
using Byt3Console.Runner;

namespace Byt3.Console
{
    internal static class ResolverManager
    {
        private class DefaultLibResolver : IResolver
        {
            public string FileExtension => ".dll";

            public string ResolveLibrary(string libraryFile)
            {
                return libraryFile;
            }

            public void Dispose()
            {
            }
        }

        public static Dictionary<string, ResolverWrapper> LoadResolvers()
        {

            string[] files = Directory.GetFiles(Program.InternalsFolder, Program.RESOLVER_FILE_SEARCH_PATTERN, SearchOption.AllDirectories);

            Dictionary<string, ResolverWrapper> ret = new Dictionary<string, ResolverWrapper>();

            ret.Add(".dll", new ResolverWrapper(new DefaultLibResolver())); //Default Resolver


            Queue<ResolverWrapper> queue = new Queue<ResolverWrapper>();
            queue.Enqueue(ret[".dll"]);

            int passId = 1;

            List<string> ignoreList = new List<string>();
            while (queue.Count != 0)
            {
                ResolverWrapper current = queue.Dequeue();

                System.Console.WriteLine($"Loading Resolver Pass {passId}: [{current.FileExtension}]");

                List<ResolverWrapper> loadedResolvers = LoadResolverWithResolver(current.FileExtension, files, current, ignoreList);
                for (int i = 0; i < loadedResolvers.Count; i++)
                {
                    System.Console.WriteLine($"Adding Resolver: {loadedResolvers[i].ActualResolverName} for extension {loadedResolvers[i].FileExtension}");
                    if (ret.ContainsKey(loadedResolvers[i].FileExtension))
                    {
                        System.Console.WriteLine($"Duplicate Resolver Entry Found.. Replacing {ret[loadedResolvers[i].FileExtension].ActualResolverName} with {loadedResolvers[i].ActualResolverName}");
                    }

                    ret[loadedResolvers[i].FileExtension] = loadedResolvers[i];
                    queue.Enqueue(loadedResolvers[i]);

                }

                passId++;
            }

            return ret;
        }

        public static List<ResolverWrapper> LoadResolverWithResolver(string ext, string[] allFiles, ResolverWrapper resolver, List<string> fileIgnoreList)
        {
            string[] files = allFiles.Where(x => x.EndsWith(ext) && !fileIgnoreList.Contains(x)).ToArray(); //Filter for extensions and ignore list
            List<ResolverWrapper> ret = new List<ResolverWrapper>();
            for (int i = 0; i < files.Length; i++)
            {
                try
                {
                    string resolvedName = resolver.ResolveLibrary(files[i]);
                    Assembly asm = Assembly.LoadFrom(resolvedName);

                    Type[] resolvers = asm.GetExportedTypes();

                    for (int j = 0; j < resolvers.Length; j++)
                    {
                        ResolverWrapper ir = new ResolverWrapper(Activator.CreateInstance(resolvers[j]));
                        if (ret.All(x => x.FileExtension != ir.FileExtension))
                        {
                            ret.Add(ir);
                        }
                    }
                    fileIgnoreList.Add(files[i]);
                }
                catch (Exception e)
                {
                    System.Console.WriteLine("Can not load Resolver: " + files[i] + ": " + e.Message);
                }
            }

            return ret;
        }
    }

    internal class Program
    {
        public static readonly string InternalsFolder = Path.Combine(Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).AbsolutePath), "internals");

        private static Dictionary<string, ResolverWrapper> FileResolvers;

        public static readonly string ResolverQualifiedName = typeof(IResolver).AssemblyQualifiedName;
        public const string RESOLVER_FILE_SEARCH_PATTERN = "*.Resolver.*";
        public const string RUNNER_FILE_SEARCH_PATTERN = "*.Runner.*";

        private static void Main(string[] args)
        {
            //AppDomain.CurrentDomain.AssemblyLoad+= CurrentDomainOnAssemblyLoad;
            //AppDomain.CurrentDomain.AssemblyResolve += CurrentDomainOnAssemblyResolve;
            if (!Directory.Exists(InternalsFolder)) Directory.CreateDirectory(InternalsFolder);

            FileResolvers = ResolverManager.LoadResolvers();
            string[] runnerLibs = Directory.GetFiles(InternalsFolder, RUNNER_FILE_SEARCH_PATTERN, SearchOption.AllDirectories);

            if (runnerLibs.Length==0)
            {
                System.Console.WriteLine("Can not Locate a Valid Runner Library.");
                System.Console.WriteLine("Path is Empty: " + InternalsFolder);
                System.Console.ReadLine();
                return;
            }

            MethodInfo mi=null;
            for (int i = 0; i < runnerLibs.Length; i++)
            {
                try
                {
                    Assembly asm = Assembly.LoadFrom(FileResolvers[Path.GetExtension(runnerLibs[i])].ResolveLibrary(runnerLibs[i]));
                    System.Console.WriteLine("Loading Runner: " + asm.GetName().Version);
                    Type t = asm.GetExportedTypes().First(x => x.Name == "ConsoleRunner");

                    mi = t.GetMethod("Run");
                    break;
                }
                catch (Exception)
                {
                    System.Console.WriteLine("Error Loading Runner: "+ runnerLibs[i]);
                }
            }

            try
            {
                Dictionary<string, object> fr = FileResolvers.ToDictionary(x => x.Key, x => (object) x.Value);
                //mi.Invoke(null, new object[] { args, fr });
                ConsoleRunner.Run(args, fr);
            }
            catch (Exception e)
            {
                Exception ex = e.InnerException ?? e;
                System.Console.WriteLine("Runner Command Errored.");
                System.Console.WriteLine("Exception " + ex.GetType().Name + ": " + ex.Message);
                System.Console.WriteLine(ex.StackTrace);
                System.Console.ReadLine();
            }

           
#if DEBUG
            //System.Console.ReadLine();
#endif
        }
    }
}