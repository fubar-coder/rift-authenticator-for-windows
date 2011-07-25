using System;
using System.Collections.Generic;
using System.Text;

namespace RiftAuthenticator.CommandLine
{
    class GlobalOptions
    {
        public GlobalOptions()
        {
            // Load default configuration
            Configuration = new Library.Configuration();
            Configuration.Load();
        }

        public bool ShowHelp { get; set; }
        public int VerboseLevel { get; set; }

        public RiftAuthenticator.Library.Configuration Configuration { get; set; }
    }
}
