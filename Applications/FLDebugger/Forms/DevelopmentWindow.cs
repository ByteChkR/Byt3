using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FLDebugger.Properties;

namespace FLDebugger.Forms
{
    public partial class DevelopmentWindow : Form
    {
        private FLScriptEditor Editor;
        private bool preventClose = true;
        public DevelopmentWindow(FLScriptEditor editor)
        {
            Editor = editor;
            Editor.Closing += Editor_Closing;
            InitializeComponent();
            Icon = Resources.OpenFL_Icon;
            Closing += DevelopmentWindow_Closing;
        }

        private void Editor_Closing(object sender, CancelEventArgs e)
        {
            preventClose = false;
        }

        private void DevelopmentWindow_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = preventClose;
        }

        private void btnRestart_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void cbExperimentalKernelLoading_CheckedChanged(object sender, EventArgs e)
        {
            FLScriptEditor.Settings.ExperimentalKernelLoading = cbExperimentalKernelLoading.Checked;
        }
    }
}
