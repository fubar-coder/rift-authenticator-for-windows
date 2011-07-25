using System;
using System.Collections.Generic;
using System.Text;

namespace RiftAuthenticator.CommandLine
{
    interface ICommand
    {
        string[] Commands { get; }
        string Description { get; }
        NDesk.Options.OptionSet OptionSet { get; }
        void Execute(GlobalOptions globalOptions, string[] args);
    }
}
