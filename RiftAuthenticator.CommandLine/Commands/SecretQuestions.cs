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
