using Byt3.BuildSystem.Settings;
using Byt3.BuildSystem.Stages;

namespace TestingProject.BuildStages
{
    public class TestStage2 : BuilderStage<string, string>
    {
        public class TestStage2Settings : BuildStageSettings<TestStage2> { }

        public TestStage2()
        {
            SettingsObj = new TestStage2Settings();
        }
        public override string Process(string process)
        {
            return process;
        }
    }
}