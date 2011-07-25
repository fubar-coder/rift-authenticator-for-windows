using System;
using System.Collections.Generic;
using System.Text;

namespace RiftAuthenticator.CommandLine
{
    class Program
    {
        static GlobalOptions GlobalOptions = new GlobalOptions();
        internal static List<ICommand> SupportedCommands = new List<ICommand>()
        {
            new Commands.Help(),
            new Commands.TimeSync(),
        };

        static NDesk.Options.OptionSet GlobalOptionSet = new NDesk.Options.OptionSet
        {
            { "h|help", "Show help", x => GlobalOptions.ShowHelp = x != null },
            { "v|verbose", "Set verbose level", x => GlobalOptions.VerboseLevel += (x==null ? -1 : 1) },
        };

        static int Main(string[] args)
        {
            try
            {
                var unknownArgs = GlobalOptionSet.Parse(args);
                if (unknownArgs.Count == 0)
                    GlobalOptions.ShowHelp = true;
                if (GlobalOptions.ShowHelp)
                {
                    ShowMainHelp(HelpMessageParts.All);
                }
                else
                {
                    ProcessCommand(unknownArgs);
                }
            }
            catch (CommandArgumentException ex)
            {
                ShowMainHelp(HelpMessageParts.None);
                Console.Error.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return 1;
            }
            return 0;
        }

        private static void ProcessCommand(List<string> args)
        {
            string cmdName = null;
            for (int i = 0; i != args.Count; ++i)
            {
                var arg = args[i];
                if (!arg.StartsWith("-") && !arg.StartsWith("/"))
                {
                    cmdName = arg;
                    args.RemoveAt(i);
                    break;
                }
            }
            if (cmdName == null)
                throw new ApplicationException("No command name specified.");
            ICommand cmd = null;
            foreach (var supportedCommand in SupportedCommands)
            {
                for (int i = 0; i != supportedCommand.Commands.Length; ++i)
                {
                    if (supportedCommand.Commands[i] == cmdName)
                    {
                        cmd = supportedCommand;
                        break;
                    }
                }
            }
            if (cmd == null)
                throw new ApplicationException(string.Format("No valid command specified: {0}", cmdName));
            cmd.Execute(GlobalOptions, args.ToArray());
        }

        internal static void ShowMainHelp(HelpMessageParts parts)
        {
            var asm = System.Reflection.Assembly.GetEntryAssembly();
            var appName = System.IO.Path.GetFileNameWithoutExtension(asm.Location);
            Console.WriteLine("{0} [global-options] <command-name> [command-options]", appName);
            if ((parts & HelpMessageParts.GlobalOptions) != HelpMessageParts.None)
            {
                Console.WriteLine();
                Console.WriteLine("Global options:");
                GlobalOptionSet.WriteOptionDescriptions(Console.Out);
            }
            if ((parts & HelpMessageParts.CommandList) != HelpMessageParts.None)
            {
                Console.WriteLine();
                Console.WriteLine("Commands:");
                foreach (var cmd in SupportedCommands)
                    Console.WriteLine("{0}\t{1}", cmd.Commands[0], cmd.Description);
            }
        }
    }
}
