using System;

namespace Chucksoft.Core.Extensions.Time
{
    public static class IntDateExtensions
    {
        /// <summary>
        /// Returns the date the number of years ago
        /// </summary>      
        public static DateTime YearsAgo(this int years)
        {
            return SystemTime.Now().AddYears(-years);
        }
        /// <summary>
        /// Returns the date the number of months ago
        /// </summary>
        public static DateTime MonthsAgo(this int months)
        {
            return SystemTime.Now().AddMonths(-months);
        }
        /// <summary>
        /// Returns the date the number of days ago
        /// </summary>
        public static DateTime DaysAgo(this int days)
        {
            return SystemTime.Now().AddDays(-days);
        }
        /// <summary>
        /// Returns the date the number of hours ago
        /// </summary>
        public static DateTime HoursAgo(this int hours)
        {
            return SystemTime.Now().AddHours(-hours);
        }
        /// <summary>
        /// Returns the date the number of minutes ago
        /// </summary>
        public static DateTime MinutesAgo(this int minutes)
        {
            return SystemTime.Now().AddMinutes(-minutes);
        }
        /// <summary>
        /// Returns the date the seconds of years ago
        /// </summary>
        public static DateTime SecondsAgo(this int seconds)
        {
            return SystemTime.Now().AddSeconds(-seconds);
        }
        /// <summary>
        /// Returns the date the number of years from now
        /// </summary>
        public static DateTime YearsFromNow(this int years)
        {
            return SystemTime.Now().AddYears(years);
        }
        /// <summary>
        /// Returns the date the number of months from now
        /// </summary>
        public static DateTime MonthsFromNow(this int months)
        {
            return SystemTime.Now().AddMonths(months);
        }
        /// <summary>
        /// Returns the date the number of days from now
        /// </summary>
        public static DateTime DaysFromNow(this int days)
        {
            return SystemTime.Now().AddDays(days);
        }
        /// <summary>
        /// Returns the date the number of hours from now
        /// </summary>
        public static DateTime HoursFromNow(this int hours)
        {
            return SystemTime.Now().AddHours(hours);
        }
        /// <summary>
        /// Returns the date the number of minutes from now
        /// </summary>
        public static DateTime MinutesFromNow(this int minutes)
        {
            return SystemTime.Now().AddMinutes(minutes);
        }
        /// <summary>
        /// Returns the date the number of seconds from now
        /// </summary>
        public static DateTime SecondsFromNow(this int seconds)
        {
            return SystemTime.Now().AddSeconds(seconds);
        }

        /// <summary>
        /// Returns true if the given year is a leap year
        /// </summary>
        public static bool IsLeapYear(this int year)
        {
            return year % 4 == 0 && (year % 100 != 0 || year % 400 == 0);
        }

        /// <summary>
        /// Returns a timespan for the number of days
        /// </summary>      
        public static TimeSpan Days(this int days)
        {
            return new TimeSpan(days, 0, 0, 0);
        }
        /// <summary>
        /// Returns a timespan for the number of hours
        /// </summary>
        public static TimeSpan Hours(this int hours)
        {
            return new TimeSpan(0, hours, 0, 0);
        }
        /// <summary>
        /// Returns a timespan for the number of minutes
        /// </summary>
        public static TimeSpan Minutes(this int minutes)
        {
            return new TimeSpan(0, 0, minutes, 0);
        }
        /// <summary>
        /// Returns a timespan for the number of seconds
        /// </summary>
        public static TimeSpan Seconds(this int seconds)
        {
            return new TimeSpan(0, 0, 0, seconds);
        }
    }
}