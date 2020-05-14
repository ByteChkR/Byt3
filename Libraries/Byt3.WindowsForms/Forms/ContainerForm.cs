using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Byt3.WindowsForms.Forms
{
    public partial class ContainerForm : Form
    {
        private readonly Control Control;
        private readonly Control OriginalParent;
        private readonly DockStyle OriginalDockStyle;
        private readonly Action<Control, CancelEventArgs> OnClose;

        public ContainerForm(Control ctrl, Action<Control, CancelEventArgs> onClose, string title, Icon icon, FormBorderStyle borderStyle, Size minSize, Size maxSize)
        {
            InitializeComponent();
            if (icon != null)
            {
                Icon = icon;
            }
            Control = ctrl;
            OriginalParent = Control.Parent;
            OriginalDockStyle = Control.Dock;

            Control.Parent = this;
            Control.Dock = DockStyle.Fill;

            OnClose = onClose;



            Text = title;
            FormBorderStyle = borderStyle;
            MinimumSize = minSize;
            MaximumSize = maxSize;
            Closing += ContainerForm_Closing;
        }

        private void ContainerForm_Closing(object sender, CancelEventArgs e)
        {
            Control.Parent = OriginalParent;
            Control.Dock = OriginalDockStyle;
            OnClose?.Invoke(Control, e);
        }

        public static ContainerForm CreateContainer(Control ctrl, Action<Control, CancelEventArgs> onClose)
        {
            return CreateContainer(ctrl, onClose, ctrl.Name, null);
        }
        public static ContainerForm CreateContainer(Control ctrl, Action<Control, CancelEventArgs> onClose, string title, Icon icon)
        {
            return CreateContainer(ctrl, onClose, title, icon, FormBorderStyle.SizableToolWindow);
        }
        public static ContainerForm CreateContainer(Control ctrl, Action<Control, CancelEventArgs> onClose, string title, Icon icon, FormBorderStyle borderStyle)
        {
            return CreateContainer(ctrl, onClose, title, icon, borderStyle, SystemInformation.BorderSize + ctrl.Size);
        }
        public static ContainerForm CreateContainer(Control ctrl, Action<Control, CancelEventArgs> onClose, string title, Icon icon, FormBorderStyle borderStyle, Size minSize)
        {
            return CreateContainer(ctrl, onClose, title, icon, borderStyle, minSize, new Size(0,0));
        }
        public static ContainerForm CreateContainer(Control ctrl, Action<Control, CancelEventArgs> onClose, string title, Icon icon, FormBorderStyle borderStyle, Size minSize, Size maxSize)
        {
            ContainerForm form = new ContainerForm(ctrl, onClose, title, icon, borderStyle, minSize, maxSize);
            form.Show();
            return form;
        }
    }
}
