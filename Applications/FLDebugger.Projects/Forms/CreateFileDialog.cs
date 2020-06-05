using System;
using System.IO;
using System.Windows.Forms;

namespace FLDebugger.Projects.Forms
{
    public partial class CreateFileDialog : Form
    {
        private readonly string Path;

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