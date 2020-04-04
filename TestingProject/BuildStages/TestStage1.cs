using System;
using Byt3.BuildSystem.Settings;
using Byt3.BuildSystem.Stages;

namespace TestingProject.BuildStages
{
    public class TestStage1 : BuilderStage<string, string>
    {
        public class TestStage1Settings : BuildStageSettings<TestStage1>
        {
            public string writeText = "TestStage1";
            public bool enable;
        }
        public TestStage1()
        {
            SettingsObj = new TestStage1Settings();
        }
        public override string Process(string process)
        {
            if (((TestStage1Settings)SettingsObj).enable)
            {
                Console.WriteLine(((TestStage1Settings)SettingsObj).writeText);
            }

            return ((TestStage1Settings)SettingsObj).writeText;
        }
    }
}