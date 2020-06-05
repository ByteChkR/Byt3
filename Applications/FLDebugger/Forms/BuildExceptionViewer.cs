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



            FLScriptEditor.RegisterDefaultTheme(lbEx);
            FLScriptEditor.RegisterDefaultTheme(panel1);
            FLScriptEditor.RegisterDefaultTheme(panel2);
            FLScriptEditor.RegisterDefaultTheme(rtbExText);
            FLScriptEditor.RegisterDefaultTheme(gbLoadedKernels);
            FLScriptEditor.RegisterDefaultTheme(gbBuildOut);
            FLScriptEditor.RegisterDefaultTheme(btnRetry);
    }

        private void BuildExceptionViewer_Load(object sender, EventArgs e)
        {
            lbEx.Items.AddRange(exception.BuildResults.Cast<object>().ToArray());
        }

        private void lbEx_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateOutput(lbEx.SelectedIndex);
        }

        private void UpdateOutput(int id)
        {
            if (id != -1)
            {
                CLProgramBuildResult br = (CLProgramBuildResult)lbEx.SelectedItem;
                if (ShowSource)
                {
                    string txt = br.Source;
                    
                    rtbExText.Text = txt;
                }
                else
                {
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

        private void btnRetry_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Retry;
            Close();
        }

        private bool ShowSource;
        private void btnShowSource_Click(object sender, EventArgs e)
        {
            ShowSource = !ShowSource;
            if (ShowSource)
            {
                btnShowSource.Text = "Errors";

            }
            else
            {
                btnShowSource.Text = "Source";
            }
            UpdateOutput(lbEx.SelectedIndex);
        }
    }
}