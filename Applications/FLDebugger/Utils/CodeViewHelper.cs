using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace FLDebugger.Utils
{
    public static class CodeViewHelper
    {
        internal static Color SourceBackColor =
            Color.FromArgb((byte) (Color.DimGray.R / 1.3f), (byte) (Color.DimGray.G / 1.3f),
                (byte) (Color.DimGray.B / 1.3f));

        public static void AppendText(this RichTextBox box, string text, Color color, Color backColor)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            box.SelectionColor = color;
            box.SelectionBackColor = backColor;
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;
        }

        public static void WriteSource(this RichTextBox rtb, string source)
        {
            rtb.SuspendLayout();
            int oldPos = rtb.SelectionStart;
            rtb.Text = "";

            string[] lines = source.Replace("\r", "").Split('\n');

            for (int lineIdex = 0; lineIdex < lines.Length; lineIdex++)
            {
                string line = lines[lineIdex];
                string[] parts = line.Split('#');
                string trimmedSourcePart = parts[0].Trim();
                string[] sourceParts = trimmedSourcePart.Split(' ');
                bool isFunction = sourceParts.FirstOrDefault(x => x.EndsWith(":")) != null;

                if (trimmedSourcePart.StartsWith("--")) //Define and Set Declaration
                {
                    rtb.AppendText(parts[0], Color.Orange, SourceBackColor);
                }
                else if (trimmedSourcePart.StartsWith("~")) //PP Directives
                {
                    rtb.AppendText(parts[0], Color.Green, SourceBackColor);
                }
                else if (isFunction) //Function Declaration
                {
                    rtb.AppendText(parts[0], Color.Crimson, SourceBackColor);
                }
                else if (!string.IsNullOrWhiteSpace(parts[0])) //Some Instructions
                {
                    rtb.AppendText(parts[0], Color.Black, SourceBackColor);
                }
                else
                {
                    rtb.AppendText(parts[0]); //Can only be whiteline
                }

                bool appendNewL = lineIdex != lines.Length - 1;


                if (parts.Length > 1)
                {
                    for (int i = 1; i < parts.Length; i++)
                    {
                        string text = parts[i];
                        if (i == 1)
                        {
                            text = '#' + text;
                        }

                        if (appendNewL && i == parts.Length - 1)
                        {
                            text += "\n";
                        }

                        rtb.AppendText(text, Color.Aqua, SourceBackColor);
                    }
                }
                else if (appendNewL)
                {
                    rtb.AppendText("\n", Color.Black, SourceBackColor);
                }
            }

            rtb.ScrollToCaret();
            Application.DoEvents();
            rtb.SelectionStart = oldPos;
            rtb.ResumeLayout();
        }
    }
}