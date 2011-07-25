using System;
using System.Collections.Generic;
using System.Text;

namespace RiftAuthenticator.CommandLine.Commands
{
    class TimeSync : ICommand
    {
        private static string[] commands = new string[] {
            "t",
            "ts",
            "time-sync",
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
            get { return "Time synchronization with TRION's login server"; }
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
            globalOptions.Configuration.TimeOffset = Library.TrionServer.GetTimeOffset();
            globalOptions.Configuration.Save();
        }
    }
}
