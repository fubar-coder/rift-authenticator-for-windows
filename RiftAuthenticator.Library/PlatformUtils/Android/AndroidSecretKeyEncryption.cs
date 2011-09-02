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
    /// The secret key encryption class that's used on the android platform
    /// </summary>
    public class AndroidSecretKeyEncryption : ISecretKeyEncryption
    {
        static readonly System.Text.Encoding Encoding = System.Text.Encoding.UTF8;

        const string SecretKeyDigestSeed = "TrionMasterKey_031611";

        /// <summary>
        /// Encrypt a secret key for an account
        /// </summary>
        /// <param name="account">The account to encrypt the secret key for</param>
        /// <param name="secretKey">The secret key to encrypt</param>
        /// <returns>The encrypted secret key</returns>
        public string Encrypt(IAccount account, string secretKey)
        {
            if (string.IsNullOrEmpty(secretKey))
                return string.Empty;
            return EncryptSecretKey(secretKey);
        }

        /// <summary>
        /// Decrypt an encrypted secret key for an account
        /// </summary>
        /// <param name="account">The account to decrypt the secret key for</param>
        /// <param name="encryptedSecretKey">The encrypted secret key to decrypt</param>
        /// <returns>The decrypted secret key</returns>
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

        /// <summary>
        /// Decrypt an encrypted secret key for an account
        /// </summary>
        /// <param name="encryptedSecretKey">The encrypted secret key to decrypt</param>
        /// <returns>The decrypted secret key</returns>
        protected string DecryptSecretKey(string encryptedSecretKey)
        {
            return DecryptSecretKey(Util.HexToBytes(encryptedSecretKey));
        }

        /// <summary>
        /// Decrypt an encrypted secret key for an account
        /// </summary>
        /// <param name="encryptedSecretKey">The encrypted secret key to decrypt</param>
        /// <returns>The decrypted secret key</returns>
        protected virtual string DecryptSecretKey(byte[] encryptedSecretKey)
        {
            var aes = CreateCipher();
            var decryptor = aes.CreateDecryptor();
            var decryptedSecretKey = decryptor.TransformFinalBlock(encryptedSecretKey, 0, encryptedSecretKey.Length);
            return Encoding.GetString(decryptedSecretKey, 0, decryptedSecretKey.Length);
        }

        /// <summary>
        /// Encrypt a secret key for an account
        /// </summary>
        /// <param name="decryptedSecretKey">The secret key to encrypt</param>
        /// <returns>The encrypted secret key</returns>
        protected string EncryptSecretKey(string decryptedSecretKey)
        {
            return EncryptSecretKey(Encoding.GetBytes(decryptedSecretKey));
        }

        /// <summary>
        /// Encrypt a secret key for an account
        /// </summary>
        /// <param name="decryptedSecretKey">The secret key to encrypt</param>
        /// <returns>The encrypted secret key</returns>
        protected virtual string EncryptSecretKey(byte[] decryptedSecretKey)
        {
            var aes = CreateCipher();
            var encryptor = aes.CreateEncryptor();
            var encryptedSecretKey = encryptor.TransformFinalBlock(decryptedSecretKey, 0, decryptedSecretKey.Length);
            return Util.BytesToHex(encryptedSecretKey);
        }

        private static System.Security.Cryptography.AesManaged CreateCipher()
        {
            var seed = Encoding.GetBytes(SecretKeyDigestSeed);
            var prng = new Org.Apache.Harmony.Security.Provider.Crypto.Sha1Prng();
            prng.AddSeedMaterial(seed);
            var aesKey = new byte[16];
            prng.NextBytes(aesKey);
            var aes = new System.Security.Cryptography.AesManaged();
            aes.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            //aes.Mode = System.Security.Cryptography.CipherMode.ECB;
            aes.KeySize = 128;
            aes.Key = aesKey;
            return aes;
        }
    }
}
