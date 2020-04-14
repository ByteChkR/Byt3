using System.Collections.Generic;
using System.Text;
using Byt3.ADL;
using Byt3.ExtPP.Base.Plugins;
using Byt3.ExtPP.Plugins;

namespace Byt3.ExtPP.API.Configuration
{
    /// <summary>
    /// The Default PreProcessor Configuration
    /// </summary>
    public class DefaultPreProcessorConfig : APreProcessorConfig
    {
        private static readonly StringBuilder Sb = new StringBuilder();
        protected override Verbosity VerbosityLevel { get; } = Verbosity.Silent;

        protected override List<AbstractPlugin> Plugins =>
            new List<AbstractPlugin>
            {
                new FakeGenericsPlugin(),
                new IncludePlugin(),
                new ConditionalPlugin {EnableDefine = true},
                new ExceptionPlugin(),
                new MultiLinePlugin()
            };

        public override string GetGenericInclude(string filename, string[] genType)
        {
            Sb.Append(" ");
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