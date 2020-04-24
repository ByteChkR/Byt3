using System.Drawing;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;
using Byt3.OpenFL.Common.WFC;

namespace Byt3.OpenFL.Common.Buffers.BufferCreators.BuiltIn.WFC
{
    public class SerializableWaveFunctionCollapseFLBuffer : SerializableFLBuffer
    {
        public WFCParameterObject Parameter { get; }

        public SerializableWaveFunctionCollapseFLBuffer(string name, WFCParameterObject parameter) : base(name)
        {
            Parameter = parameter;
        }

        public override FLBuffer GetBuffer()
        {
            LazyLoadingFLBuffer info = new LazyLoadingFLBuffer(root =>
            {
                Bitmap bmp;
                WFCOverlayMode wfc = new WFCOverlayMode(Parameter.SourceImage.GetBitmap(), Parameter.N, Parameter.Width,
                    Parameter.Height, Parameter.PeriodicInput, Parameter.PeriodicOutput, Parameter.Symmetry,
                    Parameter.Ground);
                if (Parameter.Force)
                {
                    do
                    {
                        wfc.Run(Parameter.Limit);
                        bmp = new Bitmap(wfc.Graphics(), new Size(Parameter.Width, Parameter.Height)); //Apply scaling
                    } while (!wfc.Success);
                }
                else
                {
                    wfc.Run(Parameter.Limit);
                    bmp = new Bitmap(wfc.Graphics(), new Size(Parameter.Width, Parameter.Height)); //Apply scaling
                }

                Bitmap b = new Bitmap(bmp, root.Dimensions.x, root.Dimensions.y);

                return new FLBuffer(root.Instance, b, "WFCBuffer."+Name);
            });
            return info;
        }
    }
}