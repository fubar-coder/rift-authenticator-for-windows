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

namespace RiftAuthenticator.CommandLine.Commands
{
    class NewAccount : ICommand
    {
        private static string[] commands = new string[] {
            "new-account",
            "n",
        };
        private NDesk.Options.OptionSet commandOptionSet;

        private string accountName = null;

        public NewAccount()
        {
            commandOptionSet = new NDesk.Options.OptionSet
            {
                { "n|name=", Resources.Strings.opt_new_account_name, x => accountName = x },
            };
        }

        public string[] Commands
        {
            get { return commands; }
        }

        public string Description
        {
            get { return Resources.Strings.opt_new_account_description; }
        }

        public NDesk.Options.OptionSet OptionSet
        {
            get
            {
                return commandOptionSet;
            }
        }

        public void Execute(GlobalOptions globalOptions, string[] args)
        {
            var remainingArgs = OptionSet.Parse(args);
            if (remainingArgs.Count != 0)
                throw new CommandArgumentException(this, string.Format(Resources.Strings.app_unknown_args, string.Join(" ", remainingArgs.ToArray())));
            if (!globalOptions.Account.IsEmpty || !string.IsNullOrEmpty(globalOptions.Account.Description))
            {
                globalOptions.Account = globalOptions.AccountManager.CreateAccount();
                globalOptions.AccountManager.Add(globalOptions.Account);
            }
            var newAccountName = accountName ?? string.Format(Resources.Strings.opt_new_account_default_name, globalOptions.AccountManager.IndexOf(globalOptions.Account) + 1);
            globalOptions.Account.Description = newAccountName;
            globalOptions.AccountManager.SaveAccounts();
        }
    }
}
