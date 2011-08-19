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
    class Recover : ICommand
    {
        private static string[] commands = new string[] {
            "recover",
            "r",
        };
        private NDesk.Options.OptionSet commandOptionSet;

        private string userName;
        private string password;
        private string deviceId = Library.TrionServer.GetOrCreateRandomDeviceId();

        public Recover()
        {
            commandOptionSet = new NDesk.Options.OptionSet
            {
                { "d|device-id=", "Manually specify a device id", x => deviceId = x },
                { "u|e|email|user|user-name=", "Email address used for login", x => userName = x },
                { "p|password=", "Password used for login", x => password = x },
            };
        }

        public string[] Commands
        {
            get { return commands; }
        }

        public string Description
        {
            get { return "Recover the authenticators configuration"; }
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
            if (string.IsNullOrEmpty(userName))
                throw new CommandArgumentException(this, "No email address (user name) for login specified");
            if (string.IsNullOrEmpty(password))
                throw new CommandArgumentException(this, "No password for login specified");
            var securityQuestions = Library.TrionServer.GetSecurityQuestions(userName, password);
            var securityAnswers = new string[securityQuestions.Length];
            var argIndex = 0;
            for (int i = 0; i != securityQuestions.Length; ++i)
            {
                if (!string.IsNullOrEmpty(securityQuestions[i]))
                {
                    if (argIndex >= remainingArgs.Count)
                        throw new CommandArgumentException(this, string.Format("No answer for security question {0} ({1}) given", i + 1, securityQuestions[i]));
                    securityAnswers[i] = remainingArgs[argIndex++];
                }
            }
            if (argIndex < remainingArgs.Count)
                throw new CommandArgumentException(this, string.Format("Unknown arguments found: {0}", string.Join(" ", remainingArgs.ToArray())));
            var cfg = new Library.Configuration
            {
                DeviceId = deviceId,
            };
            Library.TrionServer.RecoverSecurityKey(userName, password, securityAnswers, cfg);
            cfg.TimeOffset = Library.TrionServer.GetTimeOffset();
            cfg.Save();
            globalOptions.Configuration.Load();
            Program.ShowConfiguration(globalOptions.Configuration);
        }
    }
}
