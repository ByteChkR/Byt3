using System.Collections.Generic;
using System.Text;
using Byt3.ExtPP.Base;
using Byt3.ExtPP.Base.settings;
using Byt3.ExtPP.Plugins;

namespace Byt3.OpenCL.Common.ExtPP.API
{
    /// <summary>
    /// The PreProcessor Configuration used for OpenGL and OpenCL files
    /// </summary>
    public class GLCLPreProcessorConfig : APreProcessorConfig
    {
        private static StringBuilder _sb = new StringBuilder();
        protected override Verbosity VerbosityLevel { get; } = Verbosity.SILENT;

        protected override List<AbstractPlugin> Plugins =>
            new List<AbstractPlugin>
            {
                new FakeGenericsPlugin(),
                new IncludePlugin(),
                new ConditionalPlugin(),
                new ExceptionPlugin(),
                new MultiLinePlugin()
            };

        public override string GetGenericInclude(string filename, string[] genType)
        {
            _sb.Clear();
            foreach (string gt in genType)
            {
                _sb.Append(gt);
                _sb.Append(' ');
            }


            string gens = _sb.Length == 0 ? "" : _sb.ToString();
            return "#include " + filename + " " + gens;
        }
    }
}