using System;
using System.Drawing;
using System.Windows.Forms;

namespace Byt3.WindowsForms.Forms
{
    public partial class ExceptionViewer : Form
    {
        public ExceptionViewer(Exception ex, bool retryPossible = true)
        {
            InitializeComponent();
            if (!retryPossible) btnRestart.Visible = btnRestart.Enabled = false;

            Icon = SystemIcons.Error;
            DialogResult = DialogResult.Cancel;
            tvExceptionView.Nodes.Add(ExceptionToTreeNode(ex));
            tvExceptionView.AfterSelect += TvExceptionView_AfterSelect;
        }

        private void TvExceptionView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (tvExceptionView.SelectedNode != null)
            {
                ExceptionTreeNode tn = (ExceptionTreeNode)tvExceptionView.SelectedNode;
                lblExeptionType.Text = "Exception Type: " + tn.Exception.GetType().Name;
                lblHResult.Text = "HResult: " + tn.Exception.HResult;
                lblHelpLink.Text = "Help Link: " + (tn.Exception.HelpLink ?? "null");
                lblSource.Text = "Source: " + tn.Exception.Source;
                lblTargetSite.Text = "Target Site: " + tn.Exception.TargetSite?.Name;
                rtbCallStack.Text = tn.Exception.StackTrace ?? "null";
                rtbMessage.Text = tn.Exception.Message ?? "null";
            }
        }

        private static TreeNode ExceptionToTreeNode(Exception ex)
        {
            TreeNode tn = new ExceptionTreeNode(ex);
            if (ex is AggregateException ag)
            {
                foreach (Exception agInnerException in ag.InnerExceptions)
                {
                    tn.Nodes.Add(ExceptionToTreeNode(agInnerException));
                }
            }
            else if (ex.InnerException != null)
            {
                tn.Nodes.Add(ExceptionToTreeNode(ex.InnerException));
            }

            return tn;
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Retry;
            Close();
        }

        private class ExceptionTreeNode : TreeNode
        {
            public readonly Exception Exception;

            public ExceptionTreeNode(Exception ex)
            {
                Exception = ex;
                Text = Name = ex.GetType().Name;
            }
        }
    }
}