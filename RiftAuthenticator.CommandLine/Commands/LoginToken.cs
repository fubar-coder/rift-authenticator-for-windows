using System;
using System.Collections.Generic;
using System.Text;

namespace RiftAuthenticator.CommandLine.Commands
{
    class LoginToken : ICommand
    {
        private static string[] commands = new string[] {
            "login-token",
            "l",
            "lt",
        };
        private NDesk.Options.OptionSet commandOptionSet;

        public LoginToken()
        {
            commandOptionSet = new NDesk.Options.OptionSet
            {
            };
        }

        public string[] Commands
        {
            get { return commands; }
        }

        public string Description
        {
            get { return "Show current login token"; }
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
            var loginToken = globalOptions.Configuration.CalculateToken();
            Console.Out.WriteLine("Login token: {0}", loginToken.Token);
            Console.Out.WriteLine("Remaining time (ms): {0}", loginToken.RemainingMillis);
        }
    }
}
