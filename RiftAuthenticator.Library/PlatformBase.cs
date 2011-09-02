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
    /// This is the abstract base class for all platform objects
    /// </summary>
    public abstract class PlatformBase : IPlatform
    {
        /// <summary>
        /// The device ID where the authenticator executes on
        /// </summary>
        public abstract string DeviceId { get; }

        /// <summary>
        /// 
        /// </summary>
        public abstract string UserAgent { get; }

#if !WINDOWS_PHONE
        /// <summary>
        /// Create a well known platform object for a given platform ID
        /// </summary>
        /// <param name="platformId">Platform ID to create a platform object for</param>
        /// <returns>The newly created platform object</returns>
        public static IPlatform LoadPlatform(string platformId)
        {
            switch (platformId)
            {
                case "win32":
                case "windows":
                case "RiftAuthenticator.Library.Platform.Windows":
                    platformId = "RiftAuthenticator.Library.Platform.Windows";
                    break;
                default:
                    throw new NotSupportedException(platformId);
            }
            var assemblyName = platformId;
            var typeName = string.Format("{0}.Platform", platformId);
            return
                (RiftAuthenticator.Library.IPlatform)Activator.CreateInstance(assemblyName, typeName).Unwrap();
        }
#endif
    }
}
