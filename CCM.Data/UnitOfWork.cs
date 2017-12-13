using Microsoft.Extensions.Logging;
using Npgsql;

namespace CCM.Data
{
    public class UnitOfWork :
        AbstractUnitOfWork
    {
        #region PRIVATE FIELDS
        private readonly ILogger<UnitOfWork> _logger;
        #endregion

        #region CONSTRUCTORS
        public UnitOfWork(
            string id,
            IUnitOfWorkConfig config,
            ILogger<UnitOfWork> logger) : base(id, config)
        {
            this._logger = logger;
        }
        #endregion

        #region PUBLIC METHODS
        public NpgsqlConnection GetConnection() => base.Connection;
        #endregion

        #region PROTECTED METHODS
        protected override void OnBeginInit() => this.Log("Creating new transaction");
        protected override void OnBeginComplete() => this.Log("Transaction started");
        protected override void OnBeginError() => this.Log("Transaction creation failed");

        protected override void OnCommitInit() => this.Log("Committing transaction");
        protected override void OnCommitComplete() => this.Log("Transaction committed");
        protected override void OnCommitError() => this.Log("Transaction commit failed");

        protected override void OnRollbackInit() => this.Log("Rolling back transaction");
        protected override void OnRollbackComplete() => this.Log("Transaction rolled back");
        protected override void OnRollbackError() => this.Log("Rollback failed");
        #endregion

        #region PRIVATE METHODS
        private void Log(string message) => this._logger.LogTrace($"Request[{Id}] -> {message}");
        #endregion
    }
}
