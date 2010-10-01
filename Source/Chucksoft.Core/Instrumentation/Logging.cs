using System;
using System.IO;
using System.Text;

namespace Chucksoft.Core.Instrumentation
{
    public class Logging
    {
        /// <summary>
        /// Debugs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="folderPath">The folder path.</param>
        public static void Debug(string message, string folderPath)
        {
            LogException(folderPath, null, message);
        }

        /// <summary>
        /// Logs the exception.
        /// </summary>
        /// <param name="folderPath">The folder path.</param>
        /// <param name="ex">The ex.</param>
        public static void Exception(string folderPath, Exception ex)
        {
            LogException(folderPath, ex, string.Empty);
        }

        /// <summary>
        /// Logs the exception.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="ex">The ex.</param>
        /// <param name="folderPath">The folder path.</param>
        public static void Exception(string message, Exception ex, string folderPath)
        {
            LogException(folderPath, ex, message);
        }

        /// <summary>
        /// Logs the exception.
        /// </summary>
        /// <param name="folderPath">The folder path.</param>
        /// <param name="ex">The ex.</param>
        /// <param name="message">The message.</param>
        private static void LogException(string folderPath, Exception ex, string message)
        {
            string path = GetPath(folderPath);
            string error = (ex != null ? GetError(ex) : LogHeader());

            if (!string.IsNullOrEmpty(message))
            {
                error += string.Format("\r\n{0}\r\n", message);
            }

            File.AppendAllText(path, error);
        }

        /// <summary>
        /// Gets the path.
        /// </summary>
        /// <param name="folderPath">The folder path.</param>
        /// <returns></returns>
        private static string GetPath(string folderPath)
        {
            string folder = folderPath;

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            return folder + string.Format(@"\{0}_{1}_{2}.txt", DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Year);
        }

        /// <summary>
        /// Gets the error.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        private static string GetError(Exception ex)
        {
            string error = FormatExceptionHeader(ex);
            error += FormatExeception(ex);

            return error;
        }

        /// <summary>
        /// Formats the exception header.
        /// </summary>
        /// <returns></returns>
        private static string LogHeader()
        {
            string error = string.Format("[{0}] Debugging:\r\n", DateTime.Now);
            return error;
        }

        /// <summary>
        /// Formats the exception header.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        private static string FormatExceptionHeader(Exception ex)
        {
            string error = string.Format("[{0}] Error: {1} \r\n", DateTime.Now, ex.Message);
            return error;
        }

        /// <summary>
        /// Formats the exeception.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        private static string FormatExeception(Exception ex)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("Message: {0} \r\nMethodName: {1} \r\nHelpLink: {2} \r\nClassName: {3} \r\nSource: {4}\r\n", ex.Message, ex.TargetSite.Name, ex.HelpLink, ((ex.TargetSite)).ReflectedType.Name, ex.Source);
            builder.AppendLine(string.Format("StackTrace: {0}\r\n", ex.StackTrace));

            //retrieve inner exceptions
            if (ex.InnerException != null)
            {
                string innerException = FormatExeception(ex.InnerException);
                builder.Append(innerException);
            }

            return builder.ToString();
        }
    }
}
