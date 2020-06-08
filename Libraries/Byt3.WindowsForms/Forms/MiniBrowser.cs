using System;
using System.IO;
using System.Windows.Forms;

namespace Byt3.WindowsForms.Forms
{
    public partial class MiniBrowser : Form
    {
        private Action<WebBrowserNavigatingEventArgs> OnNavigate;

        public MiniBrowser(string url, Action<WebBrowserNavigatingEventArgs> onNavigate)
        {
            OnNavigate = onNavigate;
            InitializeComponent();
            Uri u = new Uri(url);
            webBrowser1.Url = u;
            webBrowser1.Navigating += WebBrowser1_Navigating;
            Text = Path.GetFileName(u.AbsolutePath);
        }

        private void WebBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            OnNavigate?.Invoke(e);
        }
    }
}