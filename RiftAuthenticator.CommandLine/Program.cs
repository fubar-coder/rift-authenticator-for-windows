﻿/**
 * This file is part of RIFT™ Authenticator for Windows.
 *
 * RIFT™ Authenticator for Windows is free software: you can redistribute 
 * it and/or modify it under the terms of the GNU General Public License 
 * as published by the Free Software Foundation, either version 3 of the 
 * License, or (at your option) any later version.
 *
 * RIFT™ Authenticator for Windows is distributed in the hope that it will 
 * be useful, but WITHOUT ANY WARRANTY; without even the implied warranty 
 * of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU 
 * General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with RIFT™ Authenticator for Windows.  If not, see 
 * <http://www.gnu.org/licenses/>.
 */

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
            new Commands.NewAccount(),
            new Commands.DeleteAccount(),
            new Commands.Info(),
            new Commands.Init(),
            new Commands.LoginToken(),
            new Commands.SecretQuestions(),
            new Commands.Recover(),
            new Commands.TimeSync(),
        };

        static NDesk.Options.OptionSet GlobalOptionSet = new NDesk.Options.OptionSet
        {
            { "h|help", Resources.Strings.opt_global_help, x => GlobalOptions.ShowHelp = x != null },
            { "v|verbose", Resources.Strings.opt_global_verbose, x => GlobalOptions.VerboseLevel += (x==null ? -1 : 1) },
            { "m|manager|account-manager=", Resources.Strings.opt_global_account_manager, x => GlobalOptions.AccountManagerId = x },
            { "a|account=", Resources.Strings.opt_global_account, x => GlobalOptions.AccountId = x },
            { "platform=", Resources.Strings.opt_global_platform, x => GlobalOptions.AccountId = x },
        };

        static int Main(string[] args)
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = Library.TrionServer.CertificateIsValid;
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
                    if (!string.IsNullOrEmpty(GlobalOptions.PlatformId))
                        SelectPlatform(GlobalOptions.PlatformId);
                    if (!string.IsNullOrEmpty(GlobalOptions.AccountManagerId))
                        SelectAccountManager(GlobalOptions.AccountManagerId);
                    if (!string.IsNullOrEmpty(GlobalOptions.AccountId))
                        SelectAccount(GlobalOptions.AccountId);
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
                Console.Error.WriteLine(ex.ToString());
                return 1;
            }
            return 0;
        }

        private static void SelectPlatform(string platformId)
        {
            Library.TrionServer.Platform = Library.PlatformBase.LoadPlatform(platformId);
        }

        static void SelectAccount(string accountId)
        {
            GlobalOptions.Account = GlobalOptions.AccountManager.FindAccount(accountId);
        }

        static void SelectAccountManager(string accountManagerId)
        {
            GlobalOptions.AccountManager = Library.AccountManagerBase.LoadAccountManager(accountManagerId);
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
                throw new ApplicationException(Resources.Strings.app_no_command);
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
                throw new ApplicationException(string.Format(Resources.Strings.app_invalid_command, cmdName));
            cmd.Execute(GlobalOptions, args.ToArray());
        }

        internal static void ShowMainHelp(HelpMessageParts parts)
        {
            var asm = System.Reflection.Assembly.GetEntryAssembly();
            var appName = System.IO.Path.GetFileNameWithoutExtension(asm.Location);
            Console.WriteLine(Resources.Strings.app_command_line, appName);
            if ((parts & HelpMessageParts.GlobalOptions) != HelpMessageParts.None)
            {
                Console.WriteLine();
                Console.WriteLine(Resources.Strings.app_opt_global);
                GlobalOptionSet.WriteOptionDescriptions(Console.Out);
            }
            if ((parts & HelpMessageParts.CommandList) != HelpMessageParts.None)
            {
                Console.WriteLine();
                Console.WriteLine(Resources.Strings.app_commands);
                foreach (var cmd in SupportedCommands)
                    Console.WriteLine("{0,-20}\t{1}", cmd.Commands[0], cmd.Description);
            }
        }

        internal static void ShowConfiguration(Library.IAccount account)
        {
            Console.Out.WriteLine(Resources.Strings.app_account_n_of_m, GlobalOptions.AccountManager.IndexOf(account) + 1, GlobalOptions.AccountManager.Count);
            if (account.IsEmpty && string.IsNullOrEmpty(account.Description))
            {
                Console.Out.WriteLine(Resources.Strings.app_no_config);
            }
            else
            {
                Console.Out.WriteLine(Resources.Strings.app_info_description, account.Description);
                Console.Out.WriteLine(Resources.Strings.app_info_device_id, account.DeviceId);
                Console.Out.WriteLine(Resources.Strings.app_info_serial_key, account.FormattedSerialKey);
                Console.Out.WriteLine(Resources.Strings.app_info_encrypted_secret_key, Library.TrionServer.SecretKeyEncryption.Encrypt(account, account.SecretKey));
                Console.Out.WriteLine(Resources.Strings.app_info_time_offset, account.TimeOffset);
            }
        }
    }
}
