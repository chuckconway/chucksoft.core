using System;
using System.Collections.Specialized;
using System.Text;
using System.Web;
using Chucksoft.Core.Instrumentation;

namespace Chucksoft.Core.Web.Instrumentation
{
    public class HttpLogging
    {
        private static readonly string _logPath = HttpContext.Current.Request.PhysicalApplicationPath + @"log";

        /// <summary>
        /// Logs the exception.
        /// </summary>
        /// <param name="ex">The ex.</param>
        public static void Exception(Exception ex)
        {
            Exception(ex, string.Empty);
        }

        /// <summary>
        /// Debugs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void Debug(string message)
        {
            string serverVariables = RertrieveServerVariables(HttpContext.Current);
            message = message + Environment.NewLine + serverVariables;

            Logging.Debug(message, _logPath);
        }

        /// <summary>
        /// Logs the exception.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="message">The message.</param>
        public static void Exception(Exception ex, string message)
        {
            Logging.Exception(message, ex, _logPath);
        }

        /// <summary>
        /// Logs the exception.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="context">The context.</param>
        private static void Exception(Exception ex, HttpContext context)
        {
            string serverVariables = RertrieveServerVariables(context);
            Logging.Exception(serverVariables, ex, _logPath);
        }

        /// <summary>
        /// Rertrieves the server variables.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        private static string RertrieveServerVariables(HttpContext context)
        {
            StringBuilder builder = new StringBuilder();

            string statusCode = FillSection("Status Code", context.Response.StatusCode.ToString());
            builder.AppendLine(statusCode);

            string queryValues = GenerateSection("Query Values", context.Request.QueryString);
            builder.AppendLine(queryValues);

            string cookieValues = GenerateCookieSection("Request Cookie Values", context.Request.Cookies);
            builder.AppendLine(cookieValues);

            string formValues = GenerateSection("Form Values", context.Request.Form);
            builder.AppendLine(formValues);

            string serverVariables = GenerateSection("Server Variables", context.Request.ServerVariables);
            builder.AppendLine(serverVariables);

            return builder.ToString();
        }

        /// <summary>
        /// Generates the section.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="collection">The collection.</param>
        /// <returns></returns>
        private static string GenerateCookieSection(string title, HttpCookieCollection collection)
        {
            string keyPairs = GenerateCookieKeyPairs(collection);
            string section = FillSection(title, keyPairs);

            return section;
        }

        /// <summary>
        /// Servers the variables.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <returns></returns>
        private static string GenerateCookieKeyPairs(HttpCookieCollection collection)
        {
            StringBuilder builder = new StringBuilder();

            for (int index = 0; index < collection.Count; index++)
            {
                builder.AppendLine(string.Format("{0}:{1}", collection.GetKey(index), collection[index]));
            }

            return builder.ToString();
        }

        /// <summary>
        /// Generates the section.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="collection">The collection.</param>
        /// <returns></returns>
        private static string GenerateSection(string title, NameValueCollection collection)
        {
            string keyPairs = GenerateKeyPairs(collection);
            string section = FillSection(title, keyPairs);

            return section;
        }

        /// <summary>
        /// Servers the variables.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <returns></returns>
        private static string GenerateKeyPairs(NameValueCollection collection)
        {
            StringBuilder builder = new StringBuilder();

            for (int index = 0; index < collection.Count; index++)
            {
                builder.AppendLine(string.Format("{0}:{1}", collection.GetKey(index), collection[index]));
            }

            return builder.ToString();
        }

        /// <summary>
        /// Fills the section.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        private static string FillSection(string title, string content)
        {
            string section =
                @"+----------------------------------------------------------------------+
                                {0}
+----------------------------------------------------------------------+
{1}
+----------------------------------------------------------------------+" + Environment.NewLine;

            return string.Format(section, title, content);
        }

        /// <summary>
        /// Called when [application error].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public static void OnApplicationError(object sender, EventArgs e)
        {
            if (HttpContext.Current.Server != null)
            {
                Exception ex = HttpContext.Current.Server.GetLastError();

                if (ex != null && HttpContext.Current.Response.StatusCode != 404)
                {
                    Exception(ex, HttpContext.Current);
                }
            }
        }

    }
}