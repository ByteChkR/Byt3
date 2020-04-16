using System.Drawing;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenCL.Wrapper.Exceptions;
using Byt3.OpenFL.Parsing.Exceptions;
using Byt3.OpenFL.Parsing.WFC;

namespace Byt3.OpenFL.Parsing.Instructions
{
    public static class WFCDefineTexture
    {

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

                        UnloadedDefinedFLBufferInfo info = new UnloadedDefinedFLBufferInfo(root =>
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

                            Bitmap b = new Bitmap(bmp, root.Dimensions.x, root.Dimensions.y);

                            return new FLBufferInfo(root.Instance, b);
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