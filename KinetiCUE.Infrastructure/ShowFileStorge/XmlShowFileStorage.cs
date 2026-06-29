using KinetiCUE.Core.Interfaces;
using KinetiCUE.Core.Models.KQ;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace KinetiCUE.Infrastructure.ShowFileStorge
{
    public class XmlShowFileStorage : IShowFileStorage
    {
        public KQFile LoadFromFileSystem(string path)
        {
            var serializer = new XmlSerializer(typeof(KQFile));
            using var stream = new StreamReader(path);
            return (KQFile)serializer.Deserialize(stream);
        }

        public void SaveAsToFileSystem(KQFile show, string path)
        {

            throw new NotImplementedException();   
        }

        public void SaveToFileSystem(KQFile show, string path)
        {
            var serializer = new XmlSerializer(typeof(KQFile));
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
