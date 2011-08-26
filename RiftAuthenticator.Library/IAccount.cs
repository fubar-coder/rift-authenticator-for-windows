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

namespace RiftAuthenticator.Library
{
    public interface IAccount
    {
        string Description { get; set; }
        string DeviceId { get; set; }
        string SerialKey { get; set; }
        string SecretKey { get; set; }
        long TimeOffset { get; set; }

        string FormattedSerialKey { get; }
        bool IsEmpty { get; }

        LoginToken CalculateToken();

        void Load(IAccountManager accountManager, int accountIndex);
        void Save(IAccountManager accountManager, int accountIndex);
    }
}
