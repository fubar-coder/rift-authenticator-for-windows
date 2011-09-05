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
    /// This is an interface to platform specific values/functions
    /// </summary>
    public interface IPlatform
    {
        /// <summary>
        /// The ID for the device where this authenticator executes on
        /// </summary>
        string DeviceId { get; }

        /// <summary>
        /// This is the user agent used for the HTTP communication
        /// </summary>
        string UserAgent { get; }

        /// <summary>
        /// Get the secret key encryption object
        /// </summary>
        /// <remarks>
        /// This secret key encryption object is used to encrypt and decrypt
        /// the secret key in a platform specific way.
        /// </remarks>
        ISecretKeyEncryption SecretKeyEncryption { get; }
    }
}
