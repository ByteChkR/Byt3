using System.Collections.Generic;
using System.Text;
using Byt3.ExtPP.Base;
using Byt3.ExtPP.Base.settings;
using Byt3.ExtPP.Plugins;

namespace Byt3.OpenCL.Common.ExtPP.API
{
    /// <summary>
    /// The PreProcessor Configuration used for OpenFL files
    /// </summary>
    public class FLPreProcessorConfig : APreProcessorConfig
    {
        private static StringBuilder _sb = new StringBuilder();
        protected override Verbosity VerbosityLevel { get; } = Verbosity.SILENT;

        protected override List<AbstractPlugin> Plugins
        {
            get
            {
                IncludePlugin inc = new IncludePlugin
                {
                    IncludeInlineKeyword = "pp_includeinl:",
                    IncludeKeyword = "pp_include:"
                };
                ConditionalPlugin cond = new ConditionalPlugin
                {
                    StartCondition = "pp_if:",
                    ElseIfCondition = "pp_elseif:",
                    ElseCondition = "pp_else:",
                    EndCondition = "pp_endif:",
                    DefineKeyword = "pp_define:",
                    UndefineKeyword = "pp_undefine:"
                };

                return new List<AbstractPlugin>
                {
                    new FakeGenericsPlugin(),
                    inc,
                    cond,
                    new ExceptionPlugin(),
                    new MultiLinePlugin()
                };
            }
        }

        public override string GetGenericInclude(string filename, string[] genType)
        {
            _sb.Clear();
            foreach (string gt in genType)
            {
                _sb.Append(gt);
                _sb.Append(' ');
            }

            string gens = _sb.Length == 0 ? "" : _sb.ToString();
            return "#pp_include: " + filename + " " + gens;
        }
    }
}