using System;
using System.Collections.Generic;
using System.Text;

namespace RiftAuthenticator.CommandLine.Commands
{
    class Info : ICommand
    {
        private static string[] commands = new string[] {
            "info",
        };
        private NDesk.Options.OptionSet commandOptionSet;

        public Info()
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
            get { return "Show current configuration"; }
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
            Program.ShowConfiguration(globalOptions.Configuration);
        }
    }
}
