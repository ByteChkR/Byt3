using System;
using Byt3.BuildSystem.Settings;
using Byt3.BuildSystem.Stages;

namespace TestingProject.BuildStages
{
    public class TestStage0 : BuilderStage<string, string>
    {
        public class TestStage0Settings : BuildStageSettings<TestStage0>
        {
            public string writeText = "TestStage0";
            public bool enable;
        }
        public TestStage0()
        {
            SettingsObj = new TestStage0Settings();
        }
        public override string Process(string process)
        {
            if (((TestStage0Settings)SettingsObj).enable)
            {
                Console.WriteLine(((TestStage0Settings)SettingsObj).writeText);
            }

            return ((TestStage0Settings)SettingsObj).writeText;
        }
    }
}