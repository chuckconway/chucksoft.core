using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Chucksoft.Core.Web.Controls
{
    public class BreadCrumbs : Label
    {
        /// <summary>
        /// Initialize the properties
        /// </summary>
        public BreadCrumbs()
        {
            Links = new List<Control>();
            Separator = string.Empty;
        }

        /// <summary>
        /// Change the tag to the paragraph tag.
        /// </summary>
        protected override HtmlTextWriterTag TagKey
        {
            get { return HtmlTextWriterTag.P; }
        }

        /// <summary>
        /// Add hyperlinks to the breadcrumb
        /// </summary>
        public List<Control> Links { get; set; }

        /// <summary>
        /// designate your seperator
        /// </summary>
        public string Separator { get; set; }

        /// <summary>
        /// Renders the BreadCrumb control
        /// </summary>
        /// <param name="writer"></param>
        public override void RenderControl(HtmlTextWriter writer)
        {
            //iterate over the Control collection and add the controls
            for (int index = 0; index < Links.Count; index++)
            {
                Controls.Add(Links[index]);

                //Don't add the Sepatator on the last element
                if (index != (Links.Count - 1))
                {
                    Controls.Add(new LiteralControl(" " + Separator + " "));
                }
            }

            //Render everything.
            base.RenderControl(writer);
        }
    }
}