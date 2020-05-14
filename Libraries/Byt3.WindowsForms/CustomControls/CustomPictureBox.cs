using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Byt3.WindowsForms.CustomControls
{
    public class CustomPictureBox : PictureBox
    {
        public InterpolationMode InterpolationMode { get; set; } = InterpolationMode.Default;

        protected override void OnPaint(PaintEventArgs pe)
        {
            pe.Graphics.InterpolationMode = InterpolationMode;
            base.OnPaint(pe);
        }
    }
}