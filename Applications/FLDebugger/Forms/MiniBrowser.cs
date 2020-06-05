using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Byt3.AutoUpdate.Helper;

namespace FLDebugger.Forms
{
    public partial class MiniBrowser : Form
    {
        private readonly bool DirectInstall;

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
                    UpdateChecker.Direct("http://213.109.162.193/flrepo/", "FLDebugger",
                        Assembly.GetExecutingAssembly().Location, Version.Parse("0.0.0.0"),
                        Version.Parse(Path.GetFileNameWithoutExtension(e.Url.AbsolutePath)));
                    Thread.Sleep(1000);
                    Application.Exit();
                }
                else
                {
                    Process.Start(e.Url.ToString());
                }
            }
        }
    }
}