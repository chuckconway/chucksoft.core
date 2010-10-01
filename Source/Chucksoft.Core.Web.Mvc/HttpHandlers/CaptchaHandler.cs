using System.Drawing;
using System.Drawing.Imaging;
using System.Web;
using Chucksoft.Core.Web.Validation;


namespace Chucksoft.Core.Web.Mvc.HttpHandlers
{
    public class CaptchaHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string word = context.Request["l"];
            CaptchaImage captchaImage = new CaptchaImage(word, Color.White, 175, 50);

            context.Response.ContentType = @"image/jpg";
            context.Response.Expires = 0;

            captchaImage.Image.Save(context.Response.OutputStream, ImageFormat.Jpeg);
        }

        /// <summary>
        /// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler"/> instance.
        /// </summary>
        /// <value></value>
        /// <returns>true if the <see cref="T:System.Web.IHttpHandler"/> instance is reusable; otherwise, false.
        /// </returns>
        public bool IsReusable
        {
            get { return true; }
        }
    }
}