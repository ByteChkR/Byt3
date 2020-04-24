using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Byt3.OpenFL.Benchmarking
{
    public class PerformanceTester
    {
        public class PerformanceTarget
        {
            public string TestName;
            public decimal Target;
            public decimal Variance;
            public bool LowerIsBetter;

            public PerformanceTarget() { }
            public PerformanceTarget(string nameOfTest, decimal target, int n)
            {
                TestName = nameOfTest;
                Target = target;
                Variance = Target / 2;
                LowerIsBetter = true;
            }
        }

        public class PerformanceResult
        {
            public bool Matched => (!LowerIsBetter || TargetAndVariance >= Result) &&
                                  (LowerIsBetter || TargetAndVariance <= Result);
            public decimal TargetAndVariance => LowerIsBetter ? Target + Variance : Target - Variance;
            public decimal DeltaFromTarget => Target - Result;
            public decimal Percentage => Math.Round(Result / Target * 100, 4);

            public bool LowerIsBetter;
            public string TestName;
            public int N;
            public decimal Result;
            public decimal Target;
            public decimal Variance;

            public PerformanceResult() { }

            public PerformanceResult(PerformanceTarget target, int n, decimal result)
            {
                LowerIsBetter = target.LowerIsBetter;
                TestName = target.TestName;
                N = n;
                Result = result;
                Target = target.Target;
                Variance = target.Variance;
            }

            public override string ToString()
            {
                return
                    $"{TestName}: {Result}ms ({Percentage}%); Matched: {Matched}; Target: {Target}ms; Delta: {DeltaFromTarget}ms";
            }
        }

        private List<PerformanceTarget> Targets;
        private const string TARGET_DIRECTORY = "performance_targets";

        public static readonly PerformanceTester Tester = new PerformanceTester();



        private PerformanceTester()
        {
            Targets = new List<PerformanceTarget>();
            Directory.CreateDirectory(TARGET_DIRECTORY);
            string[] files = Directory.GetFiles(TARGET_DIRECTORY, "*.xml", SearchOption.AllDirectories);
            XmlSerializer xs = new XmlSerializer(typeof(PerformanceTarget));
            for (int i = 0; i < files.Length; i++)
            {
                Stream s = null;
                try
                {
                    s = File.OpenRead(files[i]);
                    Targets.Add((PerformanceTarget)xs.Deserialize(s));
                    s.Close();
                }
                catch (Exception e)
                {
                    s?.Close();
                }
            }
        }

        private void WriteTarget(PerformanceTarget target)
        {
            Stream s = null;
            try
            {
                s = File.Create(Path.Combine(TARGET_DIRECTORY, target.TestName + ".xml"));
                XmlSerializer xs = new XmlSerializer(typeof(PerformanceTarget));
                xs.Serialize(s, target);
                s.Close();
            }
            catch (Exception e)
            {
                s?.Close();
            }
        }

        private PerformanceTarget GetTarget(string name)
        {
            return Targets.FirstOrDefault(x => x.TestName == name);
        }

        public PerformanceResult RunTest(string testName, int testCount, Action beforeTest, Action test, Action afterTest)
        {
            PerformanceTarget target = GetTarget(testName);
            Stopwatch sw = new Stopwatch();
            for (int i = 0; i < testCount; i++)
            {
                beforeTest?.Invoke();
                sw.Start();
                test();
                sw.Stop();
                afterTest?.Invoke();
            }
            decimal result = (decimal)sw.Elapsed.TotalMilliseconds;
            if (target == null)
            {
                target = new PerformanceTarget(testName, result, testCount);
                Targets.Add(target);
                WriteTarget(target);
            }
            return new PerformanceResult(target, testCount, result);
        }

        //public bool MatchesTarget(string nameOfTest, decimal value, out decimal deltaFromTarget, out decimal targetActual)
        //{
        //    if (!Initialized) InitializeTargets();
        //    PerformanceTarget target = Targets.FirstOrDefault(x => x.TestName == nameOfTest);
        //    if (target == null)
        //    {
        //        target = new PerformanceTarget { TestName = nameOfTest, Target = value, Variance = value / 2, LowerIsBetter = true };
        //        Targets.Add(target);
        //        WriteTarget(target);
        //        targetActual = value;
        //        deltaFromTarget = 0;
        //        return true; //First Run.
        //    }

        //    targetActual = target.Target;

        //    decimal targetAndVariance;
        //    if (target.LowerIsBetter)
        //    {
        //        targetAndVariance = target.Target + target.Variance;
        //    }
        //    else
        //    {
        //        targetAndVariance = target.Target - target.Variance;
        //    }
        //    deltaFromTarget = target.Target - value;


        //    return (!target.LowerIsBetter || targetAndVariance >= value) &&
        //           (target.LowerIsBetter || targetAndVariance <= value);
        //}

    }
}