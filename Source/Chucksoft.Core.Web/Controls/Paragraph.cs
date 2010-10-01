using System.Web.UI;
using System.Web.UI.WebControls;

namespace Chucksoft.Core.Web.Controls
{
    public class Paragraph : Label
    {
        /// <summary>
        /// Gets the HTML tag that is used to render the <see cref="T:System.Web.UI.WebControls.Label"/> control.
        /// </summary>
        /// <value></value>
        /// <returns>The <see cref="T:System.Web.UI.HtmlTextWriterTag"/> value used to render the <see cref="T:System.Web.UI.WebControls.Label"/>.</returns>
        protected override HtmlTextWriterTag TagKey
        {
            get { return HtmlTextWriterTag.P; }
        }
    }
}