//using System.Web.UI;

//namespace Chucksoft.Core.Web.Controls
//{
//    public class Image : Image
//    {
//        /// <summary>
//        /// Adds the attributes of an <see cref="T:System.Web.UI.WebControls.Image"/> to the output stream for rendering on the client.
//        /// </summary>
//        /// <param name="writer">A <see cref="T:System.Web.UI.HtmlTextWriter"/> that contains the output stream to render on the client browser.</param>
//        protected override void AddAttributesToRender(HtmlTextWriter writer)
//        {
//            string imageUrl = ImageUrl;

//            if ((imageUrl.Length > 0) )
//            {
//                writer.AddAttribute(HtmlTextWriterAttribute.Src, imageUrl);
//            }

//            imageUrl = DescriptionUrl;

//            if (imageUrl.Length != 0)
//            {
//                writer.AddAttribute(HtmlTextWriterAttribute.Longdesc, ResolveClientUrl(imageUrl));
//            }

//            imageUrl = AlternateText;

//            if ((imageUrl.Length > 0) || GenerateEmptyAlternateText)
//            {
//                writer.AddAttribute(HtmlTextWriterAttribute.Alt, imageUrl);
//            }

//        }

//    }
//}
