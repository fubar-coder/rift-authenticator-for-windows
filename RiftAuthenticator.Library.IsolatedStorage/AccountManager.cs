﻿/**
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

namespace RiftAuthenticator.Library.IsolatedStorage
{
    public class AccountManager : Library.AccountManagerBase
    {
        private const string StoredAccountsKey = "stored_accounts";

        public AccountManager()
        {
        }

        private string GetGlobalSettingsFileName()
        {
            return "settings.xml";
        }

        public override IAccount CreateAccount()
        {
            return new Account();
        }

        protected override int StoredAccounts
        {
            get
            {
                var configFileName = GetGlobalSettingsFileName();
                try
                {
                    var map = Account.ReadMap(configFileName);
                    if (!map.ContainsKey(StoredAccountsKey))
                        return 0;
                    return (int)map[StoredAccountsKey];
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                var configFileName = GetGlobalSettingsFileName();
                Dictionary<string, object> map;
                try
                {
                    map = Account.ReadMap(configFileName);
                }
                catch
                {
                    map = new Dictionary<string, object>();
                }
                map[StoredAccountsKey] = value;
                Account.WriteMap(configFileName, map);
            }
        }
    }
}
