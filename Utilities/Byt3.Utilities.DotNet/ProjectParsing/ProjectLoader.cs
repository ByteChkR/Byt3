using System.IO;
using System.Xml;

namespace Byt3.Utilities.DotNet.ProjectParsing
{
    public class ProjectLoader
    {

        public static CSharpProject LoadProject(string path)
        {
            Stream s = File.OpenRead(path);
            XmlDocument d = new XmlDocument();
            d.Load(s);
            s.Close();
            return new CSharpProject(d);
        }

    }
}