using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using System.Windows.Forms;
using Byt3.OpenCL.Memory;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenFL.Common.Buffers;
using Byt3.WindowsForms.CustomControls;

namespace FLDebugger.Forms
{
    public partial class BufferView : Form
    {
        private static readonly List<BufferView> OpenForms = new List<BufferView>();

        private readonly FLBuffer Buffer;
        private readonly int height;
        private readonly int width;

        private readonly CLAPI Instance;

        private bool isRefreshing;
        private readonly CustomPictureBox pbBufferContent;

        public BufferView(CLAPI instance, FLBuffer buffer, int width, int height)
        {
            Instance = instance;
            InitializeComponent();

            MemoryBuffer buf = buffer.Buffer;

            if (buffer.Depth == 1)
            {
                nudFrame.Enabled = false;
            }
            else
            {
                nudFrame.Minimum = 0;
                nudFrame.Maximum = buffer.Depth - 1;
            }

            pbBufferContent = CreatePreviewBox(panelImage);
            Buffer = buffer;
            this.width = width;
            this.height = height;
            Text = buffer.DefinedBufferName + $"[{buffer.Buffer}]";


            comboBox1.Items.AddRange(Enum.GetNames(typeof(InterpolationMode)));
            comboBox1.SelectedIndex = 0;

            FLScriptEditor.RegisterDefaultTheme(panelSide);
            FLScriptEditor.RegisterDefaultTheme(btnReload);
            FLScriptEditor.RegisterDefaultTheme(panelImage);
            FLScriptEditor.RegisterDefaultTheme(comboBox1);
            FLScriptEditor.RegisterDefaultTheme(pbLoading);
            FLScriptEditor.RegisterDefaultTheme(pbIdle);
            FLScriptEditor.RegisterPreviewTheme(pbBufferContent);

            RefreshImage(Instance);
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

        private void DisplayContent(CLAPI instance)
        {
            if (Buffer.Buffer.IsDisposed)
            {
                throw new InvalidOperationException("Can not Use a Buffer that has been Disposed.");
            }

            pbBufferContent.Image?.Dispose();

            Bitmap bmp = new Bitmap(width, height);


            byte[] data = CLAPI.ReadBuffer<byte>(instance, Buffer.Buffer, (int)Buffer.Size);

            int start = 4 * Buffer.Width * Buffer.Height * (int) nudFrame.Value;
            int length = 4 * Buffer.Width * Buffer.Height;

            Span<byte> s = new Span<byte>(data, start, length);

            CLAPI.UpdateBitmap(instance, bmp, s.ToArray());
            pbBufferContent.Image = bmp;
        }

        private void SetLoadingImage(bool loading)
        {
            if (loading)
            {
                pbLoading.BringToFront();
            }
            else
            {
                pbIdle.BringToFront();
            }
        }

        private void RefreshImage(CLAPI instance)
        {
            if (isRefreshing)
            {
                return;
            }

            isRefreshing = true;
            SetLoadingImage(true);
            Task t = new Task(() => { DisplayContent(instance); });
            //DisplayContent(instance);
            t.Start();
            while (!t.IsCompleted)
            {
                Application.DoEvents();
            }

            if (t.IsFaulted)
            {
                MessageBox.Show("Can not show content of a disposed Buffer, form exiting", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }

            SetLoadingImage(false);
            isRefreshing = false;
        }

        private void BufferView_Load(object sender, EventArgs e)
        {
            Icon = Properties.Resources.OpenFL_Icon;
            OpenForms.Add(this);
            Closing += BufferView_Closing;

            Application.DoEvents();
        }

        private void BufferView_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            OpenForms.Remove(this);
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            if (isRefreshing)
            {
                return;
            }

            RefreshImage(Instance);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            pbBufferContent.InterpolationMode =
                (InterpolationMode)Enum.Parse(typeof(InterpolationMode),
                    comboBox1.Items[comboBox1.SelectedIndex].ToString());
            pbBufferContent.Invalidate();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (sfdExport.ShowDialog() == DialogResult.OK)
            {
                pbBufferContent.Image.Save(sfdExport.FileName);
            }
        }
    }
}