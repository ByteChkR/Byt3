using System;
using Byt3.BuildSystem;
using TestingProject.BuildStages;

namespace TestingProject
{
    class Program
    {
        static void Main(string[] args)
        {
            MakeTest();
            Console.WriteLine("Hello World!");
        }

        private static void MakeTest()
        {
            Builder b = new Builder();
            b.AddBuilderStage(new TestStage0());
            b.AddBuilderStage(new TestStage1());
            b.AddBuilderStage(new TestStage2());
            b.GenerateBuildSettings(".\\test.build.config");
            b.LoadBuildSettings(".\\test.build.config");
        }
    }
}
