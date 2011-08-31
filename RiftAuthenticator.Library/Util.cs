/*
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
    /// <summary>
    /// Utility functions
    /// </summary>
    public static class Util
    {
        /// <summary>
        /// The ticks for the date 1970-01-01
        /// </summary>
        private static long MillisStartTicks = new System.DateTime(1970, 1, 1).Ticks;

        /// <summary>
        /// Get the current time in milliseconds
        /// </summary>
        /// <returns>Current time in milliseconds</returns>
        public static long CurrentTimeMillis()
        {
            return TimeToMillis(DateTime.Now);
        }

        private static long TicksToMillis(long ticks)
        {
            var timeStart = MillisStartTicks;
            var timeDiff = ticks - timeStart;
            return timeDiff / 10000L;
        }

        /// <summary>
        /// Convert the given timestamp to milliseconds since 1970-01-01
        /// </summary>
        /// <param name="time">The timestamp to convert to milliseconds</param>
        /// <returns>Returns the timestamp in milliseconds sinze 1970-01-01</returns>
        public static long TimeToMillis(DateTime time)
        {
            return TicksToMillis(time.ToUniversalTime().Ticks);
        }

        /// <summary>
        /// Converts a HEX string to a byte array
        /// </summary>
        /// <param name="hexString">The HEX string to convert</param>
        /// <returns>The byte array created from the HEX string</returns>
        public static byte[] HexToBytes(string hexString)
        {
            System.Diagnostics.Debug.Assert((hexString.Length & 1) == 0);
            var result = new byte[hexString.Length / 2];
            var resultIndex = 0;
            for (int i = 0; i != hexString.Length; i += 2)
                result[resultIndex++] = byte.Parse(hexString.Substring(i, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
            return result;
        }

        /// <summary>
        /// Converts a byte array to a HEX string
        /// </summary>
        /// <param name="bytes">Array of bytes to convert</param>
        /// <returns>The resulting HEX string</returns>
        public static string BytesToHex(byte[] bytes)
        {
            var result = new StringBuilder();
            for (int i = 0; i != bytes.Length; ++i)
                result.AppendFormat("{0:X02}", bytes[i]);
            return result.ToString();
        }
    }
}
