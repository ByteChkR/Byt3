using System.Windows.Forms;
using Byt3.Utilities.ConsoleInternals;

namespace BenchmarkResultViewer
{
    public class FLBenchmarkViewerConsole : AConsole
    {
        public override string ConsoleKey => "benchview";
        public override string ConsoleTitle => "Open FL Benchmark Viewer GUI";
        public override bool Run(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(args));
            return true;
        }
    }
}