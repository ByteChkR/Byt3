using Byt3.OpenFL.Benchmarking;
using Byt3.OpenFL.Common.Exceptions;
using Byt3.OpenFL.Parsing.Stages;
using NUnit.Framework;

namespace Byt3.OpenFL.Tests
{
    public class FL_ProgramCheck_Tests
    {



        [Test]
        public void OpenFL_FilePathValidator_Incorrect_Test()
        {

            TestSetup.SetupTestingSession();
            FLSetup setup = new FLSetup(nameof(OpenFL_FilePathValidator_Incorrect_Test), "resources/kernel");

            Assert.Catch<FLProgramCheckException>(() =>
                    setup.Parser.Process(
                        new FLParserInput("resources/filter/program_checks/filepath_validator_incorrect.fl")),
                "Did not detect Wrong FilePath");
            setup.Dispose();
        }

        [Test]
        public void OpenFL_InstructionArgumentValidator_Correct_Test()
        {
            TestSetup.SetupTestingSession();
            FLSetup setup = new FLSetup(nameof(OpenFL_InstructionArgumentValidator_Correct_Test), "resources/kernel");


            setup.Parser.Process(
                new FLParserInput("resources/filter/program_checks/instructionargument_validator_correct.fl"));

            setup.Dispose();
        }

        [Test]
        public void OpenFL_InstructionArgumentValidator_Incorrect_Test()
        {
            TestSetup.SetupTestingSession();
            FLSetup setup = new FLSetup(nameof(OpenFL_InstructionArgumentValidator_Incorrect_Test), "resources/kernel");

            Assert.Catch<FLProgramCheckException>(() =>
                    setup.Parser.Process(
                        new FLParserInput(
                            "resources/filter/program_checks/instructionargument_validator_incorrect.fl")),
                "Did not detect Wrong Argument");
            setup.Dispose();
        }
    }
}