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
    /// The account manager that manages all accounts for a storage device
    /// </summary>
    public interface IAccountManager : IList<IAccount>
    {
        /// <summary>
        /// Creates a new account object
        /// </summary>
        /// <returns>The new account object</returns>
        IAccount CreateAccount();

        /// <summary>
        /// Find an account object with a given ID
        /// </summary>
        /// <remarks>
        /// A valid account ID is:
        /// - the index into the account list
        /// - the description
        /// - the device ID
        /// - the authenticator serial key
        /// </remarks>
        /// <param name="accountId">The ID to search for</param>
        /// <returns>The found account object or null</returns>
        IAccount FindAccount(string accountId);

        /// <summary>
        /// Load all account objects from the underlying storage device
        /// </summary>
        void LoadAccounts();

        /// <summary>
        /// Save all account objects to the underlying storage device
        /// </summary>
        void SaveAccounts();
    }
}
