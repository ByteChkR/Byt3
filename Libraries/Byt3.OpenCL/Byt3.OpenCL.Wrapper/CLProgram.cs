using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Byt3.Callbacks;
using Byt3.ExtPP.API;
using Byt3.OpenCL.Kernels;
using Byt3.OpenCL.Programs;
using Byt3.Utilities.FastString;

namespace Byt3.OpenCL.Wrapper
{
    public enum ErrorType { ProgramBuild, KernelBuild, }
    public struct CLProgramBuildError
    {
        public readonly ErrorType Error;
        public readonly Exception Exception;
        public string Message => Exception.InnerException != null ? Exception.InnerException.Message : Exception.Message;

        public CLProgramBuildError(ErrorType error, Exception exception)
        {
            Error = error;
            Exception = exception;
        }

        public override string ToString()
        {
            return $"[{Error}] {Exception.GetType().Name} {Message}";
        }
    }

    public struct CLProgramBuildResult
    {

        public readonly string TargetFile;
        public bool Success => BuildErrors.Count == 0;
        public readonly List<CLProgramBuildError> BuildErrors;

        public CLProgramBuildResult(string targetFile, List<CLProgramBuildError> errors)
        {
            TargetFile = targetFile;
            BuildErrors = errors;
        }

        public AggregateException GetAggregateException() => new AggregateException(BuildErrors.Select(x => x.Exception));

        public static implicit operator bool(CLProgramBuildResult result)
        {
            return result.Success;
        }

        public override string ToString()
        {
            return $"{TargetFile}: \n\t{BuildErrors.Select(x => x.ToString()).Unpack("\n\t")}";
        }
    }

    /// <summary>
    /// A wrapper class for a OpenCL Program.
    /// </summary>
    public class CLProgram
    {
        /// <summary>
        /// The filepath of the program source
        /// </summary>
        private readonly string filePath;


        ///// <summary>
        ///// Public Constructor
        ///// </summary>
        ///// <param name="instance">CLAPI Instance for the current thread</param>
        ///// <param name="filePath">The FilePath where the source is located</param>
        ///// <param name="genType">The Gen Type used</param>
        //public CLProgram(CLAPI instance, string filePath)
        //{
        //    this.filePath = filePath;

        //    ContainedKernels = new Dictionary<string, CLKernel>();

        //    Initialize(instance);
        //}

        private CLProgram(string filePath, Dictionary<string, CLKernel> kernels)
        {
            this.filePath = filePath;
            ContainedKernels = kernels;
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
            ClProgramHandle?.Dispose();
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

        public static CLProgramBuildResult TryBuildProgram(CLAPI instance, string filePath, out CLProgram program)
        {
            string source = TextProcessorAPI.PreprocessSource(IOManager.ReadAllLines(filePath),
                Path.GetDirectoryName(filePath), Path.GetExtension(filePath), new Dictionary<string, bool>());

            //            program = null;
            CLProgramBuildResult result = new CLProgramBuildResult(filePath, new List<CLProgramBuildError>());
            Program prgHandle;

            try
            {
                prgHandle = CLAPI.CreateClProgramFromSource(instance, source);
            }
            catch (Exception e)
            {
                result.BuildErrors.Add(new CLProgramBuildError(ErrorType.ProgramBuild, e));
                program = null;
                return result; //We can not progress
            }


            string[] kernelNames = FindKernelNames(source);
            Dictionary<string, CLKernel> kernels = new Dictionary<string, CLKernel>();
            foreach (string kernelName in kernelNames)
            {
                try
                {
                    Kernel k = CLAPI.CreateKernelFromName(prgHandle, kernelName);
                    int kernelNameIndex = source.IndexOf(" " + kernelName + " ", StringComparison.InvariantCulture);
                    kernelNameIndex = kernelNameIndex == -1
                        ? source.IndexOf(" " + kernelName + "(", StringComparison.InvariantCulture)
                        : kernelNameIndex;
                    KernelParameter[] parameter = KernelParameter.CreateKernelParametersFromKernelCode(source,
                        kernelNameIndex,
                        source.Substring(kernelNameIndex, source.Length - kernelNameIndex).IndexOf(')') + 1);
                    if (k == null)
                    {
                        result.BuildErrors.Add(new CLProgramBuildError(ErrorType.KernelBuild, new OpenClException($"Header parser completed on {kernelName} but the kernel could not be loaded.")));
                        kernels.Add(kernelName, new CLKernel(instance, null, kernelName, parameter));
                    }
                    else
                    {
                        kernels.Add(kernelName, new CLKernel(instance, k, kernelName, parameter));
                    }
                }
                catch (Exception e)
                {
                    result.BuildErrors.Add(new CLProgramBuildError(ErrorType.KernelBuild, e));
                    //We can try other kernels
                }
            }

            program = new CLProgram(filePath, kernels);
            return result;
        }

        /// <summary>
        /// Loads the source and initializes the CLProgram
        /// </summary>
        private void Initialize(CLAPI instance)
        {
            string source = TextProcessorAPI.PreprocessSource(IOManager.ReadAllLines(filePath),
                Path.GetDirectoryName(filePath), Path.GetExtension(filePath), new Dictionary<string, bool>());
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