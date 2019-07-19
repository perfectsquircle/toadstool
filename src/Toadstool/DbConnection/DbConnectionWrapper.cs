using System;
using System.Data;

namespace Toadstool
{
    internal class DbConnectionWrapper : IDbConnectionWrapper, IDbTransactionWrapper
    {
        public IDbConnection DbConnection { get; private set; }
        public IDbTransaction DbTransaction { get; private set; }
        public CommandBehavior CommandBehavior => _transactionIsComplete ? CommandBehavior.CloseConnection : CommandBehavior.Default;
        private bool _transactionIsComplete;

        public DbConnectionWrapper(IDbConnection dbConnection)
        {
            DbConnection = dbConnection;
            _transactionIsComplete = true;
        }

        public DbConnectionWrapper(IDbConnection dbConnection, IDbTransaction transaction)
        {
            DbConnection = dbConnection;
            DbTransaction = transaction;
        }

        public IDbCommand CreateCommand()
        {
            var command = DbConnection.CreateCommand();
            command.Connection = DbConnection;
            command.Transaction = DbTransaction;
            return command;
        }

        public void Commit()
        {
            _transactionIsComplete = true;
            DbTransaction?.Commit();
        }

        public void Rollback()
        {
            _transactionIsComplete = true;
            DbTransaction?.Rollback();
        }

        public void Dispose()
        {
            Dispose(false);
        }

        public void Dispose(bool force = false)
        {
            if (force || _transactionIsComplete)
            {
                DbTransaction?.Dispose();
                DbConnection?.Dispose();
            }
        }
    }
}