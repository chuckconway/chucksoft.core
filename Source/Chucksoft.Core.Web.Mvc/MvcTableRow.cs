using System.Collections.Generic;

namespace Chucksoft.Core.Web.Mvc
{
    public class MvcTableRow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MvcTableRow"/> class.
        /// </summary>
        /// <param name="cells">The cells.</param>
        public MvcTableRow(List<string> cells)
        {
            Cells = cells;
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="MvcTableRow"/> class.
        /// </summary>
        public MvcTableRow()
        {
            Cells = new List<string>();
        }

        /// <summary>
        /// Gets or sets the cells.
        /// </summary>
        /// <value>The cells.</value>
        public List<string> Cells { get; set; }

        /// <summary>
        /// Adds the cell.
        /// </summary>
        /// <param name="newRow">The new row.</param>
        public void AddCell(string newRow)
        {
            Cells.Add(newRow);
        }
    }
}