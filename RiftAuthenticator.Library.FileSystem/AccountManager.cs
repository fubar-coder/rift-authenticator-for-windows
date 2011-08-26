using System;
using System.Collections.Generic;
using System.Text;

namespace RiftAuthenticator.Library.FileSystem
{
    public class AccountManager : Library.AccountManagerBase
    {
        private const string StoredAccountsKey = "stored_accounts";

        public AccountManager()
            : this(new Library.PlatformUtils.Android.AndroidSecretKeyEncryption())
        {
        }

        public AccountManager(ISecretKeyEncryption secretKeyEncryption)
            : base(secretKeyEncryption)
        {
        }

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
