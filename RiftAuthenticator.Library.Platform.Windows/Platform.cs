/**
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

namespace RiftAuthenticator.Library.Platform.Windows
{
    public class Platform : IPlatform
    {
        public string DeviceId
        {
            get
            {
                try
                {
                    using (var regKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion"))
                    {
                        var productId = regKey.GetValue("ProductId");
                        if (productId != null)
                        {
                            var realProductId = Convert.ToString(productId);
                            var digest = new System.Security.Cryptography.SHA1Managed();
                            var productIdBytes = Encoding.Default.GetBytes(realProductId);
                            digest.TransformFinalBlock(productIdBytes, 0, productIdBytes.Length);
                            return Util.BytesToHex(digest.Hash);
                        }
                    }
                }
                catch
                {
                }
                return null;
            }
        }
    }
}
