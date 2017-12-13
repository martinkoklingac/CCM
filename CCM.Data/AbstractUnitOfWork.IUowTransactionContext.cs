using System;
using System.Data;

namespace CCM.Data
{
    public abstract partial class AbstractUnitOfWork :
        IUowTransactionContext
    {
        #region PRIVATE PROPERTIES
        private bool CanBeginTransaction => this._transactionState == TransactionState.None;
        private bool CanCommitTransaction =>
            this._transactionState == TransactionState.Started
            && this._transaction != null
            && !this._transaction.IsCompleted;
        private bool CanRollbackTransaction =>
            this._transactionState == TransactionState.Started
            && this._transaction != null;
        #endregion

        #region PUBLIC METHODS
        public void Begin()
        {
            if (!this.CanBeginTransaction)
                return;

            try
            {
                this.OnBeginInit();
                this._transaction = this._connection
                    .BeginTransaction(IsolationLevel.ReadCommitted);
                this._transactionState = TransactionState.Started;
                this.OnBeginComplete();
            }
            catch (Exception ex)
            {
                this._transactionState = TransactionState.StartError;
                this.OnBeginError();

                throw new UnitOfWorkException(
                    $"[{this._id}] transaction init failed", ex);
            }
        }

        public void Commit()
        {
            if (!this.CanCommitTransaction)
                return;

            try
            {
                this.OnCommitInit();
                this._transaction.Commit();
                this._transactionState = TransactionState.Committed;
                this.OnCommitComplete();
            }
            catch (Exception ex)
            {
                this._transactionState = TransactionState.CommitError;
                this.OnCommitError();

                throw new UnitOfWorkException(
                    $"[{this._id}] transaction commit failed", ex);
            }
        }

        public void Rollback()
        {
            if (!this.CanRollbackTransaction)
                return;

            try
            {
                this.OnRollbackInit();
                this._transaction.Rollback();
                this._transactionState = TransactionState.RolledBack;
                this.OnRollbackComplete();
            }
            catch (Exception ex)
            {
                this._transactionState = TransactionState.RollbackError;
                this.OnRollbackError();

                throw new UnitOfWorkException(
                    $"[{this._id}] transaction rollback failed", ex);
            }
        }
        #endregion

        #region PROTECTED METHODS
        protected virtual void OnBeginInit() { }
        protected virtual void OnBeginComplete() { }
        protected virtual void OnBeginError() { }

        protected virtual void OnCommitInit() { }
        protected virtual void OnCommitComplete() { }
        protected virtual void OnCommitError() { }

        protected virtual void OnRollbackInit() { }
        protected virtual void OnRollbackComplete() { }
        protected virtual void OnRollbackError() { }
        #endregion
    }
}
