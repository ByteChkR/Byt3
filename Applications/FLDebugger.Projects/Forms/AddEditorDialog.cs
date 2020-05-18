using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace FLDebugger.Projects.Forms
{
    public partial class AddEditorDialog : Form
    {
        public AddEditorDialog()
        {
            InitializeComponent();
        }

        private string[] Extensions =>
            tbExtensions.Text.Replace(" ", "").Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);

        private void btnBrowseEditor_Click(object sender, EventArgs e)
        {
            if (ofdEditorPath.ShowDialog() == DialogResult.OK)
            {
                tbEditorPath.Text = ofdEditorPath.FileName;
            }
        }

        private void tbEditorPath_TextChanged(object sender, EventArgs e)
        {
            if (File.Exists(tbEditorPath.Text))
            {
                tbEditorPath.BackColor = Color.Green;
                UpdateOutput();
            }
            else
            {
                tbEditorPath.BackColor = Color.DimGray;
                lbOutput.Items.Clear();
            }
        }


        private void UpdateOutput()
        {
            lbOutput.Items.Clear();

            foreach (string ext in Extensions)
            {
                string exampleFile = $@"C:\WorkingDirectory\myfile{ext}";
                string exampleFolder = Path.GetDirectoryName(exampleFile);
                string outP = "Format Error";
                try
                {
                    outP = string.Format(tbFormat.Text, exampleFile, exampleFolder);
                }
                catch (Exception)
                {
                }

                lbOutput.Items.Add($"{tbEditorPath.Text} {outP}");
            }
        }

        private void tbExtensions_TextChanged(object sender, EventArgs e)
        {
            UpdateOutput();
        }

        private void tbFormat_TextChanged(object sender, EventArgs e)
        {
            UpdateOutput();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            foreach (string extension in Extensions)
            {
                string filePath = Path.Combine(StartupSplash.EditorConfigPath, $"{extension.Remove(0, 1)}_Config.xml");
                ExternalProgramEditor editor = new ExternalProgramEditor(extension)
                    {Format = tbFormat.Text, Target = tbEditorPath.Text, SetWorkingDir = cbSetWorkingDir.Checked};
                if (!File.Exists(filePath) || MessageBox.Show($"Overwrite File: {filePath}?", "Confirm Overwrite",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    XmlSerializer xs = new XmlSerializer(typeof(ExternalProgramEditor));
                    Stream s = File.Create(filePath);
                    xs.Serialize(s, editor);
                    s.Close();
                }
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void AddEditorDialog_Load(object sender, EventArgs e)
        {
            Icon = Properties.Resources.OpenFL_Icon;
        }
    }
}