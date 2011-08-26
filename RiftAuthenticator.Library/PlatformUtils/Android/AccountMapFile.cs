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

namespace RiftAuthenticator.Library.PlatformUtils.Android
{
    public abstract class AccountMapFile : AccountBase
    {
        protected abstract string GetFileName(IAccountManager accountManager, int accountIndex);
        protected abstract Dictionary<string, object> ReadMapFile(string fileName);
        protected abstract void WriteMapFile(string fileName, Dictionary<string, object> map);

        public override void Load(IAccountManager accountManager, int accountIndex)
        {
            var configFileName = GetFileName(accountManager, accountIndex);
            var map = ReadMapFile(configFileName);
            AccountMap.SetMap(accountManager, this, map);
        }

        public override void Save(IAccountManager accountManager, int accountIndex)
        {
            var configFileName = GetFileName(accountManager, accountIndex);
            var map = AccountMap.GetMap(accountManager, this);
            WriteMapFile(configFileName, map);
        }
    }
}
