/*
 * This file is part of RIFT™ Authenticator for Windows.
 *
 * RIFT™ Authenticator for Windows is free software: you can redistribute 
 * it and/or modify it under the terms of the GNU General Public License 
 * as published by the Free Software Foundation, either version 3 of the 
 * License, or (at your option) any later version.
 *
 * RIFT™ Authenticator for Windows is distributed in the hope that it will 
 * be useful, but WITHOUT ANY WARRANTY; without even the implied warranty 
 * of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU 
 * General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with RIFT™ Authenticator for Windows.  If not, see 
 * <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace RiftAuthenticator.Library
{
    class TrionServerException : Exception
    {
        private static string ErrorCodeToMessage(string errorCode)
        {
            switch (errorCode)
            {
                case "account_not_available":
                    return Resources.Strings.Error_account_not_available;
                case "account_missing":
                    return Resources.Strings.Error_account_missing;
                case "account_securityAnswer_missing":
                    return Resources.Strings.Error_account_securityAnswer_missing;
                case "account_securityAnswers_incorrect":
                    return Resources.Strings.Error_account_securityAnswers_incorrect;
                case "device_id_missing":
                    return Resources.Strings.Error_device_id_missing;
            }
            return string.Format(Resources.Strings.Error_unknown, errorCode);
        }

        public TrionServerException(string errorCode)
            : base(ErrorCodeToMessage(errorCode))
        {
        }
    }
}
