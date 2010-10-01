namespace Chucksoft.Core.Extensions.Time
{
    /// <remarks>
    /// These extensions were created by Fredrik Kalseth, 
    /// http://iridescence.no/
    /// 
    /// For details about these extensions, visit
    /// http://iridescence.no/Posts/A-Set-of-Useful-Extension-Methods-for-DateTime.aspx
    /// </remarks>   
    public static class TimeExtensions
    {
        /// <summary>
        /// Gets a DateTime representing midnight on the current date
        /// </summary>
        /// <param name="current">The current date</param>
        public static System.DateTime Midnight(this System.DateTime current)
        {
            return new System.DateTime(current.Year, current.Month, current.Day);         
        }

        /// <summary>
        /// Gets a DateTime representing noon on the current date
        /// </summary>
        /// <param name="current">The current date</param>
        public static System.DateTime Noon(this System.DateTime current)
        {
            return new System.DateTime(current.Year, current.Month, current.Day, 12, 0, 0);         
        }
        /// <summary>
        /// Sets the time of the current date with minute precision
        /// </summary>
        /// <param name="current">The current date</param>
        /// <param name="hour">The hour</param>
        /// <param name="minute">The minute</param>
        public static System.DateTime SetTime(this System.DateTime current, int hour, int minute)
        {
            return SetTime(current, hour, minute, 0, 0);
        }

        /// <summary>
        /// Sets the time of the current date with second precision
        /// </summary>
        /// <param name="current">The current date</param>
        /// <param name="hour">The hour</param>
        /// <param name="minute">The minute</param>
        /// <param name="second">The second</param>
        /// <returns></returns>
        public static System.DateTime SetTime(this System.DateTime current, int hour, int minute, int second)
        {
            return SetTime(current, hour, minute, second, 0);
        }

        /// <summary>
        /// Sets the time of the current date with millisecond precision
        /// </summary>
        /// <param name="current">The current date</param>
        /// <param name="hour">The hour</param>
        /// <param name="minute">The minute</param>
        /// <param name="second">The second</param>
        /// <param name="millisecond">The millisecond</param>
        /// <returns></returns>
        public static System.DateTime SetTime(this System.DateTime current, int hour, int minute, int second, int millisecond)
        {
            return new System.DateTime(current.Year, current.Month, current.Day, hour, minute, second, millisecond);         
        }
    }
}