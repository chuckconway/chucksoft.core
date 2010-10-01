using System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;

namespace Chucksoft.Core.Web.Mvc
{
    public abstract class MvcTable<T> 
    {
        private readonly List<T> _collection;

        /// <summary>
        /// Initializes a new instance of the <see cref="MvcTable&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="collection">The collection.</param>
        protected MvcTable(List<T> collection)
        {
            _collection = collection;
        }

        /// <summary>
        /// Gets the index of the current.
        /// </summary>
        /// <value>The index of the current.</value>
        public int CurrentRowIndex { get; private set; }

        /// <summary>
        /// Gets or sets the alternate row CSS class.
        /// </summary>
        /// <value>The alternate row CSS class.</value>
        public string AlternateRowCssClass { get; set; }
         
        /// <summary>
        /// Gets or sets the CSS class.
        /// </summary>
        /// <value>The CSS class.</value>
        public string CssClass { get; set; }
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the empty table message.
        /// </summary>
        /// <value>The empty table message.</value>
        public string EmptyTableMessage { get; set;}

        /// <summary>
        /// Creates the table.
        /// </summary>
        /// <returns></returns>
        public string CreateTable()
        {
            return CreateTable(string.Empty);
        }

        /// <summary>
        /// Creates the requesting table.
        /// </summary>
        /// <returns></returns>
        public string CreateTable(string emptyTableMessage)
        {
            EmptyTableMessage = emptyTableMessage;
            string html = (string.IsNullOrEmpty(EmptyTableMessage) ? string.Empty : "<p class=\"emptytablemessage \" >" + EmptyTableMessage + "</p>");

            if (_collection != null && _collection.Count > 0)
            {
                using (StringWriter stringWriter = new StringWriter())
                {
                    using (HtmlTextWriter writer = new HtmlTextWriter(stringWriter))
                    {
                        writer.Indent = 1;
                        WriteTableBeginTag(writer);
                        GetTable(writer);
                        writer.WriteEndTag("table");
                        html = stringWriter.ToString();
                    }
                }
            }

            return html;
        }

        /// <summary>
        /// Sets the rows.
        /// </summary>
        /// <returns></returns>
        public List<MvcTableRow> BuildRows()
        {
            List<MvcTableRow> rows = new List<MvcTableRow>();

            for (int index = 0; index < _collection.Count; index++)
            {
                CurrentRowIndex = index;
                List<string> cells = BindCells(_collection[index]);

                if (cells.Count > 0)
                {
                    MvcTableRow row = new MvcTableRow(cells);
                    rows.Add(row);
                }
            }

            return rows;
        }

        /// <summary>
        /// Sets the rows.
        /// </summary>
        /// <returns></returns>
        public abstract List<string> BindCells(T item);

        /// <summary>
        /// Sets the columns.
        /// </summary>
        /// <returns></returns>
        public abstract List<string> BindColumns();

        /// <summary>
        /// Gets the table.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <returns></returns>
        private void GetTable(HtmlTextWriter writer)
        {
            List<string> columns = BindColumns();
            List<MvcTableRow> rows = BuildRows();

            CreateHeader(writer, columns);
            AddRow(writer, rows);
        }

        /// <summary>
        /// Sets the row.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="rows">The _rows.</param>
        private void AddRow(HtmlTextWriter writer, IList<MvcTableRow> rows)
        {
            writer.WriteFullBeginTag("tbody");

            for (int index = 0; index < rows.Count; index++)
            {
                CreateRow(writer, index, rows);
            }

            writer.WriteEndTag("tbody");
        }

        /// <summary>
        /// Creates the row.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="index">The index.</param>
        /// <param name="rows">The rows.</param>
        protected virtual void CreateRow(HtmlTextWriter writer, int index, IList<MvcTableRow> rows)
        {
            SetClassOnAlternateRows(writer, index, rows);

            for (int idx = 0; idx < rows[index].Cells.Count; idx++)
            {
                CreateTableRowCell(writer, rows[index].Cells[idx]);
            }

            writer.WriteEndTag("tr");
        }

        /// <summary>
        /// Creates the header.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="headerNames">The header names.</param>
        protected void CreateHeader(HtmlTextWriter writer, IList<string> headerNames)
        {
            writer.WriteFullBeginTag("thead");
            writer.WriteFullBeginTag("tr");

            for (int index = 0; index < headerNames.Count; index++)
            {
                CreateTableHeaderCell(writer, headerNames[index]);
            }

            writer.WriteEndTag("tr");
            writer.WriteEndTag("thead");
        }

        /// <summary>
        /// Adds the row attributes.
        /// </summary>
        /// <param name="currentIndex">Index of the current.</param>
        /// <param name="rows">The rows.</param>
        /// <returns></returns>
        public virtual IEnumerator<KeyValuePair<HtmlTextWriterAttribute, string>> AddRowAttributes(int currentIndex, IList<MvcTableRow> rows)
        {
            IDictionary<HtmlTextWriterAttribute, string> dictionary = new Dictionary<HtmlTextWriterAttribute, string>();
            return dictionary.GetEnumerator();
        }

        /// <summary>
        /// Sets the class on alternate _rows.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="currentRowCount">The current row count.</param>
        /// <param name="rows">The rows.</param>
        protected virtual void SetClassOnAlternateRows(HtmlTextWriter writer, int currentRowCount, IList<MvcTableRow> rows)
        {
            bool hasAltText = !string.IsNullOrEmpty(AlternateRowCssClass);

            if ((currentRowCount % 2) != 0)
            {
                writer.WriteBeginTag("tr");
                AddAttributes(currentRowCount, writer, rows);


                if (hasAltText)
                {
                    writer.WriteAttribute("class", AlternateRowCssClass);
                }

                writer.Write(">");
            }
            else
            {
                writer.WriteFullBeginTag("tr");
            }
        }

        /// <summary>
        /// Adds the attributes.
        /// </summary>
        /// <param name="currentRowCount">The current row count.</param>
        /// <param name="writer">The writer.</param>
        /// <param name="rows">The rows.</param>
        private void AddAttributes(int currentRowCount, HtmlTextWriter writer, IList<MvcTableRow> rows)
        {
            IEnumerator<KeyValuePair<HtmlTextWriterAttribute, string>> attribs = AddRowAttributes(currentRowCount, rows);
                
            while(attribs.MoveNext())
            {
                writer.AddAttribute(attribs.Current.Key, attribs.Current.Value);
            }
        }

        /// <summary>
        /// Writes the table begin tag.
        /// </summary>
        /// <param name="writer">The writer.</param>
        private void WriteTableBeginTag(HtmlTextWriter writer)
        {
            writer.WriteBeginTag("table");

            if (!string.IsNullOrEmpty(CssClass))
            {
                writer.WriteAttribute("class", CssClass);
            }

            if (!string.IsNullOrEmpty(Id))
            {
                writer.WriteAttribute("id", Id);
            }

            writer.Write(">");
        }

        /// <summary>
        /// Creates the table header.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="headerText">The header text.</param>
        protected void CreateTableHeaderCell(HtmlTextWriter writer, string headerText)
        {
            CreateTableRowCell(writer, "th", headerText);
        }

        /// <summary>
        /// Creates the table row cell.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="rowTag">The row tag.</param>
        /// <param name="headerText">The header text.</param>
        protected virtual void CreateTableRowCell(HtmlTextWriter writer, string rowTag, string headerText)
        {
            writer.Write(Environment.NewLine);
            writer.WriteFullBeginTag(rowTag);
            writer.Write(Environment.NewLine);
            writer.WriteLine(headerText);
            writer.WriteEndTag(rowTag);
        }

        /// <summary>
        /// Creates the table header.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="headerText">The header text.</param>
        protected virtual void CreateTableRowCell(HtmlTextWriter writer, string headerText)
        {
            CreateTableRowCell(writer, "td", headerText);
        }
    }
}