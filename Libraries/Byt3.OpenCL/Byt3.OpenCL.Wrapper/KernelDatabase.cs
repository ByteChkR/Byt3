using System;
using System.Collections.Generic;
using System.IO;
using Byt3.ADL;
using Byt3.ExtPP.API;
using Byt3.ExtPP.Base.Interfaces;
using Byt3.OpenCL.Wrapper.ExtPP.API;

namespace Byt3.OpenCL.Wrapper
{
    /// <summary>
    /// A class used to store and manage Kernels
    /// </summary>
    public class KernelDatabase : ALoggable<LogType>
    {

        static KernelDatabase()
        {
            TextProcessorAPI.Configs[".cl"] = new CLPreProcessorConfig();
        }

        //private readonly ADLLogger<LogType> logger = new ADLLogger<LogType>(OpenCLDebugConfig.Settings, "CL-KernelDB");
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
        /// <param name="genDataVectorType">The DataVectorTypes used to compile the FL Database</param>
        public KernelDatabase(CLAPI instance, string folderName, TypeEnums.DataVectorTypes genDataVectorType) : base(OpenCLDebugConfig.Settings)
        {
            GenDataType = KernelParameter.GetDataString(genDataVectorType);
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

            Logger.Log(LogType.Log, "Creating CLProgram from file: " + file,3);
            CLProgram program = new CLProgram(instance, path, GenDataType);

            foreach (KeyValuePair<string, CLKernel> containedKernel in program.ContainedKernels)
            {
                if (!LoadedKernels.ContainsKey(containedKernel.Key))
                {
                    Logger.Log(LogType.Log, "Adding Kernel: " + containedKernel.Key,4);
                    LoadedKernels.Add(containedKernel.Key, containedKernel.Value);
                }
                else
                {
                    Logger.Log(LogType.Log,
                        "Kernel with name: " + containedKernel.Key + " is already loaded. Skipping...",5);
                }
            }
        }

        public CLKernel GetClKernel(string name)
        {
            return LoadedKernels[name];
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