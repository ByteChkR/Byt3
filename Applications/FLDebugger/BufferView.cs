using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenFL.Common.Buffers;

namespace FLDebugger
{
    

    public partial class BufferView : Form
    {
        private readonly FLBuffer Buffer;
        private readonly int width;
        private readonly int height;
        private CustomPictureBox pbBufferContent;
        public BufferView(FLBuffer buffer, int width, int height)
        {
            InitializeComponent();
            pbBufferContent = CreatePreviewBox(panelImage);
            Buffer = buffer;
            this.width = width;
            this.height = height;
            Text = buffer.DefinedBufferName + $"[{buffer.Buffer}]";
            
            comboBox1.Items.AddRange(Enum.GetNames(typeof(InterpolationMode)));
            comboBox1.SelectedIndex = 0;
        }

        private CustomPictureBox CreatePreviewBox(Control parent)
        {
            CustomPictureBox ret = new CustomPictureBox();
            ret.Parent = parent;
            ret.BackColor = Color.DimGray;
            ret.Dock = DockStyle.Fill;
            ret.Location = new Point(0, 0);
            ret.Name = "pbBufferContent";
            ret.Size = new Size(749, 508);
            ret.SizeMode = PictureBoxSizeMode.Zoom;
            ret.TabIndex = 2;
            ret.TabStop = false;
            return ret;
        }

        private void DisplayContent()
        {
            if (Buffer.Buffer.IsDisposed)
            {
                MessageBox.Show("Can not show content of a disposed Buffer, form exiting", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }
            pbBufferContent.Image?.Dispose();
            Bitmap bmp = new Bitmap(width, height);
            CLAPI.UpdateBitmap(CLAPI.MainThread, bmp, Buffer.Buffer);
            pbBufferContent.Image = bmp;
        }

        private void BufferView_Load(object sender, EventArgs e)
        {
            DisplayContent();
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            DisplayContent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            pbBufferContent.InterpolationMode =
                (InterpolationMode) Enum.Parse(typeof(InterpolationMode), comboBox1.Items[comboBox1.SelectedIndex].ToString());
            pbBufferContent.Invalidate();
        }
    }
}
