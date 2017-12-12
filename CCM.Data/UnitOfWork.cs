using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Data;

namespace CCM.Data
{
    public class UnitOfWork :
        IUnitOfWork
    {
        #region PRIVATE FIELDS
        private readonly IUnitOfWorkConfig _config;
        private readonly NpgsqlConnection _connection;
        private readonly ILogger<UnitOfWork> _logger;
        #endregion

        #region CONSTRUCTORS
        public UnitOfWork(
            string id,
            IUnitOfWorkConfig config,
            ILogger<UnitOfWork> logger)
        {
            this.Id = id;
            this._config = config;
            this._logger = logger;

            this.Log("Creating NpgsqlConnection");
            this._connection = new NpgsqlConnection(
                this._config.ConnectionString)
                .Init();
        }
        #endregion

        #region PUBLIC PROPERTIES
        public string Id { get; }
        #endregion

        #region PRIVATE PROPERTIES
        private NpgsqlTransaction Transaction { get; set; }
        #endregion

        #region PUBLIC METHODS
        public NpgsqlConnection GetConnection() => this._connection;

        public void Begin()
        {
            try
            {
                this.Log("Begin transaction");
                this.Transaction = this._connection
                    .BeginTransaction(IsolationLevel.ReadCommitted);
            }
            catch (Exception ex)
            {
                throw new UnitOfWorkException(
                    $"[{Id}] Begin() failed", ex);
            }
        }

        public void Commit()
        {
            try
            {
                this.Log($"Begin commit transaction IsComplete:{this.Transaction.IsCompleted}");
                if (this.Transaction != null && !this.Transaction.IsCompleted)
                {
                    this.Transaction.Commit();
                    this.Log($"Transaction committed IsComplete:{this.Transaction.IsCompleted}");
                }
            }
            catch (Exception ex)
            {
                throw new UnitOfWorkException(
                    $"[{Id}] Commit() failed", ex);
            }
        }

        public void Rollback()
        {
            try
            {
                if (this.Transaction != null)
                    this.Transaction.Rollback();
            }
            catch (Exception ex)
            {
                throw new UnitOfWorkException(
                    $"[{Id}] Rollback() failed", ex);
            }
        }

        public void Dispose()
        {
            this.Transaction.Dispose();
            this._connection.Dispose();
        }
        #endregion

        #region PRIVATE METHODS
        private void Log(string message)
        {
            this._logger.LogTrace($"Request[{Id}] - {message}");
        }
        #endregion
    }
}
