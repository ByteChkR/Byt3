using System.Drawing;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;
using Byt3.OpenFL.Common.WFC;

namespace Byt3.OpenFL.Common.Buffers.BufferCreators.BuiltIn.WFC
{
    public class SerializableWaveFunctionCollapseFLBuffer : SerializableFLBuffer
    {
        public WFCParameterObject Parameter { get; }
        public readonly int Size;
        public SerializableWaveFunctionCollapseFLBuffer(string name, WFCParameterObject parameter, bool isArray) : base(name, isArray)
        {
            Parameter = parameter;
        }

        public override FLBuffer GetBuffer()
        {
            if (IsArray)
            {
                return new LazyLoadingFLBuffer(root =>
                {
                    WFCOverlayMode wfc = new WFCOverlayMode(Parameter.SourceImage.Bitmap, Parameter.N, Parameter.Width,
                        Parameter.Height, Parameter.PeriodicInput, Parameter.PeriodicOutput, Parameter.Symmetry,
                        Parameter.Ground);
                    if (Parameter.Force)
                    {
                        do
                        {
                            wfc.Run(Parameter.Limit);
                        } while (!wfc.Success);
                    }
                    else
                    {
                        wfc.Run(Parameter.Limit);
                    }


                    Bitmap bmp = wfc.Graphics();
                    return new FLBuffer(root.Instance, bmp, "WFCBuffer." + Name);
                });
            }

            LazyLoadingFLBuffer info = new LazyLoadingFLBuffer(root =>
            {
                Bitmap bmp;
                WFCOverlayMode wfc = new WFCOverlayMode(Parameter.SourceImage.Bitmap, Parameter.N, Parameter.Width,
                    Parameter.Height, Parameter.PeriodicInput, Parameter.PeriodicOutput, Parameter.Symmetry,
                    Parameter.Ground);
                if (Parameter.Force)
                {
                    do
                    {
                        wfc.Run(Parameter.Limit);
                        bmp = new Bitmap(wfc.Graphics(), new Size(root.Dimensions.x, root.Dimensions.y)); //Apply scaling
                    } while (!wfc.Success);
                }
                else
                {
                    wfc.Run(Parameter.Limit);
                    bmp = new Bitmap(wfc.Graphics(), new Size(root.Dimensions.x, root.Dimensions.y)); //Apply scaling
                }
                

                return new FLBuffer(root.Instance, bmp, "WFCBuffer." + Name);
            });
            return info;
        }

        public override string ToString()
        {
            return $"{FLKeywords.DefineTextureKey} {Name}: wfc{(Parameter.Force?"f":"")} {Parameter}";
        }
    }
}