using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using FLDebugger.Projects.ProjectObjects;

namespace FLDebugger.Projects
{
    [Serializable]
    public class ExternalProgramEditor
    {
        [XmlIgnore] public Dictionary<string, Process> processes = new Dictionary<string, Process>();

        public ExternalProgramEditor()
        {
        }

        public ExternalProgramEditor(string extension)
        {
            Extension = extension;
        }

        public string Extension { get; set; }
        public string Target { get; set; }
        public bool SetWorkingDir { get; set; }
        public string Format { get; set; } = "{0}";


        public void StartProgram(WorkingDirectory dir, FSFile file)
        {
            if (processes.ContainsKey(file.EntryPath) && !processes[file.EntryPath].HasExited)
            {
                return;
            }

            ProcessStartInfo psi = new ProcessStartInfo(Target, string.Format(Format, file.EntryPath, dir.Directory));
            if (SetWorkingDir)
            {
                psi.WorkingDirectory = dir.Directory;
            }

            processes[file.EntryPath] = Process.Start(psi);
        }

        public override string ToString()
        {
            return $" [{Extension}] {Path.GetFileName(Target)} {Format}";
        }
    }
}