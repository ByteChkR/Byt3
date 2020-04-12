using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using Byt3.ADL;
using Byt3.OpenCL.Common;
using Byt3.OpenCL.Common.Exceptions;
using Byt3.OpenCL.Memory;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenFL.WFC;

namespace Byt3.OpenFL
{
    /// <summary>
    /// partial class that contains the logic how to Parse Defines that are referencing a texture or other keywords
    /// </summary>
    public partial class FLInterpreter
    {
        /// <summary>
        /// Define handler that loads defined Textures
        /// </summary>
        /// <param name="instance">CLAPI Instance of the Current Thread</param>
        /// <param name="arg">Args from the FL Script</param>
        /// <param name="defines">Defines</param>
        /// <param name="width">width of the input buffer</param>
        /// <param name="height">height of the input buffer</param>
        /// <param name="depth">depth of the input buffer</param>
        /// <param name="channelCount">channel count of the input buffer</param>
        /// <param name="kernelDb">the kernel database to use</param>
        private void DefineTexture(CLAPI instance, string[] arg, Dictionary<string, CLBufferInfo> defines,
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
                Logger.Log(DebugChannel.Error, Verbosity.Level1, "Overwriting " + varname,
                    DebugChannel.Warning | DebugChannel.OpenFL, 10);
                defines.Remove(varname);
            }

            MemoryFlag flags = MemoryFlag.ReadWrite;
            string[] flagTest = varname.Split(' ');
            if (flagTest.Length > 1)
            {
                varname = flagTest[1];
                if (flagTest[0] == "r")
                {
                    flags = MemoryFlag.ReadOnly;
                }

                else if (flagTest[0] == "w")
                {
                    flags = MemoryFlag.WriteOnly;
                }
            }

            string[] args = arg[1].Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);


            string filename = args[0].Trim();
            byte[] activeChannels = new byte[channelCount];
            for (int i = 0; i < activeChannels.Length; i++)
            {
                activeChannels[i] = 1;
            }

            int inputBufferSize = width * height * depth * channelCount;

            if (IsSurroundedBy(filename, FILEPATH_INDICATOR))
            {
                string fn = filename.Replace(FILEPATH_INDICATOR, "");
                if (File.Exists(fn))
                {
                    Bitmap bmp = new Bitmap((Bitmap) System.Drawing.Image.FromFile(fn), width, height);
                    CLBufferInfo info = new CLBufferInfo(CLAPI.CreateFromImage(instance, bmp,
                        MemoryFlag.CopyHostPointer | flags), true);
                    info.SetKey(varname);
                    defines.Add(varname, info);
                }
                else
                {
                    throw
                        new FLInvalidFunctionUseException(DEFINE_KEY, "Invalid Filepath",
                            new InvalidFilePathException(fn));
                }
            }
            else if (filename == "rnd")
            {
                MemoryBuffer buf =
                    CLAPI.CreateEmpty<byte>(instance, inputBufferSize, flags | MemoryFlag.CopyHostPointer);
                CLAPI.WriteRandom(instance, buf, Randombytesource, activeChannels, false);

                CLBufferInfo info = new CLBufferInfo(buf, true);
                info.SetKey(varname);
                defines.Add(varname, info);
            }
            else if (filename == "urnd")
            {
                MemoryBuffer buf =
                    CLAPI.CreateEmpty<byte>(instance, inputBufferSize, flags | MemoryFlag.CopyHostPointer);
                CLAPI.WriteRandom(instance, buf, Randombytesource, activeChannels, true);

                CLBufferInfo info = new CLBufferInfo(buf, true);
                info.SetKey(varname);
                defines.Add(varname, info);
            }
            else if (filename == "empty")
            {
                CLBufferInfo info = new CLBufferInfo(CLAPI.CreateEmpty<byte>(instance, inputBufferSize, flags), true);
                info.SetKey(varname);
                defines.Add(varname, info);
            }
            else if (filename == "wfc" || filename == "wfcf")
            {
                bool force = filename == "wfcf";
                if (args.Length < 10)
                {
                    throw new FLInvalidFunctionUseException("wfc", "Invalid WFC Define statement");
                }
                else if (!int.TryParse(args[2], out int n))
                {
                    throw new FLInvalidFunctionUseException("wfc", "Invalid WFC Define statement");
                }
                else if (!int.TryParse(args[3], out int widh))
                {
                    throw new FLInvalidFunctionUseException("wfc", "Invalid WFC Define statement");
                }
                else if (!int.TryParse(args[4], out int heigt))
                {
                    throw new FLInvalidFunctionUseException("wfc", "Invalid WFC Define statement");

                }
                else if (!bool.TryParse(args[5], out bool periodicInput))
                {
                    throw new FLInvalidFunctionUseException("wfc", "Invalid WFC Define statement");

                }
                else if (!bool.TryParse(args[6], out bool periodicOutput))
                {
                    throw new FLInvalidFunctionUseException("wfc", "Invalid WFC Define statement");
                }
                else if (!int.TryParse(args[7], out int symmetry))
                {
                    throw new FLInvalidFunctionUseException("wfc", "Invalid WFC Define statement");
                }
                else if (!int.TryParse(args[8], out int ground))
                {
                    throw new FLInvalidFunctionUseException("wfc", "Invalid WFC Define statement");
                }
                else if (!int.TryParse(args[9], out int limit))
                {
                    throw new FLInvalidFunctionUseException("wfc", "Invalid WFC Define statement");
                }
                else
                {
                    string fn = args[1].Trim().Replace(FILEPATH_INDICATOR, "");
                    if (CLAPI.FileExists(fn))
                    {
                        Bitmap bmp;
                        WFCOverlayMode wfc = new WFCOverlayMode(fn, n, widh,
                            heigt, periodicInput, periodicOutput, symmetry, ground);
                        if (force)
                        {
                            do
                            {
                                wfc.Run(limit);
                                bmp = new Bitmap(wfc.Graphics(), new Size(width, height)); //Apply scaling
                            } while (!wfc.Success);
                        }
                        else
                        {
                            wfc.Run(limit);
                            bmp = new Bitmap(wfc.Graphics(), new Size(width, height)); //Apply scaling
                        }

                        CLBufferInfo info = new CLBufferInfo(CLAPI.CreateFromImage(instance, bmp,
                            MemoryFlag.CopyHostPointer | flags), true);
                        info.SetKey(varname);
                        defines.Add(varname, info);
                    }
                    else
                    {
                        throw
                            new FLInvalidFunctionUseException("wfc", "Invalid WFC Image statement",
                                new InvalidFilePathException(fn));
                    }
                }
            }

            else
            {
                StringBuilder s = new StringBuilder();
                foreach (string s1 in args)
                {
                    s.Append(s1 + " ");
                }

                throw new FLInvalidFunctionUseException(DEFINE_KEY, "Define statement wrong: " + s);
            }
        }
    }
}