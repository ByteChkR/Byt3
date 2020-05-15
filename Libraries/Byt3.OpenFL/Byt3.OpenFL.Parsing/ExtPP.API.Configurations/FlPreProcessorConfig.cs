using System.Collections.Generic;
using System.Text;
using Byt3.ExtPP.API.Configuration;
using Byt3.ExtPP.Base.Plugins;
using Byt3.ExtPP.Plugins;
using Byt3.OpenFL.Common;

namespace Byt3.OpenFL.Parsing.ExtPP.API.Configurations
{
    /// <summary>
    /// The PreProcessor Configuration used for OpenFL files
    /// </summary>
    public class FLPreProcessorConfig : APreProcessorConfig
    {
        public override string FileExtension => ".fl";
        private static readonly StringBuilder Sb = new StringBuilder();

        protected override List<AbstractPlugin> Plugins
        {
            get
            {
                IncludePlugin inc = new IncludePlugin
                {
                    IncludeInlineKeyword = FLKeywords.IncludeInlineKeyword,
                    IncludeKeyword = FLKeywords.IncludeKeyword
                };

                ConditionalPlugin cond = new ConditionalPlugin
                {
                    StartCondition = FLKeywords.StartCondition,
                    ElseIfCondition = FLKeywords.ElseIfCondition,
                    ElseCondition = FLKeywords.ElseCondition,
                    EndCondition = FLKeywords.EndCondition,
                    DefineKeyword = FLKeywords.DefineKeyword,
                    UndefineKeyword = FLKeywords.UndefineKeyword
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
            Sb.Clear();
            foreach (string gt in genType)
            {
                Sb.Append(gt);
                Sb.Append(' ');
            }

            string gens = Sb.Length == 0 ? "" : Sb.ToString();
            return "#pp_include: " + filename + " " + gens;
        }
    }
}