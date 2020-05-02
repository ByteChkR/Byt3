using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Byt3.OpenFL.Common;
using Byt3.OpenFL.Common.Buffers;
using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;

namespace FLDebugger
{
    public static class FLDebugHelper
    {
        public static Dictionary<FLParsedObject, int> ToString(this FLProgram prog, out string s)
        {
            Dictionary<FLParsedObject, int> ret =
                new Dictionary<FLParsedObject, int>();
            StringBuilder sb = new StringBuilder();

            int lineCount = 0;

            foreach (KeyValuePair<string, FLBuffer> definedBuffer in prog.DefinedBuffers)
            {
                string f = definedBuffer.Value.ToString();
                ret.Add(definedBuffer.Value, lineCount);
                sb.AppendLine(f);
                lineCount++;
            }


            foreach (KeyValuePair<string, ExternalFlFunction> externalFlFunction in prog.DefinedScripts)
            {
                string f = externalFlFunction.Value.ToString();
                ret.Add(externalFlFunction.Value, lineCount);
                sb.AppendLine(f);
                lineCount++;
            }


            foreach (KeyValuePair<string, FLFunction> keyValuePair in prog.FlFunctions)
            {
                sb.AppendLine(keyValuePair.Key + ":");
                lineCount++;
                foreach (FLInstruction valueInstruction in keyValuePair.Value.Instructions)
                {
                    string f = "\t" + valueInstruction;
                    ret.Add(valueInstruction, lineCount);
                    sb.AppendLine(f);
                    lineCount++;
                }
                sb.AppendLine();
                lineCount++;
            }

            s = sb.ToString();
            return ret;
        }

    }

    public partial class CodeView : Form, IDebugger
    {

        private FLProgram Program;
        private Dictionary<FLParsedObject, int> marks;
        private int selectedLine;
        private bool nohalt = false;
        private bool exitDirect;

        private CustomCheckedListBox clbCode;

        public CodeView()
        {
            FLProgram.Debugger = this;
            InitializeComponent();
        }

        public void Register(FLProgram program)
        {
            if (Program != null) return;
            Program = program;
            marks = Program.ToString(out string source);
            clbCode.Items.AddRange(source.Split(new[] { '\n' }));

            UpdateSidePanel();
            btnContinue.Text = "Start";
            btnContinue.Enabled = true;

            continueEx = false;
            Text = "WAITING FOR START";
            while (!continueEx && !nohalt)
            {
                Application.DoEvents();
            }
            btnContinue.Text = "Continue";
            btnContinue.Enabled = false;

        }

        private void CodeView_Load(object sender, EventArgs e)
        {
            clbCode = new CustomCheckedListBox(GetCodeItemBackColor, GetCodeItemForeColor);
            Controls.Add(clbCode);
            clbCode.BackColor = Color.DimGray;
            clbCode.ForeColor = Color.Black;
            clbCode.CheckOnClick = true;
            clbCode.Dock = DockStyle.Fill;
            Closing += CodeView_Closing;
            lbBuffers.MouseDoubleClick += LbBuffers_MouseDoubleClick;
            DoubleBuffered = true;
            ignoreChanged = true;
        }

        private Color GetCodeItemForeColor(CustomCheckedListBox listbox, DrawItemEventArgs e)
        {

            return listbox.ForeColor;

        }


        private Color GetCodeItemBackColor(CustomCheckedListBox listbox, DrawItemEventArgs e)
        {
            if (e.Index == selectedLine)
            {
                return Color.Orange;
            }
            else if (listbox.GetItemChecked(e.Index))
            {
                return Color.DarkRed;
            }
            else
            {
                return listbox.BackColor;
            }
        }

        private void CodeView_Closing(object sender, CancelEventArgs e)
        {
            exitDirect = true;
            nohalt = true;
        }

        private void LbBuffers_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            BufferView bvv = new BufferView(Program.GetBufferWithName(lbBuffers.SelectedItem.ToString(), false), Program.Dimensions.x, Program.Dimensions.y);
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

        public void ProgramExit(FLProgram program)
        {
            if (Program == program)
            {
                UpdateSidePanel();
                btnContinue.Text = "Close";
                btnContinue.Enabled = true;
                clbCode.Enabled = false;

                continueEx = false;
                Text = "DEBUGGING FINISHED";
                while (!continueEx && !exitDirect)
                {
                    Application.DoEvents();
                }
                btnContinue.Enabled = false;

                Program = null;
                FLProgram.Debugger = null;
                Close();
            }
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
            else s += " NULL";
            lblActiveChannels.Text = s;

            lbBuffers.Items.Clear();
            lbBuffers.Items.AddRange(Program.BufferNames.Cast<object>().ToArray());
        }

        public void ProcessEvent(FLParsedObject obj)
        {
            if (nohalt) return;

            int line = GetLineOfObject(obj);
            if (!clbCode.GetItemChecked(line)) return;


            UpdateSidePanel();
            btnContinue.Enabled = true;
            btnRunToEnd.Enabled = true;
            MarkInstruction(line);
            clbCode.Invalidate();


            continueEx = false;
            Text = "IN HALT MODE";
            while (!continueEx && !nohalt)
            {
                Application.DoEvents();
            }
            btnContinue.Enabled = false;
            btnRunToEnd.Enabled = true;

            Text = "Debugging";
        }

        public void MarkInstruction(int instr)
        {
            ignoreChanged = true;
            SelectLine(instr);
        }

        private bool ignoreChanged;

        private bool continueEx = false;
        private void btnContinue_Click(object sender, EventArgs e)
        {
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
    }
}
