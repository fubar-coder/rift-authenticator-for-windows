using System;
using System.Collections.Generic;
using System.Text;

namespace RiftAuthenticator.Library
{
    public interface IAccountManager : IList<IAccount>
    {
        IAccount CreateAccount();

        void LoadAccounts();
        void SaveAccounts();

        ISecretKeyEncryption SecretKeyEncryption { get; }
    }
}
