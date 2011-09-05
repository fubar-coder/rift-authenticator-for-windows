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
    class Init : ICommand
    {
        private static string[] commands = new string[] {
            "init",
            "i",
        };
        private NDesk.Options.OptionSet commandOptionSet;

        private bool forceOverwriteConfig = false;
        private string deviceId = Library.TrionServer.GetOrCreateRandomDeviceId();

        public Init()
        {
            commandOptionSet = new NDesk.Options.OptionSet
            {
                { "f|force", Resources.Strings.opt_init_opt_force, x => forceOverwriteConfig = x != null },
                { "d|device-id=", Resources.Strings.opt_init_opt_device_id, x => deviceId = x },
            };
        }

        public string[] Commands
        {
            get { return commands; }
        }

        public string Description
        {
            get { return Resources.Strings.opt_init_description; }
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
            if (!globalOptions.Account.IsEmpty && !forceOverwriteConfig)
                throw new CommandArgumentException(this, Resources.Strings.opt_init_config_found);
            var ar = Library.TrionServer.BeginCreateSecurityKey(null, null, globalOptions.Account, deviceId);
            ar.AsyncWaitHandle.WaitOne();
            Library.TrionServer.EndCreateSecurityKey(ar);
            ar = Library.TrionServer.BeginGetTimeOffset(null, null);
            ar.AsyncWaitHandle.WaitOne();
            globalOptions.Account.TimeOffset = Library.TrionServer.EndGetTimeOffset(ar);
            globalOptions.AccountManager.SaveAccounts();
            Program.ShowConfiguration(globalOptions.Account);
        }
    }
}
