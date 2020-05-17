using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace FLDebugger.Projects.Instancing
{
    public static class DomainHelper
    {
        private static readonly List<AppDomain> SubDomains = new List<AppDomain>();
        private static bool Init;

        public static void CreateSubInstance(string folder)
        {
            if (!Init)
            {
                Init = true;
                AppDomain.CurrentDomain.DomainUnload += (sender, args) => UnloadAllSubDomains();
            }

            AppDomainSetup setup = new AppDomainSetup();
            setup.ApplicationBase = folder;

            AppDomain domain =
                AppDomain.CreateDomain($"{AppDomain.CurrentDomain.FriendlyName}_{Path.GetFileName(folder)}");

            SubDomains.Add(domain);

            domain.ExecuteAssembly(Assembly.GetExecutingAssembly().Location, new[] {"-no-update", folder});
        }

        private static void UnloadAllSubDomains()
        {
            if (Init)
            {
                foreach (AppDomain subDomain in SubDomains)
                {
                    AppDomain.Unload(subDomain);
                }

                SubDomains.Clear();
            }
        }
    }
}