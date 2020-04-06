using System.Linq;
using Byt3.OpenCL.Common;
using Byt3.OpenCL.Wrapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Byt3.OpenCL.Tests
{
    [TestClass]
    public class ClSignatureAnalysis
    {
       
        [TestMethod]
        public void OpenCL_KernelSignatureAnalysis_Test()
        {
            DebugHelper.ThrowOnAllExceptions = true;
            DebugHelper.SeverityFilter = 10;

            Assert.IsTrue(TestSetup.KernelDb.TryGetClKernel("addv", out CLKernel kernel), "Didnt find kernel");

            Assert.IsTrue(kernel.Parameter.Count == 6, "Kernel header is not == 6");

            Assert.IsTrue(kernel.Parameter.ElementAt(0).Value.IsArray, "Image is not detected as array");
            Assert.IsTrue(kernel.Parameter.ElementAt(0).Value.DataType == Wrapper.TypeEnums.DataTypes.Uchar1,
                "Image is not detected as type uchar1");
            Assert.IsTrue(kernel.Parameter.ElementAt(0).Value.Id == 0, "Image has the wrong id");
            Assert.IsTrue(kernel.Parameter.ElementAt(0).Value.Name == "image", "Image has wrong argument name");

            Assert.IsFalse(kernel.Parameter.ElementAt(1).Value.IsArray, "Dimensions is detected as array");
            Assert.IsTrue(kernel.Parameter.ElementAt(1).Value.DataType == Wrapper.TypeEnums.DataTypes.Int3, "Dimensions has the wrong type");
            Assert.IsTrue(kernel.Parameter.ElementAt(1).Value.Id == 1, "dimensions has the wrong id");
            Assert.IsTrue(kernel.Parameter.ElementAt(1).Value.Name == "dimensions", "dimensions has wrong argument name");

            Assert.IsFalse(kernel.Parameter.ElementAt(2).Value.IsArray, "channelCount is detected as array");
            Assert.IsTrue(kernel.Parameter.ElementAt(2).Value.DataType == Wrapper.TypeEnums.DataTypes.Int1, "channelCount has the wrong type");
            Assert.IsTrue(kernel.Parameter.ElementAt(2).Value.Id == 2, "channelCount has the wrong id");
            Assert.IsTrue(kernel.Parameter.ElementAt(2).Value.Name == "channelCount",
                "channelCount has wrong argument name");

            Assert.IsFalse(kernel.Parameter.ElementAt(3).Value.IsArray, "maxValue is detected as array");
            Assert.IsTrue(kernel.Parameter.ElementAt(3).Value.DataType == Wrapper.TypeEnums.DataTypes.Float1, "maxValue has the wrong type");
            Assert.IsTrue(kernel.Parameter.ElementAt(3).Value.Id == 3, "maxValue has the wrong id");
            Assert.IsTrue(kernel.Parameter.ElementAt(3).Value.Name == "maxValue", "maxValue has wrong argument name");

            Assert.IsTrue(kernel.Parameter.ElementAt(4).Value.IsArray, "channelEnableState is not detected as array");
            Assert.IsTrue(kernel.Parameter.ElementAt(4).Value.DataType == Wrapper.TypeEnums.DataTypes.Uchar1,
                "channelEnableState has the wrong type");
            Assert.IsTrue(kernel.Parameter.ElementAt(4).Value.Id == 4, "channelEnableState has the wrong id");
            Assert.IsTrue(kernel.Parameter.ElementAt(4).Value.Name == "channelEnableState",
                "channelEnableState has wrong argument name");

            Assert.IsFalse(kernel.Parameter.ElementAt(5).Value.IsArray, "value is detected as array");
            Assert.IsTrue(kernel.Parameter.ElementAt(5).Value.DataType == Wrapper.TypeEnums.DataTypes.Float1, "value has the wrong type");
            Assert.IsTrue(kernel.Parameter.ElementAt(5).Value.Id == 5, "value has the wrong id");
            Assert.IsTrue(kernel.Parameter.ElementAt(5).Value.Name == "value", "value has wrong argument name");
        }
    }
}