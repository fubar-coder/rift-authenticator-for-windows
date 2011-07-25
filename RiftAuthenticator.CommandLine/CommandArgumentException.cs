using System;
using System.Collections.Generic;
using System.Text;

namespace RiftAuthenticator.CommandLine
{
    class CommandArgumentException : ApplicationException
    {
        public ICommand Command { get; private set; }

        public CommandArgumentException(ICommand command, string message)
            : base(message)
        {
            Command = command;
        }
    }
}
