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
    class Help : ICommand
    {
        private static string[] commands = new string[] {
            "help",
            "h",
        };
        private static NDesk.Options.OptionSet commandOptionSet = new NDesk.Options.OptionSet
        {
        };

        public string[] Commands
        {
            get { return commands; }
        }

        public string Description
        {
            get { return "Show help for a command."; }
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
            if (remainingArgs.Count != 1)
                throw new CommandArgumentException(this, string.Format("Unknown arguments found: {0}", string.Join(" ", remainingArgs.ToArray())));

            var cmdName = remainingArgs[0];
            ICommand cmd = null;
            foreach (var supportedCommand in Program.SupportedCommands)
            {
                for (int i = 0; i != supportedCommand.Commands.Length; ++i)
                {
                    if (supportedCommand.Commands[i] == cmdName)
                    {
                        cmd = supportedCommand;
                        break;
                    }
                }
            }
            if (cmd == null)
                throw new CommandArgumentException(this, string.Format("No valid command specified: {0}", cmdName));
            Program.ShowMainHelp(HelpMessageParts.GlobalOptions);
            Console.WriteLine();
            Console.WriteLine("Options for command {0} ({1});", cmd.Commands[0], cmd.Description);
            cmd.OptionSet.WriteOptionDescriptions(Console.Out);
        }
    }
}
