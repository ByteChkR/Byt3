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
        private readonly ADLLogger<LogType> logger = new ADLLogger<LogType>("CL-KernelDB");
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
        /// <param name="instance">CLAPI Instance for the current thread</param>
        /// <param name="folderName">Folder name where the kernels are located</param>
        /// <param name="genDataType">The DataTypes used to compile the FL Database</param>
        public KernelDatabase(CLAPI instance, string folderName, TypeEnums.DataTypes genDataType)
        {
            GenDataType = KernelParameter.GetDataString(genDataType);
            if (!CLAPI.DirectoryExists(folderName))
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
        private void Initialize(CLAPI instance)
        {
            string[] files = CLAPI.GetFiles(folderName, "*.cl");

            foreach (string file in files)
            {
                AddProgram(instance, file);
            }
        }


        /// <summary>
        /// Manually adds a Program to the database
        /// </summary>
        /// <param name="instance">CLAPI Instance for the current thread</param>
        /// <param name="file">Path fo the file</param>
        public void AddProgram(CLAPI instance, string file)
        {
            if (!CLAPI.FileExists(file))
            {
                throw new Exception(file);
            }


            string path = Path.GetFullPath(file);

            logger.Log(LogType.Log, "Creating CLProgram from file: " + file);
            CLProgram program = new CLProgram(instance, path, GenDataType);

            foreach (KeyValuePair<string, CLKernel> containedKernel in program.ContainedKernels)
            {
                if (!LoadedKernels.ContainsKey(containedKernel.Key))
                {
                    logger.Log(LogType.Log, "Adding Kernel: " + containedKernel.Key);
                    LoadedKernels.Add(containedKernel.Key, containedKernel.Value);
                }
                else
                {
                    logger.Log(LogType.Log, "Kernel with name: " + containedKernel.Key + " is already loaded. Skipping...");
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