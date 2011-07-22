﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RiftAuthenticator
{
    class TrionServerException : ApplicationException
    {
        private static string ErrorCodeToMessage(string errorCode)
        {
            switch (errorCode)
            {
                case "account_not_available":
                    return "Invalid user name or password.";
                case "account_missing":
                    return "No RIFT-Account or Device ID doesn't match.";
                case "account_securityAnswers_incorrect":
                    return "Security answer(s) incorrect.";
            }
            return string.Format("Unknown error code: {0}", errorCode);
        }

        public TrionServerException(string errorCode)
            : base(ErrorCodeToMessage(errorCode))
        {
        }
    }
}
