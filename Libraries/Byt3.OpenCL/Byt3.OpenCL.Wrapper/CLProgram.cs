using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Byt3.Callbacks;
using Byt3.ExtPP.API;
using Byt3.OpenCL.Kernels;
using Byt3.OpenCL.Programs;

namespace Byt3.OpenCL.Wrapper
{
    /// <summary>
    /// A wrapper class for a OpenCL Program.
    /// </summary>
    public class CLProgram
    {
        /// <summary>
        /// The filepath of the program source
        /// </summary>
        private readonly string filePath;


        /// <summary>
        /// Public Constructor
        /// </summary>
        /// <param name="instance">CLAPI Instance for the current thread</param>
        /// <param name="filePath">The FilePath where the source is located</param>
        /// <param name="genType">The Gen Type used</param>
        public CLProgram(CLAPI instance, string filePath)
        {
            this.filePath = filePath;

            ContainedKernels = new Dictionary<string, CLKernel>();

            Initialize(instance);
        }

        /// <summary>
        /// The kernels that are contained in the Program
        /// </summary>
        public Dictionary<string, CLKernel> ContainedKernels { get; }

        /// <summary>
        /// The Compiled OpenCL Program
        /// </summary>
        public Program ClProgramHandle { get; set; }

        public void Dispose()
        {
            ClProgramHandle.Dispose();
            foreach (KeyValuePair<string, CLKernel> containedKernel in ContainedKernels)
            {
                containedKernel.Value.Dispose();
            }
        }

        /// <summary>
        /// Returns the N of the VectorN types
        /// </summary>
        /// <param name="dtStr">the cl type in use</param>
        /// <returns>the amount of dimensions in the vector type</returns>
        public static int GetVectorNum(string dtStr)
        {
            if (!char.IsNumber(dtStr.Last()))
            {
                return 1;
            }

            if (dtStr.Last() == '2')
            {
                return 2;
            }

            if (dtStr.Last() == '3')
            {
                return 3;
            }

            if (dtStr.Last() == '4')
            {
                return 4;
            }

            if (dtStr.Last() == '8')
            {
                return 8;
            }

            if (dtStr.Last() == '6')
            {
                return 16;
            }

            return 0;
        }

        /// <summary>
        /// Loads the source and initializes the CLProgram
        /// </summary>
        private void Initialize(CLAPI instance)
        {
            string source = TextProcessorAPI.PreprocessSource(IOManager.ReadAllLines(filePath), Path.GetDirectoryName(filePath), new Dictionary<string, bool>());
            string[] kernelNames = FindKernelNames(source);

            ClProgramHandle = CLAPI.CreateClProgramFromSource(instance, source);


            foreach (string kernelName in kernelNames)
            {
                Kernel k = CLAPI.CreateKernelFromName(ClProgramHandle, kernelName);
                int kernelNameIndex = source.IndexOf(" " + kernelName + " ", StringComparison.InvariantCulture);
                kernelNameIndex = kernelNameIndex == -1
                    ? source.IndexOf(" " + kernelName + "(", StringComparison.InvariantCulture)
                    : kernelNameIndex;
                KernelParameter[] parameter = KernelParameter.CreateKernelParametersFromKernelCode(source,
                    kernelNameIndex,
                    source.Substring(kernelNameIndex, source.Length - kernelNameIndex).IndexOf(')') + 1);
                if (k == null)
                {
                    ContainedKernels.Add(kernelName, new CLKernel(instance, null, kernelName, parameter));
                }
                else
                {
                    ContainedKernels.Add(kernelName, new CLKernel(instance, k, kernelName, parameter));
                }
            }
        }


        /// <summary>
        /// Extracts the kernel names from the program source
        /// </summary>
        /// <param name="source">The complete source of the program</param>
        /// <returns>A list of kernel names</returns>
        private static string[] FindKernelNames(string source)
        {
            List<string> kernelNames = new List<string>();
            string[] s = source.Split(' ');
            List<string> parts = new List<string>();
            foreach (string part in s)
            {
                parts.AddRange(part.Split('\n'));
            }

            for (int i = 0; i < parts.Count; i++)
            {
                if (parts[i] == "__kernel" || parts[i] == "kernel")
                {
                    if (i < parts.Count - 2 && parts[i + 1] == "void")
                    {
                        if (parts[i + 2].Contains('('))
                        {
                            kernelNames.Add(
                                parts[i + 2]. //The Kernel name
                                    Substring(0,
                                        parts[i + 2].IndexOf('(')
                                    )
                            );
                        }
                        else
                        {
                            kernelNames.Add(parts[i + 2]);
                        }
                    }
                }
            }

            return kernelNames.ToArray();
        }
    }
}