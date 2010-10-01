using System.Data;
using System.Data.SqlClient;

namespace Hypersonic
{
    /// <summary>
    /// 
    /// </summary>
    public class MsSqlDatabase : DatabaseBase<SqlConnection, SqlCommand, SqlParameter>
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="MsSqlDatabase"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        public MsSqlDatabase(string key) : base(key) {}


        /// <summary>
        /// Initializes a new instance of the <see cref="MsSqlDatabase"/> class.
        /// </summary>
        public MsSqlDatabase(){}

        /// <summary>
        /// Initializes a new instance of the <see cref="MsSqlDatabase"/> class.
        /// </summary>
        /// <param name="commandType">Type of the command.</param>
        public MsSqlDatabase(CommandType commandType) :base(commandType) { }

    }
}