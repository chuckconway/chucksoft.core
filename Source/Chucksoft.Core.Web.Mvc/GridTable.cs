using System;
using System.Collections.Generic;

namespace Chucksoft.Core.Web.Mvc
{
    public class GridTable<T>: MvcTable<T>
    {
        private readonly Func<List<string>> _columns;
        private readonly Func<T, List<string>> _row;


        /// <summary>
        /// Initializes a new instance of the <see cref="GridTable&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="columns">The columns.</param>
        /// <param name="row">The row.</param>
        /// <param name="cssClass">The CSS class.</param>
        /// <param name="id">The id.</param>
        public GridTable(List<T> collection, Func<List<string>> columns, Func<T, List<string>> row, string cssClass, string id): base(collection)
        {
            _columns = columns;
            _row = row;
            CssClass = cssClass;
            Id = id;
        }

        /// <summary>
        /// Binds the cells.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public override List<string> BindCells(T item)
        {
            return _row(item);
        }

        /// <summary>
        /// Binds the columns.
        /// </summary>
        /// <returns></returns>
        public override List<string> BindColumns()
        {
            return _columns();
        }
    }
}