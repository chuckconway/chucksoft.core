using System;

namespace Chucksoft.Core.Extensions.Time
{
    /// <remarks>
    /// These extensions were created by Fredrik Kalseth, 
    /// http://iridescence.no/
    /// 
    /// For details about these extensions, visit
    /// http://iridescence.no/Posts/A-Set-of-Useful-Extension-Methods-for-DateTime.aspx
    /// </remarks>   
    public static class DayExtensions
    {
        /// <summary>
        /// Gets a DateTime representing the first day in the current month
        /// </summary>
        /// <param name="current">The current date</param>
        /// <returns></returns>
        public static DateTime First(this DateTime current)
        {
            return current.AddDays(1 - current.Day);
        }

        /// <summary>
        /// Gets a DateTime representing the first specified day in the current month
        /// </summary>
        /// <param name="current">The current day</param>
        /// <param name="dayOfWeek">The current day of week</param>
        /// <returns></returns>
        public static DateTime First(this DateTime current, DayOfWeek dayOfWeek)
        {
            DateTime first = current.First();
            if (first.DayOfWeek != dayOfWeek)
            {
                first = first.Next(dayOfWeek);
            }
            return first;
        }

        /// <summary>
        /// Gets a DateTime representing the last day in the current month
        /// </summary>
        /// <param name="current">The current date</param>
        /// <returns></returns>
        public static DateTime Last(this DateTime current)
        {
            int daysInMonth = DateTime.DaysInMonth(current.Year, current.Month);
            return current.First().AddDays(daysInMonth - 1);
        }

        /// <summary>
        /// Gets a DateTime representing the last specified day in the current month
        /// </summary>
        /// <param name="current">The current date</param>
        /// <param name="dayOfWeek">The current day of week</param>
        /// <returns></returns>
        public static DateTime Last(this DateTime current, DayOfWeek dayOfWeek)
        {
            DateTime last = current.Last();
            return last.AddDays(Math.Abs(dayOfWeek - last.DayOfWeek) * -1);
        }

        /// <summary>
        /// Gets a DateTime representing the first date following the current date which falls on the given day of the week
        /// </summary>
        /// <param name="current">The current date</param>
        /// <param name="dayOfWeek">The day of week for the next date to get</param>
        public static DateTime Next(this DateTime current, DayOfWeek dayOfWeek)
        {
            int offsetDays = dayOfWeek - current.DayOfWeek;
            if (offsetDays <= 0)
            {
                offsetDays += 7;
            }
            return current.AddDays(offsetDays);
        }
    }
}