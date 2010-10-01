using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace Chucksoft.Core.Web.Mvc.HtmlHelpers
{
    public static class GridHelper
    {
        /// <summary>
        /// Grids the specified helper.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="helper">The helper.</param>
        /// <param name="collection">The collection.</param>
        /// <returns></returns>
        public static string Grid<T>(this HtmlHelper helper, List<T> collection)
        {
            List<string> columns = GetColumns<T>();
            GridTable<T> table = new GridTable<T>(collection, () => columns, GetRow, string.Empty, string.Empty);

            return table.CreateTable();
        }

        /// <summary>
        /// Gets the columns.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static List<string> GetColumns<T>()
        {
            return typeof(T).GetProperties().Select(p => p.Name).ToList();
        }

        /// <summary>
        /// Gets the row.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        private static List<string> GetRow<T>(T item)
        {
            List<string> rowValues = new List<string>();
            List<PropertyInfo> propertyInfos = typeof (T).GetProperties().ToList();
            propertyInfos.ForEach(p => rowValues.Add((p.GetValue(item, null) ?? string.Empty).ToString()));

            return rowValues;
        }

        /// <summary>
        /// Grids the specified helper.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="helper">The helper.</param>
        /// <param name="collection">The collection.</param>
        /// <param name="columns">The columns.</param>
        /// <returns></returns>
        public static string Grid<T>(this HtmlHelper helper, List<T> collection, List<string> columns)
        {
            GridTable<T> table = new GridTable<T>(collection, () => columns, GetRow, string.Empty, string.Empty);
            
            return table.CreateTable();
        }

        /// <summary>
        /// Grids the specified helper.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="helper">The helper.</param>
        /// <param name="collection">The collection.</param>
        /// <param name="columns">The columns.</param>
        /// <param name="row">The row.</param>
        /// <param name="cssClass">The CSS class.</param>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public static string Grid<T>(this HtmlHelper helper, List<T> collection, Func<List<string>> columns, Func<T, List<string>> row, string cssClass, string id)
        {
            GridTable<T> table = new GridTable<T>(collection, columns, row, cssClass, id);
            return table.CreateTable();
        }

        /// <summary>
        /// Grids the specified helper.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="helper">The helper.</param>
        /// <param name="collection">The collection.</param>
        /// <param name="columns">The columns.</param>
        /// <param name="row">The row.</param>
        /// <param name="cssClass">The CSS class.</param>
        /// <param name="id">The id.</param>
        /// <param name="emptyTableMessage">The empty table message.</param>
        /// <param name="altRowCss">The alt row CSS.</param>
        /// <returns></returns>
        public static string Grid<T>(this HtmlHelper helper, List<T> collection, Func<List<string>> columns, Func<T, List<string>> row, string cssClass, string id, string emptyTableMessage, string altRowCss)
        {
            GridTable<T> table = new GridTable<T>(collection, columns, row, cssClass, id) {AlternateRowCssClass = altRowCss};
            return table.CreateTable(emptyTableMessage);
        }
    }
}