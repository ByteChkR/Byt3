using System.Drawing;
using System.Windows.Forms;

namespace FLDebugger
{
    using System;
    using System.Runtime.CompilerServices;

    public class ResizeableControl
    {
        private Control _mControl;

        private Control mControl
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get => _mControl;

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_mControl != null)
                {
                    _mControl.MouseDown -= mControl_MouseDown;
                    _mControl.MouseUp -= mControl_MouseUp;
                    _mControl.MouseMove -= mControl_MouseMove;
                    _mControl.MouseLeave -= mControl_MouseLeave;
                }

                _mControl = value;
                if (_mControl != null)
                {
                    _mControl.MouseDown += mControl_MouseDown;
                    _mControl.MouseUp += mControl_MouseUp;
                    _mControl.MouseMove += mControl_MouseMove;
                    _mControl.MouseLeave += mControl_MouseLeave;
                }
            }
        }

        public bool DrawOutline;
        public Color OutlineColor;
        private Brush OutlineBrush => new SolidBrush(OutlineColor);
        public EdgeEnum Edges = EdgeEnum.All;
        private bool mMouseDown = false;
        private EdgeEnum mEdge = EdgeEnum.None;
        private int mWidth = 10;
        private bool mOutlineDrawn = false;

        [Flags]
        public enum EdgeEnum : int
        {
            All = -1,
            None = 0,
            Right = 1,
            Left = 2,
            Top = 4,
            Bottom = 8,
            TopLeft = 16,
            //TopRight = 32,
            //BottomLeft = 64,
            //BottomRight = 128,
        }

        public ResizeableControl(Control Control, params Control[] dockedChildren)
        {
            mControl = Control;

            for (int i = 0; i < dockedChildren.Length; i++)
            {
                RegisterToEvents(dockedChildren[i]);
            }
        }

        private void UnregisterFromEvents(Control c)
        {
            c.MouseDown -= mControl_MouseDown;
            c.MouseUp -= mControl_MouseUp;
            c.MouseMove -= mControl_MouseMove;
            c.MouseLeave -= mControl_MouseLeave;
        }

        private void RegisterToEvents(Control c)
        {
            c.MouseDown += mControl_MouseDown;
            c.MouseUp += mControl_MouseUp;
            c.MouseMove += mControl_MouseMove;
            c.MouseLeave += mControl_MouseLeave;
        }

        private void mControl_MouseDown(object sender, MouseEventArgs e)

        {
            if (e.Button == MouseButtons.Left)
            {
                mMouseDown = true;
            }
        }

        private void mControl_MouseUp(object sender, MouseEventArgs e)

        {
            mMouseDown = false;
        }

        private void mControl_MouseMove(object sender, MouseEventArgs e)

        {
            Control c = (Control)mControl;
            Graphics g = (sender as Control).CreateGraphics();
            EdgeEnum switchExpr = mEdge;
            if ((Edges & mEdge) != 0)
            {
                switch (switchExpr)
                {
                    case EdgeEnum.TopLeft:
                        {
                            g.FillRectangle(OutlineBrush, 0, 0, mWidth * 4, mWidth * 4);
                            mOutlineDrawn = true;
                            break;
                        }

                    case EdgeEnum.Left:
                        {
                            g.FillRectangle(OutlineBrush, 0, 0, mWidth, c.Height);
                            mOutlineDrawn = true;
                            break;
                        }

                    case EdgeEnum.Right:
                        {
                            g.FillRectangle(OutlineBrush, c.Width - mWidth, 0, c.Width, c.Height);
                            mOutlineDrawn = true;
                            break;
                        }

                    case EdgeEnum.Top:
                        {
                            g.FillRectangle(OutlineBrush, 0, 0, c.Width, mWidth);
                            mOutlineDrawn = true;
                            break;
                        }

                    case EdgeEnum.Bottom:
                        {
                            g.FillRectangle(OutlineBrush, 0, c.Height - mWidth, c.Width, mWidth);
                            mOutlineDrawn = true;
                            break;
                        }

                    case EdgeEnum.None:
                        {
                            if (mOutlineDrawn)
                            {
                                c.Refresh();
                                mOutlineDrawn = false;
                            }

                            break;
                        }
                }
            }


            if ((Edges & mEdge) != 0 && mMouseDown & (mEdge != EdgeEnum.None))
            {
                c = mControl;
                c.SuspendLayout();
                EdgeEnum switchExpr1 = mEdge;
                switch (switchExpr1)
                {
                    case EdgeEnum.TopLeft:
                        {
                            c.SetBounds(c.Left + e.X, c.Top + e.Y, c.Width, c.Height);
                            break;
                        }

                    case EdgeEnum.Left:
                        {
                            c.SetBounds(c.Left + e.X, c.Top, c.Width - e.X, c.Height);
                            break;
                        }

                    case EdgeEnum.Right:
                        {
                            c.SetBounds(c.Left, c.Top, c.Width - (c.Width - e.X), c.Height);
                            break;
                        }

                    case EdgeEnum.Top:
                        {
                            c.SetBounds(c.Left, c.Top + e.Y, c.Width, c.Height - e.Y);
                            break;
                        }

                    case EdgeEnum.Bottom:
                        {
                            c.SetBounds(c.Left, c.Top, c.Width, c.Height - (c.Height - e.Y));
                            break;
                        }
                }
                c.ResumeLayout();
            }
            else
            {
                switch (true)
                {
                    case object _ when (e.X <= mWidth * 4) & (e.Y <= mWidth * 4):
                        // top left corner
                        {
                            if ((Edges & EdgeEnum.TopLeft) != 0)
                            {
                                c.Cursor = Cursors.SizeAll;
                            }
                            mEdge = EdgeEnum.TopLeft;
                            break;
                        }

                    case object _ when e.X <= mWidth: // left edge
                        {
                            if ((Edges & EdgeEnum.Left) != 0)
                            {
                                c.Cursor = Cursors.VSplit;
                            }
                            mEdge = EdgeEnum.Left;
                            break;
                        }

                    case object _ when e.X > c.Width - (mWidth + 1): // right edge
                        {
                            if ((Edges & EdgeEnum.Right) != 0)
                            {
                                c.Cursor = Cursors.VSplit;
                            }
                            mEdge = EdgeEnum.Right;
                            break;
                        }

                    case object _ when e.Y <= mWidth: // top edge
                        {
                            if ((Edges & EdgeEnum.Top) != 0)
                            {
                                c.Cursor = Cursors.HSplit;
                            }
                            mEdge = EdgeEnum.Top;
                            break;
                        }

                    case object _ when e.Y > c.Height - (mWidth + 1): // bottom edge
                        {
                            if ((Edges & EdgeEnum.Bottom) != 0)
                            {
                                c.Cursor = Cursors.HSplit;
                            }
                            mEdge = EdgeEnum.Bottom; // no edge
                            break;
                        }

                    default:
                        {
                            c.Cursor = Cursors.Default;
                            mEdge = EdgeEnum.None;
                            break;
                        }
                }
            }
        }

        private void mControl_MouseLeave(object sender, EventArgs e)

        {
            Control c = (Control)sender;
            mEdge = EdgeEnum.None;
            c.Refresh();
        }
    }
}