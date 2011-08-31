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

namespace RiftAuthenticator.Library
{
    /// <summary>
    /// This is the base account class where all account objects are derived from
    /// </summary>
    public abstract class AccountBase : IAccount
    {
        static readonly System.Text.Encoding Encoding = System.Text.Encoding.Default;

        /// <summary>
        /// Description for the account (any text)
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// The device ID where the authenticator account is bound to
        /// </summary>
        public string DeviceId { get; set; }
        /// <summary>
        /// The serial key for the authenticator (created by TRION)
        /// </summary>
        public string SerialKey { get; set; }
        /// <summary>
        /// The shared secret for the authenticator
        /// </summary>
        public string SecretKey { get; set; }
        /// <summary>
        /// Time difference in milliseconds between the client and the TRION server
        /// </summary>
        public long TimeOffset { get; set; }

        /// <summary>
        /// Add dashes to the serial key
        /// </summary>
        public string FormattedSerialKey
        {
            get
            {
                var result = new StringBuilder();
                if (!string.IsNullOrEmpty(SerialKey))
                {
                    for (int i = 0; i < SerialKey.Length; i += 4)
                    {
                        var remaining = Math.Min(SerialKey.Length - i, 4);
                        if (i != 0)
                            result.Append("-");
                        result.Append(SerialKey.Substring(i, remaining));
                    }
                }
                return result.ToString();
            }
        }

        /// <summary>
        /// Is this account empty?
        /// </summary>
        /// <remarks>
        /// Returns true if the authenticator is empty (i.e. not initialized, 
        /// but may have a description)
        /// </remarks>
        public bool IsEmpty
        {
            get
            {
                return string.IsNullOrEmpty(DeviceId);
            }
        }

        /// <summary>
        /// Load the account information from the underlying storage device
        /// </summary>
        /// <param name="accountManager">The account manager that this account is assigned to</param>
        /// <param name="accountIndex">The index that this account has in the list of accounts in the account manager (must be unique)</param>
        public abstract void Load(IAccountManager accountManager, int accountIndex);
        /// <summary>
        /// Save the account information to the underlying storage device
        /// </summary>
        /// <param name="accountManager">The account manager that this account is assigned to</param>
        /// <param name="accountIndex">The index that this account has in the list of accounts in the account manager (must be unique)</param>
        public abstract void Save(IAccountManager accountManager, int accountIndex);

        /// <summary>
        /// Calculates a login for RIFT
        /// </summary>
        /// <returns></returns>
        public LoginToken CalculateToken()
        {
            var secretKey = Encoding.GetBytes(SecretKey);
            var now = DateTime.Now;
            var nowMillis = Util.TimeToMillis(now);
            long intervalNumber = (nowMillis + TimeOffset) / 30000;
            // Value must be written as big endian byte buffer
            var intervalByteBuffer = Mono.DataConverter.Pack("^l", intervalNumber);

            var digest = new System.Security.Cryptography.HMACSHA1(secretKey);
            var hash = digest.ComputeHash(intervalByteBuffer);

            var rawTokenPos = hash[hash.Length - 1] & 0xF;

            // Raw token value must be read as big endian 32 bit integer value
            var rawTokenValue = (long)(uint)Mono.DataConverter.Unpack("^I", hash, rawTokenPos)[0];
            var remainingMillis = ((intervalNumber + 1) * 30000 - TimeOffset) - nowMillis;

            var tokenValue = rawTokenValue % 100000000;
            return new LoginToken(string.Format("{0:D08}", tokenValue), remainingMillis);
        }
    }
}
