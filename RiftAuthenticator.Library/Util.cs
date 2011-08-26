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
using System.Text;

namespace RiftAuthenticator.Library
{
    public static class Util
    {
        public static long MillisStartTicks = new System.DateTime(1970, 1, 1).Ticks;

        public static long CurrentTimeMillis()
        {
            return TimeToMillis(DateTime.Now);
        }

        public static long TicksToMillis(long ticks)
        {
            var timeStart = MillisStartTicks;
            var timeDiff = ticks - timeStart;
            return timeDiff / 10000L;
        }

        public static long MillisToTicks(long millis)
        {
            var timeStart = MillisStartTicks;
            var timeDiff = millis * 10000L;
            var timeCurrent = timeDiff + timeStart;
            return timeCurrent;
        }

        public static long TimeToMillis(DateTime time)
        {
            return TicksToMillis(time.ToUniversalTime().Ticks);
        }

        public static DateTime MillisToTime(long millis)
        {
            return new DateTime(MillisToTicks(millis), DateTimeKind.Utc).ToLocalTime();
        }

        internal static byte[] HexToBytes(string hexString)
        {
            System.Diagnostics.Debug.Assert((hexString.Length & 1) == 0);
            var result = new byte[hexString.Length / 2];
            var resultIndex = 0;
            for (int i = 0; i != hexString.Length; i += 2)
                result[resultIndex++] = byte.Parse(hexString.Substring(i, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
            return result;
        }

        internal static string BytesToHex(byte[] bytes)
        {
            var result = new StringBuilder();
            for (int i = 0; i != bytes.Length; ++i)
                result.AppendFormat("{0:X02}", bytes[i]);
            return result.ToString();
        }
    }
}
