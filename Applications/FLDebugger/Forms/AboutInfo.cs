using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
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
    }
}