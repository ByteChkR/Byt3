using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Byt3.ADL.Configs;

namespace Byt3.ADL
{
    public class ALogger
    {
        private Dictionary<int, string> ProcessPrefixes(string[] prefixes)
        {
            if (prefixes.Length > sizeof(int) * 8) throw new Exception("Can not add more than " + sizeof(int) * 8 + " prefixes");
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

        internal Dictionary<int, string> Prefixes =>
            hasProcessedPrefixes ? _prefixes : _prefixes = ProcessPrefixes(ProjectMaskPrefixes);
        /// <summary>
        ///     Dictionary of Prefixes for the corresponding Masks
        /// </summary>
        private Dictionary<int, string> _prefixes = new Dictionary<int, string>();

        private bool hasProcessedPrefixes = false;

        public readonly string ProjectName;
        public virtual string[] ProjectMaskPrefixes { get; } = new string[0];
        public virtual PrefixLookupSettings LookupSettings { get; set; } = PrefixLookupSettings.Addprefixifavailable;
        public ALogger(string projectName)
        {
            ProjectName = projectName;
        }

        public void Log(int mask, string message)
        {
            Debug.Log(this, mask, $"[{ProjectName}:] {message}");
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

    }

    public class ALogger<T> : ALogger
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
                    if (!IsPowerOfTwo((int) Enum.Parse(typeof(T), names[i])))
                    {
                        names.RemoveAt(i);
                    }
                }

                return names.ToArray();
            }
        }

        public ALogger(string projectName) : base(projectName) { }

        public void Log(T mask, string message)
        {
            Log(Convert.ToInt32(mask), message);
        }
    }
}