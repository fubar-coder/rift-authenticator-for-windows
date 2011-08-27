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
    public abstract class AccountManagerBase : List<IAccount>, IAccountManager
    {
        public abstract IAccount CreateAccount();
        public abstract int StoredAccounts { get; set; }
        public ISecretKeyEncryption SecretKeyEncryption { get; protected set; }

        public AccountManagerBase(ISecretKeyEncryption secretKeyEncryption)
        {
            SecretKeyEncryption = secretKeyEncryption;
        }

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

        public virtual void SaveAccounts()
        {
            for (int i = 0; i != Count; ++i)
            {
                this[i].Save(this, i);
            }
            StoredAccounts = Count;
        }

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

        public IAccount FindAccount(string accountId)
        {
            Library.IAccount foundAccount = null;
            for (int i = 0; i != Count; ++i)
            {
                var account = this[i];
                if (i.ToString() == accountId || account.Description == accountId || account.DeviceId == accountId)
                {
                    foundAccount = account;
                    break;
                }
            }
            return foundAccount;
        }
    }
}
