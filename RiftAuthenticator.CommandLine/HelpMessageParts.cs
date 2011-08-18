using System;
using System.Collections.Generic;
using System.Text;

namespace RiftAuthenticator.CommandLine
{
    [Flags]
    enum HelpMessageParts
    {
        None = 0x00,
        GlobalOptions = 0x01,
        CommandList = 0x02,
        All = 0x0F,
    }
}
