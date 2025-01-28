using System.IO;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DataAccess
{
    public static class XmlSerializingExtensions
    {
        /// <summary>
        /// Extension method that serialize the specified type of object into an XML element
        /// </summary>
        /// <typeparam name="T">The type of object to be serialized</typeparam>
        /// <param name="obj">The object to be serialized</param>
        /// <returns>The desired type of object serilized from the XML element</returns>
        public static XElement ToXElement<T>(this object obj)
        {
            using (MemoryStream memoryStream = new())
            {
                using (TextWriter streamWriter = new StreamWriter(memoryStream))
                {
                    XmlSerializerNamespaces ns = new();
                    ns.Add("", ""); //Evita que se agregen Namespaces predeterminados

                    XmlSerializer xmlSerializer = new(typeof(T));
                    xmlSerializer.Serialize(streamWriter, obj, ns);
                    //return XElement.Parse(Encoding.ASCII.GetString(memoryStream.ToArray()));
                    return XElement.Parse(Encoding.UTF8.GetString(memoryStream.ToArray()));
                }
            }
        }

        /// <summary>
        /// Extension method that deserialize the XML element into the specified type of object
        /// </summary>
        /// <typeparam name="T">The type of object</typeparam>
        /// <param name="xElement">The XML element to be deserialized</param>
        /// <returns>The XML element deserialized into the specified type of object</returns>
        public static T? FromXElement<T>(this XElement xElement)
        {
            XmlSerializer xmlSerializer = new(typeof(T?));
            return (T?)xmlSerializer.Deserialize(xElement.CreateReader());
        }
    }
}
