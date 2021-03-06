﻿using System;
using System.Collections.Generic;
using System.Linq;
using Byt3.ADL.Configs;

namespace Byt3.ADL
{
    public class ADLLogger
    {
        private readonly IProjectDebugConfig ProjectDebugConfig;
        private readonly string SubProjectName;

        private Dictionary<int, string> prefixes = new Dictionary<int, string>();
        private bool hasProcessedPrefixes;
        public virtual string[] ProjectMaskPrefixes { get; } = new string[0];


        internal Dictionary<int, string> Prefixes
        {
            get
            {
                if (!hasProcessedPrefixes) prefixes = ProcessPrefixes(ProjectMaskPrefixes);
                return prefixes;
            }
        }

        /// <summary>
        ///     Dictionary of Prefixes for the corresponding Masks
        /// </summary>
        public ADLLogger(IProjectDebugConfig projectDebugConfig, string subProjectName = "")
        {
            ProjectDebugConfig = projectDebugConfig;
            SubProjectName = subProjectName;
        }

        public void Log(int mask, string message, int severity)
        {
            if (ProjectDebugConfig.GetMinSeverity() < severity)
            {
                return;
            }

            string subp = "";
            if (!string.IsNullOrEmpty(SubProjectName))
            {
                subp = $".{SubProjectName}";
            }

            Debug.Log(this, mask, $"[{ProjectDebugConfig.GetProjectName()}{subp}][S:{severity}]: {message}");
        }

        public string GetMaskPrefix(BitMask mask)
        {
            return Debug.GetMaskPrefix(Prefixes, mask);
        }

        public bool GetPrefixMask(string prefix, out BitMask mask)
        {
            return Debug.GetPrefixMask(Prefixes, prefix, out mask);
        }

        public void SetAllPrefixes(params string[] prefixNames)
        {
            Debug.SetAllPrefixes(Prefixes, prefixNames);
        }

        public void RemoveAllPrefixes()
        {
            Debug.RemoveAllPrefixes(Prefixes);
        }

        public void RemovePrefixForMask(BitMask mask)
        {
            Debug.RemovePrefixForMask(Prefixes, mask);
        }

        public void AddPrefixForMask(BitMask mask, string prefix)
        {
            Debug.AddPrefixForMask(Prefixes, mask, prefix);
        }

        public Dictionary<int, string> GetAllPrefixes()
        {
            return Debug.GetAllPrefixes(Prefixes);
        }

        private Dictionary<int, string> ProcessPrefixes(string[] prefixes)
        {
            if (prefixes.Length > sizeof(int) * 8)
            {
                throw new InvalidOperationException("Can not add more than " + sizeof(int) * 8 + " prefixes");
            }

            Dictionary<int, string> ret = new Dictionary<int, string>();
            int s = 1;
            for (int i = 0; i < prefixes.Length; i++)
            {
                ret[s] = prefixes[i];
                s <<= 1;
            }

            hasProcessedPrefixes = true;
            return ret;
        }
    }

    public class ADLLogger<T> : ADLLogger
        where T : struct
    {
        protected bool IsPowerOfTwo(int value)
        {
            return value != 0 && (value & (value - 1)) == 0;
        }

        public override string[] ProjectMaskPrefixes
        {
            get
            {
                List<string> names = Enum.GetNames(typeof(T)).ToList();
                for (int i = names.Count - 1; i >= 0; i--)
                {
                    if (!IsPowerOfTwo((int)Enum.Parse(typeof(T), names[i])))
                    {
                        names.RemoveAt(i);
                    }
                }

                return names.ToArray();
            }
        }

        public ADLLogger(IProjectDebugConfig projectDebugConfig, string subProjectname = "") : base(projectDebugConfig,
            subProjectname)
        {
        }

        public void Log(T mask, string message, int severity)
        {
            Log(Convert.ToInt32(mask), message, severity);
        }
    }
}