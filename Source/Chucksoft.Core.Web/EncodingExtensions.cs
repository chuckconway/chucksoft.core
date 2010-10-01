using System;
using System.Web;
using Chucksoft.Core.Extensions.Strings;
using Microsoft.Security.Application;

namespace Chucksoft.Core.Web
{
    public static class EncodingExtensions
    {
        /// <summary>
        /// UrlEncodes a string
        /// </summary>      
        public static string EncodeUrl(this string @string)
        {
            return AntiXss.UrlEncode(@string);
        }

        /// <summary>
        /// UrlDecodes a string
        /// </summary>      
        public static string DecodeUrl(this string @string)
        {
            return HttpUtility.UrlDecode(@string);
        }

        /// <summary>
        /// HtmlEncodes a string
        /// </summary>      
        public static string EncodeHtml(this string @string)
        {
            return AntiXss.HtmlEncode(@string);
        }

        /// <summary>
        /// HtmlDecodes a string
        /// </summary>
        public static string DecodeHtml(this string @string)
        {
            return HttpUtility.HtmlDecode(@string);
        }

        /// <summary>
        /// Base64's a string with the default encoding type
        /// </summary>
        public static string EncodeBase64(this string @string)
        {
            return @string.EncodeBase64(EncodingType.Default);
        }

        /// <summary>
        /// Base64's a string with the specified encoding type
        /// </summary>      
        public static string EncodeBase64(this string @string, EncodingType encoding)
        {
            return Convert.ToBase64String(@string.ToBytes(encoding));
        }

        /// <summary>
        /// Decodes a base64 string using the default encoding type
        /// </summary>
        public static string DecodeBase64(this string base64String)
        {
            return base64String.DecodeBase64(EncodingType.Default);
        }

        /// <summary>
        /// Decodes a base64 string using the specified encoding type
        /// </summary>
        public static string DecodeBase64(this string base64String, EncodingType encoding)
        {
            return StringExtensions.GetEncoding(encoding).GetString(Convert.FromBase64String(base64String));
        }

        /// <summary>
        /// Escapes the single quotes within a string
        /// </summary>      
        public static string EncodeJavascript(this string @string)
        {
            return @string.Replace("'", "\\'");
        }
    }
}
