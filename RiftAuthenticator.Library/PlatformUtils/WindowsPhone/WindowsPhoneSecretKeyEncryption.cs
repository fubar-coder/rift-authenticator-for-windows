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

namespace RiftAuthenticator.Library.PlatformUtils.WindowsPhone
{
    /// <summary>
    /// The secret key encryption class that's used on the Windows Phone platform
    /// </summary>
    public class WindowsPhoneSecretKeyEncryption : Library.PlatformUtils.Android.AndroidSecretKeyEncryption
    {
        static readonly System.Text.Encoding Encoding = System.Text.Encoding.UTF8;

        const string SecretKeyDigestSeed = "TrionMasterKey_031611";

        private byte[] _aesInitVector = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        /// <summary>
        /// Creates a symmetric cipher used to encrypt and decrypt the secret keys
        /// </summary>
        /// <returns>Symmetric cipher used for encryption and decvryption</returns>
        protected override System.Security.Cryptography.SymmetricAlgorithm CreateCipher()
        {
            var seed = Encoding.GetBytes(SecretKeyDigestSeed);
            var prng = new Org.Apache.Harmony.Security.Provider.Crypto.Sha1Prng();
            prng.AddSeedMaterial(seed);
            var aesKey = new byte[16];
            prng.NextBytes(aesKey);
            var aes = new System.Security.Cryptography.AesManaged();
            aes.IV = _aesInitVector;
            aes.KeySize = 128;
            aes.Key = aesKey;
            return aes;
        }
    }
}
