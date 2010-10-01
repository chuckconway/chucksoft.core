using System.Data;
using System.Data.Common;

namespace Hypersonic
{
    public class Transaction : IDbTransaction 
    {
        private readonly DbTransaction _transaction;

        /// <summary>
        /// Initializes a new instance of the <see cref="Transaction"/> class.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        public Transaction(DbTransaction transaction)
        {
            _transaction = transaction;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _transaction.Dispose();
        }

        /// <summary>
        /// Commits the database transaction.
        /// </summary>
        /// <exception cref="T:System.Exception">An error occurred while trying to commit the transaction. </exception>
        /// <exception cref="T:System.InvalidOperationException">The transaction has already been committed or rolled back.-or- The connection is broken. </exception>
        public void Commit()
        {
            try
            {
                _transaction.Commit();
            }
            finally
            {
                _transaction.Connection.Close();
            }
        }

        /// <summary>
        /// Rolls back a transaction from a pending state.
        /// </summary>
        /// <exception cref="T:System.Exception">An error occurred while trying to commit the transaction. </exception>
        /// <exception cref="T:System.InvalidOperationException">The transaction has already been committed or rolled back.-or- The connection is broken. </exception>
        public void Rollback()
        {
            try
            {
                _transaction.Rollback();
            }
            finally
            {
                _transaction.Connection.Close();
            }
        }

        /// <summary>
        /// Specifies the Connection object to associate with the transaction.
        /// </summary>
        /// <value></value>
        /// <returns>The Connection object to associate with the transaction.</returns>
        public IDbConnection Connection
        {
            get { return _transaction.Connection; }
        }

        /// <summary>
        /// Specifies the <see cref="T:System.Data.IsolationLevel"/> for this transaction.
        /// </summary>
        /// <value></value>
        /// <returns>The <see cref="T:System.Data.IsolationLevel"/> for this transaction. The default is ReadCommitted.</returns>
        public IsolationLevel IsolationLevel
        {
            get { return _transaction.IsolationLevel; }
        }
    }
}
