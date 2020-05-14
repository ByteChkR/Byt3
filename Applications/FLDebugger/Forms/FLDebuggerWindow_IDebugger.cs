using System;
using System.Diagnostics;
using System.Windows.Forms;
using Byt3.OpenFL.Common.Buffers;
using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;

namespace FLDebugger.Forms
{
    public partial class FLDebuggerWindow : IProgramDebugger
    {
        public bool FollowScripts { get; private set; }
        private Stopwatch instrTimer = new Stopwatch();
        private Stopwatch totalTimer = new Stopwatch();
        public void ProcessEvent(FLParsedObject obj)
        {
            if (nohalt) return;
            instrTimer.Stop();
            totalTimer.Stop();
            double millis = instrTimer.Elapsed.TotalMilliseconds;
            double totalMillis = totalTimer.Elapsed.TotalMilliseconds;
            int line = GetLineOfObject(obj);


            string newLine =
                $"{clbCode.Items[line].ToString().TrimEnd()}\t| {Math.Round(totalMillis, 4)} ms ({Math.Round(millis, 4)} ms)";
            clbCode.Items[line] = newLine;

            clbCode.Invalidate();

            if (!clbCode.GetItemChecked(line))
            {
                instrTimer.Restart();
                totalTimer.Start();
                return;
            }

            btnContinue.Text = "Start";

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
            instrTimer.Restart();
            totalTimer.Start();
        }

        public void ProgramExit()
        {

            UpdateSidePanel();
            btnContinue.Text = "Close";
            btnContinue.Enabled = true;

            continueEx = false;
            Text = "DEBUGGING FINISHED";
            while (!continueEx && !exitDirect)
            {
                Application.DoEvents();
            }
            btnContinue.Enabled = false;

            Program = null;
            Close();

        }

        public void SubProgramStarted()
        {
            instrTimer.Stop();
            totalTimer.Stop();
            if (nohalt) return;
            Enabled = false;
            continueEx = false;
            Show();
            btnContinue.Enabled = true;
            Text = "WAITING FOR SUBPROGRAM";
        }

        public void SubProgramEnded()
        {
            Enabled = true;
            continueEx = true;
            btnContinue.Enabled = false;
            btnContinue.Text = "Continue";
            instrTimer.Start();
            totalTimer.Start();
        }

        public void ProgramStart()
        {
            continueEx = false;
            Show();
            Text = "WAITING FOR PROGRAM START";
            while (!continueEx && !exitDirect)
            {
                Application.DoEvents();
            }
            btnContinue.Enabled = false;
            btnContinue.Text = "Continue";
        }

        public void OnAddInternalBuffer(FLBuffer buffer)
        {
            //TODO
            lbInternalBuffers.Items.Add(buffer);
        }
    }
}