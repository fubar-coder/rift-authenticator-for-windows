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
    /// This is the base class for all account manager objects
    /// </summary>
    public abstract class AccountManagerBase : List<IAccount>, IAccountManager
    {
        /// <summary>
        /// Creates a new account object
        /// </summary>
        /// <returns>The new account object</returns>
        public abstract IAccount CreateAccount();

        /// <summary>
        /// Set or get the number of stored accounts
        /// </summary>
        protected abstract int StoredAccounts { get; set; }

        /// <summary>
        /// Load all account objects from the underlying storage device
        /// </summary>
        public virtual void LoadAccounts()
        {
            var accounts = new List<IAccount>();
            for (int i = 0; i != StoredAccounts; ++i)
            {
                var account = CreateAccount();
                account.Load(this, i);
                accounts.Add(account);
            }
            Clear();
            AddRange(accounts);
        }

        /// <summary>
        /// Save all account objects to the underlying storage device
        /// </summary>
        public virtual void SaveAccounts()
        {
            for (int i = 0; i != Count; ++i)
            {
                this[i].Save(this, i);
            }
            StoredAccounts = Count;
        }

#if !WINDOWS_PHONE
        /// <summary>
        /// Create a well known account manager object for a given account manager ID
        /// </summary>
        /// <param name="accountManagerId">Account manager ID to create an account manager object for</param>
        /// <returns>The newly created account manager object</returns>
        public static IAccountManager LoadAccountManager(string accountManagerId)
        {
            switch (accountManagerId)
            {
                case "win32":
                case "registry":
                case "RiftAuthenticator.Library.Registry":
                    accountManagerId = "RiftAuthenticator.Library.Registry";
                    break;
                case "fs":
                case "file-system":
                case "filesystem":
                case "RiftAuthenticator.Library.FileSystem":
                    accountManagerId = "RiftAuthenticator.Library.FileSystem";
                    break;
                case "is":
                case "isolated-storage":
                case "storage":
                    accountManagerId = "RiftAuthenticator.Library.IsolatedStorage";
                    break;
                default:
                    throw new NotSupportedException(accountManagerId);
            }
            var assemblyName = accountManagerId;
            var typeName = string.Format("{0}.AccountManager", accountManagerId);
            return (RiftAuthenticator.Library.IAccountManager)Activator.CreateInstance(assemblyName, typeName).Unwrap();
        }
#endif

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
        public IAccount FindAccount(string accountId)
        {
            Library.IAccount foundAccount = null;
            for (int i = 0; i != Count; ++i)
            {
                var account = this[i];
                if (i.ToString() == accountId || account.Description == accountId || account.DeviceId == accountId || account.SerialKey == accountId)
                {
                    foundAccount = account;
                    break;
                }
            }
            return foundAccount;
        }
    }
}
