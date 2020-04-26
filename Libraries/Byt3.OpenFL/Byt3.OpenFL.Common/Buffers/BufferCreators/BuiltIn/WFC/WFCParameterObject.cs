using Byt3.Callbacks;
using Byt3.OpenCL.Wrapper.Exceptions;
using Byt3.OpenFL.Common.Buffers.BufferCreators.BuiltIn.FromFile;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;
using Byt3.OpenFL.Common.Exceptions;

namespace Byt3.OpenFL.Common.Buffers.BufferCreators.BuiltIn.WFC
{
    public class WFCParameterObject
    {
        public int N { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Symmetry { get; set; }
        public int Ground { get; set; }
        public int Limit { get; set; }
        public bool PeriodicInput { get; set; }
        public bool PeriodicOutput { get; set; }
        public bool Force { get; set; }
        public SerializableFromBitmapFLBuffer SourceImage { get; set; }


        public WFCParameterObject(SerializableFromBitmapFLBuffer input, int n, int width, int height, int symmetry,
            int ground, int limit, bool periodicInput,
            bool periodicOutput, bool force)
        {
            N = n;
            Width = width;
            Height = height;
            Symmetry = symmetry;
            Ground = ground;
            Limit = limit;
            PeriodicInput = periodicInput;
            PeriodicOutput = periodicOutput;
            Force = force;
            SourceImage = input;
        }

        public static SerializableFLBuffer CreateBuffer(string name, string[] args, bool force)
        {
            if (args.Length < 10)
            {
                throw new FLInvalidFunctionUseException("wfc", "Invalid WFC Define statement");
            }

            if (!int.TryParse(args[2], out int n))
            {
                throw new FLInvalidFunctionUseException("wfc", "Invalid WFC Define statement");
            }

            if (!int.TryParse(args[3], out int widh))
            {
                throw new FLInvalidFunctionUseException("wfc", "Invalid WFC Define statement");
            }

            if (!int.TryParse(args[4], out int heigt))
            {
                throw new FLInvalidFunctionUseException("wfc", "Invalid WFC Define statement");
            }

            if (!bool.TryParse(args[5], out bool periodicInput))
            {
                throw new FLInvalidFunctionUseException("wfc", "Invalid WFC Define statement");
            }

            if (!bool.TryParse(args[6], out bool periodicOutput))
            {
                throw new FLInvalidFunctionUseException("wfc", "Invalid WFC Define statement");
            }

            if (!int.TryParse(args[7], out int symmetry))
            {
                throw new FLInvalidFunctionUseException("wfc", "Invalid WFC Define statement");
            }

            if (!int.TryParse(args[8], out int ground))
            {
                throw new FLInvalidFunctionUseException("wfc", "Invalid WFC Define statement");
            }

            if (!int.TryParse(args[9], out int limit))
            {
                throw new FLInvalidFunctionUseException("wfc", "Invalid WFC Define statement");
            }

            string fn = args[1].Trim().Replace("\"", "");
            if (IOManager.FileExists(fn))
            {
                SerializableFromFileFLBuffer input = new SerializableFromFileFLBuffer("WFCInputBuffer", fn);
                WFCParameterObject wfcPO = new WFCParameterObject(input, n, widh, heigt, symmetry, ground, limit,
                    periodicInput, periodicOutput, force);
                return new SerializableWaveFunctionCollapseFLBuffer(name, wfcPO);
            }
            else
            {
                throw
                    new FLInvalidFunctionUseException("wfc", "Invalid WFC Image statement",
                        new InvalidFilePathException(fn));
            }
        }
    }
}