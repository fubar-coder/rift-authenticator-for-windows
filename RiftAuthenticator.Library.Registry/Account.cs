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

namespace RiftAuthenticator.Library.Registry
{
    public class Account : RiftAuthenticator.Library.AccountBase
    {
        const string ConfigVersionKey = "Version";
        const string DescriptionKey = "Description";
        const string DeviceIdKey = "DeviceId";
        const string SerialKeyKey = "SerialKey";
        const string SecretKeyKey = "SecretKey";
        const string TimeOffsetKey = "TimeOffset";

        internal static readonly string RiftAuthenticatorRegistryKey = "SOFTWARE\\Public Domain\\Rift Authenticator";

        internal static string GetAccountRegistryPathPart(int accountIndex)
        {
            return string.Format("Account {0}", accountIndex + 1);
        }

        internal static string GetAccountRegistryPath(int accountIndex)
        {
            return string.Format("{0}\\{1}", RiftAuthenticatorRegistryKey, GetAccountRegistryPathPart(accountIndex));
        }

        public override void Load(IAccountManager accountManager, int accountIndex)
        {
            using (var key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(GetAccountRegistryPath(accountIndex)))
            {
                var configVersion = Convert.ToInt32(key.GetValue(ConfigVersionKey, 0));
                Description = (string)key.GetValue(DescriptionKey, string.Format("Account {0}", accountIndex + 1));
                DeviceId = (string)key.GetValue(DeviceIdKey, string.Empty);
                SerialKey = (string)key.GetValue(SerialKeyKey, string.Empty);
                SecretKey = (string)key.GetValue(SecretKeyKey, string.Empty);
                TimeOffset = Convert.ToInt64(key.GetValue(TimeOffsetKey, 0));
                switch (configVersion)
                {
                    case 0:
                        break;
                    case 1:
                        SecretKey = TrionServer.SecretKeyEncryption.Decrypt(this, SecretKey);
                        break;
                }
            }
        }

        public override void Save(IAccountManager accountManager, int accountIndex)
        {
            using (var key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(GetAccountRegistryPath(accountIndex)))
            {
                key.SetValue(ConfigVersionKey, 1);
                key.SetValue(DescriptionKey, Description ?? string.Empty);
                key.SetValue(DeviceIdKey, DeviceId ?? string.Empty);
                key.SetValue(SerialKeyKey, SerialKey ?? string.Empty);
                key.SetValue(SecretKeyKey, TrionServer.SecretKeyEncryption.Encrypt(this, SecretKey ?? string.Empty));
                key.SetValue(TimeOffsetKey, TimeOffset);
            }
        }
    }
}
