using System.Collections.Generic;
using System.Data.Common;
using Hypersonic.Services;

namespace Hypersonic.Queries
{
    public class QueryBase<D>: IQuery where D: IParameterBuilder, new() 
    {
        private readonly IParameterBuilder _parameterBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryBase&lt;D&gt;"/> class.
        /// </summary>
        protected QueryBase()
        {
            _parameterBuilder = new D();
        }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public List<DbParameter> GetParameters<T>(T parameters)
        {
            return _parameterBuilder.GetParameters(parameters);
        }

        /// <summary>
        /// Gets or sets the procedure.
        /// </summary>
        /// <value>The procedure.</value>
        public string Procedure { get; set; }

        /// <summary>
        /// Sets the parameters.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters">The parameters.</param>
        public void SetParameters<T>(T parameters) where T: class
        {
            List<DbParameter> dbParameters = GetParameters(parameters);
            Parameters = dbParameters;
        }

        /// <summary>
        /// Gets or sets the parameters.
        /// </summary>
        /// <value>The parameters.</value>
        public List<DbParameter> Parameters { get; private set;}
    }
}
