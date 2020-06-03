using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace FLDebugger.Forms
{
    public partial class AboutInfo : Form
    {
        public AboutInfo()
        {
            InitializeComponent();



            FLScriptEditor.RegisterDefaultTheme(panelTop);
            FLScriptEditor.RegisterDefaultTheme(lblVersionInfo);
            FLScriptEditor.RegisterDefaultTheme(gbLoadedAssemblies);
            FLScriptEditor.RegisterDefaultTheme(lbLoadedAssemblies);

            
            lblVersionInfo.Text = "Debugger Version: " + Assembly.GetExecutingAssembly().GetName().Version;
            InitializeLibraryList();
        }


        public void InitializeLibraryList()
        {
            Icon = Properties.Resources.OpenFL_Icon;
            List<Assembly> asm = AppDomain.CurrentDomain.GetAssemblies().ToList();
            asm.Sort((x, y) => string.Compare(x.GetName().Name, y.GetName().Name, StringComparison.Ordinal));
            foreach (Assembly assembly in asm)
            {
                AssemblyName name = assembly.GetName();
                if (name.Name.StartsWith("System") || name.Name == "mscorlib" || name.Name == "netstandard" || name.Name == "Accessibility")
                {
                    continue;
                }

                lbLoadedAssemblies.Items.Add($"[{name.Version}]{name.Name}");
            }
        }

        private void llblVersionArchive_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MiniBrowser mb = new MiniBrowser("http://213.109.162.193/flrepo/FLDebugger", true);
            mb.ShowDialog();
            mb.Dispose();
        }

        private void llblFLProjects_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MiniBrowser mb = new MiniBrowser("http://213.109.162.193/flrepo/FLDebugger_Projects", false);
            mb.ShowDialog();
            mb.Dispose();
        }

        private void lblGithub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(@"https://github.com/ByteChkR/Byt3");
        }
    }
}