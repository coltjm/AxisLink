using AxisLink.Core.Interfaces;
using AxisLink.Core.Models.Extended;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace AxisLink.Infrastructure.ShowFileStorage
{
    public class XmlShowFileStorage : IShowFileStorage
    {
        public ExtendedFile LoadFromFileSystem(string path)
        {
            var serializer = new XmlSerializer(typeof(ExtendedFile));
            using var stream = new StreamReader(path);
            return (ExtendedFile)serializer.Deserialize(stream);
        }

        public void SaveAsToFileSystem(ExtendedFile show, string path)
        {

            throw new NotImplementedException();   
        }

        public void SaveToFileSystem(ExtendedFile show, string path)
        {
            var serializer = new XmlSerializer(typeof(ExtendedFile));
            string xml;
            using var stream = new MemoryStream();
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, show);
                xml = writer.ToString();
                File.WriteAllText(path, xml);
            }
        }
        // Reads and updates the show file


    }
}
