using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace Chucksoft.Core
{
    public class ResourceFileHelper
    {
        /// <summary>
        /// Converts the stream resource to unicode string.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="encodingType">Type of the encoding.</param>
        /// <returns></returns>
        private static string ConvertStreamResourceToUnicodeString(Type type, string resourceName, Encoding encodingType)
        {
            string data = string.Empty;
            Assembly assembly = Assembly.GetAssembly(type);

            using (Stream file = assembly.GetManifestResourceStream(resourceName))
            {
                if (file != null)
                {
                    var bytes = new byte[file.Length];
                    file.Position = 0;
                    file.Read(bytes, 0, (int)file.Length);
                    data = encodingType.GetString(bytes);
                }
            }

            return data;
        }

        /// <summary>
        /// Converts the stream resource to UT f8 string.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <returns></returns>
        public static string ConvertStreamResourceToUTF8String(Type type, string resourceName)
        {
            string returnValue = ConvertStreamResourceToUnicodeString(type, resourceName, Encoding.UTF8);
            return returnValue;
        }

        /// <summary>
        /// Converts the stream resource to unicode string.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <returns></returns>
        public static string ConvertStreamResourceToUnicodeString(Type type, string resourceName)
        {
            string returnValue = ConvertStreamResourceToUnicodeString(type, resourceName, Encoding.Unicode);
            return returnValue;
        }
    }
}
