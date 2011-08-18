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
