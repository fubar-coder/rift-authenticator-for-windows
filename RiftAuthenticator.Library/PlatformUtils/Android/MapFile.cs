/*
 * This file is part of RIFT™ Authenticator for Windows.
 *
 * RIFT™ Authenticator for Windows is free software: you can redistribute 
 * it and/or modify it under the terms of the GNU General Public License 
 * as published by the Free Software Foundation, either version 3 of the 
 * License, or (at your option) any later version.
 *
 * RIFT™ Authenticator for Windows is distributed in the hope that it will 
 * be useful, but WITHOUT ANY WARRANTY; without even the implied warranty 
 * of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU 
 * General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with RIFT™ Authenticator for Windows.  If not, see 
 * <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace RiftAuthenticator.Library.PlatformUtils.Android
{
    /// <summary>
    /// Map file helper functions
    /// </summary>
    public static class MapFile
    {
        /// <summary>
        /// Read a map from a stream
        /// </summary>
        /// <param name="stream">The stream to read the map data from</param>
        /// <returns>The map object read from the map file</returns>
        public static Dictionary<string, object> ReadMap(System.IO.Stream stream)
        {
            var result = new Dictionary<string, object>();
            ReadMap(stream, result);
            return result;
        }

        /// <summary>
        /// Read a map from a stream
        /// </summary>
        /// <param name="stream">The stream to read the map data from</param>
        /// <param name="map">The map object where the data from the file gets written to</param>
        public static void ReadMap(System.IO.Stream stream, Dictionary<string, object> map)
        {
            var doc = System.Xml.Linq.XDocument.Load(stream);
            foreach (var xmlSetting in doc.Element("map").Elements())
            {
                var key = xmlSetting.Attribute("name").Value;
                object value;
                switch (xmlSetting.Name.LocalName)
                {
                    case "string":
                        value = xmlSetting.Value;
                        break;
                    case "long":
                        value = Convert.ToInt64(xmlSetting.Attribute("value").Value);
                        break;
                    case "int":
                        value = Convert.ToInt32(xmlSetting.Attribute("value").Value);
                        break;
                    default:
                        throw new NotSupportedException(xmlSetting.Name.LocalName);
                }
                map[key] = value;
            }
        }

        /// <summary>
        /// Write a map to a stream
        /// </summary>
        /// <param name="stream">The stream to write the map data to</param>
        /// <param name="map">The map object to write to the map file</param>
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
