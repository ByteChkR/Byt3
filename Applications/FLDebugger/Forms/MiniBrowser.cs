using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Byt3.AutoUpdate.Helper;

namespace FLDebugger.Forms
{
    public partial class MiniBrowser : Form
    {
        private bool DirectInstall;
        public MiniBrowser(string url, bool directInstall)
        {
            DirectInstall = directInstall;
            InitializeComponent();
            Uri u = new Uri(url);
            webBrowser1.Url = u;
            webBrowser1.Navigating += WebBrowser1_Navigating;
            Text = Path.GetFileName(u.AbsolutePath);
        }

        private void WebBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            e.Cancel = true;
            if (!e.Url.AbsolutePath.EndsWith("/"))
            {
                if (DirectInstall && UpdateChecker.UpdaterPresent)
                {
                    UpdateChecker.Direct("http://213.109.162.193/flrepo/", "FLDebugger", Assembly.GetExecutingAssembly().Location, Version.Parse("0.0.0.0"),
                        Version.Parse(Path.GetFileNameWithoutExtension(e.Url.AbsolutePath)));
                    return;
                }
                else
                {
                    Process.Start(e.Url.ToString());
                }
            }
        }
    }
}
