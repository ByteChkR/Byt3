using System;
using System.Collections.Generic;
using System.Linq;
using Byt3.ADL;
using Byt3.ExtPP.Base;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenFL.Common;
using Byt3.OpenFL.Common.Buffers.BufferCreators;
using Byt3.OpenFL.Common.ProgramChecks;
using Byt3.Utilities.FastString;
using Byt3.Utilities.TypeFinding;

namespace Byt3Console.OpenFL
{
    public class FLConsoleSettings
    {
        public int Verbosity;
        public bool MultiThread;
        public int WorkSizeMultiplier;
        public string ProgramChecks;
        public string BufferCreators;

        /// <summary>
        /// Folder relative to the Assembly Code Base
        /// </summary>
        public string KernelFolder;

        public Resolution Resolution;

        public List<Type> BufferCreatorTypes
        {
            get
            {
                string[] creators = BufferCreators.Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries);
                List<Type> ret = new List<Type>();
                for (int i = 0; i < creators.Length; i++)
                {
                    ret.Add(Type.GetType(creators[i], true, true));
                }

                return ret;
            }
        }

        public List<Type> ProgramCheckTypes
        {
            get
            {
                string[] creators = ProgramChecks.Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries);
                List<Type> ret = new List<Type>();
                for (int i = 0; i < creators.Length; i++)
                {
                    ret.AddRange(TypeAccumulator<FLProgramCheck>.GetTypesByName(creators[i]));
                    //ret.Add(Type.GetType(creators[i], true, true));
                }

                return ret;
            }
        }
        

        public static FLConsoleSettings Default
        {
            get
            {
                FLConsoleSettings settings = new FLConsoleSettings
                {
                    MultiThread = false,
                    WorkSizeMultiplier = 2,
                    ProgramChecks = FLProgramCheckBuilder.Default.Select(x=>x.GetType().Name).Unpack(";"),
                    BufferCreators = GetBuiltInTypesAssignableFrom(typeof(ASerializableBufferCreator))
                        .Select(x => x.AssemblyQualifiedName).Unpack(";"),
                    KernelFolder = "resources/kernel",
                    Resolution = new Resolution(128, 128),
                    Verbosity = 1,
                };
                return settings;
            }
        }

        public void SetVerbosity()
        {
            ExtPPDebugConfig.Settings.MinSeverity = (Verbosity) Verbosity;
            OpenFLDebugConfig.Settings.MinSeverity = (Verbosity) Verbosity;
            OpenCLDebugConfig.Settings.MinSeverity = (Verbosity) Verbosity;
        }

        private static Type[] GetBuiltInTypesAssignableFrom(Type t)
        {
            List<Type> ret = t.Assembly.GetTypes().ToList();
            for (int i = ret.Count - 1; i >= 0; i--)
            {
                if (ret[i] == t || ret[i].IsAbstract || !t.IsAssignableFrom(ret[i]))
                {
                    ret.RemoveAt(i);
                }
            }

            return ret.ToArray();
        }
    }
}