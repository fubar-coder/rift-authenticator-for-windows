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
    /// Interface for Authenticator/RIFT accounts
    /// </summary>
    public interface IAccount
    {
        /// <summary>
        /// Description for the account (any text)
        /// </summary>
        string Description { get; set; }
        /// <summary>
        /// The device ID where the authenticator account is bound to
        /// </summary>
        string DeviceId { get; set; }
        /// <summary>
        /// The serial key for the authenticator (created by TRION)
        /// </summary>
        string SerialKey { get; set; }
        /// <summary>
        /// The shared secret for the authenticator
        /// </summary>
        string SecretKey { get; set; }
        /// <summary>
        /// Time difference in milliseconds between the client and the TRION server
        /// </summary>
        long TimeOffset { get; set; }

        /// <summary>
        /// Add dashes to the serial key
        /// </summary>
        string FormattedSerialKey { get; }

        /// <summary>
        /// Is this account empty?
        /// </summary>
        /// <remarks>
        /// Returns true if the authenticator is empty (i.e. not initialized, 
        /// but may have a description)
        /// </remarks>
        bool IsEmpty { get; }

        /// <summary>
        /// Calculates a login for RIFT
        /// </summary>
        /// <returns></returns>
        LoginToken CalculateToken();

        /// <summary>
        /// Load the account information from the underlying storage device
        /// </summary>
        /// <param name="accountManager">The account manager that this account is assigned to</param>
        /// <param name="accountIndex">The index that this account has in the list of accounts in the account manager (must be unique)</param>
        void Load(IAccountManager accountManager, int accountIndex);

        /// <summary>
        /// Save the account information to the underlying storage device
        /// </summary>
        /// <param name="accountManager">The account manager that this account is assigned to</param>
        /// <param name="accountIndex">The index that this account has in the list of accounts in the account manager (must be unique)</param>
        void Save(IAccountManager accountManager, int accountIndex);
    }
}
