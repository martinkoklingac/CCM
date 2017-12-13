using Npgsql;
using System;

namespace CCM.Data
{
    public partial class AbstractUnitOfWork :
        IDisposable,
        IUowTransactionContext
    {
        #region PRIVATE FIELDS
        private readonly string _id;
        private readonly IUnitOfWorkConfig _config;
        private readonly NpgsqlConnection _connection;
        private NpgsqlTransaction _transaction;
        private TransactionState _transactionState;
        #endregion

        #region CONSTRUCTORS
        protected AbstractUnitOfWork(
            string id,
            IUnitOfWorkConfig config)
        {
            this._id = id;
            this._config = config;
            this._connection = this.CreateConnection();
            this._transactionState = TransactionState.None;
        }
        #endregion

        #region PUBLIC PROPERTIES
        public string Id => this._id;
        #endregion

        #region PROTECTED PROPERTIES
        protected NpgsqlConnection Connection => this._connection;
        protected NpgsqlTransaction Transaction => this._transaction;
        #endregion

        #region PUBLIC METHODS
        public void Dispose()
        {
            if (this._transaction != null)
                this._transaction.Dispose();

            this._connection.Dispose();
        }
        #endregion

        #region PRIVATE METHODS
        private NpgsqlConnection CreateConnection()
        {
            try { return new NpgsqlConnection(this._config.ConnectionString).Init(); }
            catch (Exception ex)
            {
                throw new UnitOfWorkException(
                    $"[{this._id}] connection failed", ex);
            }
        }
        #endregion

        #region INTERNAL TYPES
        private enum TransactionState
        {
            None,
            Started,
            Committed,
            RolledBack,
            StartError,
            CommitError,
            RollbackError
        }
        #endregion
    }
}
