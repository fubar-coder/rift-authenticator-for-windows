using System;
using System.Collections.Generic;
using System.Text;

namespace RiftAuthenticator.Library
{
    public class AccountProxy : Library.IAccount
    {
        public Library.IAccount OriginalAccount { get; private set; }

        public AccountProxy(Library.IAccount originalAccount)
        {
            OriginalAccount = originalAccount;
        }

        public virtual string Description
        {
            get
            {
                return OriginalAccount.Description;
            }
            set
            {
                OriginalAccount.Description = value;
            }
        }

        public virtual string DeviceId
        {
            get
            {
                return OriginalAccount.DeviceId;
            }
            set
            {
                OriginalAccount.DeviceId = value;
            }
        }

        public virtual string SerialKey
        {
            get
            {
                return OriginalAccount.SerialKey;
            }
            set
            {
                OriginalAccount.SerialKey = value;
            }
        }

        public virtual string SecretKey
        {
            get
            {
                return OriginalAccount.SecretKey;
            }
            set
            {
                OriginalAccount.SecretKey = value;
            }
        }

        public virtual long TimeOffset
        {
            get
            {
                return OriginalAccount.TimeOffset;
            }
            set
            {
                OriginalAccount.TimeOffset = value;
            }
        }

        public virtual string FormattedSerialKey
        {
            get { return OriginalAccount.FormattedSerialKey; }
        }

        public virtual bool IsEmpty
        {
            get { return OriginalAccount.IsEmpty; }
        }

        public virtual Library.LoginToken CalculateToken()
        {
            if (OriginalAccount.IsEmpty)
                throw new InvalidOperationException();
            return OriginalAccount.CalculateToken();
        }

        public virtual void Load(Library.IAccountManager accountManager, int accountIndex)
        {
            OriginalAccount.Load(accountManager, accountIndex);
        }

        public virtual void Save(Library.IAccountManager accountManager, int accountIndex)
        {
            OriginalAccount.Save(accountManager, accountIndex);
        }
    }
}
