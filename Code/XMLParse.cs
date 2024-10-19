using System;
using System.IO;
using System.Xml.Serialization;

namespace Lumberjack
{
    class XMLParse
    {
        public static void SerializeToXml(string filePath, object data)
        {
            // Create an instance of XmlSerializer for the specified type
            XmlSerializer serializer = new XmlSerializer(data.GetType());

            // Create a FileStream to write the XML file
            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                // Serialize the data to the FileStream
                serializer.Serialize(stream, data);
            }
        }

        public static T DeserializeFromXml<T>(string filePath)
        {
            // Create an instance of XmlSerializer for the specified type
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            // Create a FileStream to read the XML file
            using (FileStream stream = new FileStream(filePath, FileMode.Open))
            {
                // Deserialize the data from the FileStream
                return (T)serializer.Deserialize(stream);
            }
        }
    }
}
