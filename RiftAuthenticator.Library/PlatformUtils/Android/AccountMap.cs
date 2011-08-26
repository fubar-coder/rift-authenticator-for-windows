using System;
using System.Collections.Generic;
using System.Text;

namespace RiftAuthenticator.Library.PlatformUtils.Android
{
    public static class AccountMap
    {
        public const string DescriptionKey = "description";
        public const string DeviceIdKey = "device_id";
        public const string SerialKeyKey = "serial_key";
        public const string SecretKeyKey = "secret_key";
        public const string TimeOffsetKey = "time_offset";

        public static void SetMap(IAccountManager accountManager, IAccount account, Dictionary<string, object> map)
        {
            if (map.ContainsKey(DeviceIdKey))
                account.DeviceId = (string)map[DeviceIdKey];
            if (map.ContainsKey(DescriptionKey))
                account.Description = (string)map[DescriptionKey];
            if (map.ContainsKey(SerialKeyKey))
                account.SerialKey = (string)map[SerialKeyKey];
            if (map.ContainsKey(TimeOffsetKey))
                account.TimeOffset = Convert.ToInt64(map[TimeOffsetKey]);
            if (map.ContainsKey(SecretKeyKey))
                account.SecretKey = (string)map[SecretKeyKey];
            account.SecretKey = accountManager.SecretKeyEncryption.Decrypt(account, account.SecretKey);
        }

        public static Dictionary<string, object> GetMap(IAccountManager accountManager, IAccount account)
        {
            var map = new Dictionary<string, object>()
            {
                { DescriptionKey, account.Description ?? string.Empty },
                { DeviceIdKey, account.DeviceId ?? string.Empty },
                { SerialKeyKey, account.SerialKey ?? string.Empty },
                { TimeOffsetKey, account.TimeOffset },
                { SecretKeyKey, accountManager.SecretKeyEncryption.Encrypt(account, account.SecretKey ?? string.Empty) },
            };
            return map;
        }
    }
}
