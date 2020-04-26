using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Byt3Console.Runner
{
    public class ConsoleItem
    {
        public string LibPath { get; set; }
        public string FileHash { get; set; }
        public string TypeSignature { get; set; }
        public string ConsoleKey { get; set; }
        public string ConsoleTitle { get; set; }



        public bool Run(string[] args)
        {
            AppDomainController adc = AppDomainController.Create(ConsoleTitle, new[] {Path.GetDirectoryName(LibPath)});
            object ret = adc.LoadTypeAndRunFunction(LibPath, TypeSignature, "Run", new object[] {args});
            adc.Dispose();
            return ret == null || ret is bool b && b;

        }

        public ConsoleItem()
        {
        }

        public ConsoleItem(AppDomainController adc, string libPath, Type type)
        {
            LibPath = libPath;
            FileHash = GetHash(LibPath);


            TypeSignature = type.FullName;
            try
            {
                ConsoleTitle = (string) adc.GetPropertyValue(libPath, TypeSignature, "ConsoleTitle");
            }
            catch (Exception)
            {
                ConsoleTitle = Path.GetFileNameWithoutExtension(libPath);
            }

            ConsoleKey = (string) adc.GetPropertyValue(libPath, TypeSignature, "ConsoleKey");
        }

        public static string GetHash(string File)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in ComputeHash(File))
            {
                sb.Append(b.ToString("X2"));
            }

            return sb.ToString();
        }

        private static byte[] ComputeHash(string file)
        {
            using (HashAlgorithm algorithm = SHA256.Create())
            {
                Stream s = File.OpenRead(file);
                byte[] ret = algorithm.ComputeHash(s);
                s.Close();
                return ret;
            }
        }
    }
}