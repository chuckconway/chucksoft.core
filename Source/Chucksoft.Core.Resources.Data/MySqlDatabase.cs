using MySql.Data.MySqlClient;

namespace Chucksoft.Core.Data
{
    public class MySqlDatabase : DatabaseBase<MySqlConnection, MySqlCommand, MySqlParameter>, IDatabase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MsSqlDatabase"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        public MySqlDatabase(string key) : base(key) {}


        /// <summary>
        /// Initializes a new instance of the <see cref="MsSqlDatabase"/> class.
        /// </summary>
        public MySqlDatabase()
        {
            CommandType = System.Data.CommandType.Text;
        }
    }
}