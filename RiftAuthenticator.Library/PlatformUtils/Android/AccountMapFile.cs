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
    /// This is a generic account base class that uses map objects for account persistance
    /// </summary>
    public abstract class AccountMapFile : AccountBase
    {
        /// <summary>
        /// Creates a file name of the map file to read/write the data from/to
        /// </summary>
        /// <param name="accountManager">The account manager for the account at index "accountIndex"</param>
        /// <param name="accountIndex">The index of the account to create the file name from</param>
        /// <returns>Map file name</returns>
        protected abstract string GetFileName(IAccountManager accountManager, int accountIndex);

        /// <summary>
        /// Reads a map file
        /// </summary>
        /// <param name="fileName">The name of the map file</param>
        /// <returns>The map object with the data from the map file</returns>
        protected abstract Dictionary<string, object> ReadMapFile(string fileName);

        /// <summary>
        /// Writes a map file
        /// </summary>
        /// <param name="fileName">The name of the map file</param>
        /// <param name="map">The map object with the data to write to the map file</param>
        protected abstract void WriteMapFile(string fileName, Dictionary<string, object> map);

        /// <summary>
        /// Loads the data from the map file
        /// </summary>
        /// <param name="accountManager">The account manager that this account is assigned to</param>
        /// <param name="accountIndex">The index that this account has in the list of accounts in the account manager (must be unique)</param>
        public override void Load(IAccountManager accountManager, int accountIndex)
        {
            var configFileName = GetFileName(accountManager, accountIndex);
            var map = ReadMapFile(configFileName);
            AccountMap.SetMap(accountManager, this, map);
        }

        /// <summary>
        /// Writes the data to the map file
        /// </summary>
        /// <param name="accountManager">The account manager that this account is assigned to</param>
        /// <param name="accountIndex">The index that this account has in the list of accounts in the account manager (must be unique)</param>
        public override void Save(IAccountManager accountManager, int accountIndex)
        {
            var configFileName = GetFileName(accountManager, accountIndex);
            var map = AccountMap.GetMap(accountManager, this);
            WriteMapFile(configFileName, map);
        }
    }
}
