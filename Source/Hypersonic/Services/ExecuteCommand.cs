using System;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Text;

namespace Hypersonic.Services
{
    public class ExecuteCommand
    {
        /// <summary>
        /// Executes the non query.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        public int ExecuteNonQuery(DbCommand command)
        {
            int returnValue;
            using (DbConnection connnection = command.Connection)
            {
                GenerateDebugInformation(command, connnection);
                connnection.Open();

                DateTime start = DateTime.Now;
                Debug.WriteLine(string.Format("Start: {0}", start));

                returnValue = command.ExecuteNonQuery();

                DateTime end = DateTime.Now;
                Debug.WriteLine(string.Format("End: {0}", end));
                Debug.WriteLine(string.Format("Elapsed Time: {0}", (end - start)));
            }
            return returnValue;
        }

        /// <summary>
        /// Executes the non query in transaction.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        public int ExecuteNonQueryInTransaction(DbCommand command)
        {
            DbConnection connnection = command.Connection;
            GenerateDebugInformation(command, connnection);
            connnection.Open();

            DateTime start = DateTime.Now;
            Debug.WriteLine(string.Format("Start: {0}", start));

            int returnValue = command.ExecuteNonQuery();

            DateTime end = DateTime.Now;
            Debug.WriteLine(string.Format("End: {0}", end));
            Debug.WriteLine(string.Format("Elapsed Time: {0}", (end - start)));

            return returnValue;
        }

        /// <summary>
        /// Generates the debug information.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="connnection">The connnection.</param>
        private static void GenerateDebugInformation(DbCommand command, DbConnection connnection)
        {
            Debug.WriteLine(string.Empty);
            Debug.WriteLine("-----------------------------------------------------------------");
            Debug.WriteLine(string.Format("Connection String: {0}", connnection.ConnectionString));
            Debug.WriteLine(string.Format("Procedure/CommandText: {0}", command.CommandText));
            Debug.WriteLine(string.Format("CommandType: {0}", command.CommandType));
            Debug.WriteLine(string.Format("{0}", GetParameters(command)));
        }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        private static string GetParameters(DbCommand command)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("[Parameters]");

            if (command.Parameters.Count > 0)
            {

                foreach (DbParameter parameter in command.Parameters)
                {
                    const string format = "Name: {0}, Value: {1}, Type: {2}, Direction: {3},";
                    builder.AppendLine(string.Format(format, parameter.ParameterName,
                                                     parameter.Value,
                                                     parameter.DbType,
                                                     parameter.Direction));
                }
            }
            else
            {
                builder.Append("None");
            }

            builder.Append(Environment.NewLine +"[/Parameters]");

            return builder.ToString();
        }

        /// <summary>
        /// Executes the scalar.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command">The command.</param>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        public T ExecuteScalar<T>(DbCommand command, T result)
        {
            using (DbConnection connnection = command.Connection)
            {
                GenerateDebugInformation(command, connnection);
                connnection.Open();

                DateTime start = DateTime.Now;
                Debug.WriteLine(string.Format("Start: {0}", start));
                
                object val = command.ExecuteScalar();

                DateTime end = DateTime.Now;
                Debug.WriteLine(string.Format("End: {0}", end));
                Debug.WriteLine(string.Format("Elapsed Time: {0}", (end - start)));

                //catch DBNull return value
                if (!(val.GetType().Equals(DBNull.Value.GetType())))
                {
                    result = (T)val;
                }
            }
            return result;
        }

        /// <summary>
        /// Executes the scalar in transaction.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command">The command.</param>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        public T ExecuteScalarInTransaction<T>(DbCommand command, T result)
        {
            DbConnection connnection = command.Connection;
            GenerateDebugInformation(command, connnection);
            connnection.Open();

            DateTime start = DateTime.Now;
            Debug.WriteLine(string.Format("Start: {0}", start));

            object val = command.ExecuteScalar();

            DateTime end = DateTime.Now;
            Debug.WriteLine(string.Format("End: {0}", end));
            Debug.WriteLine(string.Format("Elapsed Time: {0}", (end - start)));

            //catch DBNull return value
            if (!(val.GetType().Equals(DBNull.Value.GetType())))
            {
                result = (T)val;
            }

            return result;
        }

        /// <summary>
        /// Executes the reader.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        public DbDataReader ExecuteReader(DbCommand command)
        {
            GenerateDebugInformation(command, command.Connection);

            DateTime start = DateTime.Now;
            Debug.WriteLine(string.Format("Start: {0}", start));

            DbDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);

            DateTime end = DateTime.Now;
            Debug.WriteLine(string.Format("End: {0}", end));
            Debug.WriteLine(string.Format("Elapsed Time: {0}", (end - start)));

            return reader;
        }

        /// <summary>
        /// Executes the reader in transaction.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        public DbDataReader ExecuteReaderInTransaction(DbCommand command)
        {
            GenerateDebugInformation(command, command.Connection);

            DateTime start = DateTime.Now;
            Debug.WriteLine(string.Format("Start: {0}", start));

            DbDataReader reader = command.ExecuteReader();

            DateTime end = DateTime.Now;
            Debug.WriteLine(string.Format("End: {0}", end));
            Debug.WriteLine(string.Format("Elapsed Time: {0}", (end - start)));

            return reader;
        }
    }
}
