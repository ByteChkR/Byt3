using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Byt3.OpenFL.Common.Buffers;
using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;
using Byt3.WindowsForms.CustomControls;
using FLDebugger.Utils;

namespace FLDebugger.Forms
{
    public partial class FLDebuggerWindow : Form
    {
        private readonly Dictionary<IParsedObject, int> marks;
        private readonly string Source;
        private readonly CustomCheckedListBox clbCode;


        private bool continueEx;
        private bool exitDirect;
        private bool nohalt;
        private FLProgram Program;
        private int selectedLine;
        private bool started;

        public FLDebuggerWindow(FLProgram program)
        {
            Program = program;
            marks = Program.ToString(out string source);
            Source = source;
            InitializeComponent();

            clbCode = new CustomCheckedListBox(GetCodeItemBackColor, GetCodeItemForeColor);
            Controls.Add(clbCode);
            clbCode.BackColor = Color.DimGray;
            clbCode.ForeColor = Color.Black;
            clbCode.CheckOnClick = true;
            clbCode.Dock = DockStyle.Fill;
            Closing += CodeView_Closing;

            FLScriptEditor.RegisterDefaultTheme(btnContinue);
            FLScriptEditor.RegisterDefaultTheme(panelSidePanel);
            FLScriptEditor.RegisterDefaultTheme(lblActiveBuffer);
            FLScriptEditor.RegisterDefaultTheme(lblActiveChannels);
            FLScriptEditor.RegisterDefaultTheme(lbBuffers);
            FLScriptEditor.RegisterDefaultTheme(btnHideSidePanel);
            FLScriptEditor.RegisterDefaultTheme(btnShowSidePanel);
            FLScriptEditor.RegisterDefaultTheme(btnRunToEnd);
            FLScriptEditor.RegisterDefaultTheme(cbFollowScripts);
            FLScriptEditor.RegisterDefaultTheme(lblInternalBuffers);
            FLScriptEditor.RegisterDefaultTheme(lbInternalBuffers);
            FLScriptEditor.RegisterDefaultTheme(clbCode);
        }


        private void CodeView_Load(object sender, EventArgs e)
        {
            Icon = Properties.Resources.OpenFL_Icon;


            lbInternalBuffers.MouseDoubleClick += LbInternalBuffersOnMouseDoubleClick;
            lbBuffers.MouseDoubleClick += LbBuffers_MouseDoubleClick;
            DoubleBuffered = true;


            clbCode.Items.AddRange(Source.Split('\n'));

            UpdateSidePanel();
            btnContinue.Text = "Start";
            btnContinue.Enabled = true;
        }

        private void LbInternalBuffersOnMouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lbInternalBuffers.SelectedItem == null || !started)
            {
                return;
            }

            FLBuffer buf = lbInternalBuffers.SelectedItem as FLBuffer;
            BufferView bvv = new BufferView(Program.Instance, buf, Program.Dimensions.x, Program.Dimensions.y);
            bvv.Show();
        }

        private Color GetCodeItemForeColor(CustomCheckedListBox listbox, DrawItemEventArgs e)
        {
            return listbox.ForeColor;
        }


        private Color GetCodeItemBackColor(CustomCheckedListBox listbox, DrawItemEventArgs e)
        {
            if (e.Index == selectedLine)
            {
                return FLScriptEditor.Theme.DebuggerBreakpointHitColor;
            }

            if (listbox.GetItemChecked(e.Index))
            {
                return FLScriptEditor.Theme.DebuggerBreakpointColor;
            }

            return listbox.BackColor;
        }

        private void CodeView_Closing(object sender, CancelEventArgs e)
        {
            exitDirect = true;
            nohalt = true;
        }

        private void LbBuffers_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lbBuffers.SelectedItem == null || !started)
            {
                return;
            }

            BufferView bvv = new BufferView(Program.Instance,
                Program.GetBufferWithName(lbBuffers.SelectedItem.ToString(), false), Program.Dimensions.x,
                Program.Dimensions.y);
            bvv.Show();
        }

        private int GetLineOfObject(FLParsedObject obj)
        {
            return marks[obj];
        }

        private void SelectLine(int line)
        {
            selectedLine = line;
            //string[] lines = rtbCode.Text.Split(new[] { '\n' });
            //int idx = line - 1;
            //int len = 0;
            //for (int i = 0; i < line; i++)
            //{
            //    if (i < line - 1)
            //    {
            //        idx += lines[i].Length;
            //    }
            //    else
            //    {
            //        len = lines[i].Length;
            //    }
            //}

            //if (idx == -1) idx = Text.Length;
            //rtbCode.Select(idx, len);
        }


        private void UpdateSidePanel()
        {
            FLBuffer active = Program.GetActiveBuffer(false);
            string activeBufferName = active == null ? "NULL" : active.DefinedBufferName;
            lblActiveBuffer.Text = "Active Buffer:\n" + activeBufferName;
            string s = "Active Channels:";
            if (Program.ActiveChannels != null)
            {
                for (int i = 0; i < Program.ActiveChannels.Length; i++)
                {
                    s += " " + Program.ActiveChannels[i];
                }
            }
            else
            {
                s += " NULL";
            }

            lblActiveChannels.Text = s;

            lbBuffers.Items.Clear();
            lbBuffers.Items.AddRange(Program.BufferNames.Cast<object>().ToArray());
        }

        public void MarkInstruction(int instr)
        {
            SelectLine(instr);
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            started = true;
            continueEx = true;
        }

        private void btnShowSidePanel_Click(object sender, EventArgs e)
        {
            panelSidePanel.Visible = true;
            btnShowSidePanel.SendToBack();
        }

        private void btnHideSidePanel_Click(object sender, EventArgs e)
        {
            panelSidePanel.Visible = false;
            btnShowSidePanel.BringToFront();
        }

        private void btnRunToEnd_Click(object sender, EventArgs e)
        {
            nohalt = true;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            FollowScripts = cbFollowScripts.Checked;
        }

        private void lbBuffers_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
    }
}