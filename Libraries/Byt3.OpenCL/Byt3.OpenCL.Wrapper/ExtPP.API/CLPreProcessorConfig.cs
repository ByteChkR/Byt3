using System.Collections.Generic;
using System.Text;
using Byt3.ExtPP.API.Configuration;
using Byt3.ExtPP.Base.Plugins;
using Byt3.ExtPP.Plugins;

namespace Byt3.OpenCL.Wrapper.ExtPP.API
{
    /// <summary>
    /// The PreProcessor Configuration used for OpenGL and OpenCL files
    /// </summary>
    public class CLPreProcessorConfig : APreProcessorConfig
    {
        public override string FileExtension => ".cl";
        private static readonly StringBuilder Sb = new StringBuilder();

        protected override List<AbstractPlugin> Plugins =>
            new List<AbstractPlugin>
            {
                new FakeGenericsPlugin(){Stage = "onload"},
                new IncludePlugin(),
                new ConditionalPlugin(){Stage = "onload"},
                new ExceptionPlugin(),
                new MultiLinePlugin()
            };

        public override string GetGenericInclude(string filename, string[] genType)
        {
            Sb.Clear();
            foreach (string gt in genType)
            {
                Sb.Append(gt);
                Sb.Append(' ');
            }


            string gens = Sb.Length == 0 ? "" : Sb.ToString();
            return "#include " + filename + " " + gens;
        }
    }
}