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
            if (System.IO.File.Exists(fileName))
            {
                using (var stream = new System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                {
                    return PlatformUtils.Android.MapFile.ReadMap(stream);
                }
            }
            else
            {
                return new Dictionary<string, object>();
            }
        }

        internal static void WriteMap(string fileName, Dictionary<string, object> map)
        {
            using (var stream = new System.IO.FileStream(fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write))
            {
                PlatformUtils.Android.MapFile.WriteMap(stream, map);
            }
        }

        public override void Load(IAccountManager accountManager, int accountIndex)
        {
            var configFileName = GetAccountPath(accountIndex);
            if (System.IO.File.Exists(configFileName))
            {
                var map = ReadMap(configFileName);
                if (map.ContainsKey(DeviceIdKey))
                    DeviceId = (string)map[DeviceIdKey];
                if (map.ContainsKey(DescriptionKey))
                    Description = (string)map[DescriptionKey];
                if (map.ContainsKey(SerialKeyKey))
                    SerialKey = (string)map[SerialKeyKey];
                if (map.ContainsKey(TimeOffsetKey))
                    TimeOffset = Convert.ToInt64(map[TimeOffsetKey]);
                if (map.ContainsKey(SecretKeyKey))
                    SecretKey = (string)map[SecretKeyKey];
                SecretKey = accountManager.SecretKeyEncryption.Decrypt(this, SecretKey);
            }
        }

        public override void Save(IAccountManager accountManager, int accountIndex)
        {
            var configFileName = GetAccountPath(accountIndex);
            var map = new Dictionary<string, object>()
            {
                { DescriptionKey, Description },
                { DeviceIdKey, DeviceId },
                { SerialKeyKey, SerialKey },
                { TimeOffsetKey, TimeOffset },
                { SecretKeyKey, accountManager.SecretKeyEncryption.Encrypt(this, SecretKey) },
            };

            WriteMap(configFileName, map);
        }
    }
}
