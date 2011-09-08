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

namespace RiftAuthenticator.Library.IsolatedStorage
{
    public class Account : PlatformUtils.Android.AccountMapFile
    {
        private static System.IO.IsolatedStorage.IsolatedStorageFile CreateIsolatedStorageFile()
        {
#if SILVERLIGHT
            return System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForApplication();
#else
            return System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForDomain();
#endif
        }

        internal static bool HasFile(System.IO.IsolatedStorage.IsolatedStorageFile storage, string fileName)
        {
            var fileNames = storage.GetFileNames();
            int foundFileNameIndex = -1;
            for (int i = 0; i != fileNames.Length; ++i)
            {
                if (fileNames[i] == fileName)
                {
                    foundFileNameIndex = i;
                    break;
                }
            }
            return foundFileNameIndex != -1;
        }

        internal static void ReadMap(string fileName, Dictionary<string, object> map)
        {
            using (var storage = CreateIsolatedStorageFile())
            {
                if (HasFile(storage, fileName))
                {
                    using (var stream = new System.IO.IsolatedStorage.IsolatedStorageFileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read, storage))
                    {
                        PlatformUtils.Android.MapFile.ReadMap(stream, map);
                        stream.Close();
                    }
                }
            }
        }

        internal static Dictionary<string, object> ReadMap(string fileName)
        {
            var result = new Dictionary<string, object>();
            ReadMap(fileName, result);
            return result;
        }

        internal static void WriteMap(string fileName, Dictionary<string, object> map)
        {
            using (var storage = CreateIsolatedStorageFile())
            {
                using (var stream = new System.IO.IsolatedStorage.IsolatedStorageFileStream(fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write, storage))
                {
                    PlatformUtils.Android.MapFile.WriteMap(stream, map);
                    stream.Close();
                }
            }
        }

        protected override Dictionary<string, object> ReadMapFile(string fileName)
        {
            return ReadMap(fileName);
        }

        protected override void WriteMapFile(string fileName, Dictionary<string, object> map)
        {
            WriteMap(fileName, map);
        }

        protected override string GetFileName(IAccountManager accountManager, int accountIndex)
        {
            return string.Format("account_{0}.xml", accountIndex + 1);
        }
    }
}
