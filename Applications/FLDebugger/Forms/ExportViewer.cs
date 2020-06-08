using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenFL.Common;
using Byt3.OpenFL.Common.Buffers;
using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;
using Byt3.OpenFL.Common.Instructions.InstructionCreators;
using Byt3.OpenFL.Serialization;
using FLDebugger.Properties;
using FLDebugger.Utils;

namespace FLDebugger.Forms
{
    public partial class ExportViewer : Form
    {
        private FLDataContainer container;
        public ExportViewer(string file)
        {
            InitializeComponent();
            Icon = Resources.OpenFL_Icon;

            FLScriptEditor.RegisterPreviewTheme(pbExportView);

            container = new FLDataContainer("resources/kernel");
            Stream s = File.OpenRead(file);

            SerializableFLProgram prog = FLSerializer.LoadProgram(s, container.InstructionSet);
            FLProgram p = prog.Initialize(container.Instance, container.InstructionSet);
            FLBuffer input = new FLBuffer(container.Instance, 512, 512, 1, "Input");
            p.Run(input, true);

            Bitmap bmp = new Bitmap(512, 512);
            CLAPI.UpdateBitmap(container.Instance, bmp, p.GetActiveBuffer(false).Buffer);

            pbExportView.Image = bmp;

        }
    }
}
