using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Chucksoft.Core.Web.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public class Script : Literal
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Script"/> class.
        /// </summary>
        /// <param name="innerText">The inner text.</param>
        /// <param name="scriptType">Type of the script.</param>
        public Script(string innerText, string scriptType)
        {
            ScriptText = innerText;
            ScriptType = scriptType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Script"/> class.
        /// </summary>
        public Script()
        {
        }

        /// <summary>
        /// Gets or sets the script text.
        /// </summary>
        /// <value>The script text.</value>
        public string ScriptText { get; set; }

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>The source.</value>
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets the type of the script.
        /// </summary>
        /// <value>The type of the script.</value>
        public string ScriptType { get; set; }

        /// <summary>
        /// Outputs server control content to a provided <see cref="T:System.Web.UI.HtmlTextWriter"/> object and stores tracing information about the control if tracing is enabled.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"/> object that receives the control content.</param>
         public override void RenderControl(HtmlTextWriter writer)
        {
            //If there is ScriptText, don't render the source.
            string src = (string.IsNullOrEmpty(ScriptText) ? string.Format("src=\"{0}\"", Source) : string.Empty);

            //Generate Script tags.
            string scriptTag = string.Format("<script {0} type=\"{1}\">{2}</script>{3}", src, ScriptType, ScriptText,
                                             Environment.NewLine);
            writer.Write(scriptTag);

            base.RenderControl(writer);
        }
    }
}