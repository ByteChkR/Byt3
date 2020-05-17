using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Byt3.OpenFL.Common.Instructions.InstructionCreators;
using Byt3.OpenFL.Common.Instructions.SignatureParsing;
using Byt3.OpenFL.Common.ProgramChecks;
using FLDebugger.Properties;

namespace FLDebugger.Forms
{
    public partial class InstructionViewer : Form
    {
        private readonly List<string> InstructionKeys;
        private readonly List<FLInstructionCreator> InstructionSet;

        public InstructionViewer(FLInstructionSet instructionSet)
        {
            InstructionSet = new List<FLInstructionCreator>();

            InstructionKeys = instructionSet.GetInstructionNames().ToList();


            for (int i = 0; i < instructionSet.CreatorCount; i++)
            {
                InstructionSet.Add(instructionSet.GetCreatorAt(i));
            }


            InitializeComponent();
            Text = $"Viewing {InstructionKeys.Count} Instructions";
            Icon = Resources.OpenFL_Icon;
        }

        private void InstructionViewer_Load(object sender, EventArgs e)
        {
            InstructionKeys.Sort();
            lbInstructions.Items.AddRange(InstructionKeys.Select(x => x.StartsWith("_") ? "(INCOMPATIBLE)" + x : x)
                .Cast<object>().ToArray());
        }

        private void lbInstructions_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbOverloads.Items.Clear();
            lblInstructionName.Text = "Instruction: ";

            if (lbInstructions.SelectedIndex != -1)
            {
                string inst = lbInstructions.SelectedItem.ToString();
                if (inst.StartsWith("(INCOMPATIBLE)"))
                {
                    inst = inst.Remove(0, "(INCOMPATIBLE)".Length);
                }

                FLInstructionCreator c = FindCreator(inst);
                string args = c.GetArgumentSignatureForInstruction(inst);

                bool ret = SignatureParser.ParseCreatorSig(args, out List<InstructionArgumentSignature> sig);

                lblInstructionName.Text = "Instruction: " + inst;

                rtbInstructionDescription.Text = c.GetDescriptionForInstruction(inst);

                lbOverloads.Items.Clear();
                for (int i = 0; i < sig.Count; i++)
                {
                    lbOverloads.Items.Add(sig[i]);
                }
            }
        }

        private FLInstructionCreator FindCreator(string key)
        {
            for (int i = 0; i < InstructionSet.Count; i++)
            {
                if (InstructionSet[i].InstructionKeys.Contains(key))
                {
                    return InstructionSet[i];
                }
            }

            return null;
        }
    }
}