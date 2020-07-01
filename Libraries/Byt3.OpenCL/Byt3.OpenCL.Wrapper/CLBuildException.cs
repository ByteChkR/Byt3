using System.Collections.Generic;
using System.Linq;
using Byt3.Utilities.Exceptions;
using Byt3.Utilities.FastString;

namespace Byt3.OpenCL.Wrapper
{
    public class CLBuildException : Byt3Exception
    {
        public readonly List<CLProgramBuildResult> BuildResults;

        public CLBuildException(CLProgramBuildResult result) : this(new List<CLProgramBuildResult> {result})
        {
        }

        public CLBuildException(List<CLProgramBuildResult> results) : base(GetErrorText(results))
        {
            BuildResults = results;
        }

        private static string GetErrorText(List<CLProgramBuildResult> results)
        {
            string text = "";

            text += results.Select(x => x.ToString()).Unpack("\n\t, ");

            return text;
        }
    }
}