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
    /// An interface to a secret key encryption/decryption object
    /// </summary>
    /// <remarks>
    /// This secret key encryption object is used to encrypt and decrypt
    /// the secret key in a platform specific way.
    /// </remarks>
    public interface ISecretKeyEncryption
    {
        /// <summary>
        /// Encrypt a secret key for an account
        /// </summary>
        /// <param name="account">The account to encrypt the secret key for</param>
        /// <param name="secretKey">The secret key to encrypt</param>
        /// <returns>The encrypted secret key</returns>
        string Encrypt(IAccount account, string secretKey);

        /// <summary>
        /// Decrypt an encrypted secret key for an account
        /// </summary>
        /// <param name="account">The account to decrypt the secret key for</param>
        /// <param name="encryptedSecretKey">The encrypted secret key to decrypt</param>
        /// <returns>The decrypted secret key</returns>
        string Decrypt(IAccount account, string encryptedSecretKey);
    }
}
