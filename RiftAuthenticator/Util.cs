/**
 * This file is part of RIFT Authenticator for Windows.
 *
 * RIFT Authenticator for Windows is free software: you can redistribute 
 * it and/or modify it under the terms of the GNU General Public License 
 * as published by the Free Software Foundation, either version 3 of the 
 * License, or (at your option) any later version.
 *
 * RIFT Authenticator for Windows is distributed in the hope that it will 
 * be useful, but WITHOUT ANY WARRANTY; without even the implied warranty 
 * of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU 
 * General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with RIFT Authenticator for Windows.  If not, see 
 * <http://www.gnu.org/licenses/>.
 */

using System;

namespace RiftAuthenticator
{
    public static class Util
    {
        public static long MillisStartTicks = new System.DateTime(1970, 1, 1).Ticks;

        public static long currentTimeMillis()
        {
            return timeToMillis(DateTime.Now);
        }

        public static long ticksToMillis(long ticks)
        {
            var timeStart = MillisStartTicks;
            var timeDiff = ticks - timeStart;
            return timeDiff / 10000L;
        }

        public static long millisToTicks(long millis)
        {
            var timeStart = MillisStartTicks;
            var timeDiff = millis * 10000L;
            var timeCurrent = timeDiff + timeStart;
            return timeCurrent;
        }

        public static long timeToMillis(DateTime time)
        {
            return ticksToMillis(time.ToUniversalTime().Ticks);
        }

        public static DateTime millisToTime(long millis)
        {
            return new DateTime(millisToTicks(millis), DateTimeKind.Utc).ToLocalTime();
        }
    }
}
