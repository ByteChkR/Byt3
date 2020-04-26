using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Byt3.OpenFL.Tests
{
    public class PerformanceTester
    {
        public class PerformanceTarget
        {
            public string TestName;
            public decimal Target;
            public decimal Variance;
            public bool LowerIsBetter;
        }

        private List<PerformanceTarget> Targets;
        private bool Initialized => Targets != null;
        private const string TARGET_DIRECTORY = "performance_targets";

        public static readonly PerformanceTester Tester = new PerformanceTester();

        private void InitializeTargets()
        {
            Targets=new List<PerformanceTarget>();
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
                catch (Exception)
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
                s = File.Create(Path.Combine(TARGET_DIRECTORY, target.TestName+".xml"));
                XmlSerializer xs = new XmlSerializer(typeof(PerformanceTarget));
                xs.Serialize(s, target);
                s.Close();
            }
            catch (Exception)
            {
                s?.Close();
            }
        }

        public bool MatchesTarget(string nameOfTest, decimal value, out decimal deltaFromTarget, out decimal targetActual)
        {
            if (!Initialized) InitializeTargets();
            PerformanceTarget target = Targets.FirstOrDefault(x => x.TestName == nameOfTest);
            if (target == null)
            {
                target = new PerformanceTarget { TestName = nameOfTest , Target = value, Variance = value/2, LowerIsBetter = true};
                Targets.Add(target);
                WriteTarget(target);
                targetActual = value;
                deltaFromTarget = 0;
                return true; //First Run.
            }

            targetActual = target.Target;

            decimal targetAndVariance;
            if (target.LowerIsBetter)
            {
                targetAndVariance = target.Target + target.Variance;
            }
            else
            {
                targetAndVariance = target.Target - target.Variance;
            }
            deltaFromTarget = target.Target - value;
            

            return (!target.LowerIsBetter || targetAndVariance >= value) &&
                   (target.LowerIsBetter || targetAndVariance <= value);
        }

    }
}