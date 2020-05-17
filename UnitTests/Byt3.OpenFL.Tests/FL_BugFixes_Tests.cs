using Byt3.OpenCL.Wrapper;
using Byt3.OpenFL.Benchmarking;
using Byt3.OpenFL.Common;
using Byt3.OpenFL.Common.Buffers;
using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;
using Byt3.OpenFL.Common.Parsing.StageResults;
using NUnit.Framework;

namespace Byt3.OpenFL.Tests
{
    public class FL_BugFixes_Tests
    {
        [Test]
        public void CorrectUnderlyingBuffer_For_IN_Test()
        {
            TestSetup.SetupTestingSession();
            string file = "resources/filter/bug_checks/set_temp_to_in_bug.fl";

            FLSetup s = new FLSetup(nameof(CorrectUnderlyingBuffer_For_IN_Test), "resources/kernel");
            SerializableFLProgram p = s.Parser.Process(new FLParserInput(file));
            FLProgram pp = p.Initialize(CLAPI.MainThread, s.InstructionSet);
            FLBuffer b = new FLBuffer(CLAPI.MainThread, 128, 128, "TestInput");
            pp.Run(b, true);

            FLBuffer i = pp.GetActiveBuffer(false);
            byte[] c = CLAPI.ReadBuffer<byte>(CLAPI.MainThread, i.Buffer, (int) i.Size);
            for (int j = 0; j < c.Length; j++)
            {
                if (c[j] != 255)
                {
                    Assert.Fail("Output does not match expected");
                }
            }


            pp.FreeResources();
        }
    }
}