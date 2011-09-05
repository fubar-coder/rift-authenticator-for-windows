/**
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

namespace RiftAuthenticator.Library.FileSystem
{
    public class Account : RiftAuthenticator.Library.PlatformUtils.Android.AccountMapFile
    {
        internal static string GetAccountFileName(int accountIndex)
        {
            return string.Format("Account {0}.xml", accountIndex + 1);
        }

        internal static string GetAccountFolder()
        {
            var appSettings = System.Configuration.ConfigurationManager.AppSettings;
            var result = appSettings["account-manager-fs-path"];
            if (string.IsNullOrEmpty(result))
                result = "default";
            switch (result)
            {
                case "default":
                case "user":
                case "user-profile":
                    result = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "RiftAuthenticator");
                    break;
                case "app":
                case "app-folder":
                case "application":
                case "application-folder":
                    result = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "Data");
                    break;
                default:
                    break;
            }
            result = Environment.ExpandEnvironmentVariables(result);
            System.IO.Directory.CreateDirectory(result);
            return result;
        }

        internal static string GetAccountPath(int accountIndex)
        {
            return System.IO.Path.Combine(GetAccountFolder(), GetAccountFileName(accountIndex));
        }

        internal static Dictionary<string, object> ReadMap(string fileName)
        {
            if (System.IO.File.Exists(fileName))
            {
                using (var stream = new System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                {
                    return PlatformUtils.Android.MapFile.ReadMap(stream);
                }
            }
            else
            {
                return new Dictionary<string, object>();
            }
        }

        internal static void WriteMap(string fileName, Dictionary<string, object> map)
        {
            using (var stream = new System.IO.FileStream(fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write))
            {
                PlatformUtils.Android.MapFile.WriteMap(stream, map);
            }
        }

        protected override string GetFileName(IAccountManager accountManager, int accountIndex)
        {
            return GetAccountPath(accountIndex);
        }

        protected override Dictionary<string, object> ReadMapFile(string fileName)
        {
            return ReadMap(fileName);
        }

        protected override void WriteMapFile(string fileName, Dictionary<string, object> map)
        {
            WriteMap(fileName, map);
        }
    }
}
