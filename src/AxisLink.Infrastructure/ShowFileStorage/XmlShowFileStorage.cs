using AxisLink.Core.Interfaces;
using AxisLink.Core.Models.Show;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Xml.Serialization;

namespace AxisLink.Infrastructure.ShowFileStorage
{
    public class XmlShowFileStorage : IShowFileStorage
    {
        public ShowFile LoadFromFileSystem(string path)
        {
            var serializer = new XmlSerializer(typeof(ShowFile));
            using var stream = new StreamReader(path);
            return (ShowFile)serializer.Deserialize(stream);
        }

        public void SaveAsToFileSystem(ShowFile show, string path)
        {

            throw new NotImplementedException();   
        }

        public void SaveToFileSystem(ShowFile show, string path)
        {
            var serializer = new XmlSerializer(typeof(ShowFile));
            string xml;
            using var stream = new MemoryStream();
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, show);
                xml = writer.ToString();
                File.WriteAllText(path, xml);
            }
        }


    }
}
