using System;
using System.Collections.Generic;
using System.Text;

namespace RiftAuthenticator.Library.PlatformUtils.Android
{
    public static class MapFile
    {
        public static Dictionary<string, object> ReadMap(System.IO.Stream stream)
        {
            var result = new Dictionary<string, object>();
            var doc = new System.Xml.XmlDocument();
            doc.Load(stream);
            foreach (System.Xml.XmlElement xmlSetting in doc.SelectNodes("/map/*"))
            {
                var key = xmlSetting.GetAttribute("name");
                object value;
                switch (xmlSetting.LocalName)
                {
                    case "string":
                        value = xmlSetting.InnerText;
                        break;
                    case "long":
                        value = Convert.ToInt64(xmlSetting.GetAttribute("value"));
                        break;
                    case "int":
                        value = Convert.ToInt32(xmlSetting.GetAttribute("value"));
                        break;
                    default:
                        throw new NotSupportedException(xmlSetting.LocalName);
                }
                result.Add(key, value);
            }
            return result;
        }

        public static void WriteMap(System.IO.Stream stream, Dictionary<string, object> map)
        {
            var writerSettings = new System.Xml.XmlWriterSettings()
            {
                Indent = true,
            };
            using (var writer = System.Xml.XmlWriter.Create(stream, writerSettings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("map");
                foreach (var mapItem in map)
                {
                    var key = mapItem.Key;
                    var value = mapItem.Value;
                    if (value == null)
                        throw new NotSupportedException();
                    string mapType;
                    if (value is string)
                    {
                        mapType = "string";
                    }
                    else if (value is long)
                    {
                        mapType = "long";
                    }
                    else if (value is int)
                    {
                        mapType = "int";
                    }
                    else
                        throw new NotSupportedException(value.GetType().FullName);
                    writer.WriteStartElement(mapType);
                    writer.WriteAttributeString("name", key);
                    if (mapType == "string")
                    {
                        writer.WriteString((string)value);
                    }
                    else if (mapType == "long")
                    {
                        writer.WriteAttributeString("value", System.Xml.XmlConvert.ToString((long)value));
                    }
                    else if (mapType == "int")
                    {
                        writer.WriteAttributeString("value", System.Xml.XmlConvert.ToString((int)value));
                    }
                    else
                    {
                        System.Diagnostics.Debug.Assert(false);
                    }
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Close();
            }
        }

    }
}
