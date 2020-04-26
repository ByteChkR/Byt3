namespace Byt3.Utilities.Versioning
{
    //public static class VersionAccumulatorManager
    //{
    //    private static readonly Dictionary<string, Version> entries = new Dictionary<string, Version>();
    //    private static bool searched;

    //    private static string LibraryList
    //    {
    //        get
    //        {
    //            StringBuilder sb = new StringBuilder();
    //            sb.AppendLine("Loading Assemblies:");
    //            foreach (KeyValuePair<string, Version> keyValuePair in entries)
    //            {
    //                sb.AppendLine($"\t{keyValuePair.Key} : {keyValuePair.Value}");
    //            }

    //            return sb.ToString();
    //        }
    //    }

    //    public static void SearchForAssemblies()
    //    {
    //        if (searched)
    //        {
    //            return;
    //        }

    //        searched = true;


    //        List<Assembly> asms = GetAssemblies().ToList();
    //        for (int i = 0; i < asms.Count; i++)
    //        {
    //            AssemblyName name = asms[i].GetName();
    //            if (!entries.ContainsKey(name.Name) && name.Name.StartsWith("Byt3"))
    //            {
    //                entries.Add(name.Name, name.Version);
    //            }
    //        }

    //        string[] files = Directory.GetFiles(Directory.GetCurrentDirectory(), "Byt3.*.dll",
    //            SearchOption.AllDirectories);
    //        for (int i = 0; i < files.Length; i++)
    //        {
    //            string name = Path.GetFileNameWithoutExtension(files[i]);
    //            if (!entries.ContainsKey(name))
    //            {
    //                Version v = Version.Parse(FileVersionInfo.GetVersionInfo(files[i]).FileVersion);
    //                entries.Add(name, v);
    //            }
    //        }

    //        System.Console.WriteLine(LibraryList);
    //    }

    //    public static IEnumerable<Assembly> GetAssemblies()
    //    {
    //        List<string> list = new List<string>();
    //        Stack<Assembly> stack = new Stack<Assembly>();

    //        stack.Push(Assembly.GetEntryAssembly());

    //        do
    //        {
    //            Assembly asm = stack.Pop();

    //            yield return asm;

    //            foreach (AssemblyName reference in asm.GetReferencedAssemblies())
    //            {
    //                if (!list.Contains(reference.FullName))
    //                {
    //                    stack.Push(Assembly.Load(reference));
    //                    list.Add(reference.FullName);
    //                }
    //            }
    //        } while (stack.Count > 0);
    //    }
    //}
}