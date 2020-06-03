using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Byt3.ADL;
using Byt3.ADL.Configs;

namespace FLDebugger.Forms
{
    public partial class LogDisplay : Form
    {
        private bool allowLogEdit;
        private bool ignoreSeveritySet;
        private readonly StringBuilder logOut = new StringBuilder();


        public LogDisplay()
        {
            InitializeComponent();
            cbMinSeverity.Items.AddRange(Enum.GetNames(typeof(Verbosity)));
            Closing += LogDisplay_Closing;

            FLScriptEditor.RegisterLogTheme(rtbLogOut);
            FLScriptEditor.RegisterDefaultTheme(btnClear);
            FLScriptEditor.RegisterDefaultTheme(panelSide);
            FLScriptEditor.RegisterDefaultTheme(lbLogger);
            FLScriptEditor.RegisterDefaultTheme(cbProjects);
            FLScriptEditor.RegisterDefaultTheme(panelMain);
            FLScriptEditor.RegisterDefaultTheme(btnRefreshProjects);
            FLScriptEditor.RegisterDefaultTheme(cbMinSeverity);
            FLScriptEditor.RegisterDefaultTheme(lblLogSeverity);
            FLScriptEditor.RegisterDefaultTheme(panelSideMain);
            FLScriptEditor.RegisterDefaultTheme(panelSideTop);


        }

        private void LogDisplay_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        public void Append(string str)
        {
            logOut.Append(str);
            allowLogEdit = true;
            rtbLogOut.AppendText(str);
            allowLogEdit = false;
        }

        private void rtbLogOut_TextChanged(object sender, EventArgs e)
        {
            if (!allowLogEdit)
            {
                rtbLogOut.Text = logOut.ToString();
            }

            rtbLogOut.ScrollToCaret();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            allowLogEdit = true;
            logOut.Clear();
            rtbLogOut.Text = "";
            allowLogEdit = false;
        }


        private void cbProjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbProjects.SelectedIndex != -1)
            {
                ReadOnlyDictionary<IProjectDebugConfig, List<ADLLogger>> map = ADLLogger.GetReadOnlyLoggerMap();
                IProjectDebugConfig config = (IProjectDebugConfig)cbProjects.SelectedItem;
                lbLogger.Items.Clear();
                lbLogger.Items.AddRange(map[config].ToArray());
                ignoreSeveritySet = true;
                cbMinSeverity.SelectedIndex =
                    cbMinSeverity.Items.IndexOf(((Verbosity)config.GetMinSeverity()).ToString());
                ignoreSeveritySet = false;
            }
        }

        private void btnRefreshProjects_Click(object sender, EventArgs e)
        {
            RefreshProjects();
        }

        private void cbMinSeverity_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!ignoreSeveritySet && cbProjects.SelectedIndex != -1)
            {
                IProjectDebugConfig config = (IProjectDebugConfig)cbProjects.SelectedItem;
                config.SetMinSeverity((int)Enum.Parse(typeof(Verbosity), cbMinSeverity.SelectedItem.ToString()));
            }
        }


        private void RefreshProjects()
        {
            cbProjects.Items.Clear();
            lbLogger.Items.Clear();
            ReadOnlyDictionary<IProjectDebugConfig, List<ADLLogger>> map = ADLLogger.GetReadOnlyLoggerMap();
            cbProjects.Items.AddRange(map.Keys.ToArray());
        }

        private void LogDisplay_Load(object sender, EventArgs e)
        {
            Icon = Properties.Resources.OpenFL_Icon;
            RefreshProjects();
        }
    }
}