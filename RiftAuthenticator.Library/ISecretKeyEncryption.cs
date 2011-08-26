using System;
using System.Collections.Generic;
using System.Text;

namespace RiftAuthenticator.Library
{
    public interface ISecretKeyEncryption
    {
        string Encrypt(IAccount account, string secretKey);
        string Decrypt(IAccount account, string encryptedSecretKey);
    }
}
