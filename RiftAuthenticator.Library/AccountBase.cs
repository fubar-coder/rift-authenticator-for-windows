using System;
using System.Collections.Generic;
using System.Text;

namespace RiftAuthenticator.Library
{
    public abstract class AccountBase : IAccount
    {
        static readonly System.Text.Encoding Encoding = System.Text.Encoding.Default;

        const string SecretKeyDigestSeed = "TrionMasterKey_031611";

        public string Description { get; set; }
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
                if (string.IsNullOrEmpty(SecretKey))
                    return string.Empty;
                return EncryptSecretKey(SecretKey);
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    SecretKey = string.Empty;
                }
                else
                {
                    SecretKey = DecryptSecretKey(value);
                }
            }
        }

        public bool IsEmpty
        {
            get
            {
                return string.IsNullOrEmpty(DeviceId);
            }
        }

        public abstract void Load(int accountIndex);
        public abstract void Save(int accountIndex);

        protected string DecryptSecretKey(string encryptedSecretKey)
        {
            return DecryptSecretKey(Util.HexToBytes(encryptedSecretKey));
        }

        protected virtual string DecryptSecretKey(byte[] encryptedSecretKey)
        {
            var aes = CreateCipher();
            var decryptor = aes.CreateDecryptor();
            var decryptedSecretKey = decryptor.TransformFinalBlock(encryptedSecretKey, 0, encryptedSecretKey.Length);
            return Encoding.GetString(decryptedSecretKey);
        }

        protected string EncryptSecretKey(string decryptedSecretKey)
        {
            return EncryptSecretKey(Encoding.GetBytes(decryptedSecretKey));
        }

        protected virtual string EncryptSecretKey(byte[] decryptedSecretKey)
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
