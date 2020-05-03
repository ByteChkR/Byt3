using System.Windows.Forms;
using Byt3.Utilities.ConsoleInternals;

namespace FLDebugger
{
    public class DebugConsole : AConsole
    {
        public override string ConsoleKey => "fldbg";
        public override string ConsoleTitle => "Open FL Debugger GUI";
        public override bool Run(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (args.Length == 1)
            {
                Application.Run(new frmOptimizationView(args[0]));
            }
            else
            {
                Application.Run(new frmOptimizationView());
            }

            return true;
        }
    }
}