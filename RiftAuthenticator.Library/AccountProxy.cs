using System;
using System.Collections.Generic;
using System.Text;

namespace RiftAuthenticator.Library
{
    public class AccountProxy : Library.AccountBase
    {
        public Library.IAccount OriginalAccount { get; private set; }

        public AccountProxy(Library.IAccount originalAccount)
        {
            OriginalAccount = originalAccount;
            CopyDataFromOriginalAccount();
        }

        private void CopyDataFromOriginalAccount()
        {
            this.Description = OriginalAccount.Description;
            this.DeviceId = OriginalAccount.DeviceId;
            this.SerialKey = OriginalAccount.SerialKey;
            this.SecretKey = OriginalAccount.SecretKey;
            this.TimeOffset = OriginalAccount.TimeOffset;
        }

        private void CopyDataToOriginalAccount()
        {
            OriginalAccount.Description = this.Description;
            OriginalAccount.DeviceId = this.DeviceId;
            OriginalAccount.SerialKey = this.SerialKey;
            OriginalAccount.SecretKey = this.SecretKey;
            OriginalAccount.TimeOffset = this.TimeOffset;
        }

        public override void Load(IAccountManager accountManager, int accountIndex)
        {
            //OriginalAccount.Load(accountManager, accountIndex);
            CopyDataFromOriginalAccount();
        }

        public override void Save(IAccountManager accountManager, int accountIndex)
        {
            CopyDataToOriginalAccount();
            //OriginalAccount.Save(accountManager, accountIndex);
        }
    }
}
