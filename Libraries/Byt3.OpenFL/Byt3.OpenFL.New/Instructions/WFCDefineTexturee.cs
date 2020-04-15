using System;
using System.Drawing;
using Byt3.OpenCL.Common.Exceptions;
using Byt3.OpenCL.Memory;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenFL.New.DataObjects;
using Byt3.OpenFL.New.Exceptions;
using Byt3.OpenFL.WFC;

namespace Byt3.OpenFL.New.Instructions
{
    public static class WFCDefineTexture
    {
        private static readonly Random Rnd = new Random();
        /// <summary>
        /// A function used as RandomFunc of type byte>
        /// </summary>
        /// <returns>a random byte</returns>
        private static byte Randombytesource()
        {
            return (byte)Rnd.Next();
        }

        public static FLBufferInfo ComputeRnd(ParsedSource Root, string[] args)
        {
            MemoryBuffer buf =
                CLAPI.CreateEmpty<byte>(Root.Instance, Root.InputSize, MemoryFlag.ReadWrite | MemoryFlag.CopyHostPointer);
            CLAPI.WriteRandom(Root.Instance, buf, Randombytesource, new byte[] { 1, 1, 1, 1 }, false);

            FLBufferInfo info = new FLBufferInfo(buf, Root.Dimensions.x, Root.Dimensions.y);
            info.SetKey("RNDBuffer");
            return info;
        }

        public static FLBufferInfo ComputeUrnd(ParsedSource Root, string[] args)
        {
            MemoryBuffer buf =
                CLAPI.CreateEmpty<byte>(Root.Instance, Root.InputSize, MemoryFlag.ReadWrite | MemoryFlag.CopyHostPointer);
            CLAPI.WriteRandom(Root.Instance, buf, Randombytesource, new byte[] { 1, 1, 1, 1 }, true);

            FLBufferInfo info = new FLBufferInfo(buf, Root.Dimensions.x, Root.Dimensions.y);
            info.SetKey("URNDBuffer");
            return info;
        }

        public static UnloadedDefinedFLBufferInfo ComputeWFC(CLAPI instance, string[] args)
        {
            string filename = args[0];
            if (filename == "wfc" || filename == "wfcf")
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
                    string fn = args[1].Trim().Replace("\"", "");
                    if (CLAPI.FileExists(fn))
                    {

                        UnloadedDefinedFLBufferInfo info = new UnloadedDefinedFLBufferInfo(() =>
                        {
                            Bitmap bmp;
                            WFCOverlayMode wfc = new WFCOverlayMode(fn, n, widh,
                                heigt, periodicInput, periodicOutput, symmetry, ground);
                            if (force)
                            {
                                do
                                {
                                    wfc.Run(limit);
                                    bmp = new Bitmap(wfc.Graphics(), new Size(widh, heigt)); //Apply scaling
                                } while (!wfc.Success);
                            }
                            else
                            {
                                wfc.Run(limit);
                                bmp = new Bitmap(wfc.Graphics(), new Size(widh, heigt)); //Apply scaling
                            }

                            return bmp;
                        });
                        info.SetKey("WFCBuffer");
                        return info;
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
                throw
                    new FLInvalidFunctionUseException("wfc", "Invalid WFC Image statement");
            }
        }
    }
}