using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Byt3.AutoUpdate;

namespace ProgramInstaller
{
    public partial class SelectProductForm : Form
    {
        private readonly WebClient client = new WebClient();
        private struct Product
        {
            public string name;
            public string startFile;
            public string repo;
            public Version[] versions;

            public Product(string name, string repo, string startFile, Version[] vers)
            {
                this.name = name;
                this.startFile = startFile.Trim();
                this.repo = repo;
                versions = vers;

            }
        }
        private readonly Dictionary<string, Product> Products = new Dictionary<string, Product>();


        public SelectProductForm()
        {
            InitializeComponent();
            InitializeRepos();

            cbVersion.Enabled = false;
            tbInstallDir.Enabled = false;
            btnInstall.Enabled = false;
            btnSelectInstallDir.Enabled = false;
        }

        private void InitializeRepos()
        {
            string[] repos = LoadRepos();
            Products.Clear();
            foreach (string repo in repos)
            {
                string[] products = GetProducts(repo);
                foreach (string product in products)
                {
                    Version[] vers = GetVersionsForProduct(repo, product);
                    Products.Add(product, new Product(product, repo, GetStartFileForProduct(repo, product), vers));
                }
            }

            List<string> prds = Products.Keys.ToList();
            prds.Sort();
            cbProduct.Items.AddRange(prds.Cast<object>().ToArray());
        }

        private string[] LoadRepos()
        {
            Stream s = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("ProgramInstaller.installer_config.repos_url.txt");
            TextReader tr = new StreamReader(s);
            string[] l = tr.ReadToEnd().Replace("\r", "").Split('\n');
            s.Close();
            return l;
        }

        private string GetStartFileForProduct(string repo, string productName)
        {
            return client.DownloadString($"{repo}{productName}/startfile.txt");
        }

        private string[] GetProducts(string repo)
        {
            string[] vers = client.DownloadString($"{repo}/listing.php")
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToArray();
            return vers;
        }

        private Version[] GetVersionsForProduct(string repo, string productName)
        {
            string[] vers = client.DownloadString($"{repo}{productName}/listing.php")
                .Split(' ').Where(x => x.EndsWith(".zip")).Select(ver => ver.Replace(".zip", "")).ToArray();

            List<Version> ret = new List<Version>();
            foreach (string ver in vers)
            {
                if (Version.TryParse(ver, out Version v))
                {
                    ret.Add(v);
                }
            }

            ret.Sort();
            ret.Reverse();
            return ret.ToArray();
        }

        private void btnInstall_Click(object sender, EventArgs e)
        {
            AutoUpdateEntry.TargetVersion = cbVersion.Items[cbVersion.SelectedIndex] as Version;
            AutoUpdateEntry.CurrentVersion = Version.Parse("0.0.0.0");
            AutoUpdateEntry.ProjectName = cbProduct.Items[cbProduct.SelectedIndex].ToString();
            AutoUpdateEntry.DestinationFile = Path.Combine(tbInstallDir.Text, Products[cbProduct.Items[cbProduct.SelectedIndex].ToString()].startFile);
            AutoUpdateEntry.TargetURL = Products[cbProduct.Items[cbProduct.SelectedIndex].ToString()].repo;
            AutoUpdateEntry.Direct = true;
            AutoUpdateEntry.CloseOnFinish = false;
            AutoUpdateEntry.Args = new [] {"-no-update"};

            if (!Directory.Exists(AutoUpdateEntry.DestinationFolder))
                Directory.CreateDirectory(AutoUpdateEntry.DestinationFolder);

            UpdateWindow window = new UpdateWindow();
            window.ShowDialog();
        }

        private void btnSelectInstallDir_Click(object sender, EventArgs e)
        {
            if (fbdInstallDir.ShowDialog() == DialogResult.OK)
            {
                tbInstallDir.Text = fbdInstallDir.SelectedPath;
            }
        }

        private void tbInstallDir_TextChanged(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(tbInstallDir.Text))
            {
                btnInstall.Enabled = false;
            }
            else
            {
                btnInstall.Enabled = true;
            }

        }

        private void vbProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbProduct.SelectedIndex == -1)
            {
                cbVersion.Enabled = false;
                tbInstallDir.Enabled = false;
                btnInstall.Enabled = false;
                btnSelectInstallDir.Enabled = false;
            }
            else
            {
                cbVersion.Items.Clear();
                cbVersion.Items.AddRange(Products[cbProduct.Items[cbProduct.SelectedIndex].ToString()].versions.Cast<object>().ToArray());
                if (cbVersion.Items.Count > 0) cbVersion.SelectedIndex = 0;
                cbVersion.Enabled = true;
            }
        }

        private void cbVersion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbVersion.SelectedIndex == -1)
            {
                btnInstall.Enabled = false;
            }
            else
            {
                tbInstallDir.Enabled = true;
                btnSelectInstallDir.Enabled = true;
            }
        }
    }
}
