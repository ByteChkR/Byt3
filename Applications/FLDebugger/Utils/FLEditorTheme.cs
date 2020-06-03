using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace FLDebugger.Utils
{
    public class FLEditorTheme
    {
        public XMLColor PrimaryBackgroundColor = Color.DimGray;
        public XMLColor PrimaryFontColor = Color.Black;
        public XMLColor ErrorColor = Color.Red;
        public XMLColor SuccessColor = Color.Green;
        public XMLColor BufferViewBackgroundColor = Color.DimGray;
        public XMLColor DebuggerBreakpointColor = Color.DarkRed;
        public XMLColor DebuggerBreakpointHitColor = Color.Orange;

        public XMLColor FLComments = Color.Aqua;
        public XMLColor FLDefines = Color.Orange;
        public XMLColor FLFunctions = Color.Crimson;
        public XMLColor PPKeys = Color.Green;

        public float CodeFontSize = 8.25f;
        public float LogFontSize = 8.25f;


        private Action<FLEditorTheme> onThemeChange;
        public void Register(Action<FLEditorTheme> action)
        {
            onThemeChange += action;
            action(this);
        }

        public static void TransferEvents(FLEditorTheme from, FLEditorTheme to)
        {
            to.onThemeChange = from.onThemeChange;
            to.onThemeChange(to);
        }

        public void SetValue(FieldInfo info, object value)
        {
            info.SetValue(this, value);
            onThemeChange(this);
        }
    }
}