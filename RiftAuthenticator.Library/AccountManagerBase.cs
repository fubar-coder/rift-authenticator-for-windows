using System;
using System.Collections.Generic;
using System.Text;

namespace RiftAuthenticator.Library
{
    public abstract class AccountManagerBase : List<IAccount>, IAccountManager
    {
        public abstract IAccount CreateAccount();
        public abstract int StoredAccounts { get; set; }

        public virtual void LoadAccounts()
        {
            var accounts = new List<IAccount>();
            for (int i = 0; i != StoredAccounts; ++i)
            {
                var account = CreateAccount();
                account.Load(i);
                accounts.Add(account);
            }
            Clear();
            AddRange(accounts);
        }

        public virtual void SaveAccounts()
        {
            for (int i = 0; i != Count; ++i)
            {
                this[i].Save(i);
            }
            StoredAccounts = Count;
        }
    }
}
