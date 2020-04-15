using System;
using System.Collections.Generic;
using Byt3.ADL;
using Byt3.OpenCL.Common;
using Byt3.OpenCL.Common.Exceptions;
using Byt3.OpenCL.Memory;
using Byt3.OpenCL.Wrapper;

namespace Byt3.OpenFL
{
    /// <summary>
    /// partial class that contains the logic how to Parse Defines that are referencing another fl script
    /// </summary>
    public partial class FLInterpreter
    {
        /// <summary>
        /// Define handler that loads defined scripts
        /// </summary>
        /// <param name="instance">CLAPI Instance of the Current Thread</param>
        /// <param name="arg">Args from the FL Script</param>
        /// <param name="defines">Defines</param>
        /// <param name="width">width of the input buffer</param>
        /// <param name="height">height of the input buffer</param>
        /// <param name="depth">depth of the input buffer</param>
        /// <param name="channelCount">channel count of the input buffer</param>
        /// <param name="kernelDb">the kernel database to use</param>
        private void DefineScript(CLAPI instance, string[] arg, Dictionary<string, CLBufferInfo> defines,
            int width, int height,
            int depth, int channelCount, KernelDatabase kernelDb)
        {
            if (arg.Length < 2)
            {
                throw new FLInvalidFunctionUseException(SCRIPT_DEFINE_KEY, "Invalid Define statement");
            }

            string varname = arg[0].Trim();
            if (defines.ContainsKey(varname))
            {
                Logger.Log(LogType.Error, Verbosity.Level1, $"Overwriting {varname}");
                defines.Remove(varname);
            }

            string[] args = arg[1].Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);


            string filename = args[0].Trim();

            int inputBufferSize = width * height * depth * channelCount;

            if (IsSurroundedBy(filename, FILEPATH_INDICATOR))
            {
                Logger.Log(LogType.Error, Verbosity.Level1, "Loading SubScript...");

                MemoryBuffer buf =
                    CLAPI.CreateEmpty<byte>(instance, inputBufferSize, MemoryFlag.ReadWrite);


                string fn = filename.Replace(FILEPATH_INDICATOR, "");


                if (CLAPI.FileExists(fn))
                {
                    FLInterpreter flInterpreter = new FLInterpreter(instance, fn, buf, width, height,
                        depth, channelCount, kernelDb, true);

                    do
                    {
                        flInterpreter.Step();
                    } while (!flInterpreter.Terminated);

                    CLBufferInfo info = flInterpreter.GetActiveBufferInternal();
                    info.SetKey(varname);
                    defines.Add(varname, info);
                    flInterpreter.ReleaseResources();
                }
                else
                {
                    throw
                        new FLInvalidFunctionUseException(SCRIPT_DEFINE_KEY, "Not a valid filepath as argument.",
                            new InvalidFilePathException(fn));
                }
            }
            else
            {
                throw new FLInvalidFunctionUseException(SCRIPT_DEFINE_KEY, "Not a valid filepath as argument.");
            }
        }
    }
}