using System;
using System.Text;
using UnityEngine;

public static class TimeUtils
{
    public const int DAY_SEC = 86400;
    public const int HOUR_SEC = 3600;
    public const int MIN_SEC = 60;

    private static DateTime _baseTime = new DateTime(2020, 6, 1, 0, 0, 0);

    public static string GetTimeZoneInfo()
    {
        var timeZoneInfo = TimeZoneInfo.Local.BaseUtcOffset;
        string hour = timeZoneInfo.Hours.ToString("D2");
        string min = timeZoneInfo.Minutes.ToString("D2");
        string timeZoneInfoString = timeZoneInfo.Hours >= 0 ? $"+{hour}:{min}" : $"{hour}:{min}";

        return timeZoneInfoString;
    }

    /// <summary>
    /// Offset from Jun 1 2020
    /// </summary>
    public static int GetCurrentTimeInSec()
    {
        TimeSpan timeSpan = DateTime.UtcNow - _baseTime;
        return (int)timeSpan.TotalSeconds;
    }

    public static double GetCurrentTimeInMiliSec()
    {
        TimeSpan timeSpan = DateTime.UtcNow - _baseTime;
        return timeSpan.TotalMilliseconds;
    }

    public static double GetCurrentTimeInCentiSec()
    {
        return GetCurrentTimeInMiliSec() / 10;
    }

    public static string FromSecToDayHourMin(int sec)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(sec / DAY_SEC).Append("d")
        .Append(sec % DAY_SEC / HOUR_SEC).Append("h")
        .Append(sec % DAY_SEC % HOUR_SEC / MIN_SEC).Append("m");
        return sb.ToString();
    }

    /// <summary>
    /// Rounded by largest unit, remaining is ignored
    /// For eg: 90 secs -> 1 min; 30 secs -> 1 min; 3700 secs -> 1 hour; 
    /// </summary>
    public static string FromSecToFlexibleDayHourMin(int sec)
    {
        StringBuilder sb = new StringBuilder();

        int day = sec / DAY_SEC;
        if (day > 0)
        {
            sb.Append(day);
            if (day > 1)
            {
                sb.Append(" days");
            }
            else
            {
                sb.Append(" day");
            }

            return sb.ToString();
        }

        int hour = sec / HOUR_SEC;
        if (hour > 0)
        {
            sb.Append(hour);
            if (hour > 1)
            {
                sb.Append(" hours");
            }
            else
            {
                sb.Append(" hour");
            }

            return sb.ToString();
        }

        int min = sec / MIN_SEC;
        if (sec > 0)
        {
            if (min > 1)
            {
                sb.Append(min).Append(" mins");
            }
            else
            {
                sb.Append("1 min");
            }
            return sb.ToString();
        }
        return "0 min";
    }
}
