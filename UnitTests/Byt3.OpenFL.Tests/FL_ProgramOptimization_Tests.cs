using Byt3.OpenFL.Benchmarking;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;
using Byt3.OpenFL.Common.Parsing.StageResults;
using NUnit.Framework;

namespace Byt3.OpenFL.Tests
{
    public class FL_ProgramOptimization_Tests
    {
        [Test]
        public void OpenFL_RemoveUnusedBuffers_Test()
        {
            TestSetup.SetupTestingSession();
            FLSetup setup = new FLSetup(nameof(OpenFL_RemoveUnusedBuffers_Test), "resources/kernel");


            SerializableFLProgram ret = setup.Parser.Process(
                new FLParserInput("resources/filter/program_checks/optimizations/remove_unused_buffers.fl"));

            Assert.AreEqual(1,
                ret.DefinedBuffers.Count); //1 and not 0 because the "in" (input) buffer is also in the defined buffers.
            setup.Dispose();
        }

        [Test]
        public void OpenFL_RemoveUnusedScripts_Test()
        {
            TestSetup.SetupTestingSession();
            FLSetup setup = new FLSetup(nameof(OpenFL_RemoveUnusedScripts_Test), "resources/kernel");


            SerializableFLProgram ret = setup.Parser.Process(
                new FLParserInput("resources/filter/program_checks/optimizations/remove_unused_scripts.fl"));

            Assert.AreEqual(0, ret.ExternalFunctions.Count);

            setup.Dispose();
        }

        [Test]
        public void OpenFL_RemoveUnusedFunctions_Test()
        {
            TestSetup.SetupTestingSession();
            FLSetup setup = new FLSetup(nameof(OpenFL_RemoveUnusedFunctions_Test), "resources/kernel");


            SerializableFLProgram ret = setup.Parser.Process(
                new FLParserInput("resources/filter/program_checks/optimizations/remove_unused_functions.fl"));

            Assert.AreEqual(2, ret.Functions.Count);

            setup.Dispose();
        }
    }
}