/**
 * This file is part of RIFT Authenticator for Windows.
 *
 * RIFT Authenticator for Windows is free software: you can redistribute 
 * it and/or modify it under the terms of the GNU General Public License 
 * as published by the Free Software Foundation, either version 3 of the 
 * License, or (at your option) any later version.
 *
 * RIFT Authenticator for Windows is distributed in the hope that it will 
 * be useful, but WITHOUT ANY WARRANTY; without even the implied warranty 
 * of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU 
 * General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with RIFT Authenticator for Windows.  If not, see 
 * <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace RiftAuthenticator.Library
{
    public class Configuration
    {
        static readonly System.Text.Encoding Encoding = System.Text.Encoding.Default;

        const string SecretKeyDigestSeed = "TrionMasterKey_031611";

        const string ConfigVersionKey = "Version";
        const string DeviceIdKey = "DeviceId";
        const string SerialKeyKey = "SerialKey";
        const string SecretKeyKey = "SecretKey";
        const string TimeOffsetKey = "TimeOffset";

        public string DeviceId { get; set; }
        public string SerialKey { get; set; }
        public string SecretKey { get; set; }
        public long TimeOffset { get; set; }

        public string FormattedSerialKey
        {
            get
            {
                var result = new StringBuilder();
                for (int i = 0; i < SerialKey.Length; i += 4)
                {
                    var remaining = Math.Min(SerialKey.Length - i, 4);
                    if (i != 0)
                        result.Append("-");
                    result.Append(SerialKey.Substring(i, remaining));
                }
                return result.ToString();
            }
        }

        public string EncryptedSecretKey
        {
            get
            {
                return EncryptSecretKey(SecretKey);
            }
            set
            {
                SecretKey = DecryptSecretKey(value);
            }
        }

        public bool IsEmpty
        {
            get
            {
                return string.IsNullOrEmpty(DeviceId);
            }
        }

        string AccountRegistryPath
        {
            get
            {
                return "SOFTWARE\\Public Domain\\Rift Authenticator\\Account 1";
            }
        }

        public void Load()
        {
            using (var key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(AccountRegistryPath))
            {
                var configVersion = Convert.ToInt32(key.GetValue(ConfigVersionKey, 0));
                DeviceId = (string)key.GetValue(DeviceIdKey, string.Empty);
                SerialKey = (string)key.GetValue(SerialKeyKey, string.Empty);
                SecretKey = (string)key.GetValue(SecretKeyKey, string.Empty);
                TimeOffset = Convert.ToInt64(key.GetValue(TimeOffsetKey, 0));
                switch (configVersion)
                {
                    case 0:
                        break;
                    case 1:
                        SecretKey = DecryptSecretKey(SecretKey);
                        break;
                }
            }
        }

        public void Save()
        {
            using (var key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(AccountRegistryPath))
            {
                key.SetValue(ConfigVersionKey, 1);
                key.SetValue(DeviceIdKey, DeviceId ?? string.Empty);
                key.SetValue(SerialKeyKey, SerialKey ?? string.Empty);
                key.SetValue(SecretKeyKey, EncryptSecretKey(SecretKey ?? string.Empty));
                key.SetValue(TimeOffsetKey, TimeOffset);
            }
        }

        private string DecryptSecretKey(string encryptedSecretKey)
        {
            return DecryptSecretKey(Util.HexToBytes(encryptedSecretKey));
        }

        private string DecryptSecretKey(byte[] encryptedSecretKey)
        {
            var aes = CreateCipher();
            var decryptor = aes.CreateDecryptor();
            var decryptedSecretKey = decryptor.TransformFinalBlock(encryptedSecretKey, 0, encryptedSecretKey.Length);
            return Encoding.GetString(decryptedSecretKey);
        }

        private string EncryptSecretKey(string decryptedSecretKey)
        {
            return EncryptSecretKey(Encoding.GetBytes(decryptedSecretKey));
        }

        private string EncryptSecretKey(byte[] decryptedSecretKey)
        {
            var aes = CreateCipher();
            var encryptor = aes.CreateEncryptor();
            var encryptedSecretKey = encryptor.TransformFinalBlock(decryptedSecretKey, 0, decryptedSecretKey.Length);
            return Util.BytesToHex(encryptedSecretKey);
        }

        private static System.Security.Cryptography.RijndaelManaged CreateCipher()
        {
            var seed = Encoding.GetBytes(SecretKeyDigestSeed);
            var prng = new Org.Apache.Harmony.Security.Provider.Crypto.Sha1Prng();
            prng.AddSeedMaterial(seed);
            var aesKey = new byte[16];
            prng.NextBytes(aesKey);
            var aes = new System.Security.Cryptography.RijndaelManaged();
            aes.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            aes.Mode = System.Security.Cryptography.CipherMode.ECB;
            aes.KeySize = 128;
            aes.Key = aesKey;
            return aes;
        }

        public LoginToken CalculateToken()
        {
            var secretKey = Encoding.GetBytes(SecretKey);
            var now = DateTime.Now;
            var nowMillis = Util.timeToMillis(now);
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
