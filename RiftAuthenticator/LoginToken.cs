using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RiftAuthenticator
{
    class LoginToken
    {
        public LoginToken(string token, long remainingMillis)
        {
            Token = token;
            RemainingMillis = remainingMillis;
        }

        public string Token { get; private set; }
        public long RemainingMillis { get; private set; }
    }
}
