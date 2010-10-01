using System;
using System.Collections.Generic;
using System.Data.Common;
using Hypersonic.Services;

namespace Hypersonic.Queries
{
    public class QueryWithEvents<D> : QueryBase<D> where D : IParameterBuilder, new()
    {
        /// <summary>
        /// Occurs when [post execution].
        /// </summary>
        public event Action<List<DbParameter>, object> PostExecution = delegate { };

        /// <summary>
        /// Occurs when [pre execution].
        /// </summary>
        public event Action<List<DbParameter>> PreExecution = delegate { };

        /// <summary>
        /// Called when [pre execution].
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        public void OnPreExecution(List<DbParameter> parameters)
        {
            PreExecution(parameters);
        }

        /// <summary>
        /// Called when [pre execution].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters">The parameters.</param>
        /// <param name="type">The type.</param>
        public void OnPostExecution(List<DbParameter> parameters, object type)
        {
            PostExecution(parameters, type);
        }

    }
}
