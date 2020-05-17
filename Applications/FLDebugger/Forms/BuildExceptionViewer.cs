using System;
using System.Linq;
using System.Windows.Forms;
using Byt3.OpenCL.Wrapper;

namespace FLDebugger.Forms
{
    public partial class BuildExceptionViewer : Form
    {
        private readonly CLBuildException exception;

        public BuildExceptionViewer(CLBuildException ex)
        {
            exception = ex;
            InitializeComponent();
            Icon = Properties.Resources.OpenFL_Icon;
        }

        private void BuildExceptionViewer_Load(object sender, EventArgs e)
        {
            lbEx.Items.AddRange(exception.BuildResults.Cast<object>().ToArray());
        }

        private void lbEx_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbEx.SelectedIndex != -1)
            {
                CLProgramBuildResult br = (CLProgramBuildResult) lbEx.SelectedItem;
                string txt = $"File: {br.TargetFile} Errors: {br.BuildErrors.Count}\n";
                for (int i = 0; i < br.BuildErrors.Count; i++)
                {
                    txt +=
                        $"\t{i} [{br.BuildErrors[i].Error}] {br.BuildErrors[i].Exception.GetType().Name} : {br.BuildErrors[i].Message}";
                }

                rtbExText.Text = txt;
            }
        }
    }
}