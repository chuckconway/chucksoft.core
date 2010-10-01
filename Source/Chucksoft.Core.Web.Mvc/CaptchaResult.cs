using System.Drawing;
using System.Drawing.Imaging;
using System.Web.Mvc;
using Chucksoft.Core.Web.Validation;

namespace Chucksoft.Core.Web.Mvc
{
    public class CaptchaResult : ActionResult
    {
        private readonly string _characters;

        /// <summary>
        /// Initializes a new instance of the <see cref="CaptchaResult"/> class.
        /// </summary>
        /// <param name="characters">The characters.</param>
        public CaptchaResult(string characters)
        {
            _characters = characters;
        }

        /// <summary>
        /// Enables processing of the result of an action method by a custom type that inherits from <see cref="T:System.Web.Mvc.ActionResult"/>.
        /// </summary>
        /// <param name="context">The context within which the result is executed.</param>
        public override void ExecuteResult(ControllerContext context)
        {
            CaptchaImage captchaImage = new CaptchaImage(_characters, Color.White, 175, 50);

            context.HttpContext.Response.ContentType = @"image/jpg";
            context.HttpContext.Response.Expires = 0;

            captchaImage.Image.Save(context.HttpContext.Response.OutputStream, ImageFormat.Jpeg);
        }
    }
}