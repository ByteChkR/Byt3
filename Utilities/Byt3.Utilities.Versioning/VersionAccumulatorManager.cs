using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Byt3.Utilities.Versioning
{
    public static class VersionAccumulatorManager
    {
        private static readonly Dictionary<AssemblyName, Version> entries = new Dictionary<AssemblyName, Version>();
        private static bool searched;

        private static string LibraryList
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Loading Assemblies:");
                foreach (KeyValuePair<AssemblyName, Version> keyValuePair in entries)
                {
                    sb.AppendLine($"\t{keyValuePair.Key.Name} : {keyValuePair.Value}");
                }

                return sb.ToString();
            }
        }
        public static void SearchForAssemblies()
        {
            if (searched) return;
            //entries.Add(Assembly.GetExecutingAssembly(), Assembly.GetExecutingAssembly().GetName().Version);
            searched = true;
            AssemblyName n = Assembly.GetExecutingAssembly().GetName();

            

            List<Assembly> asms = GetAssemblies().ToList();
            for (int i = 0; i < asms.Count; i++)
            {
                AssemblyName name = asms[i].GetName();
                if (!entries.ContainsKey(name) && name.Name.StartsWith("Byt3"))
                    entries.Add(asms[i].GetName(), asms[i].GetName().Version);
            }

            Console.WriteLine(LibraryList);
        }

        public static IEnumerable<Assembly> GetAssemblies()
        {
            List<string> list = new List<string>();
            Stack<Assembly> stack = new Stack<Assembly>();

            stack.Push(Assembly.GetEntryAssembly());

            do
            {
                Assembly asm = stack.Pop();

                yield return asm;

                foreach (AssemblyName reference in asm.GetReferencedAssemblies())
                    if (!list.Contains(reference.FullName))
                    {
                        stack.Push(Assembly.Load(reference));
                        list.Add(reference.FullName);
                    }

            }
            while (stack.Count > 0);

        }

    }

    
}