using System;
using System.Drawing;
using System.IO;
using Byt3.CommandRunner;
using Byt3.Utilities.ConsoleInternals;
using Byt3Console.Png2Ico.Commands;

namespace Byt3Console.Png2Ico
{
    public class PNG2ICOConsole : AConsole
    {
        public override string ConsoleKey => "png2ico";
        public override string ConsoleTitle => "Converts Png Files to Icon Files";

        public override bool Run(string[] args)
        {
            Runner.AddCommand(new DefaultHelpCommand(true));
            Runner.AddCommand(new InputCommand());
            Runner.AddCommand(new OutputCommand());
            Runner.RunCommands(args);


            if (InputCommand.Input.Length != 0 && InputCommand.Input.Length == OutputCommand.Output.Length)
            {
                for (int i = 0; i < InputCommand.Input.Length; i++)
                {
                    string s = InputCommand.Input[i];
                    Image src = Image.FromFile(s);
                    Bitmap bmp;
                    if (src.Width <= 256 && src.Height <= 256)
                    {
                        bmp = new Bitmap(src);
                    }
                    else
                    {
                        bmp = new Bitmap(src, 256, 256);
                    }

                    Convert(bmp, OutputCommand.Output[i]);
                }

                return true;
            }

            return false;
        }

        private void Convert(Bitmap bmp, string output)
        {
            IntPtr ptr = bmp.GetHicon();
            Icon newIcon = Icon.FromHandle(ptr);
            Stream s = File.Create(output);
            newIcon.Save(s);
            s.Dispose();
            newIcon.Dispose();
            bmp.Dispose();
        }
    }
}