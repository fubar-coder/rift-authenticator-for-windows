/**
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
