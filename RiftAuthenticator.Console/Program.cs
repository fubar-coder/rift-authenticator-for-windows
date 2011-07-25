using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RiftAuthenticator.CommandLine
{
    class Program
    {
        static int Main(string[] args)
        {
            try
            {
                if (args.Length == 0)
                {
                    ShowMainHelp();
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return 1;
            }
            return 0;
        }

        private static void ShowMainHelp()
        {
            var asm = System.Reflection.Assembly.GetEntryAssembly();
            var appName = asm.FullName;
            Console.Out.WriteLine("{0} [global-options] <command-name> [command-options]", appName);
        }
    }
}
