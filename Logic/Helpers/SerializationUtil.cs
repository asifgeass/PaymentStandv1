using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Logic
{
    public static class SerializationUtil
    {
        public static async Task<T> Deserialize<T>(XDocument doc)
        {
            return await Task.Run(() =>
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

                using (var reader = doc.Root.CreateReader())
                {
                    return (T)xmlSerializer.Deserialize(reader);
                }
            });

        }

        public static async Task<XDocument> Serialize<T>(T value)
        {
            return await Task.Run(() =>
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

                XDocument doc = new XDocument();
                using (var writer = doc.CreateWriter())
                {
                    xmlSerializer.Serialize(writer, value);
                }

                return doc;
            });
        }
    }
}
