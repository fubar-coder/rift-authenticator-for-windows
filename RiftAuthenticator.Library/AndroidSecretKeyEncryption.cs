﻿using System;
using System.Collections.Generic;
using System.Text;

namespace RiftAuthenticator.Library
{
    public class AndroidSecretKeyEncryption : ISecretKeyEncryption
    {
        static readonly System.Text.Encoding Encoding = System.Text.Encoding.Default;

        const string SecretKeyDigestSeed = "TrionMasterKey_031611";

        public string Encrypt(IAccount account, string secretKey)
        {
            if (string.IsNullOrEmpty(secretKey))
                return string.Empty;
            return EncryptSecretKey(secretKey);
        }

        public string Decrypt(IAccount account, string encryptedSecretKey)
        {
            if (string.IsNullOrEmpty(encryptedSecretKey))
            {
                return string.Empty;
            }
            else
            {
                return DecryptSecretKey(encryptedSecretKey);
            }
        }

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
    }
}
