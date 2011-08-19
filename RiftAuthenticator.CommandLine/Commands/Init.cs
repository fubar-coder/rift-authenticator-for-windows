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
                { "f|force", "Force overwrite of current configuration", x => forceOverwriteConfig = x != null },
                { "d|device-id=", "Manually specify a device id", x => deviceId = x },
            };
        }

        public string[] Commands
        {
            get { return commands; }
        }

        public string Description
        {
            get { return "Initialization of a new authenticator"; }
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
                throw new CommandArgumentException(this, string.Format("Unknown arguments found: {0}", string.Join(" ", remainingArgs.ToArray())));
            if (!globalOptions.Configuration.IsEmpty && !forceOverwriteConfig)
                throw new CommandArgumentException(this, "A configuration already exists, but the -f (force) option wasn't specified.");
            globalOptions.Configuration.DeviceId = deviceId;
            Library.TrionServer.CreateSecurityKey(globalOptions.Configuration);
            globalOptions.Configuration.TimeOffset = Library.TrionServer.GetTimeOffset();
            globalOptions.Configuration.Save();
            Program.ShowConfiguration(globalOptions.Configuration);
        }
    }
}
