/**
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

namespace RiftAuthenticator.Library.FileSystem
{
    public class Account : RiftAuthenticator.Library.AccountBase
    {
        const string DescriptionKey = "description";
        const string DeviceIdKey = "device_id";
        const string SerialKeyKey = "serial_key";
        const string SecretKeyKey = "secret_key";
        const string TimeOffsetKey = "time_offset";

        private static Dictionary<string, System.Reflection.PropertyInfo> KeyToPropertyMap = new Dictionary<string, System.Reflection.PropertyInfo>
        {
            { DescriptionKey, typeof(IAccount).GetProperty("Description") },
            { DeviceIdKey, typeof(IAccount).GetProperty("DeviceId") },
            { SerialKeyKey, typeof(IAccount).GetProperty("SerialKey") },
            { SecretKeyKey, typeof(IAccount).GetProperty("EncryptedSecretKey") },
            { TimeOffsetKey, typeof(IAccount).GetProperty("TimeOffset") },
        };

        internal static string GetAccountFileName(int accountIndex)
        {
            return string.Format("Account {0}.xml", accountIndex + 1);
        }

        internal static string GetAccountFolder()
        {
            var result = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "RiftAuthenticator");
            System.IO.Directory.CreateDirectory(result);
            return result;
        }

        internal static string GetAccountPath(int accountIndex)
        {
            return System.IO.Path.Combine(GetAccountFolder(), GetAccountFileName(accountIndex));
        }

        internal static Dictionary<string, object> ReadMap(string fileName)
        {
            var result = new Dictionary<string, object>();
            if (System.IO.File.Exists(fileName))
            {
                var doc = new System.Xml.XmlDocument();
                doc.Load(fileName);
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
            }
            return result;
        }

        internal static void WriteMap(string fileName, Dictionary<string, object> map)
        {
            var writerSettings = new System.Xml.XmlWriterSettings()
            {
                CloseOutput = true,
                Indent = true,
            };
            using (var writer = System.Xml.XmlWriter.Create(fileName, writerSettings))
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

        public override void Load(int accountIndex)
        {
            var configFileName = GetAccountPath(accountIndex);
            if (System.IO.File.Exists(configFileName))
            {
                var map = ReadMap(configFileName);
                foreach (var mapItem in map)
                {
                    var key = mapItem.Key;
                    var value = mapItem.Value;
                    if (!KeyToPropertyMap.ContainsKey(key))
                        throw new NotSupportedException(key);
                    KeyToPropertyMap[key].SetValue(this, value, null);
                }
            }
        }

        public override void Save(int accountIndex)
        {
            var configFileName = GetAccountPath(accountIndex);
            var map = new Dictionary<string, object>();

            foreach (var keyAndPropertyInfo in KeyToPropertyMap)
            {
                var key = keyAndPropertyInfo.Key;
                var prop = keyAndPropertyInfo.Value;
                var propType = prop.PropertyType;
                var value = prop.GetValue(this, null);
                if (propType == typeof(string))
                {
                    map.Add(key, (string)value ?? string.Empty);
                }
                else if (propType == typeof(long))
                {
                    map.Add(key, (value == null ? 0 : (long)value));
                }
                else
                    throw new NotSupportedException(propType.FullName);
            }

            WriteMap(configFileName, map);
        }
    }
}
