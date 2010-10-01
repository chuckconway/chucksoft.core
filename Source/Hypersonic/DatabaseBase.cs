using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Hypersonic.Services;

namespace Hypersonic
{
    public class DatabaseBase<TConnection, TCommand, TParameter> : IDatabase
        where TConnection : DbConnection, new()
        where TCommand : DbCommand, new()
        where TParameter : DbParameter, new()
    {
        private string _key;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseBase&lt;TConnection, TCommand, TParameter&gt;"/> class.
        /// </summary>
        /// <param name="connectionName">Name of the connection.</param>
        protected DatabaseBase(string connectionName) : this()
        {
            _key = connectionName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseBase&lt;TConnection, TCommand, TParameter&gt;"/> class.
        /// </summary>
        protected DatabaseBase()
        {
            CommandType = CommandType.StoredProcedure;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseBase&lt;TConnection, TCommand, TParameter&gt;"/> class.
        /// </summary>
        protected DatabaseBase(CommandType commandType)
        {
            CommandType = commandType;
        }

        /// <summary>
        /// Build database parameters based on type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="paramters">The paramters.</param>
        /// <returns></returns>
        public List<DbParameter> GetParameters<T>(T paramters)
        {
            DbParameterBuilder<TParameter> parameterBuilder = new DbParameterBuilder<TParameter>(ParameterDelimiter);
            return parameterBuilder.GetValuesFromType(paramters);
        }

        /// <summary>
        /// Populates the specified reader.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        public T AutoPopulate<T>(INullableReader reader) where T : class, new()
        {
            TypeBuilder discoveryService = new TypeBuilder();
            T instance = discoveryService.HydrateType<T>(reader);

            return instance;
        }


        /// <summary>
        /// Gets or sets the type of the command.
        /// </summary>
        /// <value>The type of the command.</value>
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
        /// Gets the parameter delimiter.
        /// </summary>
        /// <value>The parameter delimiter.</value>
        public virtual string ParameterDelimiter
        {
            get { return "@"; }
        }

        /// <summary>
        /// Begins the transaction.
        /// </summary>
        /// <returns></returns>
        public IDbTransaction BeginTransaction()
        {
            DbService<TConnection, TCommand> dbService = GetDbService();
            DbConnection dbConnection = dbService.GetConnection(_key);

            DbTransaction beginTransaction = dbConnection.BeginTransaction();
            IDbTransaction transaction = new Transaction(beginTransaction);
            return transaction;
        }

        /// <summary>
        /// Begins the transaction.
        /// </summary>
        /// <param name="isolationLevel">The isolation level.</param>
        /// <returns></returns>
        public IDbTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            DbService<TConnection, TCommand> dbService = GetDbService();

            DbConnection dbConnection = dbService.GetConnection(_key);

            DbTransaction beginTransaction = dbConnection.BeginTransaction(isolationLevel);
            IDbTransaction transaction = new Transaction(beginTransaction);
            return transaction;
        }

        /// <summary>
        /// Gets the db service.
        /// </summary>
        /// <returns></returns>
        private DbService<TConnection, TCommand> GetDbService()
        {
            return new DbService<TConnection, TCommand> {CommandType = CommandType};
        }

        /// <summary>
        /// Scalars the specified CMD text.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cmdText">The CMD text.</param>
        /// <returns></returns>
        public T Scalar<T>(string cmdText)
        {
            return Scalar<T>(cmdText, new List<DbParameter>(), null);
        }

        /// <summary>
        /// Scalars the specified CMD text.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cmdText">The CMD text.</param>
        /// <param name="transaction">The transaction.</param>
        /// <returns></returns>
        public T Scalar<T>(string cmdText, IDbTransaction transaction)
        {
            return Scalar<T>(cmdText, new List<DbParameter>(), transaction);
        }

        /// <summary>
        /// Scalars the specified CMD text.
        /// </summary>
        /// <typeparam name="TN"></typeparam>
        /// <param name="cmdText">The CMD text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public object Scalar<TN>(string cmdText, TN parameters) where TN : class
        {
            List<DbParameter> dbParameters = GetValuesFromType(parameters);
            return Scalar<object>(cmdText, dbParameters);
        }

        /// <summary>
        /// Scalars the specified CMD text.
        /// </summary>
        /// <typeparam name="TN"></typeparam>
        /// <param name="cmdText">The CMD text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="transaction">The transaction.</param>
        /// <returns></returns>
        public object Scalar<TN>(string cmdText, TN parameters, IDbTransaction transaction) where TN : class
        {
            List<DbParameter> dbParameters = GetValuesFromType(parameters);
            return Scalar<object>(cmdText, dbParameters, transaction);
        }

        /// <summary>
        /// Scalars the specified CMD text.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cmdText">The CMD text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public T Scalar<T>(string cmdText, List<DbParameter> parameters)
        {
            DbService<TConnection, TCommand> dbService = GetDbService();
            dbService.ConnectionStringName = _key;

            return dbService.Scalar<T>(cmdText, parameters, null);
        }

        /// <summary>
        /// Scalars the specified CMD text.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cmdText">The CMD text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="transaction">The transaction.</param>
        /// <returns></returns>
        public T Scalar<T>(string cmdText, List<DbParameter> parameters, IDbTransaction transaction)
        {
            DbService<TConnection, TCommand> dbService = GetDbService();
            dbService.ConnectionStringName = _key;

            return dbService.Scalar<T>(cmdText, parameters, transaction);
        }


        /// <summary>
        /// Returns reader from database call. **!# MUST be CLOSED and DISPOSED! Suggest using a "using" block
        /// </summary>
        /// <param name="storedProcedure">Procedure to be executed</param>
        /// <returns>Result Set from procedure execution</returns>
        public DbDataReader Reader(string storedProcedure)
        {
            return Reader(storedProcedure, new List<DbParameter>(), null);
        }

        /// <summary>
        /// Returns reader from database call. **!# MUST be CLOSED and DISPOSED! Suggest using a "using" block
        /// </summary>
        /// <param name="storedProcedure">Procedure to be executed</param>
        /// <param name="transaction">The transaction.</param>
        /// <returns>Result Set from procedure execution</returns>
        public DbDataReader Reader(string storedProcedure, IDbTransaction transaction)
        {
            return Reader(storedProcedure, new List<DbParameter>(), transaction);
        }

        /// <summary>
        /// Returns reader from database call. **!# MUST be CLOSED and DISPOSED! Suggest using a "using" block
        /// </summary>
        /// <param name="storedProcedure">Procedure to be executed</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Result Set from procedure execution</returns>
        public DbDataReader Reader<TN>(string storedProcedure, TN parameters) where TN : class
        {
            List<DbParameter> dbParameters = GetValuesFromType(parameters);
            return Reader(storedProcedure, dbParameters, null);
        }

        /// <summary>
        /// Returns reader from database call. **!# MUST be CLOSED and DISPOSED! Suggest using a "using" block
        /// </summary>
        /// <typeparam name="TN"></typeparam>
        /// <param name="storedProcedure">Procedure to be executed</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="transaction">The transaction.</param>
        /// <returns>Result Set from procedure execution</returns>
        public DbDataReader Reader<TN>(string storedProcedure, TN parameters, IDbTransaction transaction) where TN : class
        {
            List<DbParameter> dbParameters = GetValuesFromType(parameters);
            return Reader(storedProcedure, dbParameters, transaction);
        }

        /// <summary>
        /// Returns reader from database call. **!# MUST be CLOSED and DISPOSED! Suggest using a "using" block
        /// </summary>
        /// <param name="storedProcedure">Procedure to be executed</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Result Set from procedure execution</returns>
        public DbDataReader Reader(string storedProcedure, List<DbParameter> parameters)
        {
            DbService<TConnection, TCommand> dbService = GetDbService();
            dbService.ConnectionStringName = _key;

            DbDataReader dbDataReader = dbService.Reader(storedProcedure, parameters, null);
            return dbDataReader;
        }

        /// <summary>
        /// Returns reader from database call. **!# MUST be CLOSED and DISPOSED! Suggest using a "using" block
        /// </summary>
        /// <param name="storedProcedure">Procedure to be executed</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="transaction">The transaction.</param>
        /// <returns>Result Set from procedure execution</returns>
        public DbDataReader Reader(string storedProcedure, List<DbParameter> parameters, IDbTransaction transaction)
        {
            DbService<TConnection, TCommand> dbService = GetDbService();
            dbService.ConnectionStringName = _key;

            DbDataReader dbDataReader = dbService.Reader(storedProcedure, parameters, transaction);
            return dbDataReader;
        }

        /// <summary>
        /// Nullables the reader.
        /// </summary>
        /// <param name="storedProcedure">The stored procedure.</param>
        /// <returns></returns>
        public NullableDataReader NullableReader(string storedProcedure)
        {
            DbService<TConnection, TCommand> dbService = GetDbService();
            dbService.ConnectionStringName = _key;

            NullableDataReader nullableDataReader = dbService.NullableReader(storedProcedure, new List<DbParameter>(),
                                                                             null);
            return nullableDataReader;
        }

        /// <summary>
        /// Nullables the reader.
        /// </summary>
        /// <param name="storedProcedure">The stored procedure.</param>
        /// <param name="transaction">The transaction.</param>
        /// <returns></returns>
        public NullableDataReader NullableReader(string storedProcedure, IDbTransaction transaction)
        {
            DbService<TConnection, TCommand> dbService = GetDbService();
            dbService.ConnectionStringName = _key;

            NullableDataReader nullableDataReader = dbService.NullableReader(storedProcedure, new List<DbParameter>(),
                                                                             transaction);
            return nullableDataReader;
        }

        /// <summary>
        /// Populate and returns the NullableReader.
        /// </summary>
        /// <param name="storedProcedure">The stored procedure.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public NullableDataReader NullableReader<TN>(string storedProcedure, TN parameters) where TN : class
        {
            List<DbParameter> dbParameters = GetValuesFromType(parameters);
            DbService<TConnection, TCommand> dbService = GetDbService();
            dbService.ConnectionStringName = _key;

            NullableDataReader nullableDataReader = dbService.NullableReader(storedProcedure, dbParameters, null);
            return nullableDataReader;
        }

        /// <summary>
        /// Populate and returns the NullableReader.
        /// </summary>
        /// <typeparam name="TN"></typeparam>
        /// <param name="storedProcedure">The stored procedure.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="transaction">The transaction.</param>
        /// <returns></returns>
        public NullableDataReader NullableReader<TN>(string storedProcedure, TN parameters, IDbTransaction transaction)
            where TN : class
        {
            List<DbParameter> dbParameters = GetValuesFromType(parameters);
            DbService<TConnection, TCommand> dbService = GetDbService();
            dbService.ConnectionStringName = _key;

            NullableDataReader nullableDataReader = dbService.NullableReader(storedProcedure, dbParameters, transaction);
            return nullableDataReader;
        }

        /// <summary>
        /// Nullables the reader.
        /// </summary>
        /// <param name="storedProcedure">The stored procedure.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public NullableDataReader NullableReader(string storedProcedure, List<DbParameter> parameters)
        {
            DbService<TConnection, TCommand> dbService = GetDbService();
            dbService.ConnectionStringName = _key;

            NullableDataReader nullableDataReader = dbService.NullableReader(storedProcedure, parameters, null);
            return nullableDataReader;
        }

        /// <summary>
        /// Nullables the reader.
        /// </summary>
        /// <param name="storedProcedure">The stored procedure.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="transaction">The transaction.</param>
        /// <returns></returns>
        public NullableDataReader NullableReader(string storedProcedure, List<DbParameter> parameters,
                                                 IDbTransaction transaction)
        {
            DbService<TConnection, TCommand> dbService = GetDbService();
            dbService.ConnectionStringName = _key;

            NullableDataReader nullableDataReader = dbService.NullableReader(storedProcedure, parameters, transaction);
            return nullableDataReader;
        }

        /// <summary>
        /// Executes the procedure
        /// </summary>
        /// <param name="storedProc">name of stored procedure</param>
        /// <returns>rows affected</returns>
        public int NonQuery(string storedProc)
        {
            return NonQuery(storedProc, new List<DbParameter>(), null);
        }

        /// <summary>
        /// Executes the procedure
        /// </summary>
        /// <param name="storedProc">name of stored procedure</param>
        /// <param name="transaction">The transaction.</param>
        /// <returns>rows affected</returns>
        public int NonQuery(string storedProc, IDbTransaction transaction)
        {
            return NonQuery(storedProc, new List<DbParameter>(), transaction);
        }

        /// <summary>
        /// Nons the query.
        /// </summary>
        /// <param name="storedProc">The stored proc.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public int NonQuery(string storedProc, List<DbParameter> parameters)
        {
            DbService<TConnection, TCommand> dbService = GetDbService();
            dbService.ConnectionStringName = _key;

            return dbService.NonQuery(storedProc, parameters, null);
        }

        /// <summary>
        /// Executes the procedure and passes the parameters to the stored procedure
        /// </summary>
        /// <param name="storedProc">name of the stored procedure to be executed</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="transaction">The transaction.</param>
        /// <returns>rows affected</returns>
        public int NonQuery(string storedProc, List<DbParameter> parameters, IDbTransaction transaction)
        {
            DbService<TConnection, TCommand> dbService = GetDbService();
            dbService.ConnectionStringName = _key;

            return dbService.NonQuery(storedProc, parameters, transaction);
        }

        /// <summary>
        /// Executes the procedure and passes the parameters to the stored procedure.
        /// </summary>
        /// <typeparam name="TN"></typeparam>
        /// <param name="storedProc">The stored proc.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public int NonQuery<TN>(string storedProc, TN parameters) where TN : class
        {
            List<DbParameter> dbParameters = GetValuesFromType(parameters);
            return NonQuery(storedProc, dbParameters, null);
        }

        /// <summary>
        /// Executes the procedure and passes the parameters to the stored procedure.
        /// </summary>
        /// <typeparam name="TN"></typeparam>
        /// <param name="storedProc">The stored proc.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="transaction">The transaction.</param>
        /// <returns></returns>
        public int NonQuery<TN>(string storedProc, TN parameters, IDbTransaction transaction) where TN : class
        {
            List<DbParameter> dbParameters = GetValuesFromType(parameters);
            return NonQuery(storedProc, dbParameters, transaction);
        }

        /// <summary>
        /// Makes the parameter.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public DbParameter MakeParameter<T>(string parameterName, T value)
        {
            DbParameter parameter = new TParameter {ParameterName = parameterName, Value = value};
            return parameter;
        }

        /// <summary>
        /// Makes the parameter.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="value">The value.</param>
        /// <param name="parameterDirection">The parameter direction.</param>
        /// <returns></returns>
        public DbParameter MakeParameter<T>(string parameterName, T value, ParameterDirection parameterDirection)
        {
            DbParameter parameter = new TParameter {ParameterName = parameterName, Value = value, Direction = parameterDirection};
            return parameter;
        }


        /// <summary>
        /// Makes the parameter
        /// </summary>
        /// <param name="parameterName">name of parameter in Stored Procedure</param>
        /// <param name="dbType">Type of SqlDbType (column type)</param>
        /// <param name="size">size of the SQL column</param>
        /// <param name="parmValue">value of the parameter</param>
        /// <returns>Returns a fully populated parameter</returns>
        public DbParameter MakeParameter(string parameterName, DbType dbType, int size, object parmValue)
        {
            DbParameter parameter = MakeParameter(parameterName, dbType, size, parmValue, ParameterDirection.Input);
            return parameter;
        }

        /// <summary>
        /// Makes the parameter
        /// </summary>
        /// <param name="parameterName">name of parameter in Stored Procedure</param>
        /// <param name="dbType">Type of SqlDbType (column type)</param>
        /// <param name="size">size of the SQL column</param>
        /// <param name="parmValue">value of the parameter</param>
        /// <param name="direction">What direction the parameter is...</param>
        /// <returns>Returns a fully populated parameter</returns>
        public DbParameter MakeParameter(string parameterName, DbType dbType, int size, object parmValue,
                                         ParameterDirection direction)
        {
            DbParameter parm = new TParameter
                                   {
                                       Value = parmValue,
                                       Direction = direction,
                                       ParameterName = parameterName,
                                       Size = size,
                                       DbType = dbType
                                   };
            return parm;
        }


        /// <summary>
        /// Populates the collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storedProcedure">The stored procedure.</param>
        /// <param name="getItem">The get item.</param>
        /// <returns></returns>
        public List<T> PopulateCollection<T>(string storedProcedure, Func<INullableReader, T> getItem)
        {
            return PopulateCollection(storedProcedure, new List<DbParameter>(), getItem);
        }

        /// <summary>
        /// Populates the collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storedProcedure">The stored procedure.</param>
        /// <param name="getItem">The get item.</param>
        /// <param name="transaction">The transaction.</param>
        /// <returns></returns>
        public List<T> PopulateCollection<T>(string storedProcedure, Func<INullableReader, T> getItem,
                                             IDbTransaction transaction)
        {
            return PopulateCollection(storedProcedure, new List<DbParameter>(), getItem, transaction);
        }

        /// <summary>
        /// Populates the collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TN"></typeparam>
        /// <param name="storedProcedure">The stored procedure.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="getItem">The get item.</param>
        /// <returns></returns>
        public List<T> PopulateCollection<T, TN>(string storedProcedure, TN parameters, Func<INullableReader, T> getItem)
            where TN : class
        {
            List<DbParameter> dbParameters = GetValuesFromType(parameters);
            return PopulateCollection(storedProcedure, dbParameters, getItem, null);
        }

        /// <summary>
        /// Populates the collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TN"></typeparam>
        /// <param name="storedProcedure">The stored procedure.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="getItem">The get item.</param>
        /// <param name="transaction">The transaction.</param>
        /// <returns></returns>
        public List<T> PopulateCollection<T, TN>(string storedProcedure, TN parameters, Func<INullableReader, T> getItem,
                                                IDbTransaction transaction) where TN : class
        {
            List<DbParameter> dbParameters = GetValuesFromType(parameters);
            return PopulateCollection(storedProcedure, dbParameters, getItem, transaction);
        }

        /// <summary>
        /// Populates the collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storedProcedure">The stored procedure.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="getItem">The get item.</param>
        /// <returns></returns>
        public List<T> PopulateCollection<T>(string storedProcedure, List<DbParameter> parameters,
                                             Func<INullableReader, T> getItem)
        {
            DbService<TConnection, TCommand> dbService = GetDbService();
            dbService.ConnectionStringName = _key;

            List<T> populateCollection = dbService.PopulateCollection(storedProcedure, parameters, getItem, null);
            return populateCollection;
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
        public List<T> PopulateCollection<T>(string storedProcedure, List<DbParameter> parameters,
                                             Func<INullableReader, T> getItem, IDbTransaction transaction)
        {
            DbService<TConnection, TCommand> dbService = GetDbService();
            dbService.ConnectionStringName = _key;

            List<T> populateCollection = dbService.PopulateCollection(storedProcedure, parameters, getItem, transaction);
            return populateCollection;
        }

        /// <summary>
        /// Populates the item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storedProcedure">The stored procedure.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="getItem">The get item.</param>
        /// <returns></returns>
        public T PopulateItem<T>(string storedProcedure, List<DbParameter> parameters, Func<INullableReader, T> getItem)
        {
            DbService<TConnection, TCommand> dbService = GetDbService();
            dbService.ConnectionStringName = _key;

            T item = dbService.PopulateItem(storedProcedure, parameters, getItem, null);
            return item;
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
        public T PopulateItem<T>(string storedProcedure, List<DbParameter> parameters, Func<INullableReader, T> getItem,
                                 IDbTransaction transaction)
        {
            DbService<TConnection, TCommand> dbService = GetDbService();
            dbService.ConnectionStringName = _key;

            T item = dbService.PopulateItem(storedProcedure, parameters, getItem, transaction);
            return item;
        }

        /// <summary>
        /// Populates the item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TN"></typeparam>
        /// <param name="storedProcedure">The stored procedure.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="getItem">The get item.</param>
        /// <param name="transaction">The transaction.</param>
        /// <returns></returns>
        public T PopulateItem<T, TN>(string storedProcedure, TN parameters, Func<INullableReader, T> getItem,
                                    IDbTransaction transaction) where TN : class
        {
            List<DbParameter> dbParameters = GetValuesFromType(parameters);
            return PopulateItem(storedProcedure, dbParameters, getItem, transaction);
        }

        /// <summary>
        /// Populates the item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TN"></typeparam>
        /// <param name="storedProcedure">The stored procedure.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="getItem">The get item.</param>
        /// <returns></returns>
        public T PopulateItem<T, TN>(string storedProcedure, TN parameters, Func<INullableReader, T> getItem)
            where TN : class
        {
            List<DbParameter> dbParameters = GetValuesFromType(parameters);
            return PopulateItem(storedProcedure, dbParameters, getItem, null);
        }

        /// <summary>
        /// Populates the item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storedProcedure">The stored procedure.</param>
        /// <param name="getItem">The get item.</param>
        /// <param name="transaction">The transaction.</param>
        /// <returns></returns>
        public T PopulateItem<T>(string storedProcedure, Func<INullableReader, T> getItem, IDbTransaction transaction)
        {
            return PopulateItem(storedProcedure, new List<DbParameter>(), getItem, transaction);
        }

        /// <summary>
        /// Populates the item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storedProcedure">The stored procedure.</param>
        /// <param name="getItem">The get item.</param>
        /// <returns></returns>
        public T PopulateItem<T>(string storedProcedure, Func<INullableReader, T> getItem)
        {
            return PopulateItem(storedProcedure, new List<DbParameter>(), getItem, null);
        }

        /// <summary>
        /// Gets the type of the values from.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        private List<DbParameter> GetValuesFromType<T>(T instance)
        {
            DbParameterBuilder<TParameter> extractColumnNamesAndValuesService = new DbParameterBuilder<TParameter>(ParameterDelimiter);
            List<DbParameter> parameters = extractColumnNamesAndValuesService.GetValuesFromType(instance);
            return parameters;
        }
    }
}