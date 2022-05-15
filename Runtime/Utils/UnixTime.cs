// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System;

#endregion

namespace Amax
{

    public static class UnixTime 
    {

		public const int SecondsInMinute = 60;
		public const int SecondsInHour = 3600;
		public const int SecondsInDay = 86400;

        private static DateTime _unixTimeStartPoint = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        
        public static long Now => (long)(DateTime.UtcNow.Subtract(_unixTimeStartPoint)).TotalSeconds;

        public static long ToUnixTime(DateTime time, bool isArgTimeUniversal = false) 
        {
            if (isArgTimeUniversal)
            {
                return (long)(time.Subtract(_unixTimeStartPoint)).TotalSeconds;
            }
			return (long)(time.ToUniversalTime().Subtract(_unixTimeStartPoint)).TotalSeconds;
		}

		public static long LengthsInSeconds(int minutes, int hours, int days, int seconds)
			=> minutes * SecondsInMinute + hours * SecondsInHour + days * SecondsInDay + seconds;

		public static long AddToCurrentUnixTime(int days, int hours, int minutes = 0, int seconds = 0)
			=> Now + LengthsInSeconds (minutes, hours, days, seconds);
		
		public static DateTime ToLocalDateTime(long unixTime)
			=> _unixTimeStartPoint.AddSeconds(unixTime).ToLocalTime();

        public static DateTime ToUniversalDateTime(long unixTime)
	        => _unixTimeStartPoint.AddSeconds(unixTime).ToUniversalTime();

	}

}

