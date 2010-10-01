using System;
using System.Collections.Generic;
using System.Data.Common;
using Hypersonic.Queries;
using Hypersonic.Services;

namespace Hypersonic.Repositories
{
    public class Repository<D> :IRepository<D> where D :  IDatabase, new()
    {
        private readonly IDatabase _database;
        private readonly IParameterBuilder _parameterBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository&lt;D&gt;"/> class.
        /// </summary>
        public Repository()
        {
            _database = new D();
            _parameterBuilder = new D();
        }

        /// <summary>
        /// Gets the specified query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procedure">The procedure.</param>
        /// <returns></returns>
        public List<T> Get<T>(string procedure) where T : class, new()
        {
            CheckIfCommandTextIsNull(procedure);
            return _database.PopulateCollection(procedure, _database.AutoPopulate<T>);
        }

        /// <summary>
        /// Gets the specified query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public List<T> Get<T>(IQuery query) where T : class, new()
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            CheckIfCommandTextIsNull(query.Procedure);
            return _database.PopulateCollection(query.Procedure, query.Parameters, _database.AutoPopulate<T>);
        }

        /// <summary>
        /// Gets the specified query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public List<T> Get<T>(QueryWithEvents<D> query) where T : class, new()
        {
            query.OnPreExecution(query.Parameters);
            List<T> collection = _database.PopulateCollection(query.Procedure, query.Parameters, _database.AutoPopulate<T>);
            query.OnPostExecution(query.Parameters, collection);

            return collection;
        }

        /// <summary>
        /// Executes the specified query.
        /// </summary>
        /// <param name="query">The query.</param>
        public void Execute(IQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            CheckIfCommandTextIsNull(query.Procedure);
            _database.NonQuery(query.Procedure, query.Parameters);
        }

        /// <summary>
        /// Checks if command text is null.
        /// </summary>
        /// <param name="commandText">The command text.</param>
        private static void CheckIfCommandTextIsNull(string commandText)
        {
            if (string.IsNullOrEmpty(commandText))
            {
                throw new Exception("Command Text is expected.");
            }
        }

        /// <summary>
        /// Executes the specified procedure.
        /// </summary>
        /// <param name="procedure">The procedure.</param>
        public void Execute(string procedure)
        {
            CheckIfCommandTextIsNull(procedure);

            List<DbParameter> dbParameters = new List<DbParameter>();
            _database.NonQuery(procedure, dbParameters);
        }

        /// <summary>
        /// Executes the specified procedure.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procedure">The procedure.</param>
        /// <param name="parameters">The parameters.</param>
        public void Execute<T>(string procedure, T parameters) where T : class
        {
            CheckIfCommandTextIsNull(procedure);
            List<DbParameter> list = _parameterBuilder.GetParameters(parameters);
            _database.NonQuery(procedure, list);
        }
    }
}
