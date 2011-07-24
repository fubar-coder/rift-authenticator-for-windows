using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RiftAuthenticator.Console.Commands
{
    class TimeSync : ICommand
    {
        private static string[] commands = new string[] {
            "t",
            "ts",
            "time-sync",
        };

        public string[] Commands
        {
            get { return commands; }
        }

        public string Description
        {
            get { return "Time synchronization with TRION's login server"; }
        }

        public void UpdateOptions(NDesk.Options.OptionSet optionSet)
        {
            throw new NotImplementedException();
        }

        public void Execute(string[] args)
        {
            throw new NotImplementedException();
        }
    }
}
