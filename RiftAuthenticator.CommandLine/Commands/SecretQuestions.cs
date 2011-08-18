/**
 * This file is part of RIFT Authenticator for Windows.
 *
 * RIFT Authenticator for Windows is free software: you can redistribute 
 * it and/or modify it under the terms of the GNU General Public License 
 * as published by the Free Software Foundation, either version 3 of the 
 * License, or (at your option) any later version.
 *
 * RIFT Authenticator for Windows is distributed in the hope that it will 
 * be useful, but WITHOUT ANY WARRANTY; without even the implied warranty 
 * of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU 
 * General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with RIFT Authenticator for Windows.  If not, see 
 * <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace RiftAuthenticator.CommandLine.Commands
{
    class SecretQuestions : ICommand
    {
        private static string[] commands = new string[] {
            "secret-questions",
            "s",
            "sc",
        };
        private NDesk.Options.OptionSet commandOptionSet;

        private string userName;
        private string password;

        public SecretQuestions()
        {
            commandOptionSet = new NDesk.Options.OptionSet
            {
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
            get { return "Show the users secret question(s)"; }
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
            if (string.IsNullOrEmpty(userName))
                throw new CommandArgumentException(this, "No email address (user name) for login specified");
            if (string.IsNullOrEmpty(password))
                throw new CommandArgumentException(this, "No password for login specified");
            var securityQuestions = Library.TrionServer.GetSecurityQuestions(userName, password);
            for (int i = 0; i != securityQuestions.Length; ++i)
            {
                Console.Out.WriteLine("Security question {0}: {1}", i + 1, securityQuestions[i]);
            }
        }
    }
}
