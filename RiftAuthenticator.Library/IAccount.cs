using System;
using System.Collections.Generic;
using System.Text;

namespace RiftAuthenticator.Library
{
    public interface IAccount
    {
        string Description { get; set; }
        string DeviceId { get; set; }
        string SerialKey { get; set; }
        string SecretKey { get; set; }
        long TimeOffset { get; set; }

        string FormattedSerialKey { get; }
        bool IsEmpty { get; }

        LoginToken CalculateToken();

        void Load(IAccountManager accountManager, int accountIndex);
        void Save(IAccountManager accountManager, int accountIndex);
    }
}
