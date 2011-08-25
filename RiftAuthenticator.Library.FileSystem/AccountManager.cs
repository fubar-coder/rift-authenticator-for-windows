using System;
using System.Collections.Generic;
using System.Text;

namespace RiftAuthenticator.Library.FileSystem
{
    public class AccountManager : Library.AccountManagerBase
    {
        private const string StoredAccountsKey = "stored_accounts";

        private string GetGlobalSettingsFileName()
        {
            return System.IO.Path.Combine(Account.GetAccountFolder(), "settings.xml");
        }

        public override IAccount CreateAccount()
        {
            return new Account();
        }

        public override int StoredAccounts
        {
            get
            {
                var configFileName = GetGlobalSettingsFileName();
                var map = Account.ReadMap(configFileName);
                if (!map.ContainsKey(StoredAccountsKey))
                    return 0;
                return (int)map[StoredAccountsKey];
            }
            set
            {
                var configFileName = GetGlobalSettingsFileName();
                var map = Account.ReadMap(configFileName);
                map[StoredAccountsKey] = value;
                Account.WriteMap(configFileName, map);
            }
        }
    }
}
