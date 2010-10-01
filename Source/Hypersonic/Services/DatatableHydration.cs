using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Hypersonic.Services
{
    class DatatableHydration<TConnection, TCommand, TParameter> 
        where TConnection : DbConnection, new()
        where TCommand : DbCommand, new()
        where TParameter : DbParameter, new()
    {
        /// <summary>
        /// Retrieves the values.
        /// </summary>
        /// <param name="storedProcedure">The stored procedure.</param>
        /// <returns></returns>
        public DataTable RetrieveValues(string storedProcedure)
        {
            return RetrieveValues(storedProcedure, new List<DbParameter>());
        }

        /// <summary>
        /// Retrieves the values.
        /// </summary>
        /// <typeparam name="TN"></typeparam>
        /// <param name="storedProcedure">The stored procedure.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="parameterDelimiter">The parameter delimiter.</param>
        /// <returns></returns>
        public DataTable RetrieveValues<TN>(string storedProcedure, TN parameters, string parameterDelimiter) where TN : class
        {
            DbParameterBuilder<TParameter> extractColumnNamesAndValuesService = new DbParameterBuilder<TParameter>(parameterDelimiter);
            List<DbParameter> extravedParameters = extractColumnNamesAndValuesService.GetValuesFromType(parameters);
            return RetrieveValues(storedProcedure, extravedParameters);
        }

        /// <summary>
        /// Retrieves the values.
        /// </summary>
        /// <param name="storedProcedure">The stored procedure.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public DataTable RetrieveValues(string storedProcedure, List<DbParameter> parameters)
        {
            DbService<TConnection, TCommand> dbService = new DbService<TConnection, TCommand>();
            DbDataReader dataReader = dbService.Reader(storedProcedure, parameters, null);
            return RetrieveValues(dataReader);
        }

        /// <summary>
        /// Retrieves the values.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        public DataTable RetrieveValues(DbDataReader reader)
        {
            //Discover the columns
            DataTable table = DiscoverColumns(reader);

            //managing resources...
            using (reader)
            {
                //read until we run out of rows
                while (reader.Read())
                {
                    //get a new row
                    DataRow row = table.NewRow();

                    //Set the column value
                    for (int index = 0; index < reader.VisibleFieldCount; index++)
                    {
                        row[reader.GetName(index)] = reader.GetValue(index);
                    }

                    //Add our newly created and populated row.
                    table.Rows.Add(row);
                }
            }

            return table;
        }

        /// <summary>
        /// Discovers the columns.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        private static DataTable DiscoverColumns(DbDataReader reader)
        {
            var table = new DataTable();

            //Dynamically discover and add the columns in the resultSet
            for (int index = 0; index < reader.VisibleFieldCount; index++)
            {
                string columnName = reader.GetName(index);

                if (!table.Columns.Contains(columnName))
                {
                    table.Columns.Add(new DataColumn(columnName));
                }
            }

            return table;
        }
    }
}
