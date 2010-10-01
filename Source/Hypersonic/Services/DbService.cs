using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;

namespace Hypersonic.Services
{
    internal class DbService<TConnection, TCommand>
        where TConnection : DbConnection, new()
        where TCommand : DbCommand, new()
    {
              private string _key;

        public CommandType CommandType { get; set; }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public string ConnectionStringName
        {
            get { return _key; }
            set { _key = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbService&lt;TConnection, TCommand&gt;"/> class.
        /// </summary>
        /// <param name="connectionName">Name of the connection.</param>
        public DbService(string connectionName)
            : this()
        {
            _key = connectionName;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="DbService&lt;TConnection, TCommand&gt;"/> class.
        /// </summary>
        public DbService()
        {
            CommandType = CommandType.StoredProcedure;
        }

        /// <summary>
        /// Scalars the specified CMD text.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cmdText">The CMD text.</param>
        /// <param name="parms">The parms.</param>
        /// <param name="transaction">The transaction.</param>
        /// <returns></returns>
        public T Scalar<T>(string cmdText, List<DbParameter> parms, IDbTransaction transaction)
        {
            T result = default(T);

            //Get command
            using (DbCommand command = GetCommand(cmdText, parms, transaction))
            {
                ExecuteCommand executeCommand = new ExecuteCommand();
                result = (transaction != null ? executeCommand.ExecuteScalarInTransaction(command, result) : executeCommand.ExecuteScalar(command, result)); 
            }

            return result;
        }
        

        /// <summary>
        /// Returns reader from database call. **!# MUST be CLOSED and DISPOSED! Suggest using a "using" block
        /// </summary>
        /// <param name="storedProcedure">Procedure to be executed</param>
        /// <param name="values">values to be passed into procedure</param>
        /// <param name="transaction">The transaction.</param>
        /// <returns>Result Set from procedure execution</returns>
        public DbDataReader Reader(string storedProcedure, List<DbParameter> values, IDbTransaction transaction)
        {
            DbDataReader reader;

            //Get command
            using (DbCommand command = GetCommand(storedProcedure, values, transaction))
            {
                ExecuteCommand executeCommand = new ExecuteCommand();

                command.Connection.Open();
                reader = (transaction != null ? executeCommand.ExecuteReaderInTransaction(command) : executeCommand.ExecuteReader(command));
            }

            return reader;
        }

        
        /// <summary>
        /// Nullables the reader.
        /// </summary>
        /// <param name="storedProcedure">The stored procedure.</param>
        /// <param name="values">The values.</param>
        /// <param name="transaction">The transaction.</param>
        /// <returns></returns>
        public NullableDataReader NullableReader(string storedProcedure, List<DbParameter> values, IDbTransaction transaction)
        {
            DbDataReader reader = Reader(storedProcedure, values, transaction);
            NullableDataReader nullableDataReader = new NullableDataReader(reader);
            return nullableDataReader;
        }

        /// <summary>
        /// Executes the procedure and passes the parameters to the stored procedure
        /// </summary>
        /// <param name="storedProc">name of the stored procedure to be executed</param>
        /// <param name="values">The values.</param>
        /// <param name="transaction">The transaction.</param>
        /// <returns>rows affected</returns>
        public int NonQuery(string storedProc, List<DbParameter> values, IDbTransaction transaction)
        {
            int returnValue;

            //Get command
            using (DbCommand command = GetCommand(storedProc, values, transaction))
            {
                ExecuteCommand executeCommand = new ExecuteCommand();
                returnValue = (transaction != null ? executeCommand.ExecuteNonQueryInTransaction(command) : executeCommand.ExecuteNonQuery(command));
            }

            return returnValue;
        }

        /// <summary>
        /// Populates the collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storedProcedure">The stored procedure.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="getItem">The get item.</param>
        /// <param name="transaction">The transaction.</param>
        /// <returns></returns>
        public List<T> PopulateCollection<T>(string storedProcedure, List<DbParameter> parameters, Func<INullableReader, T> getItem, IDbTransaction transaction)
        {
            NullableDataReader reader = NullableReader(storedProcedure, parameters, transaction);
            List<T> lineitems = new List<T>();

            using (reader)
            {
                while (reader.Read())
                {
                    T items = getItem(reader);
                    lineitems.Add(items);
                }
            }

            return lineitems;
        }

        /// <summary>
        /// Populates the item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storedProcedure">The stored procedure.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="getItem">The get item.</param>
        /// <param name="transaction">The transaction.</param>
        /// <returns></returns>
        public T PopulateItem<T>(string storedProcedure, List<DbParameter> parameters, Func<INullableReader, T> getItem, IDbTransaction transaction)
        {
            NullableDataReader reader = NullableReader(storedProcedure, parameters, transaction);
            T lineitem = default(T);

            using (reader)
            {
                while (reader.Read())
                {
                    lineitem = getItem(reader);
                }
            }

            return lineitem;
        }

        /// <summary>
        /// Populates the Command with the commandText and array of parameters
        /// </summary>
        /// <param name="commandText">Inline Sql or stored procedure name</param>
        /// <param name="parameters">parameters to be passed into procedure call</param>
        /// <param name="transaction">The transaction.</param>
        /// <returns>
        /// A SqlCommand with associated commandText and SqlParameters
        /// </returns>
        private DbCommand GetCommand(string commandText, List<DbParameter> parameters, IDbTransaction transaction)
        {
            DbCommand cmd = GetCommand(commandText, transaction);
            cmd.Parameters.AddRange(parameters.ToArray());

            return cmd;
        }

        /// <summary>
        /// Creates the SqlCommand and sets the command Text
        /// </summary>
        /// <param name="commandText">Inline Sql or stored procedure name</param>
        /// <param name="transaction">The transaction.</param>
        /// <returns>A SqlCommand with associated commandText</returns>
        private DbCommand GetCommand(string commandText, IDbTransaction transaction)
        {
            string key = _key;

            DbConnection connection = (transaction == null ? GetConnection(key) : (DbConnection)transaction.Connection);
            DbCommand cmd = new TCommand
                                {
                                    CommandType = CommandType,
                                    CommandText = commandText,
                                    Connection = connection
                                };
            return cmd;
        }

        /// <summary>
        /// Gets first connection in the connection section of the blog file.
        /// </summary>
        /// <returns>A non opened SqlConnection</returns>
        public DbConnection GetConnection(string key)
        {
            string connectionString = GetConnectionString(key);
            if(string.IsNullOrEmpty(connectionString)) {throw new Exception("Connection string is not set.");}
            DbConnection connection = new TConnection { ConnectionString = connectionString };

            return connection;
        }

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        private static string GetConnectionString(string key)
        {
            string connectionString = string.Empty;

            if (string.IsNullOrEmpty(key))
            {
                if (ConfigurationManager.ConnectionStrings.Count > 0)
                {
                    connectionString = ConfigurationManager.ConnectionStrings[0].ConnectionString;
                }
            }
            else
            {

                connectionString = ConfigurationManager.ConnectionStrings[key].ConnectionString;
            }

            return connectionString;
        }

    }
}
