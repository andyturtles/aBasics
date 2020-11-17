using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace aBasics {

    /// <summary>
    /// Tool zum serialisieren / deserialisieren
    /// </summary>
    public class SerializeTool {

        /// <summary>
        /// Serializes the object to XML.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="oo">The object</param>
        /// <param name="noNamespaces">if set to <c>true</c> [no namespaces].</param>
        /// <returns>xml</returns>
        public static string SerializeObjectToXml<T>(T oo, bool noNamespaces) {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using ( StringWriter writer = new StringWriter() ) {
                if ( noNamespaces ) serializer.Serialize(writer, oo, new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty }));
                else                serializer.Serialize(writer, oo);
                return writer.ToString();
            }
        }

        /// <summary>Des the serialize XML to object.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml">The XML.</param>
        /// <returns>object</returns>
        public static T DeSerializeXmlToObject<T>(string xml) {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using ( StringReader reader = new StringReader(xml) ) {
                return (T)serializer.Deserialize(reader);
            }
        }

        /// <summary>
        /// json de-serialisieren
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json">The json.</param>
        /// <returns>Objekt T</returns>
        public static T DeSerializeJson<T>(string json) {
            T deserializedObj;
            using ( MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json)) ) {
                DataContractJsonSerializer ser  = new DataContractJsonSerializer(typeof(T));
                deserializedObj                 = (T)ser.ReadObject(ms);
            }
            return deserializedObj;
        }

        /// <summary>
        /// Serializes to json.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <returns>jSon String</returns>
        public static string SerializeJson<T>(T obj) {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            using ( MemoryStream ms = new MemoryStream() ) {
                ser.WriteObject(ms, obj);
                ms.Position = 0;
                using ( StreamReader sr = new StreamReader(ms) ) {
                    string s = sr.ReadToEnd();
                    return s;
                }
            }
        }

        /// <summary>
        /// Parses the json dictionary.
        /// </summary>
        /// <param name="jsonData">The json data.</param>
        /// <returns>Dict</returns>
        public static Dictionary<string, string> ParseJsonDictionary(string jsonData) {
            JavaScriptSerializer serializer     = new JavaScriptSerializer();
            Dictionary<string, string> values   = serializer.Deserialize<Dictionary<string, string>>(jsonData);
            return values;
        }

    }
}
