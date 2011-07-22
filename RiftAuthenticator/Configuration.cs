using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RiftAuthenticator
{
    class Configuration
    {
        static readonly System.Text.Encoding Encoding = System.Text.Encoding.Default;

        const string DeviceIdKey = "DeviceId";
        const string SerialKeyKey = "SerialKey";
        const string SecretKeyKey = "SecretKey";
        const string TimeOffsetKey = "TimeOffset";

        public string DeviceId { get; set; }
        public string SerialKey { get; set; }
        public string SecretKey { get; set; }
        public long TimeOffset { get; set; }

        public string FormattedSerialKey
        {
            get
            {
                var result = new StringBuilder();
                for (int i = 0; i < SerialKey.Length; i += 4)
                {
                    var remaining = Math.Min(SerialKey.Length - i, 4);
                    if (i != 0)
                        result.Append("-");
                    result.Append(SerialKey.Substring(i, remaining));
                }
                return result.ToString();
            }
        }

        public bool IsEmpty
        {
            get
            {
                return string.IsNullOrEmpty(DeviceId);
            }
        }

        string AccountRegistryPath
        {
            get
            {
                return "SOFTWARE\\Public Domain\\Rift Authenticator\\Account 1";
            }
        }

        public void Load()
        {
            using (var key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(AccountRegistryPath))
            {
                DeviceId = (string)key.GetValue(DeviceIdKey, string.Empty);
                SerialKey = (string)key.GetValue(SerialKeyKey, string.Empty);
                SecretKey = (string)key.GetValue(SecretKeyKey, string.Empty);
                TimeOffset = Convert.ToInt64(key.GetValue(TimeOffsetKey, 0));
            }
        }

        public void Save()
        {
            using (var key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(AccountRegistryPath))
            {
                key.SetValue(DeviceIdKey, DeviceId ?? string.Empty);
                key.SetValue(SerialKeyKey, SerialKey ?? string.Empty);
                key.SetValue(SecretKeyKey, SecretKey ?? string.Empty);
                key.SetValue(TimeOffsetKey, TimeOffset);
            }
        }

        public LoginToken CalculateToken()
        {
            var secretKey = Encoding.GetBytes(SecretKey);
            var now = DateTime.Now;
            var nowMillis = Util.timeToMillis(now);
            long intervalNumber = (nowMillis + TimeOffset) / 30000;
            // Value must be written as big endian byte buffer
            var intervalByteBuffer = Mono.DataConverter.Pack("^l", intervalNumber);

            var digest = new System.Security.Cryptography.HMACSHA1(secretKey);
            var hash = digest.ComputeHash(intervalByteBuffer);

            var rawTokenPos = hash[hash.Length - 1] & 0xF;

            // Raw token value must be read as big endian 32 bit integer value
            var rawTokenValue = (long)(uint)Mono.DataConverter.Unpack("^I", hash, rawTokenPos)[0];
            var remainingMillis = ((intervalNumber + 1) * 30000 - TimeOffset) - nowMillis;

            var tokenValue = rawTokenValue % 100000000;
            return new LoginToken(string.Format("{0:D08}", tokenValue), remainingMillis);
        }
    }
}
