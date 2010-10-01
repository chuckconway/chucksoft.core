using System.IO;
using System.Reflection;

namespace Chucksoft.Core
{
    public class Library
    {
        /// <summary>
        /// Gets the executing directory.
        /// </summary>
        /// <returns></returns>
        public static string GetExecutingDirectory()
        {
            string dbPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
            dbPath = dbPath.Replace("file:\\", "");

            return dbPath;
        }
    }
}