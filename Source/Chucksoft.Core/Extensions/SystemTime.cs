using System;

namespace Chucksoft.Core.Extensions
{
    public static class SystemTime
    {
        /// <summary>
        /// Returns the current DateTime.
        /// </summary>
        /// <remarks>
        /// This is an alias for DateTime.Now; however, given
        /// that it's writable, it makes unit testing of time-sensitive code far easier.
        /// 
        /// Contributed by Oren Eini, 
        /// http://www.ayende.com/Blog/archive/2008/07/07/Dealing-with-time-in-tests.aspx
        /// </remarks>
        public static Func<DateTime> Now = () => DateTime.Now;
        public static void ResetNow()
        {
            Now = () => DateTime.Now;
        }
    }
}