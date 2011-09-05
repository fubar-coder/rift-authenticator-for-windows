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
    /// Helper class for handling map objects that contain the authenticator configuration.
    /// </summary>
    public static class AccountMap
    {
        /// <summary>
        /// The map key for the description
        /// </summary>
        public const string DescriptionKey = "description";
        /// <summary>
        /// The map key for the device id
        /// </summary>
        public const string DeviceIdKey = "device_id";
        /// <summary>
        /// The map key for the serial key
        /// </summary>
        public const string SerialKeyKey = "serial_key";
        /// <summary>
        /// The map key for the (encrypted) secret key
        /// </summary>
        public const string SecretKeyKey = "secret_key";
        /// <summary>
        /// The map key for the time offset
        /// </summary>
        public const string TimeOffsetKey = "time_offset";

        /// <summary>
        /// Reads the authenticator configuration from the map object
        /// </summary>
        /// <param name="accountManager">The account manager for the "account"</param>
        /// <param name="account">The account manager to write the data to</param>
        /// <param name="map">The map object to write the data to</param>
        public static void SetMap(IAccountManager accountManager, IAccount account, Dictionary<string, object> map)
        {
            if (map.ContainsKey(DeviceIdKey))
                account.DeviceId = (string)map[DeviceIdKey];
            if (map.ContainsKey(DescriptionKey))
                account.Description = (string)map[DescriptionKey];
            if (map.ContainsKey(SerialKeyKey))
                account.SerialKey = (string)map[SerialKeyKey];
            if (map.ContainsKey(TimeOffsetKey))
                account.TimeOffset = Convert.ToInt64(map[TimeOffsetKey]);
            if (map.ContainsKey(SecretKeyKey))
                account.SecretKey = (string)map[SecretKeyKey];
            account.SecretKey = TrionServer.SecretKeyEncryption.Decrypt(account, account.SecretKey);
        }

        /// <summary>
        /// Writes the authenticator configuration into the map object
        /// </summary>
        /// <param name="accountManager">The account manager for the "account"</param>
        /// <param name="account">The account manager to get the data from</param>
        /// <returns>The newly created map object</returns>
        public static Dictionary<string, object> GetMap(IAccountManager accountManager, IAccount account)
        {
            var map = new Dictionary<string, object>()
            {
                { DescriptionKey, account.Description ?? string.Empty },
                { DeviceIdKey, account.DeviceId ?? string.Empty },
                { SerialKeyKey, account.SerialKey ?? string.Empty },
                { TimeOffsetKey, account.TimeOffset },
                { SecretKeyKey, TrionServer.SecretKeyEncryption.Encrypt(account, account.SecretKey ?? string.Empty) },
            };
            return map;
        }
    }
}
