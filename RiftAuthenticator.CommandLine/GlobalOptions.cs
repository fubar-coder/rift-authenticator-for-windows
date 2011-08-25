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

namespace RiftAuthenticator.CommandLine
{
    class GlobalOptions
    {
        private RiftAuthenticator.Library.IAccountManager _accountManager;

        public GlobalOptions()
        {
            // Load default configuration
            AccountManagerId = "RiftAuthenticator.Library.Registry";
        }

        public bool ShowHelp { get; set; }
        public int VerboseLevel { get; set; }

        public string AccountManagerId { get; set; }
        public string AccountId { get; set; }

        public RiftAuthenticator.Library.IAccountManager AccountManager
        {
            get
            {
                return _accountManager;
            }
            set
            {
                _accountManager = value;
                AccountManager.LoadAccounts();
                if (AccountManager.Count == 0)
                    AccountManager.Add(AccountManager.CreateAccount());
                Account = AccountManager[0];
            }
        }
        public RiftAuthenticator.Library.IAccount Account { get; set; }
    }
}
