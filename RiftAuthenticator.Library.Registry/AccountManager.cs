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

namespace RiftAuthenticator.Library.Registry
{
    public class AccountManager : RiftAuthenticator.Library.AccountManagerBase
    {
        const string StoredAccountsKey = "StoredAccounts";

        public AccountManager()
            : this(new Library.PlatformUtils.Android.AndroidSecretKeyEncryption())
        {
        }

        public AccountManager(ISecretKeyEncryption secretKeyEncryption)
            : base(secretKeyEncryption)
        {
        }

        public override IAccount CreateAccount()
        {
            return new Account();
        }

        protected override int StoredAccounts
        {
            get
            {
                using (var key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(Account.RiftAuthenticatorRegistryKey))
                {
                    if (!new List<string>(key.GetValueNames()).Contains(StoredAccountsKey))
                    {
                        if (new List<string>(key.GetSubKeyNames()).Contains(Account.GetAccountRegistryPathPart(0)))
                            key.SetValue(StoredAccountsKey, 1);
                    }
                    return Convert.ToInt32(key.GetValue(StoredAccountsKey, 0));
                }
            }
            set
            {
                using (var key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(Account.RiftAuthenticatorRegistryKey))
                {
                    key.SetValue(StoredAccountsKey, value);
                }
            }
        }
    }
}
