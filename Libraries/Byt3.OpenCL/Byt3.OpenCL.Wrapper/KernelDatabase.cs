using System;
using System.Collections.Generic;
using System.IO;
using Byt3.ADL;

namespace Byt3.OpenCL.Wrapper
{
    /// <summary>
    /// A class used to store and manage Kernels
    /// </summary>
    public class KernelDatabase
    {
        private readonly ALogger<LogType> Logger = new ALogger<LogType>("CL-KernelDB");
        /// <summary>
        /// The Folder that will get searched when initializing the database.
        /// </summary>
        private readonly string folderName;

        /// <summary>
        /// The currently loaded kernels
        /// </summary>
        public readonly Dictionary<string, CLKernel> LoadedKernels;

        /// <summary>
        /// Public constructor
        /// </summary>
        /// <param name="instance">Clapi Instance for the current thread</param>
        /// <param name="folderName">Folder name where the kernels are located</param>
        /// <param name="genDataType">The DataTypes used to compile the FL Database</param>
        public KernelDatabase(Clapi instance, string folderName, TypeEnums.DataTypes genDataType)
        {
            GenDataType = KernelParameter.GetDataString(genDataType);
            if (!Clapi.DirectoryExists(folderName))
            {
                throw new Exception(folderName);
            }

            this.folderName = folderName;
            LoadedKernels = new Dictionary<string, CLKernel>();
            Initialize(instance);
        }


        public string GenDataType { get; }


        /// <summary>
        /// Initializes the Kernel Database
        /// </summary>
        private void Initialize(Clapi instance)
        {
            string[] files = Clapi.GetFiles(folderName, "*.cl");

            foreach (string file in files)
            {
                AddProgram(instance, file);
            }
        }


        /// <summary>
        /// Manually adds a Program to the database
        /// </summary>
        /// <param name="instance">Clapi Instance for the current thread</param>
        /// <param name="file">Path fo the file</param>
        public void AddProgram(Clapi instance, string file)
        {
            if (!Clapi.FileExists(file))
            {
                throw new Exception(file);
            }


            string path = Path.GetFullPath(file);

            Logger.Log(LogType.Log, "Creating CLProgram from file: " + file);
            ClProgram program = new ClProgram(instance, path, GenDataType);

            foreach (KeyValuePair<string, CLKernel> containedKernel in program.ContainedKernels)
            {
                if (!LoadedKernels.ContainsKey(containedKernel.Key))
                {
                    Logger.Log(LogType.Log, "Adding Kernel: " + containedKernel.Key);
                    LoadedKernels.Add(containedKernel.Key, containedKernel.Value);
                }
                else
                {
                    Logger.Log(LogType.Log, "Kernel with name: " + containedKernel.Key + " is already loaded. Skipping...");
                }
            }
        }

        /// <summary>
        /// Tries to get the CLKernel by the specified name
        /// </summary>
        /// <param name="name">The name of the kernel</param>
        /// <param name="kernel">The kernel. Null if not found</param>
        /// <returns>Returns True if the kernel has been found</returns>
        public bool TryGetClKernel(string name, out CLKernel kernel)
        {
            if (LoadedKernels.ContainsKey(name))
            {
                kernel = LoadedKernels[name];
                return true;
            }

            kernel = null;
            return false;
        }
    }
}