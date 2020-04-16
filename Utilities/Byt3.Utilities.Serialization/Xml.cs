using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Byt3.Utilities.Serialization
{
    public static class Xml
    {
        #region Fields

        private static readonly XmlWriterSettings WriterSettings = new XmlWriterSettings
            {OmitXmlDeclaration = true, Indent = true};

        private static readonly XmlSerializerNamespaces Namespaces =
            new XmlSerializerNamespaces(new[] {new XmlQualifiedName("", "")});

        #endregion

        #region Methods

        public static string Serialize(object obj, Type[] extraTypes = null)
        {
            if (obj == null)
            {
                return null;
            }

            return DoSerialize(obj, extraTypes);
        }

        private static string DoSerialize(object obj, Type[] extraTypes = null)
        {
            if (extraTypes == null)
            {
                extraTypes = new Type[0];
            }

            using (MemoryStream ms = new MemoryStream())
            using (XmlWriter writer = XmlWriter.Create(ms, WriterSettings))
            {
                XmlSerializer serializer = new XmlSerializer(obj.GetType(), extraTypes);
                serializer.Serialize(writer, obj, Namespaces);
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        public static T Deserialize<T>(string data)
            where T : class
        {
            if (string.IsNullOrEmpty(data))
            {
                return null;
            }

            return DoDeserialize<T>(data);
        }

        private static T DoDeserialize<T>(string data) where T : class
        {
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(data)))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                return (T) serializer.Deserialize(ms);
            }
        }

        #endregion
    }
}