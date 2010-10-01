using System.Collections.Generic;
using Hypersonic.Queries;

namespace Hypersonic.Repositories
{
    public interface IRepository<D> where D : IDatabase, new()
    {
        /// <summary>
        /// Gets the specified procedure.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procedure">The procedure.</param>
        /// <returns></returns>
        List<T> Get<T>(string procedure) where T : class, new();

        /// <summary>
        /// Gets the specified query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        List<T> Get<T>(IQuery query) where T : class, new();

        /// <summary>
        /// Executes the specified query.
        /// </summary>
        /// <param name="query">The query.</param>
        void Execute(IQuery query);

        /// <summary>
        /// Executes the specified procedure.
        /// </summary>
        /// <param name="procedure">The procedure.</param>
        void Execute(string procedure);

        /// <summary>
        /// Executes the specified procedure.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procedure">The procedure.</param>
        /// <param name="parameters">The parameters.</param>
        void Execute<T>(string procedure, T parameters) where T : class;

        /// <summary>
        /// Gets the specified query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        List<T> Get<T>(QueryWithEvents<D> query) where T : class, new();
    }
}