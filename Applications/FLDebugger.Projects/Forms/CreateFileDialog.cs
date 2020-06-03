using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FLDebugger.Projects.Forms
{
    public partial class CreateFileDialog : Form
    {
        private string Path;
        public CreateFileDialog(string path)
        {
            InitializeComponent();
            Path = System.IO.Path.GetFullPath(path);
            lblPath.Text = "Path: " + Path;
        }

        private void tbFile_TextChanged(object sender, EventArgs e)
        {
            lblPath.Text = "Path: " + System.IO.Path.Combine(Path, tbFile.Text);
        }

        private void btnAddFolder_Click(object sender, EventArgs e)
        {
            Directory.CreateDirectory(System.IO.Path.Combine(Path, tbFile.Text));
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnAddFile_Click(object sender, EventArgs e)
        {
            File.Create(System.IO.Path.Combine(Path, tbFile.Text)).Dispose();
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
