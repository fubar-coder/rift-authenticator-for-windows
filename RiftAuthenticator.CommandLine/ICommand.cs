using System;
using System.Collections.Generic;
using System.Text;

namespace RiftAuthenticator.CommandLine
{
    interface ICommand
    {
        string[] Commands { get; }
        string Description { get; }
        void UpdateOptions(NDesk.Options.OptionSet optionSet);
        void Execute(string[] args);
    }
}
